using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Resources;
using System.Management.Automation;

namespace Jagabata.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Project")]
    [OutputType(typeof(Project))]
    public class GetProjectCommand : GetCommandBase<Project>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Project)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Project)]
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

    [Cmdlet(VerbsCommon.Find, "Project")]
    [OutputType(typeof(Project))]
    public class FindProjectCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(ResourceType.Organization, ResourceType.User, ResourceType.Team)]
        [ResourceCompletions(ResourceType.Organization, ResourceType.User, ResourceType.Team)]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "created", "modified", "name", "description", "local_path", "scm_type",
                           "scm_url", "scm_branch", "scm_refspec", "scm_clean", "scm_track_submodules",
                           "scm_delete_on_update", "credential", "timeout", "scm_revision", "last_job_run",
                           "last_job_failed", "next_job_run", "status", "organization", "scm_update_on_launch",
                           "scm_update_cache_timeout", "allow_override", "default_environment",
                           "signature_validation_credential", "last_update_failed", "last_updated")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource?.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/projects/",
                ResourceType.User => $"{User.PATH}{Resource.Id}/projects/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/projects/",
                _ => Project.PATH
            };
            Find<Project>(path);
        }
    }

    [Cmdlet(VerbsCommon.Get, "Playbook")]
    [OutputType(typeof(string))]
    public class GetPlaybookCommand : GetCommandBase<string[]>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Project)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Project)]
        [Alias("project", "p")]
        public override ulong[] Id { get; set; } = [];

        protected override string ApiPath => Project.PATH;

        protected override void ProcessRecord()
        {
            foreach (var playbooks in GetResource("playbooks/"))
            {
                WriteObject(playbooks, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.Get, "InventoryFile")]
    [OutputType(typeof(string))]
    public class GetInventoryFileCommand : GetCommandBase<string[]>
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.Project)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.Project)]
        [Alias("project", "p")]
        public override ulong[] Id { get; set; } = [];

        protected override string ApiPath => Project.PATH;

        protected override void ProcessRecord()
        {
            foreach (var inventoryFiles in GetResource("inventories/"))
            {
                WriteObject(inventoryFiles, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "Project", DefaultParameterSetName = "Manual", SupportsShouldProcess = true)]
    [OutputType(typeof(Project))]
    public class NewProjectCommand : NewCommandBase<Project>
    {
        [Parameter(ParameterSetName = "Manual", Mandatory = true)]
        public SwitchParameter Local { get; set; }

        [Parameter(ParameterSetName = "Git", Mandatory = true)]
        public SwitchParameter Git { get; set; }

        [Parameter(ParameterSetName = "Svn", Mandatory = true)]
        public SwitchParameter Subversion { get; set; }

        [Parameter(ParameterSetName = "Insights", Mandatory = true)]
        public SwitchParameter Insights { get; set; }

        [Parameter(ParameterSetName = "Archive", Mandatory = true)]
        public SwitchParameter RemoteArchive { get; set; }

        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true)]
        [ResourceIdTransformation(ResourceType.Organization)]
        public ulong Organization { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.ExecutionEnvironment)]
        public ulong DefaultEnvironment { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong SignatureValidationCredential { get; set; }

        [Parameter(ParameterSetName = "Manual", Mandatory = true)]
        public string LocalPath { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "Git", Mandatory = true)]
        [Parameter(ParameterSetName = "Svn", Mandatory = true)]
        [Parameter(ParameterSetName = "Archive", Mandatory = true)]
        public string ScmUrl { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [AllowEmptyString]
        public string ScmBranch { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "Git")]
        [AllowEmptyString]
        public string ScmRefspec { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [Parameter(ParameterSetName = "Insights", Mandatory = true)]
        [Parameter(ParameterSetName = "Archive")]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong Credential { get; set; }

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [Parameter(ParameterSetName = "Insights")]
        [Parameter(ParameterSetName = "Archive")]
        public SwitchParameter ScmClean { get; set; }

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [Parameter(ParameterSetName = "Insights")]
        [Parameter(ParameterSetName = "Archive")]
        public SwitchParameter ScmDeleteOnUpdate { get; set; }

        [Parameter(ParameterSetName = "Git")]
        public SwitchParameter ScmTrackSubmodules { get; set; }

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [Parameter(ParameterSetName = "Insights")]
        [Parameter(ParameterSetName = "Archive")]
        public SwitchParameter ScmUpdateOnLaunch { get; set; }

        [Parameter(ParameterSetName = "Git")]
        [Parameter(ParameterSetName = "Svn")]
        [Parameter(ParameterSetName = "Archive")]
        public SwitchParameter AllowOverride { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int Timeout { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "organization", Organization },
            };
            if (DefaultEnvironment > 0)
                sendData.Add("default_environment", DefaultEnvironment);
            if (SignatureValidationCredential > 0)
                sendData.Add("signature_validation_credential", SignatureValidationCredential);

            if (Local)
                sendData.Add("scm_type", "");
            else if (Git)
                sendData.Add("scm_type", "git");
            else if (Subversion)
                sendData.Add("scm_type", "svn");
            else if (Insights)
                sendData.Add("scm_type", "insights");
            else if (RemoteArchive)
                sendData.Add("scm_type", "archive");

            if (!string.IsNullOrEmpty(LocalPath))
                sendData.Add("local_path", LocalPath);
            if (!string.IsNullOrEmpty(ScmUrl))
                sendData.Add("scm_url", ScmUrl);
            if (ScmBranch is not null)
                sendData.Add("scm_branch", ScmBranch);
            if (ScmRefspec is not null)
                sendData.Add("scm_refspec", ScmRefspec);
            if (Credential > 0)
                sendData.Add("credential", Credential);
            if (ScmClean)
                sendData.Add("scm_clean", true);
            if (ScmDeleteOnUpdate)
                sendData.Add("scm_delete_on_update", true);
            if (ScmTrackSubmodules)
                sendData.Add("scm_track_submodules", true);
            if (ScmUpdateOnLaunch)
                sendData.Add("scm_update_on_launch", true);
            if (AllowOverride)
                sendData.Add("allow_override", true);
            if (Timeout > 0)
                sendData.Add("timeout", Timeout);

            return sendData;
        }
        protected override void ProcessRecord()
        {
            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }
        }

    }

    [Cmdlet(VerbsCommon.Remove, "Project", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveProjectCommand : RemoveCommandBase<Project>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Project)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }

    [Cmdlet(VerbsData.Update, "Project", SupportsShouldProcess = true)]
    [OutputType(typeof(Project))]
    public class UpdateProjectCommand : UpdateCommandBase<Project>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(ResourceType.Project)]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        [ResourceIdTransformation(ResourceType.Organization)]
        public ulong Organization { get; set; }

        [Parameter()]
        [AllowEmptyString]
        [ValidateSet("", "Manual", "Git", "Subversion", "Insights", "RemoteArchive")]
        public string? ScmType { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(ResourceType.ExecutionEnvironment)]
        public ulong? DefaultEnvironment { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong? SignatureValidationCredential { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? LocalPath { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmUrl { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmRefspec { get; set; }

        [Parameter()]
        [AllowNull]
        [ResourceIdTransformation(ResourceType.Credential)]
        public ulong? Credential { get; set; }

        [Parameter()]
        public bool? ScmClean { get; set; }

        [Parameter()]
        public bool? ScmDeleteOnUpdate { get; set; }

        [Parameter()]
        public bool? ScmTrackSubmodules { get; set; }

        [Parameter()]
        public bool? ScmUpdateOnLaunch { get; set; }

        [Parameter()]
        public bool? AllowOverride { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Timeout { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description is not null)
                sendData.Add("description", Description);
            if (Organization > 0)
                sendData.Add("organization", Organization);
            if (ScmType is not null)
            {
                var scmType = ScmType switch
                {
                    "Git" => "git",
                    "Subversion" => "svn",
                    "Insights" => "insights",
                    "RemoteArchive" => "archive",
                    "" => string.Empty,
                    "Manual" => string.Empty,
                    _ => throw new ArgumentException()
                };
                sendData.Add("scm_type", scmType);
            }
            if (DefaultEnvironment is not null)
                sendData.Add("default_environment", DefaultEnvironment == 0 ? null : DefaultEnvironment);
            if (SignatureValidationCredential is not null)
                sendData.Add("signature_validation_credential",
                        SignatureValidationCredential == 0 ? null : SignatureValidationCredential);
            if (LocalPath is not null)
                sendData.Add("local_path", LocalPath);
            if (ScmUrl is not null)
                sendData.Add("scm_url", ScmUrl);
            if (ScmBranch is not null)
                sendData.Add("scm_branch", ScmBranch);
            if (ScmRefspec is not null)
                sendData.Add("scm_refspec", ScmRefspec);
            if (Credential is not null)
                sendData.Add("credential", Credential == 0 ? null : Credential);
            if (ScmClean is not null)
                sendData.Add("scm_clean", ScmClean);
            if (ScmDeleteOnUpdate is not null)
                sendData.Add("scm_delete_on_update", ScmDeleteOnUpdate);
            if (ScmTrackSubmodules is not null)
                sendData.Add("scm_track_submodules", ScmTrackSubmodules);
            if (ScmUpdateOnLaunch is not null)
                sendData.Add("scm_update_on_launch", ScmUpdateOnLaunch);
            if (AllowOverride is not null)
                sendData.Add("allow_override", AllowOverride);
            if (Timeout is not null)
                sendData.Add("timeout", Timeout);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            if (TryPatch(Id, out var result))
            {
                WriteObject(result, false);
            }
        }
    }
}
