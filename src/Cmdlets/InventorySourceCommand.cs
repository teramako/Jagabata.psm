using AWX.Resources;
using System.Management.Automation;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "InventorySource")]
    [OutputType(typeof(InventorySource))]
    public class GetInventorySourceCommand : GetCommandBase<InventorySource>
    {
        protected override ResourceType AcceptType => ResourceType.InventorySource;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "InventorySource", DefaultParameterSetName = "All")]
    [OutputType(typeof(InventorySource))]
    public class FindInventorySourceCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.Project),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.Group),
                     nameof(ResourceType.Host))]
        public override ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public override ulong Id { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.Project => $"{Project.PATH}{Id}/scm_inventory_sources/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/inventory_sources/",
                ResourceType.Group => $"{Group.PATH}{Id}/inventory_sources/",
                ResourceType.Host => $"{Host.PATH}{Id}/inventory_sources/",
                _ => InventorySource.PATH
            };
            Find<InventorySource>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "InventorySource", SupportsShouldProcess = true)]
    [OutputType(typeof(InventorySource))]
    public class NewInventorySourceCommand : NewCommandBase<InventorySource>
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter(Mandatory = true)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Inventory])]
        public ulong Inventory { get; set; }

        [Parameter(Mandatory = true)]
        public InventorySourceSource Source { get ;set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Project])]
        public ulong? SourceProject { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SourcePath { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation]
        [AllowEmptyString]
        public string? SourceVars { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? Credential { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? EnabledVar { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? EnabledValue { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? HostFilter { get; set; }

        [Parameter()]
        public SwitchParameter Overwrite { get; set; }

        [Parameter()]
        public SwitchParameter OverwriteVars { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Timeout { get; set; }

        [Parameter()]
        [ValidateRange(0, 2)]
        public int? Verbosity { get; set; }

        [Parameter()]
        public string? Limit { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        public SwitchParameter UpdateOnLaunch { get; set; }

        [Parameter()]
        public int? UpdateCacheTimeout { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name },
                { "source", $"{Source}".ToLowerInvariant() },
                { "inventory", Inventory },
            };
            if (Description is not null)
                sendData.Add("description", Description);
            if (SourceProject is not null)
                sendData.Add("source_project", SourceProject);
            if (SourcePath is not null)
                sendData.Add("source_path", SourcePath);
            if (SourceVars is not null)
                sendData.Add("source_vars", SourceVars);
            if (Credential is not null)
                sendData.Add("credential", Credential);
            if (EnabledVar is not null)
                sendData.Add("enabled_var", EnabledVar);
            if (EnabledValue is not null)
                sendData.Add("enabled_value", EnabledValue);
            if (HostFilter is not null)
                sendData.Add("host_filter", HostFilter);
            if (Overwrite)
                sendData.Add("overwrite", true);
            if (OverwriteVars)
                sendData.Add("overwrite_vars", true);
            if (Timeout is not null)
                sendData.Add("timeout", Timeout);
            if (Verbosity is not null)
                sendData.Add("verbosity", Verbosity);
            if (Limit is not null)
                sendData.Add("limit", Limit);
            if (ExecutionEnvironment is not null)
                sendData.Add("execution_environment", ExecutionEnvironment);
            if (UpdateOnLaunch)
                sendData.Add("update_on_launch", true);
            if (UpdateCacheTimeout is not null)
                sendData.Add("update_cache_timeout", UpdateCacheTimeout);
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

    [Cmdlet(VerbsData.Update, "InventorySource", SupportsShouldProcess = true)]
    [OutputType(typeof(InventorySource))]
    public class UpdateInventorySourceCommand : UpdateCommandBase<InventorySource>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.InventorySource])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public InventorySourceSource? Source { get ;set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Project])]
        public ulong? SourceProject { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? SourcePath { get; set; }

        [Parameter()]
        [ExtraVarsArgumentTransformation]
        [AllowEmptyString]
        public string? SourceVars { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? ScmBranch { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Credential])]
        public ulong? Credential { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? EnabledVar { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? EnabledValue { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? HostFilter { get; set; }

        [Parameter()]
        public bool? Overwrite { get; set; }

        [Parameter()]
        public bool? OverwriteVars { get; set; }

        [Parameter()]
        [ValidateRange(0, int.MaxValue)]
        public int? Timeout { get; set; }

        [Parameter()]
        [ValidateRange(0, 2)]
        public int? Verbosity { get; set; }

        [Parameter()]
        public string? Limit { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong? ExecutionEnvironment { get; set; }

        [Parameter()]
        public bool? UpdateOnLaunch { get; set; }

        [Parameter()]
        public int? UpdateCacheTimeout { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Source is not null)
                sendData.Add("source", $"{Source}".ToLowerInvariant());
            if (Description is not null)
                sendData.Add("description", Description);
            if (SourceProject is not null)
                sendData.Add("source_project", SourceProject == 0 ? null : SourceProject);
            if (SourcePath is not null)
                sendData.Add("source_path", SourcePath);
            if (SourceVars is not null)
                sendData.Add("source_vars", SourceVars);
            if (Credential is not null)
                sendData.Add("credential", Credential == 0 ? null : Credential);
            if (EnabledVar is not null)
                sendData.Add("enabled_var", EnabledVar);
            if (EnabledValue is not null)
                sendData.Add("enabled_value", EnabledValue);
            if (HostFilter is not null)
                sendData.Add("host_filter", HostFilter);
            if (Overwrite is not null)
                sendData.Add("overwrite", Overwrite);
            if (OverwriteVars is not null)
                sendData.Add("overwrite_vars", OverwriteVars);
            if (Timeout is not null)
                sendData.Add("timeout", Timeout);
            if (Verbosity is not null)
                sendData.Add("verbosity", Verbosity);
            if (Limit is not null)
                sendData.Add("limit", Limit);
            if (ExecutionEnvironment is not null)
                sendData.Add("execution_environment", ExecutionEnvironment == 0 ? null : ExecutionEnvironment);
            if (UpdateOnLaunch is not null)
                sendData.Add("update_on_launch", UpdateOnLaunch);
            if (UpdateCacheTimeout is not null)
                sendData.Add("update_cache_timeout", UpdateCacheTimeout);
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

    [Cmdlet(VerbsCommon.Remove, "InventorySource", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveInventorySourceCommand : RemoveCommandBase<InventorySource>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.InventorySource])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
