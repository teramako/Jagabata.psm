namespace Jagabata.Resources
{
    public class WorkflowJobTemplateLaunchRequirements(bool canStartWithoutUserInput, bool askInventoryOnLaunch,
                                                       bool askLimitOnLaunch, bool askScmBranchOnLaunch,
                                                       bool askVariablesOnLaunch, bool askLabelsOnLaunch,
                                                       bool askTagsOnLaunch, bool askSkipTagsOnLaunch,
                                                       bool surveyEnabled, string[] variablesNeededToStart,
                                                       ulong[] nodeTemplatesMissing, ulong[] nodePromptsRejected,
                                                       TemplateData workflowJobTemplateData,
                                                       WorkflowJobTemplateDefaults defaults)
    {
        public bool CanStartWithoutUserInput { get; } = canStartWithoutUserInput;
        public bool AskInventoryOnLaunch { get; } = askInventoryOnLaunch;
        public bool AskLimitOnLaunch { get; } = askLimitOnLaunch;
        public bool AskScmBranchOnLaunch { get; } = askScmBranchOnLaunch;
        public bool AskVariablesOnLaunch { get; } = askVariablesOnLaunch;
        public bool AskLabelsOnLaunch { get; } = askLabelsOnLaunch;
        public bool AskTagsOnLaunch { get; } = askTagsOnLaunch;
        public bool AskSkipTagsOnLaunch { get; } = askSkipTagsOnLaunch;
        public bool SurveyEnabled { get; } = surveyEnabled;
        public string[] VariablesNeededToStart { get; } = variablesNeededToStart;

        public ulong[] NodeTemplatesMissing { get; } = nodeTemplatesMissing;
        public ulong[] NodePromptsRejected { get; } = nodePromptsRejected;

        public TemplateData WorkflowJobTemplateData { get; } = workflowJobTemplateData;
        public WorkflowJobTemplateDefaults Defaults { get; } = defaults;
    }

    public record WorkflowJobTemplateDefaults(NamedData Inventory,
                                              string Limit,
                                              string ScmBranch,
                                              NamedData[]? Labels,
                                              string JobTags,
                                              string SkipTags,
                                              string ExtraVars);
}
