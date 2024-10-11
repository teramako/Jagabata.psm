using System.Management.Automation;
using AWX.Resources;

namespace AWX.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Organization")]
    [OutputType(typeof(Organization))]
    public class GetOrganizationCommand : GetCommandBase<Organization>
    {
        protected override ResourceType AcceptType => ResourceType.Organization;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "Organization", DefaultParameterSetName = "All")]
    [OutputType(typeof(Organization))]
    public class FindOrganizationCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        [ValidateSet(nameof(ResourceType.User))]
        public override ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", ValueFromPipelineByPropertyName = true)]
        public override ulong Id { get; set; }
        [Parameter(ParameterSetName = "AssociatedWith")]
        public SwitchParameter Admin { get; set; }

        [Parameter(Position = 0)]
        public string[]? Name { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            if (Name != null)
            {
                Query.Add("name__in", string.Join(",", Name));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Type switch
            {
                ResourceType.User => $"{User.PATH}{Id}/" + (Admin ? "admin_of_organizations/" : "organizations/"),
                _ => Organization.PATH
            };
            foreach (var resultSet in GetResultSet<Organization>(path, Query, All))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.New, "Organization", SupportsShouldProcess = true)]
    [OutputType(typeof(Organization))]
    public class NewOrganizationCommand : NewCommandBase<Organization>
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public uint MaxHosts { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        public ulong DefaultEnvironment { get; set; }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>()
            {
                { "name", Name }
            };
            if (Description != null)
                sendData.Add("description", Description);
            if (MaxHosts > 0)
                sendData.Add("max_hosts", MaxHosts);
            if (DefaultEnvironment > 0)
                sendData.Add("default_environment", DefaultEnvironment);

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
    
    [Cmdlet(VerbsData.Update, "Organization", SupportsShouldProcess = true)]
    [OutputType(typeof(Organization))]
    public class UpdateOrganizationCommand : UpdateCommandBase<Organization>
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string? Name { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Description { get; set; }

        [Parameter()]
        public uint? MaxHosts { get; set; }

        [Parameter()]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.ExecutionEnvironment])]
        [AllowNull]
        public ulong? DefaultEnvironment { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(Name))
                sendData.Add("name", Name);
            if (Description != null)
                sendData.Add("description", Description);
            if (MaxHosts != null)
                sendData.Add("max_hosts", MaxHosts);
            if (DefaultEnvironment != null)
                sendData.Add("default_environment", DefaultEnvironment == 0 ? null : DefaultEnvironment);

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

    [Cmdlet(VerbsCommon.Remove, "Organization", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveOrganizationCommand : RemoveCommandBase<Organization>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.Organization])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
