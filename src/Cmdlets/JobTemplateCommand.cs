using AWX.Resources;
using System.Management.Automation;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Web;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "JobTemplate")]
    [OutputType(typeof(JobTemplate))]
    public class GetJobTemplate : GetCommandBase<JobTemplate>
    {
        protected override ResourceType AcceptType => ResourceType.JobTemplate;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "JobTemplate", DefaultParameterSetName = "All")]
    [OutputType(typeof(JobTemplate))]
    public class FindJobTemplateCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "TypeAndId", Position = 0)]
        [ValidateSet(nameof(ResourceType.Organization),
                     nameof(ResourceType.Inventory))]
        public ResourceType Type { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "TypeAndId", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Resource", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Inventory
        ])]
        public IResource? Resource { get; set; }

        [Parameter(Position = 2)]
        public string[]? Name { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["!id"];

        protected override void BeginProcessing()
        {
            if (Name is not null)
            {
                Query.Add("name__in", string.Join(',', Name));
            }
            SetupCommonQuery();
        }
        protected override void EndProcessing()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Id}/job_templates/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/job_templates/",
                _ => JobTemplate.PATH
            };
            Find<JobTemplate>(path);
        }
    }

    public abstract class LaunchJobTemplateCommandBase : LaunchJobCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, Position = 0)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "JobTemplate", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [ResourceType.JobTemplate])]
        public IResource? JobTemplate { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [ValidateSet(nameof(Resources.JobType.Run), nameof(Resources.JobType.Check))]
        public JobType? JobType { get; set; }

        [Parameter()]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong[]? Credentials { get; set; }

        [Parameter()]
        public string? Limit { get; set; }

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
        public bool? DiffMode { get; set; }

        [Parameter()]
        public JobVerbosity? Verbosity { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Forks { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? JobSliceCount { get; set; }

        [Parameter()]
        public int? Timeout { get; set; }

        [Parameter()]
        public SwitchParameter Interactive { get; set; }

        private IDictionary<string, object?> CreateSendData()
        {
            var dict = new Dictionary<string, object?>();
            if (Inventory is not null)
            {
                dict.Add("inventory", Inventory);
            }
            if (JobType is not null)
            {
                dict.Add("job_type", $"{JobType}".ToLowerInvariant());
            }
            if (ScmBranch is not null)
            {
                dict.Add("scm_branch", ScmBranch);
            }
            if (Credentials is not null)
            {
                dict.Add("credentials", Credentials);
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
            if (DiffMode is not null)
            {
                dict.Add("diff_mode", DiffMode);
            }
            if (Verbosity is not null)
            {
                dict.Add("verbosity", (int)Verbosity);
            }
            if (Forks is not null)
            {
                dict.Add("forks", Forks);
            }
            if (ExecutionEnvironment is not null)
            {
                dict.Add("execution_environment", ExecutionEnvironment);
            }
            if (JobSliceCount is not null)
            {
                dict.Add("job_slice_count", JobSliceCount);
            }
            if (Timeout is not null)
            {
                dict.Add("timeout", Timeout);
            }
            return dict;
        }
        private void ShowJobTemplateInfo(JobTemplateLaunchRequirements requirements)
        {
            var jt = requirements.JobTemplateData;
            var def = requirements.Defaults;
            var (fixedColor, implicitColor, explicitColor, requiredColor) =
                ((ConsoleColor?)null, ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.Red);
            WriteHost($"[{jt.Id}] {jt.Name} - {jt.Description}\n");
            var fmt = "{0,22} : {1}\n";
            {
                var inventoryVal = (def.Inventory.Id is not null ? $"[{def.Inventory.Id}] {def.Inventory.Name}" : "Undefined")
                                   + (requirements.AskInventoryOnLaunch && Inventory is not null ? $" => [{Inventory}]" : "");
                WriteHost(string.Format(fmt, "Inventory", inventoryVal),
                            foregroundColor: requirements.AskInventoryOnLaunch ? (Inventory is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (!string.IsNullOrEmpty(def.Limit) || Limit is not null)
            {
                var limitVal = def.Limit
                               + (requirements.AskLimitOnLaunch && Limit is not null ? $" => {Limit}" : "");
                WriteHost(string.Format(fmt, "Limit", $"{limitVal}"),
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
            {
                var diffModeVal = $"{def.DiffMode}"
                                  + (requirements.AskDiffModeOnLaunch && DiffMode is not null ? $" => {DiffMode}" : "");
                WriteHost(string.Format(fmt, "Diff Mode", diffModeVal),
                                foregroundColor: requirements.AskDiffModeOnLaunch ? (DiffMode is null ? implicitColor : explicitColor) : fixedColor);
            }
            {
                var jobTypeVal = def.JobType
                                 + (requirements.AskJobTypeOnLaunch && JobType is not null ? $" => {JobType}" : "");
                WriteHost(string.Format(fmt, "Job Type", jobTypeVal),
                                foregroundColor: requirements.AskJobTypeOnLaunch ? (JobType is null ? implicitColor : explicitColor) : fixedColor);
            }
            {
                var verbosityVal = $"{def.Verbosity:d} ({def.Verbosity})"
                                   + (requirements.AskVerbosityOnLaunch && Verbosity is not null ? $" => {Verbosity:d} ({Verbosity})" : "");
                WriteHost(string.Format(fmt, "Verbosity", verbosityVal),
                                foregroundColor: requirements.AskVerbosityOnLaunch ? (Verbosity is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (def.Credentials is not null || Credentials is not null)
            {
                var credentialsVal = string.Join(", ", def.Credentials?.Select(c => $"[{c.Id}] {c.Name}") ?? [])
                        + (requirements.AskCredentialOnLaunch && Credentials is not null ? $" => {string.Join(',', Credentials.Select(id => $"[{id}]"))}" : "");
                WriteHost(string.Format(fmt, "Credentials", credentialsVal),
                            foregroundColor: requirements.AskCredentialOnLaunch ? (Credentials is null ? implicitColor : explicitColor) : fixedColor);
            }
            if (requirements.PasswordsNeededToStart.Length > 0)
            {
                WriteHost(string.Format(fmt, "CredentialPassswords", $"[{string.Join(", ", requirements.PasswordsNeededToStart)}]"),
                            foregroundColor: requiredColor);
            }
            if (def.ExecutionEnvironment.Id is not null || ExecutionEnvironment is not null)
            {
                var eeVal = $"[{def.ExecutionEnvironment.Id}] {def.ExecutionEnvironment.Name}"
                            + (requirements.AskExecutionEnvironmentOnLaunch && ExecutionEnvironment is not null ? $" => [{ExecutionEnvironment}]" : "");
                WriteHost(string.Format(fmt, "ExecutionEnvironment", eeVal),
                            foregroundColor: requirements.AskExecutionEnvironmentOnLaunch ? (ExecutionEnvironment is null ? implicitColor : explicitColor) : fixedColor);
            }
            {
                var forksVal = $"{def.Forks}" + (requirements.AskForksOnLaunch && Forks is not null ? $" => {Forks}" : "");
                WriteHost(string.Format(fmt, "Forks", forksVal),
                                foregroundColor: requirements.AskForksOnLaunch ? (Forks is null ? implicitColor : explicitColor) : fixedColor);
            }
            {
                var jobSliceVal = $"{def.JobSliceCount}"
                                  + (requirements.AskJobSliceCountOnLaunch && JobSliceCount is not null ? $" => {JobSliceCount}" : "");
                WriteHost(string.Format(fmt, "Job Slice Count", jobSliceVal),
                                foregroundColor: requirements.AskJobSliceCountOnLaunch ? (JobSliceCount is null ? implicitColor : explicitColor) : fixedColor);
            }
            {
                var timeoutVal = $"{def.Timeout}"
                                 + (requirements.AskTimeoutOnLaunch && Timeout is not null ? $" => {Timeout}" : "");
                WriteHost(string.Format(fmt, "Timeout", timeoutVal),
                                foregroundColor: requirements.AskTimeoutOnLaunch ? (Timeout is null ? implicitColor : explicitColor) : fixedColor);
            }
        }

        private bool CheckPasswordsRequired(JobTemplateLaunchRequirements requirements,
                                            IDictionary<string, object?> sendData,
                                            out Dictionary<string, (string caption, string description)> result)
        {
            result = new();
            ulong[] credentialIds;
            if (sendData.TryGetValue("credentials", out var res))
            {
                credentialIds = res as ulong[] ?? [];
                if (credentialIds.Length == 0)
                    return false;

                var query = HttpUtility.ParseQueryString("");
                query.Set("id__in", string.Join(',', credentialIds));
                query.Set("page_size", $"{credentialIds.Length}");
                foreach (var resultSet in GetResultSet<Credential>(Credential.PATH, query, true))
                {
                    foreach (var cred in resultSet.Results)
                    {
                        string captionFmt;
                        string description = cred.Description;
                        foreach (var kv in cred.Inputs)
                        {
                            if (result.ContainsKey(kv.Key)) continue;
                            string key = kv.Key;
                            switch (kv.Key)
                            {
                                case "password":
                                    captionFmt = "Password ({0})";
                                    break;
                                case "become_password":
                                    captionFmt = "Become Password ({0})";
                                    break;
                                case "ssh_key_unlock":
                                    captionFmt = "SSH Passphrase ({0})";
                                    break;
                                case "vault_password":
                                    string vaultId = cred.Inputs["vault_id"] as string ?? "";
                                    if (string.IsNullOrEmpty(vaultId))
                                    {
                                        captionFmt = "Vault Password ({0})";
                                    }
                                    else
                                    {
                                        captionFmt = $"Vault Password ({{0}} | {vaultId})";
                                        key = $"{kv.Key}.{vaultId}";
                                    }
                                    break;
                                default:
                                    continue;
                            }
                            if (kv.Value as string != "ASK")
                                continue;

                            result.Add(key, (string.Format(captionFmt, $"[{cred.Id}]{cred.Name}"), description));
                        }
                    }
                }
            }
            else
            {
                credentialIds = requirements.Defaults.Credentials?.Select(x => x.Id).ToArray() ?? [];
                if (requirements.PasswordsNeededToStart.Length == 0)
                    return false;

                foreach (var key in requirements.PasswordsNeededToStart)
                {
                    string captionFmt;
                    string description = "";
                    switch (key)
                    {
                        case "password":
                            captionFmt = "Password ({0})";
                            break;
                        case "become_password":
                            captionFmt = "Become Password ({0})";
                            break;
                        case "ssh_key_unlock":
                            captionFmt = "SSH Passphrase ({0})";
                            break;
                        default:
                            if (!key.StartsWith("vault_password"))
                                return false;
                            string[] vaultKeys = key.Split('.', 2);
                            captionFmt = (vaultKeys.Length == 2 && !string.IsNullOrEmpty(vaultKeys[1]))
                                         ? $"Vault Password ({{0}} | {vaultKeys[1]})"
                                         : "Vault Password ({0})";
                            break;
                    }
                    var t = requirements.Defaults.Credentials?
                        .Where(cred => cred.PasswordsNeeded?.Any(passwordKey => passwordKey == key) ?? false)
                        .Select(cred => (string.Format(captionFmt, $"[{cred.Id}]{cred.Name}"), description))
                        .FirstOrDefault() ?? ("", "");

                    result.Add(key, t);
                }
            }

            return result.Count > 0;
        }
        private bool TryAskCredentials(IDictionary<string, (string caption, string description)> checkResult,
                                         IDictionary<string, object?> sendData)
        {
            if (CommandRuntime.Host is null)
                return false;

            var prompt = new AskPrompt(CommandRuntime.Host);
            var credentialPassswords = new Dictionary<string, SecureString>();
            sendData.Add("credential_passwords", credentialPassswords);

            foreach (var (key, (caption, description)) in checkResult)
            {
                if (prompt.AskPassword(caption, key, description, out var passwordAnswer))
                {
                    credentialPassswords.Add(key, passwordAnswer.Input);
                    SecureStrings.Add(passwordAnswer.Input);
                    PrintPromptResult(key, string.Empty);
                }
                else
                {   // Canceled
                    return false;
                }
            }
            return true;
        }
        // FIXME
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
        protected bool TryAskOnLaunch(JobTemplateLaunchRequirements requirements,
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
            if (requirements.InventoryNeededToStart || (checkOptional && requirements.AskInventoryOnLaunch))
            {
                key = "inventory"; label = "Inventory";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<ulong>(label, "",
                                           defaultValue: requirements.Defaults.Inventory.Id,
                                           helpMessage: "Input an Inventory ID."
                                                        + (requirements.InventoryNeededToStart ? " (Required)" : ""),
                                           required: requirements.InventoryNeededToStart,
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

            // Credentials
            if (requirements.CredentialNeededToStart || (checkOptional && requirements.AskCredentialOnLaunch))
            {
                key = "credentials"; label = "Credentials";
                if (sendData.ContainsKey(key))
                {
                    var strData = $"[{string.Join(", ", (ulong[]?)sendData[key] ?? [])}]";
                    WriteHost(string.Format(skipFormat, label, strData), dontshow: true);
                }
                else if (prompt.AskList<ulong>(label, "",
                                               defaultValues: requirements.Defaults.Credentials?.Select(x => $"[{x.Id}] {x.Name}"),
                                               helpMessage: "Enter Credential ID(s).",
                                               out var credentialsAnswer))
                {
                    if (!credentialsAnswer.IsEmpty)
                    {
                        var arr = credentialsAnswer.Input.Where(x => x > 0).ToArray();
                        sendData[key] = arr;
                        PrintPromptResult(label, $"[{string.Join(", ", arr)}]");
                    }
                    else
                    {
                        PrintPromptResult(label,
                                    $"[{string.Join(", ", requirements.Defaults.Credentials?.Select(x => $"{x}") ?? [])}]",
                                    true);
                    }
                }
                else { return false; }
            }

            // CredentialPassword
            if (CheckPasswordsRequired(requirements, sendData, out var checkResult))
            {
                if (!TryAskCredentials(checkResult, sendData))
                {
                    return false;
                }
            }

            // ExecutionEnvironment
            if (checkOptional && requirements.AskExecutionEnvironmentOnLaunch)
            {
                key = "execution_environment"; label = "Execution Environment";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<ulong>(label, "",
                                           defaultValue: requirements.Defaults.ExecutionEnvironment.Id,
                                           helpMessage: "Enter the Execution Environment ID.",
                                           required: false,
                                           out var eeAnswer))
                {
                    if (!eeAnswer.IsEmpty)
                    {
                        sendData[key] = eeAnswer.Input > 0 ? eeAnswer.Input : null;
                        PrintPromptResult(label, eeAnswer.Input > 0 ? $"{eeAnswer.Input}" : "(null)");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.ExecutionEnvironment}", true);
                    }
                }
                else { return false; }
            }

            // JobType
            if (checkOptional && requirements.AskJobTypeOnLaunch)
            {
                key = "job_type"; label = "Job Type";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.AskBool(label,
                                        defaultValue: requirements.Defaults.JobType == Resources.JobType.Run,
                                        trueParameter: ("Run", "Run: Execut the playbook when launched, running Ansible tasks on the selected hosts."),
                                        falseParameter: ("Check", "Check: Perform a \"dry run\" of the playbook. This is same as `-C` -or `--check` command-line parameter for `ansible-playbook`"),
                                        out var jobTypeAnswer))
                {
                    if (!jobTypeAnswer.IsEmpty)
                    {
                        var result = (jobTypeAnswer.Input ? Resources.JobType.Run : Resources.JobType.Check).ToString().ToLowerInvariant();
                        sendData[key] = result;
                        PrintPromptResult(label, result);
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.JobType}", true);
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

            // Forks
            if (checkOptional && requirements.AskForksOnLaunch)
            {
                key = "forks"; label = "Forks";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<int>(label, "",
                                         defaultValue: requirements.Defaults.Forks,
                                         helpMessage: "Enter the number of parallel or simultaneous procecces.",
                                         required: false,
                                         out var forksAnswer))
                {
                    if (!forksAnswer.IsEmpty)
                    {
                        sendData[key] = forksAnswer.Input;
                        PrintPromptResult(label, $"{forksAnswer.Input}");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.Forks}", true);
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

            // Verbosity
            if (checkOptional && requirements.AskVerbosityOnLaunch)
            {
                key = "verbosity"; label = "Verbosity";
                if (sendData.ContainsKey(key))
                {
                    var v = (JobVerbosity)((int)(sendData[key] ?? 0));
                    WriteHost(string.Format(skipFormat, label, $"{v:d} ({v:g})"), dontshow: true);
                }
                else if (prompt.AskEnum<JobVerbosity>(label,
                                                      defaultValue: requirements.Defaults.Verbosity,
                                                      helpMessage: "Choose the job log verbosity level.",
                                                      out var verbosityAnswer))
                {
                    if (!verbosityAnswer.IsEmpty)
                    {
                        sendData[key] = (int)verbosityAnswer.Input;
                        PrintPromptResult(label, $"{verbosityAnswer.Input} ({verbosityAnswer.Input:d})");
                    }
                    else
                    {
                        PrintPromptResult(label,
                                    $"{requirements.Defaults.Verbosity} ({requirements.Defaults.Verbosity:d})",
                                    true);
                    }
                }
                else { return false; }
            }

            // JobSliceCount
            if (checkOptional && requirements.AskJobTypeOnLaunch)
            {
                key = "job_slice_count"; label = "Job Slice Count";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<int>(label, "",
                                         defaultValue: requirements.Defaults.JobSliceCount,
                                         helpMessage: "Enter the number of slices you want this job template to run.",
                                         required: false,
                                         out var jobSliceCountAnswer))
                {
                    if (!jobSliceCountAnswer.IsEmpty)
                    {
                        sendData[key] = jobSliceCountAnswer.Input;
                        PrintPromptResult(label, $"{jobSliceCountAnswer.Input}");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.JobSliceCount}", true);
                    }
                }
                else { return false; }
            }

            // Timeout
            if (checkOptional && requirements.AskTimeoutOnLaunch)
            {
                key = "timeout"; label = "Timeout";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.Ask<int>(label, "",
                                         defaultValue: requirements.Defaults.Timeout,
                                         helpMessage: "Enter the timeout value(seconds).",
                                         required: false,
                                         out var timeoutAnswer))
                {
                    if (!timeoutAnswer.IsEmpty)
                    {
                        sendData[key] = timeoutAnswer.Input;
                        PrintPromptResult(label, $"{timeoutAnswer.Input}");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.Timeout}", true);
                    }
                }
                else { return false; }
            }

            // DiffMode
            if (checkOptional && requirements.AskDiffModeOnLaunch)
            {
                key = "diff_mode"; label = "Diff Mode";
                if (sendData.ContainsKey(key))
                {
                    WriteHost(string.Format(skipFormat, label, sendData[key]), dontshow: true);
                }
                else if (prompt.AskBool(label,
                                        defaultValue: requirements.Defaults.DiffMode,
                                        trueParameter: ("On", "On: Allows to see the changes made by Ansible tasks. This is same as `-D` or `--diff` command-line parameter for `ansible-playbook`."),
                                        falseParameter: ("Off", "Off"),
                                        out var diffModeAnswer))
                {
                    if (!diffModeAnswer.IsEmpty)
                    {
                        sendData[key] = diffModeAnswer.Input;
                        PrintPromptResult(label, $"{diffModeAnswer.Input}");
                    }
                    else
                    {
                        PrintPromptResult(label, $"{requirements.Defaults.DiffMode}", true);
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
                if (!AskSurvey(ResourceType.JobTemplate, Id, checkOptional, sendData))
                {
                    return false;
                }
            }

            return true;
        }
        protected JobTemplateJob.LaunchResult? Launch(ulong id)
        {
            var requirements = GetResource<JobTemplateLaunchRequirements>($"{Resources.JobTemplate.PATH}{id}/launch/");
            ShowJobTemplateInfo(requirements);
            var sendData = CreateSendData();
            if (!TryAskOnLaunch(requirements, sendData, checkOptional: Interactive))
            {
                ClearSecureStrings();
                WriteWarning("Launch canceled.");
                return null;
            }
            JobTemplateJob.LaunchResult? launchResult;
            try
            {
                var apiResult = CreateResource<JobTemplateJob.LaunchResult>($"{Resources.JobTemplate.PATH}{id}/launch/", sendData);
                launchResult = apiResult.Contents;
            }
            finally
            {
                ClearSecureStrings();
            }
            if (launchResult is null) return null;
            WriteVerbose($"Launch JobTemplate:{id} => Job:[{launchResult.Id}]");
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

    [Cmdlet(VerbsLifecycle.Invoke, "JobTemplate")]
    [OutputType(typeof(JobTemplateJob))]
    public class InvokeJobTemplateCommand : LaunchJobTemplateCommandBase
    {
        [Parameter()]
        [ValidateRange(5, int.MaxValue)]
        public int IntervalSeconds { get; set; } = 5;

        [Parameter()]
        public SwitchParameter SuppressJobLog { get; set; }

        protected override void ProcessRecord()
        {
            if (JobTemplate is not null)
            {
                Id = JobTemplate.Id;
            }
            var launchResult = Launch(Id);
            if (launchResult is not null)
            {
                JobProgressManager.Add(launchResult);
            }
        }
        protected override void EndProcessing()
        {
            WaitJobs("Launch JobTemplate", IntervalSeconds, SuppressJobLog);
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "JobTemplate")]
    [OutputType(typeof(JobTemplateJob.LaunchResult))]
    public class StartJobTemplateCommand : LaunchJobTemplateCommandBase
    {
        protected override void ProcessRecord()
        {
            if (JobTemplate is not null)
            {
                Id = JobTemplate.Id;
            }
            var launchResult = Launch(Id);
            if (launchResult is not null)
            {
                WriteObject(launchResult, false);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "JobTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(JobTemplate))]
    public class NewJobTemplateCommand : NewCommandBase<JobTemplate>
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ValidateSet(nameof(Resources.JobType.Run), nameof(Resources.JobType.Check))]
        public JobType? JobType { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Project])]
        public ulong? Project { get; set; }

        [Parameter(Mandatory = true)]
        public string Playbook { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Forks { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        public JobVerbosity? Verbosity { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ExtraVarsArgumentTransformation] // Translate IDictionary to JSON string
        public string? ExtraVars { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Tags { get; set; }

        [Parameter()]
        public SwitchParameter ForceHandlers { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SkipTags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? StartAtTask { get; set; }

        [Parameter()]
        public int? Timeout { get; set; }

        [Parameter()]
        public SwitchParameter UseFactCache { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? HostConfigKey { get; set; }

        [Parameter()]
        public SwitchParameter AskScmBranch { get; set; }
        [Parameter()]
        public SwitchParameter AskDiffMode { get; set; }
        [Parameter()]
        public SwitchParameter AskVariables { get; set; }
        [Parameter()]
        public SwitchParameter AskLimit { get; set; }
        [Parameter()]
        public SwitchParameter AskTags { get; set; }
        [Parameter()]
        public SwitchParameter AskSkipTags { get; set; }
        [Parameter()]
        public SwitchParameter AskJobType { get; set; }
        [Parameter()]
        public SwitchParameter AskVerbosity { get; set; }
        [Parameter()]
        public SwitchParameter AskInventory { get; set; }
        [Parameter()]
        public SwitchParameter AskCredential { get; set; }
        [Parameter()]
        public SwitchParameter AskExecutionEnvironment { get; set; }
        [Parameter()]
        public SwitchParameter AskLabels { get; set; }
        [Parameter()]
        public SwitchParameter AskForks { get; set; }
        [Parameter()]
        public SwitchParameter AskJobSliceCount { get; set; }

        [Parameter()]
        public SwitchParameter SurveyEnabled { get; set; }

        [Parameter()]
        public SwitchParameter BecomeEnabled { get; set; }

        [Parameter()]
        public SwitchParameter DiffMode { get; set; }

        [Parameter()]
        public SwitchParameter AllowSimultaneous { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? JobSliceCount { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ValidateSet("github", "gitlab", "")]
        public string? WebhookService { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? WebhookCredential { get; set; }

        [Parameter()]
        public SwitchParameter PreventInstanceGroupFallback { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var dict = new Dictionary<string, object>()
            {
                { "name", Name },
                { "playbook", Playbook },
            };
            if (Description is not null)
                dict.Add("description", Description);
            if (JobType is not null)
                dict.Add("job_type", $"{JobType}".ToLowerInvariant());
            if (Inventory is not null)
                dict.Add("inventory", Inventory);
            if (Project is not null)
                dict.Add("project", Project);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (Forks is not null)
                dict.Add("forks", Forks);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (Verbosity is not null)
                dict.Add("verbosity", (int)Verbosity);
            if (ExtraVars is not null)
                dict.Add("extra_vars", ExtraVars);
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (ForceHandlers)
                dict.Add("force_handlers", true);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (StartAtTask is not null)
                dict.Add("start_at_task", StartAtTask);
            if (Timeout is not null)
                dict.Add("timeout", Timeout);
            if (UseFactCache)
                dict.Add("use_fact_cache", true);
            if (ExecutionEnvironment is not null)
                dict.Add("execution_environment", ExecutionEnvironment);
            if (HostConfigKey is not null)
                dict.Add("host_config_key", HostConfigKey);
            if (AskScmBranch)
                dict.Add("ask_scm_branch_on_launch", true);
            if (AskDiffMode)
                dict.Add("ask_diff_mode_on_launch", true);
            if (AskVariables)
                dict.Add("ask_variables_on_launch", true);
            if (AskLimit)
                dict.Add("ask_limit_on_launch", true);
            if (AskTags)
                dict.Add("ask_tags_on_launch", true);
            if (AskSkipTags)
                dict.Add("ask_skip_tags_on_launch", true);
            if (AskJobType)
                dict.Add("ask_job_type_on_launch", true);
            if (AskVerbosity)
                dict.Add("ask_verbosity_on_launch", true);
            if (AskInventory)
                dict.Add("ask_inventory_on_launch", true);
            if (AskCredential)
                dict.Add("ask_credential_on_launch", true);
            if (AskExecutionEnvironment)
                dict.Add("ask_execution_environment_on_launch", true);
            if (AskLabels)
                dict.Add("ask_labels_on_launch", true);
            if (AskForks)
                dict.Add("ask_forks_on_launch", true);
            if (AskJobSliceCount)
                dict.Add("ask_job_slice_count_on_launch", true);
            if (SurveyEnabled)
                dict.Add("survey_enabled", true);
            if (BecomeEnabled)
                dict.Add("become_enabled", true);
            if (DiffMode)
                dict.Add("diff_mode", true);
            if (AllowSimultaneous)
                dict.Add("allow_simultaneous", true);
            if (JobSliceCount is not null)
                dict.Add("job_slice_count", JobSliceCount);
            if (WebhookService is not null)
                dict.Add("webhook_service", WebhookService);
            if (WebhookCredential is not null)
                dict.Add("webhook_credential", WebhookCredential);
            if (PreventInstanceGroupFallback)
                dict.Add("prevent_instance_group_fallback", true);

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

    [Cmdlet(VerbsData.Update, "JobTemplate", SupportsShouldProcess = true)]
    [OutputType(typeof(JobTemplate))]
    public class UpdateJobTemplateCommand : UpdateCommandBase<JobTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.JobTemplate])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ValidateSet(nameof(Resources.JobType.Run), nameof(Resources.JobType.Check))]
        public JobType? JobType { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong? Inventory { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Project])]
        public ulong? Project { get; set; }

        [Parameter()]
        public string? Playbook { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Forks { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Limit { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public JobVerbosity? Verbosity { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation] // Translate IDictionary to JSON string
        [AllowEmptyString]
        public string? ExtraVars { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Tags { get; set; }

        [Parameter()]
        public bool? ForceHandlers { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SkipTags { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? StartAtTask { get; set; }

        [Parameter()]
        public int? Timeout { get; set; }

        [Parameter()]
        public bool? UseFactCache { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? HostConfigKey { get; set; }

        [Parameter()]
        public bool? AskScmBranch { get; set; }
        [Parameter()]
        public bool? AskDiffMode { get; set; }
        [Parameter()]
        public bool? AskVariables { get; set; }
        [Parameter()]
        public bool? AskLimit { get; set; }
        [Parameter()]
        public bool? AskTags { get; set; }
        [Parameter()]
        public bool? AskSkipTags { get; set; }
        [Parameter()]
        public bool? AskJobType { get; set; }
        [Parameter()]
        public bool? AskVerbosity { get; set; }
        [Parameter()]
        public bool? AskInventory { get; set; }
        [Parameter()]
        public bool? AskCredential { get; set; }
        [Parameter()]
        public bool? AskExecutionEnvironment { get; set; }
        [Parameter()]
        public bool? AskLabels { get; set; }
        [Parameter()]
        public bool? AskForks { get; set; }
        [Parameter()]
        public bool? AskJobSliceCount { get; set; }

        [Parameter()]
        public bool? SurveyEnabled { get; set; }

        [Parameter()]
        public bool? BecomeEnabled { get; set; }

        [Parameter()]
        public bool? DiffMode { get; set; }

        [Parameter()]
        public bool? AllowSimultaneous { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? JobSliceCount { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ValidateSet("github", "gitlab", "")]
        public string? WebhookService { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? WebhookCredential { get; set; }

        [Parameter()]
        public bool? PreventInstanceGroupFallback { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var dict = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                dict.Add("name", Name);
            if (Description is not null)
                dict.Add("description", Description);
            if (Playbook is not null)
                dict.Add("playbook", Playbook);
            if (JobType is not null)
                dict.Add("job_type", $"{JobType}".ToLowerInvariant());
            if (Inventory is not null)
                dict.Add("inventory", Inventory == 0 ? null : Inventory);
            if (Project is not null)
                dict.Add("project", Project == 0 ? null : Project);
            if (ScmBranch is not null)
                dict.Add("scm_branch", ScmBranch);
            if (Forks is not null)
                dict.Add("forks", Forks);
            if (Limit is not null)
                dict.Add("limit", Limit);
            if (Verbosity is not null)
                dict.Add("verbosity", (int)Verbosity);
            if (ExtraVars is not null)
                dict.Add("extra_vars", ExtraVars);
            if (Tags is not null)
                dict.Add("job_tags", Tags);
            if (ForceHandlers is not null)
                dict.Add("force_handlers", ForceHandlers);
            if (SkipTags is not null)
                dict.Add("skip_tags", SkipTags);
            if (StartAtTask is not null)
                dict.Add("start_at_task", StartAtTask);
            if (Timeout is not null)
                dict.Add("timeout", Timeout);
            if (UseFactCache is not null)
                dict.Add("use_fact_cache", UseFactCache);
            if (ExecutionEnvironment is not null)
                dict.Add("execution_environment", ExecutionEnvironment == 0 ? null : ExecutionEnvironment);
            if (HostConfigKey is not null)
                dict.Add("host_config_key", HostConfigKey);
            if (AskScmBranch is not null)
                dict.Add("ask_scm_branch_on_launch", AskScmBranch);
            if (AskDiffMode is not null)
                dict.Add("ask_diff_mode_on_launch", AskDiffMode);
            if (AskVariables is not null)
                dict.Add("ask_variables_on_launch", AskVariables);
            if (AskLimit is not null)
                dict.Add("ask_limit_on_launch", AskLimit);
            if (AskTags is not null)
                dict.Add("ask_tags_on_launch", AskTags);
            if (AskSkipTags is not null)
                dict.Add("ask_skip_tags_on_launch", AskSkipTags);
            if (AskJobType is not null)
                dict.Add("ask_job_type_on_launch", AskJobType);
            if (AskVerbosity is not null)
                dict.Add("ask_verbosity_on_launch", AskVerbosity);
            if (AskInventory is not null)
                dict.Add("ask_inventory_on_launch", AskInventory);
            if (AskCredential is not null)
                dict.Add("ask_credential_on_launch", AskCredential);
            if (AskExecutionEnvironment is not null)
                dict.Add("ask_execution_environment_on_launch", AskExecutionEnvironment);
            if (AskLabels is not null)
                dict.Add("ask_labels_on_launch", AskLabels);
            if (AskForks is not null)
                dict.Add("ask_forks_on_launch", AskForks);
            if (AskJobSliceCount is not null)
                dict.Add("ask_job_slice_count_on_launch", AskJobSliceCount);
            if (SurveyEnabled is not null)
                dict.Add("survey_enabled", SurveyEnabled);
            if (BecomeEnabled is not null)
                dict.Add("become_enabled", BecomeEnabled);
            if (DiffMode is not null)
                dict.Add("diff_mode", DiffMode);
            if (AllowSimultaneous is not null)
                dict.Add("allow_simultaneous", AllowSimultaneous);
            if (JobSliceCount is not null)
                dict.Add("job_slice_count", JobSliceCount);
            if (WebhookService is not null)
                dict.Add("webhook_service", WebhookService);
            if (WebhookCredential is not null)
                dict.Add("webhook_credential", WebhookCredential == 0 ? null : WebhookCredential);
            if (PreventInstanceGroupFallback is not null)
                dict.Add("prevent_instance_group_fallback", PreventInstanceGroupFallback);

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

    [Cmdlet(VerbsCommon.Remove, "JobTemplate", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveJobTemplateCommand : RemoveCommandBase<JobTemplate>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.JobTemplate])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
