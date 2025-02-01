using System.Management.Automation;
using System.Security;
using System.Text;
using System.Text.Json;
using Jagabata.Cmdlets.Utilities;
using Jagabata.Resources;

namespace Jagabata.Cmdlets;

public abstract class LaunchJobCommandBase : APICmdletBase, IDisposable
{
    ~LaunchJobCommandBase()
    {
        Dispose(false);
    }
    protected readonly JobProgressManager JobProgressManager = [];
    private Sleep? _sleep;
    protected void Sleep(int milliseconds)
    {
        using (_sleep = new Sleep())
        {
            _sleep.Do(milliseconds);
        }
    }
    protected void WriteJobIndicator(JobProgress jp, bool suppressJobLog)
    {
        WriteHost($"====== [{jp.Id}] {jp.Job?.Name} ======\n",
                  foregroundColor: ConsoleColor.Magenta,
                  tags: ["Ansible", "Indicator", $"job-{jp.Id}"],
                  dontshow: suppressJobLog);
    }
    private ulong lastShownJob = 0;
    protected void WriteJobLog(JobProgress jp, bool suppressJobLog)
    {
        if (string.IsNullOrWhiteSpace(jp.CurrentLog))
        {
            return;
        }
        if (lastShownJob != jp.Id)
        {
            WriteJobIndicator(jp, suppressJobLog);
        }
        WriteHost(jp.CurrentLog,
                  tags: ["Ansible", "JobLog", $"job-{jp.Id}"],
                  dontshow: suppressJobLog);
        lastShownJob = jp.Id;
    }

    protected void WaitJobs(string activityId,
                            int intervalSeconds,
                            bool suppressJobLog)
    {
        if (JobProgressManager.Count == 0)
        {
            return;
        }
        JobProgressManager.Start(activityId, intervalSeconds);
        do
        {
            UpdateAllProgressRecordType(ProgressRecordType.Processing);
            for (var i = 1; i <= intervalSeconds; i++)
            {
                Sleep(1000);
                JobProgressManager.UpdateProgress(i);
                WriteProgress(JobProgressManager.RootProgress);
            }
            JobProgressManager.UpdateJob();
            // Remove Progressbar
            UpdateAllProgressRecordType(ProgressRecordType.Completed);

            ShowJobLog(suppressJobLog);

            WriteObject(JobProgressManager.CleanCompleted(), true);
        } while (JobProgressManager.Count > 0);
    }

    private void UpdateAllProgressRecordType(ProgressRecordType type)
    {
        JobProgressManager.RootProgress.RecordType = type;
        WriteProgress(JobProgressManager.RootProgress);
        foreach (var jp in JobProgressManager.GetAll())
        {
            jp.Progress.RecordType = type;
            WriteProgress(jp.Progress);
        }
    }

    protected void ShowJobLog(bool suppressJobLog)
    {
        var jpList = JobProgressManager.GetJobLog();
        foreach (var jp in jpList)
        {
            if (jp is null) continue;
            WriteJobLog(jp, suppressJobLog);
        }
    }

    protected override void StopProcessing()
    {
        _sleep?.Stop();
    }

    protected void PrintPromptResult(string label, string resultValue, bool notSpecified = false)
    {
        var ui = CommandRuntime.Host?.UI;
        if (ui is null) return;
        var bg = Console.BackgroundColor;
        ui.Write(ConsoleColor.Green, bg, "==> ");
        var sb = new StringBuilder();
        if (notSpecified)
        {
            sb.Append($"Not specified {label}. Will be used default");
        }
        else
        {
            sb.Append($"Accepted {label}");
        }
        if (!string.IsNullOrEmpty(resultValue))
        {
            sb.Append($": {resultValue}");
        }
        WriteHost(sb.ToString());
        ui.WriteLine("\n");
    }

    // Store SecureString gotten from Survey and Credential passwords.
    protected List<SecureString> SecureStrings = [];

    protected void ClearSecureStrings()
    {
        foreach (var ss in SecureStrings)
        {
            ss.Dispose();
        }
        SecureStrings.Clear();
    }

    /// <summary>
    /// Get SurveySpecs and show input prompts.
    /// </summary>
    /// <param name="type">Resource Type. Should be either <see cref="ResourceType.JobTemplate"/> or <see cref="ResourceType.WorkflowJobTemplate"/></param>
    /// <param name="id">ID number of either JobTemplate or WorkflowJobTemplate</param>
    /// <param name="onlyRequired">Show prompts for only required.</param>
    /// <param name="sendData">Results of input prompts will be stored into <c>extra_vars</c> key's property in this dictionary.</param>
    /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
    protected bool AskSurvey(ResourceType type, ulong id, bool onlyRequired, IDictionary<string, object?> sendData)
    {
        var surveyPath = type switch
        {
            ResourceType.JobTemplate => $"{JobTemplate.PATH}{id}/survey_spec/",
            ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{id}/survey_spec/",
            _ => throw new ArgumentException($"Invalid Resource Type: {type}")
        };

        var survey = GetResource<Resources.Survey>(surveyPath);
        var extraVars = sendData.ContainsKey("extra_vars")
            ? Yaml.DeserializeToDict(sendData["extra_vars"] as string ?? "")
            : new Dictionary<string, object?>();
        if (survey.Spec.Length > 0)
        {
            if (CommandRuntime.Host is null)
                return false;

            var prompt = new AskPrompt(CommandRuntime.Host);

            foreach (var spec in survey.Spec)
            {
                var varName = spec.Variable;
                if (extraVars.ContainsKey(varName))
                {
                    WriteHost($"Skip Survey[{varName}] prompt. Already specified: {JsonSerializer.Serialize(extraVars[varName])}",
                              dontshow: true);
                    continue;
                }
                if (onlyRequired && !spec.Required)
                    continue;

                var label = $"Survey {spec.Name}";
                var key = $"extra_vars.{varName}";
                var description = $"Variable: [{varName}]"
                    + (string.IsNullOrEmpty(spec.Description) ? "" : $", Description: {spec.Description}");
                switch (spec.Type)
                {
                    case SurveySpecType.Text:
                    case SurveySpecType.Textarea: // FIXME
                        if (prompt.Ask(label, key, spec.Default as string, description, out var stringAnswer))
                        {
                            extraVars[varName] = stringAnswer.Input;
                            PrintPromptResult(varName, $"\"{stringAnswer.Input}\"", stringAnswer.IsEmpty);
                            continue;
                        }
                        return false;
                    case SurveySpecType.Integer:
                        int? intDefault = string.IsNullOrEmpty(spec.Default as string) ? null : (int)spec.Default;
                        if (prompt.Ask<int>(label, key, (int?)spec.Default, description, spec.Required, out var intAnswer))
                        {
                            extraVars[varName] = intAnswer.Input;
                            PrintPromptResult(varName, $"{intAnswer.Input}", intAnswer.IsEmpty);
                            continue;
                        }
                        return false;
                    case SurveySpecType.Float:
                        float? floatDefault = string.IsNullOrEmpty(spec.Default as string) ? null : (float)spec.Default;
                        if (prompt.Ask<float>(label, key, floatDefault, description, spec.Required, out var floatAnswer))
                        {
                            extraVars[varName] = floatAnswer.Input;
                            PrintPromptResult(varName, $"{floatAnswer.Input}", floatAnswer.IsEmpty);
                            continue;
                        }
                        return false;
                    case SurveySpecType.MultipleChoice:
                        var choiceFields = (spec.Choices as string[] ?? []).Select(val => (val, val)).ToArray();
                        if (prompt.AskSelectOne(label, choiceFields, spec.Default as string ?? "", description, out var oneAnswer))
                        {
                            extraVars[varName] = oneAnswer.Input;
                            PrintPromptResult(varName, $"\"{oneAnswer.Input}\"", oneAnswer.IsEmpty);
                            continue;
                        }
                        return false;
                    case SurveySpecType.MultiSelect:
                        var multiFields = (spec.Choices as string[] ?? []).Select(val => (val, val)).ToArray();
                        var defaultValues = (spec.Default as string ?? "").Split('\n');
                        if (prompt.AskSelectMulti(label, key, multiFields, defaultValues, description, out var multiAnswer))
                        {
                            extraVars[varName] = multiAnswer.Input;
                            PrintPromptResult(varName, $"[{string.Join(", ", multiAnswer.Input.Select(x => $"\"{x}\""))}]", multiAnswer.IsEmpty);
                            continue;
                        }
                        return false;
                    case SurveySpecType.Password:
                        if (prompt.AskPassword(label, key, description, out var passwordAnswer))
                        {
                            extraVars[varName] = passwordAnswer.Input;
                            SecureStrings.Add(passwordAnswer.Input);
                            PrintPromptResult(varName, string.Empty);
                            continue;
                        }
                        return false;
                }
            }
            sendData["extra_vars"] = JsonSerializer.Serialize(extraVars);
        }
        return true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposed;
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_sleep is not null)
                {
                    _sleep.Dispose();
                    _sleep = null;
                }
            }
            _disposed = true;
        }
    }
}
