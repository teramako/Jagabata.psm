using System.Text;

namespace Jagabata.Resources
{
    public class JobTemplateLaunchRequirements(bool canStartWithoutUserInput, string[] passwordsNeededToStart,
                                               bool askScmBranchOnLaunch, bool askVariablesOnLaunch,
                                               bool askTagsOnLaunch, bool askDiffModeOnLaunch, bool askSkipTagsOnLaunch,
                                               bool askJobTypeOnLaunch, bool askLimitOnLaunch, bool askVerbosityOnLaunch,
                                               bool askInventoryOnLaunch, bool askCredentialOnLaunch,
                                               bool askExecutionEnvironmentOnLaunch, bool askLabelsOnLaunch,
                                               bool askForksOnLaunch, bool askJobSliceCountOnLaunch,
                                               bool askTimeoutOnLaunch, bool askInstanceGroupsOnLaunch,
                                               bool surveyEnabled, string[] variablesNeededToStart,
                                               bool credentialNeededToStart, bool inventoryNeededToStart,
                                               TemplateData jobTemplateData, JobTemplateDefaults defaults)
    {
        public bool CanStartWithoutUserInput { get; } = canStartWithoutUserInput;
        public string[] PasswordsNeededToStart { get; } = passwordsNeededToStart;
        public bool AskScmBranchOnLaunch { get; } = askScmBranchOnLaunch;
        public bool AskVariablesOnLaunch { get; } = askVariablesOnLaunch;
        public bool AskTagsOnLaunch { get; } = askTagsOnLaunch;
        public bool AskDiffModeOnLaunch { get; } = askDiffModeOnLaunch;
        public bool AskSkipTagsOnLaunch { get; } = askSkipTagsOnLaunch;
        public bool AskJobTypeOnLaunch { get; } = askJobTypeOnLaunch;
        public bool AskLimitOnLaunch { get; } = askLimitOnLaunch;
        public bool AskVerbosityOnLaunch { get; } = askVerbosityOnLaunch;
        public bool AskInventoryOnLaunch { get; } = askInventoryOnLaunch;
        public bool AskCredentialOnLaunch { get; } = askCredentialOnLaunch;
        public bool AskExecutionEnvironmentOnLaunch { get; } = askExecutionEnvironmentOnLaunch;
        public bool AskLabelsOnLaunch { get; } = askLabelsOnLaunch;
        public bool AskForksOnLaunch { get; } = askForksOnLaunch;
        public bool AskJobSliceCountOnLaunch { get; } = askJobSliceCountOnLaunch;
        public bool AskTimeoutOnLaunch { get; } = askTimeoutOnLaunch;
        public bool AskInstanceGroupsOnLaunch { get; } = askInstanceGroupsOnLaunch;
        public bool SurveyEnabled { get; } = surveyEnabled;
        public string[] VariablesNeededToStart { get; } = variablesNeededToStart;
        public bool CredentialNeededToStart { get; } = credentialNeededToStart;
        public bool InventoryNeededToStart { get; } = inventoryNeededToStart;
        public TemplateData JobTemplateData { get; } = jobTemplateData;
        public JobTemplateDefaults Defaults { get; } = defaults;
    }

    public record JobTemplateDefaults(NamedData Inventory,
                                      string Limit,
                                      string ScmBranch,
                                      NamedData[]? Labels,
                                      string JobTags,
                                      string SkipTags,
                                      string ExtraVars,
                                      bool DiffMode,
                                      JobType JobType,
                                      JobVerbosity Verbosity,
                                      CredentialData[]? Credentials,
                                      NamedData ExecutionEnvironment,
                                      int Forks,
                                      int JobSliceCount,
                                      int Timeout);
    public record TemplateData(ulong Id, string Name, string Description)
    {
        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }
    }
    public record NamedData(ulong? Id, string? Name)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Id is not null)
            {
                sb.Append($"[{Id}]");
            }
            if (!string.IsNullOrEmpty(Name))
            {
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(Name);
            }
            return sb.ToString();
        }
    }
    public record CredentialData(ulong Id, string? Name, ulong? CredentialType, string[]? PasswordsNeeded)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"[{Id}]");
            if (!string.IsNullOrEmpty(Name))
            {
                sb.Append(' ');
                sb.Append(Name);
            }
            return sb.ToString();
        }
    }
}
