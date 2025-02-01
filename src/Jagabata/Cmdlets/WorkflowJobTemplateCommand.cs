using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Cmdlets.Utilities;
using Jagabata.Resources;
using System.Management.Automation;
using System.Text;
using System.Text.Json;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "WorkflowJobTemplate")]
    [OutputType(typeof(WorkflowJobTemplate))]
    public class GetWorkflowJobTemplateCommand : GetCommandBase<WorkflowJobTemplate>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJobTemplate])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.WorkflowJobTemplate)]
        public override ulong[] Id { get; set; } = [];

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "WorkflowJobTemplate")]
    [OutputType(typeof(WorkflowJobTemplate))]
    public class FindWorkflowJobTemplateCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Organization)]
        public ulong Organization { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "last_job_run",
                           "last_job_failed", "next_job_run", "status", "organization", "survey_enabled",
                           "allow_simultaneous", "ask_variables_on_launch", "inventory",
                           "ask_inventory_on_launch", "ask_scm_branch_on_launch", "ask_limit_on_launch",
                           "webhook_service", "webhook_credential", "ask_labels_on_launch",
                           "ask_skip_tags_on_launch", "ask_tags_on_launch", "notification_templates_error",
                           "notification_templates_success", "notification_templates_approvals",
                           "notification_templates_started", "inventory", "organization", "last_job",
                           "schedules", "created_by", "modified_by", "labels", "next_schedule")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Organization > 0 ? $"{Resources.Organization.PATH}{Organization}/workflow_job_templates/" : WorkflowJobTemplate.PATH;
            Find<WorkflowJobTemplate>(path);
        }
    }

    public abstract class LaunchWorkflowJobTemplateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, Position = 0)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "WorkflowJobTemplate", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.WorkflowJobTemplate])]
        public IResource? WorkflowJobTemplate { get; set; }

        [Parameter()]
        public string? Limit { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        public string? ScmBranch { get; set; }

        [Parameter()] // XXX: Should be string[] and created if not exists ?
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Label])]
        public ulong[]? Labels { get; set; }

        [Parameter()]
        public string[]? Tags { get; set; }

        [Parameter()]
        public string[]? SkipTags { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation] // Translate IDictionary to JSON string
        public string? ExtraVars { get; set; }

        [Parameter()]
        public SwitchParameter Interactive { get; set; }

        private IDictionary<string, object?> CreateSendData()
        {
            var dict = new Dictionary<string, object?>();
            if (Inventory is not null)
            {
                dict.Add("inventory", Inventory);
            }
            if (ScmBranch is not null)
            {
                dict.Add("scm_branch", ScmBranch);
            }
            if (Limit is not null)
            {
                dict.Add("limit", Limit);
            }
            if (Labels is not null)
            {
                dict.Add("labels", Labels);
            }
            if (Tags is not null)
            {
                dict.Add("job_tags", string.Join(',', Tags));
            }
            if (SkipTags is not null)
            {
                dict.Add("skip_tags", string.Join(',', SkipTags));
            }
            if (!string.IsNullOrEmpty(ExtraVars))
            {
                dict.Add("extra_vars", ExtraVars);
            }
            return dict;
        }
        protected void GetLaunchRequirements(ulong id)
        {
            var res = base.GetResource<WorkflowJobTemplateLaunchRequirements>($"{Resources.WorkflowJobTemplate.PATH}{id}/launch/");
            WriteObject(res, false);
        }
        private void ShowJobTemplateInfo(WorkflowJobTemplateLaunchRequirements requirements)
        {
            var wjt = requirements.WorkflowJobTemplateData;
            var def = requirements.Defaults;
            var (fixedColor, implicitColor, explicitColor, requiredColor) =
                ((ConsoleColor?)null, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.Red);
            WriteHost($"[{wjt.Id}] {wjt.Name} - {wjt.Description}\n");
            var fmt = "{0,22} : {1}\n";
            if (def.Inventory.Id is not null || Inventory is not null)
            {
                var inventoryVal = $"[{def.Inventory.Id}] {def.Inventory.Name}"
                                   + (requirements.AskInventoryOnLaunch && Inventory is not null ? $" => {Inventory}" : "");
                WriteHost(string.Format(fmt, "Inventory", inventoryVal),
                            foregroundColor: requirements.AskInventoryOnLaunch ? (Inventory is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.Limit) || Limit is not null)
            {
                var limitVal = def.Limit
                               + (requirements.AskLimitOnLaunch && Limit is not null ? $" => {Limit}" : "");
                WriteHost(string.Format(fmt, "Limit", limitVal),
                            foregroundColor: requirements.AskLimitOnLaunch ? (Limit is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.ScmBranch) || ScmBranch is not null)
            {
                var branchVal = def.ScmBranch
                                + (requirements.AskScmBranchOnLaunch && ScmBranch is not null ? $" => {ScmBranch}" : "");
                WriteHost(string.Format(fmt, "Scm Branch", branchVal),
                            foregroundColor: requirements.AskScmBranchOnLaunch ? (ScmBranch is null ? implicitColor : explicitColor) : fixedColor);
            }
            if ((def.Labels is not null && def.Labels.Length > 0) || Labels is not null)
            {
                var labelsVal = string.Join(", ", def.Labels?.Select(l => $"[{l.Id}] {l.Name}") ?? [])
                                + (requirements.AskLabelsOnLaunch && Labels is not null ? $" => {string.Join(',', Labels.Select(id => $"[{id}]"))}" : "");
                WriteHost(string.Format(fmt, "Labels", labelsVal),
                            foregroundColor: requirements.AskLabelsOnLaunch ? (Labels is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.JobTags) || Tags is not null)
            {
                var tagsVal = def.JobTags
                              + (requirements.AskTagsOnLaunch && Tags is not null ? $" => {string.Join(", ", Tags)}" : "");
                WriteHost(string.Format(fmt, "Job tags", tagsVal),
                            foregroundColor: requirements.AskTagsOnLaunch ? (Tags is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.SkipTags) || SkipTags is not null)
            {
                var skipTagsVal = def.SkipTags
                                  + (requirements.AskSkipTagsOnLaunch && SkipTags is not null ? $" => {string.Join(", ", SkipTags)}" : "");
                WriteHost(string.Format(fmt, "Skip tags", skipTagsVal),
                            foregroundColor: requirements.AskSkipTagsOnLaunch ? (SkipTags is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.ExtraVars) || !string.IsNullOrEmpty(ExtraVars))
            {
                var sb = new StringBuilder();
                var lines = def.ExtraVars.Split('\n');
                var padding = "".PadLeft(25);
                sb.Append(string.Format(fmt, "Extra vars", lines[0]));
                foreach (var line in lines[1..])
                {
                    sb.AppendLine(padding + line);
                }
                if (!string.IsNullOrEmpty(ExtraVars))
                {
                    sb.AppendLine($"{padding}=> (overwrite or append)");
                    lines = ExtraVars.Split('\n');
                    foreach (var line in lines)
                    {
                        sb.AppendLine(padding + line);
                    }
                }
                WriteHost(sb.ToString(),
                            foregroundColor: requirements.AskVariablesOnLaunch ? (ExtraVars is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (requirements.SurveyEnabled)
            {
                WriteHost(string.Format(fmt, "Survey", "Enabled"), foregroundColor: requiredColor);
            }
            if (requirements.VariablesNeededToStart.Length > 0)
            {
                WriteHost(string.Format(fmt, "Variables", $"[{string.Join(", ", requirements.VariablesNeededToStart)}]"),
                            foregroundColor: requiredColor);
            }
            if (requirements.NodePromptsRejected.Length > 0)
            {
                WriteWarning("Prompt Input will be ignored in following nodes: " +
                        $"[{string.Join(", ", requirements.NodePromptsRejected)}]");
            }
        }

        /// <summary>
        /// Show input prompt and Update <paramref name="sendData"/>.
        /// </summary>
        /// <param name="requirements"></param>
        /// <param name="sendData">Dictionary object that is the source of the JSON string sent to AWX/AnsibleTower</param>
        /// <param name="checkOptional">
        ///   <c>true</c>(`Interactive` mode)  => Check both <c>***NeededToStart</c> and <c>**AskInventoryOnLaunch</c>.
        ///   <c>false</c> => Check only <c>***NeededToStart</c>.
        /// </param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        protected bool TryAskOnLaunch(WorkflowJobTemplateLaunchRequirements requirements,
                                      IDictionary<string, object?> sendData,
                                      bool checkOptional = false)
        {
            if (requirements.CanStartWithoutUserInput)
            {
                return true;
            }
            if (CommandRuntime.Host is null)
            {
                return false;
            }
            var prompt = new AskPrompt(CommandRuntime.Host);
            string key;
            string label;
            string skipFormat = "Skip {0} prompt. Already specified: {1:g}";

            // Inventory
            if (checkOptional && requirements.AskInventoryOnLaunch)
            {
                key = "inventory"; label = "Inventory";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<ulong>(label, "",
                                           defaultValue: requirements.Defaults.Inventory.Id,
                                           helpMessage: "Input an Inventory ID.",
                                           required: false,
                                           out var inventoryAnswer))
                {
                    if (!inventoryAnswer.IsEmpty && inventoryAnswer.Input > 0)
                    {
                        sendData[key] = inventoryAnswer.Input;
                        PrintPromptResult(label, $"{inventoryAnswer.Input}");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.Inventory}", true);
                    }
                }
                else { return false; }
            }

            // ScmBranch
            if (checkOptional && requirements.AskScmBranchOnLaunch)
            {
                key = "scm_branch"; label = "ScmBranch";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask(label, "",
                                    defaultValue: requirements.Defaults.ScmBranch,
                                    helpMessage: "Enter the SCM branch name (or commit hash or tag)",
                                    out var branchAnswer))
                {
                    if (!branchAnswer.IsEmpty)
                    {
                        sendData[key] = branchAnswer.Input;
                        PrintPromptResult(label, $"\"{branchAnswer.Input}\"");
                    }
                    else
                    {
                        PrintPromptResult(label, $"\"{requirements.Defaults.ScmBranch}\"", true);
                    }
                }
                else { return false; }
            }

            // Labels
            if (checkOptional && requirements.AskLabelsOnLaunch)
            {
                key = "labels"; label = "Labels";
                if (sendData.ContainsKey(key))
                {
                    var strData = $"[{string.Join(", ", (ulong[]?)sendData[key] ?? [])}]";
                    WriteHost(string.Format(skipFormat, label, strData), dontshow: true);
                }
                else if (prompt.AskList<ulong>(label, "",
                                               defaultValues: requirements.Defaults.Labels?.Select(x => $"[{x.Id}] {x.Name}") ?? [],
                                               helpMessage: "Enter Label ID(s).",
                                               out var labelsAnswer))
                {
                    if (!labelsAnswer.IsEmpty)
                    {
                        var arr = labelsAnswer.Input.Where(x => x > 0).ToArray();
                        sendData[key] = arr;
                        PrintPromptResult(label, $"[{string.Join(", ", arr)}]");
                    }
                    else
                    {
                        PrintPromptResult(label,
                                    $"[{string.Join(", ", requirements.Defaults.Labels?.Select(x => $"{x}") ?? [])}]",
                                    true);
                    }
                }
                else { return false; }
            }

            // Limit
            if (checkOptional && requirements.AskLimitOnLaunch)
            {
                key = "limit"; label = "Limit";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask(label, "",
                                    defaultValue: requirements.Defaults.Limit,
                                    helpMessage: """
                                    Enter the host pattern to further constrain the list of host managed or affected by the playbook.
                                    Multiple patterns can be separated by commas(`,`) or colons(`:`).
                                    """,
                                    out var limitAnswer))
                {
                    if (!limitAnswer.IsEmpty)
                    {
                        sendData[key] = limitAnswer.Input;
                        PrintPromptResult(label, $"\"{limitAnswer.Input}\"");
                    }
                    else
                    {
                        PrintPromptResult(label, $"\"{requirements.Defaults.Limit}\"", true);
                    }
                }
                else { return false; }
            }

            // Tags
            if (checkOptional && requirements.AskTagsOnLaunch)
            {
                key = "job_tags"; label = "Job Tags";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask(label, "",
                                    defaultValue: requirements.Defaults.JobTags,
                                    helpMessage: """
                                    Enter the tags. Multiple values can be separated by commas(`,`).
                                    This is same as the `--tags` command-line parameter for `ansible-playbook`.
                                    """,
                                    out var jobTagsAnswer))
                {
                    if (!jobTagsAnswer.IsEmpty)
                    {
                        sendData[key] = jobTagsAnswer.Input;
                        PrintPromptResult(label, $"\"{jobTagsAnswer.Input}\"");
                    }
                    else
                    {
                        PrintPromptResult(label, $"\"{requirements.Defaults.JobTags}\"", true);
                    }
                }
                else { return false; }
            }

            // SkipTags
            if (checkOptional && requirements.AskSkipTagsOnLaunch)
            {
                key = "skip_tags"; label = "Skip Tags";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask(label, "",
                                    defaultValue: requirements.Defaults.JobTags,
                                    helpMessage: """
                                    Enter the skip tags. Multiple values can be separated by commas(`,`).
                                    This is same as the `--skip-tags` command-line parameter for `ansible-playbook`.
                                    """,
                                    out var skipTagsAnswer))
                {
                    if (!skipTagsAnswer.IsEmpty)
                    {
                        sendData[key] = skipTagsAnswer.Input;
                        PrintPromptResult(label, $"\"{skipTagsAnswer.Input}\"");
                    }
                    else
                    {
                        PrintPromptResult(label, $"\"{requirements.Defaults.SkipTags}\"", true);
                    }
                }
                else { return false; }
            }

            // ExtraVars
            if (checkOptional && requirements.AskVariablesOnLaunch)
            {
                key = "extra_vars"; label = "Extra Variables";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask(label, "",
                                    defaultValue: requirements.Defaults.ExtraVars,
                                    helpMessage: """
                                    Enter the extra variables provided key/value pairs using either YAML or JSON, to be passed to the playbook.
                                    This is same as the `-e` or `--extra-vars` command-line parameter for `ansible-playbook`.
                                    """,
                                    out var extraVarsAnswer))
                {
                    if (!extraVarsAnswer.IsEmpty)
                    {
                        sendData[key] = extraVarsAnswer.Input;
                        PrintPromptResult(label, extraVarsAnswer.Input);
                    }
                    else
                    {
                        PrintPromptResult(label, requirements.Defaults.ExtraVars, true);
                    }
                }
                else { return false; }
            }

            // VariablesNeededToStart and Survey
            if (requirements.VariablesNeededToStart.Length > 0 || (checkOptional && requirements.SurveyEnabled))
            {
                if (!AskSurvey(ResourceType.WorkflowJobTemplate, Id, checkOptional, sendData))
                {
                    return false;
                }
            }

            return true;
        }

        protected WorkflowJob.LaunchResult? Launch(ulong id)
        {
            var requirements = GetResource<WorkflowJobTemplateLaunchRequirements>($"{Resources.WorkflowJobTemplate.PATH}{id}/launch/");
            ShowJobTemplateInfo(requirements);
            if (requirements.NodeTemplatesMissing.Length > 0)
            {
                var missingNodes = string.Join(", ", requirements.NodeTemplatesMissing);
                WriteError(new ErrorRecord(new InvalidOperationException($"Missing Templates in Nodes: [{missingNodes}]"),
                                           "MissingNodeTemplates",
                                           ErrorCategory.ResourceUnavailable,
                                           requirements));
                return null;
            }
            var sendData = CreateSendData();
            if (!TryAskOnLaunch(requirements, sendData, checkOptional: Interactive))
            {
                WriteWarning("Launch canceled.");
                return null;
            }
            var apiResult = CreateResource<WorkflowJob.LaunchResult>($"{Resources.WorkflowJobTemplate.PATH}{id}/launch/", sendData);
            var launchResult = apiResult.Contents;
            if (launchResult is null) return null;
            WriteVerbose($"Launch WorkflowJobTemplate:{id} => Job:[{launchResult.Id}]");
            if (launchResult.IgnoredFields.Count > 0)
            {
                foreach (var (key, val) in launchResult.IgnoredFields)
                {
                    WriteWarning($"Ignored field: {key} ({JsonSerializer.Serialize(val, Json.DeserializeOptions)})");
                }
            }
            return launchResult;
        }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "WorkflowJobTemplate")]
    [OutputType(typeof(WorkflowJob))]
    public class InvokeWorkflowJobTemplateCommand : LaunchWorkflowJobTemplateCommandBase
    {
        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (WorkflowJobTemplate is not null)
            {
                Id = WorkflowJobTemplate.Id;
            }
            var launchResult = Launch(Id);
            if (launchResult is not null)
            {
                JobProgressManager.Add(launchResult);
            }
        }
        protected override void EndProcessing()
        {
            WaitJobs("Launch WorkflowJobTemplate", IntervalSeconds, SuppressJobLog);
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "WorkflowJobTemplate")]
    [OutputType(typeof(WorkflowJob.LaunchResult))]
    public class StartWorkflowJobTemplateCommand : LaunchWorkflowJobTemplateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (WorkflowJobTemplate is not null)
            {
                Id = WorkflowJobTemplate.Id;
            }
            var launchResult = Launch(Id);
            if (launchResult is not null)
            {
                WriteObject(launchResult, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "WorkflowJobTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(WorkflowJobTemplate))]
    public class NewWorkflowJobTemplateCommand : NewCommandBase<WorkflowJobTemplate>
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong? Organization { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation]
        public string? ExtraVars { get; set; }

        [Parameter()]
        public string? Tags { get; set; }

        [Parameter()]
        public string? SkipTags { get; set; }

        [Parameter()]
        public SwitchParameter AskScmBranch { get; set; }
        [Parameter()]
        public SwitchParameter AskVariables { get; set; }
        [Parameter()]
        public SwitchParameter AskLimit { get; set; }
        [Parameter()]
        public SwitchParameter AskTags { get; set; }
        [Parameter()]
        public SwitchParameter AskSkipTags { get; set; }
        [Parameter()]
        public SwitchParameter AskInventory { get; set; }
        [Parameter()]
        public SwitchParameter AskLabels { get; set; }

        [Parameter()]
        public SwitchParameter SurveyEnabled { get; set; }

        [Parameter()]
        [ValidateSet("github", "gitlab")]
        public string? WebhookService { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? WebhookCredential { get; set; }

        [Parameter()]
        public SwitchParameter AllowSimultaneous { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var dict = new Dictionary<string, object>()
            {
                { "name", Name },
            };
            if (Description is not null)
                dict.Add("description", Description);
            if (Organization is not null)
                dict.Add("organization", Organization);
            if (Inventory is not null)
                dict.Add("inventory", Inventory);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (ExtraVars is not null)
                dict.Add("extra_vars", ExtraVars);
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (AskScmBranch)
                dict.Add("ask_scm_branch_on_launch", true);
            if (AskVariables)
                dict.Add("ask_variables_on_launch", true);
            if (AskLimit)
                dict.Add("ask_limit_on_launch", true);
            if (AskTags)
                dict.Add("ask_tags_on_launch", true);
            if (AskSkipTags)
                dict.Add("ask_skip_tags_on_launch", true);
            if (AskInventory)
                dict.Add("ask_inventory_on_launch", true);
            if (AskLabels)
                dict.Add("ask_labels_on_launch", true);
            if (SurveyEnabled)
                dict.Add("survey_enabled", true);
            if (WebhookService is not null)
                dict.Add("webhook_service", WebhookService);
            if (WebhookCredential is not null)
                dict.Add("webhook_credential", WebhookCredential);
            if (AllowSimultaneous)
                dict.Add("allow_simultaneous", true);

            return dict;
        }
        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsData.Update, "WorkflowJobTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(WorkflowJobTemplate))]
    public class UpdateWorkflowJobTemplateCommand : UpdateCommandBase<WorkflowJobTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJobTemplate])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong? Organization { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation]
        public string? ExtraVars { get; set; }

        [Parameter()]
        public string? Tags { get; set; }

        [Parameter()]
        public string? SkipTags { get; set; }

        [Parameter()]
        public bool? AskScmBranch { get; set; }
        [Parameter()]
        public bool? AskVariables { get; set; }
        [Parameter()]
        public bool? AskLimit { get; set; }
        [Parameter()]
        public bool? AskTags { get; set; }
        [Parameter()]
        public bool? AskSkipTags { get; set; }
        [Parameter()]
        public bool? AskInventory { get; set; }
        [Parameter()]
        public bool? AskLabels { get; set; }

        [Parameter()]
        public bool? SurveyEnabled { get; set; }

        [Parameter()]
        [ValidateSet("github", "gitlab")]
        public string? WebhookService { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? WebhookCredential { get; set; }

        [Parameter()]
        public bool? AllowSimultaneous { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var dict = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                dict.Add("name", Name);
            if (Description is not null)
                dict.Add("description", Description);
            if (Organization is not null)
                dict.Add("organization", Organization == 0 ? null : Organization);
            if (Inventory is not null)
                dict.Add("inventory", Inventory == 0 ? null : Inventory);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (ExtraVars is not null)
                dict.Add("extra_vars", ExtraVars);
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (AskScmBranch is not null)
                dict.Add("ask_scm_branch_on_launch", AskScmBranch);
            if (AskVariables is not null)
                dict.Add("ask_variables_on_launch", AskVariables);
            if (AskLimit is not null)
                dict.Add("ask_limit_on_launch", AskLimit);
            if (AskTags is not null)
                dict.Add("ask_tags_on_launch", AskTags );
            if (AskSkipTags is not null)
                dict.Add("ask_skip_tags_on_launch", AskSkipTags);
            if (AskInventory is not null)
                dict.Add("ask_inventory_on_launch", AskInventory);
            if (AskLabels is not null)
                dict.Add("ask_labels_on_launch", AskLabels);
            if (SurveyEnabled is not null)
                dict.Add("survey_enabled", SurveyEnabled);
            if (WebhookService is not null)
                dict.Add("webhook_service", WebhookService);
            if (WebhookCredential is not null)
                dict.Add("webhook_credential", WebhookCredential);
            if (AllowSimultaneous is not null)
                dict.Add("allow_simultaneous", AllowSimultaneous);

            return dict;
        }
        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.Remove, "WorkflowJobTemplate", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveWorkflowJobTemplateCommand : RemoveCommandBase<WorkflowJobTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.WorkflowJobTemplate])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
