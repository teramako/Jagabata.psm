using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Resources;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;

namespace Jagabata.Cmdlets
{

    [Cmdlet(VerbsCommon.Get, "Me")]
    [OutputType(typeof(User))]
    public class GetMeCommand : APICmdletBase
    {
        protected override void EndProcessing()
        {
            foreach (var resultSet in GetResultSet<User>("/api/v2/me/", true))
            {
                WriteObject(resultSet.Results, true);
            }
        }
    }

    [Cmdlet(VerbsCommon.Get, "User")]
    [OutputType(typeof(User))]
    public class GetUserCommand : GetCommandBase<User>
    {
        protected override ResourceType AcceptType => ResourceType.User;

        protected override void ProcessRecord()
        {
            GatherResourceId();
        }
        protected override void EndProcessing()
        {
            WriteObject(GetResultSet(), true);
        }
    }

    [Cmdlet(VerbsCommon.Find, "User", DefaultParameterSetName = "All")]
    [OutputType(typeof(User))]
    public class FindUserCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.Organization),
                     nameof(ResourceType.Team),
                     nameof(ResourceType.Credential),
                     nameof(ResourceType.Role))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Team,
                ResourceType.Credential,
                ResourceType.Role
        ])]
        public IResource? Resource { get; set; }

        [Parameter(Position = 2)]
        public string[]? UserName { get; set; }

        [Parameter(Position = 3)]
        public string[]? Email { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            if (UserName is not null)
            {
                Query.Add("username__in", string.Join(',', UserName));
            }
            if (Email is not null)
            {
                Query.Add("email__in", string.Join(",", Email));
            }
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Id}/users/",
                ResourceType.Team => $"{Team.PATH}{Id}/users/",
                ResourceType.Credential => $"{Credential.PATH}{Id}/owner_users/",
                ResourceType.Role => $"{Role.PATH}{Id}/users/",
                _ => User.PATH
            };
            Find<User>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "AccessList", DefaultParameterSetName = "AssociatedWith")]
    [OutputType(typeof(User))]
    public class FindAccessListCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 0)]
        [ValidateSet(nameof(ResourceType.InstanceGroup),
                     nameof(ResourceType.Organization),
                     nameof(ResourceType.User),
                     nameof(ResourceType.Project),
                     nameof(ResourceType.Team),
                     nameof(ResourceType.Credential),
                     nameof(ResourceType.Inventory),
                     nameof(ResourceType.JobTemplate),
                     nameof(ResourceType.WorkflowJobTemplate))]
        public ResourceType Type { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "AssociatedWith", Position = 1)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PipelineInput", ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.InstanceGroup,
                ResourceType.Organization,
                ResourceType.User,
                ResourceType.Project,
                ResourceType.Team,
                ResourceType.Credential,
                ResourceType.Inventory,
                ResourceType.JobTemplate,
                ResourceType.WorkflowJobTemplate
        ])]
        public IResource? Resource { get; set; }

        [Parameter()]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            if (Resource is not null)
            {
                Type = Resource.Type;
                Id = Resource.Id;
            }

            var path = Type switch
            {
                ResourceType.InstanceGroup => $"{InstanceGroup.PATH}{Id}/access_list/",
                ResourceType.Organization => $"{Organization.PATH}{Id}/access_list/",
                ResourceType.User => $"{User.PATH}{Id}/access_list/",
                ResourceType.Project => $"{Project.PATH}{Id}/access_list/",
                ResourceType.Team => $"{Team.PATH}{Id}/access_list/",
                ResourceType.Credential => $"{Credential.PATH}{Id}/access_list/",
                ResourceType.Inventory => $"{Inventory.PATH}{Id}/access_list/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Id}/access_list/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Id}/access_list/",
                _ => throw new ArgumentException($"Can't handle the type: {Type}")
            };
            Find<User>(path);
        }
    }

    [Cmdlet(VerbsCommon.New, "User", SupportsShouldProcess = true, DefaultParameterSetName = "Credential")]
    [OutputType(typeof(User))]
    public class NewUserCommand : NewCommandBase<User>
    {
        [Parameter(Mandatory = true, ParameterSetName = "Credential", Position = 0)]
        [Credential]
        public PSCredential? Credential { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "SecureString", Position = 0)]
        public string UserName { get; set; } = string.Empty;

        [Parameter(ParameterSetName = "SecureString")]
        public SecureString? Password { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string FirstName { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string LastName { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string Email { get; set; } = string.Empty;

        [Parameter()]
        public SwitchParameter IsSuperUser { get; set; }

        [Parameter()]
        public SwitchParameter IsSystemAuditor { get; set; }

        private string? _user;
        private SecureString? _password;
        private bool _passwordInputedFromPrompt = false;

        private void GatherUserAndPassword()
        {
            if (Credential is not null)
            {
                _user = Credential.UserName;
                _password = Credential.Password;
            }
            else 
            {
                _user = UserName;
                if (Password is not null)
                {
                    _password = Password;
                }
                else
                {
                    if (CommandRuntime.Host is null)
                        throw new NullReferenceException();

                    _passwordInputedFromPrompt = true;
                    var prompt = new AskPrompt(CommandRuntime.Host);
                    if (!prompt.AskPassword($"Password for {_user}", "Password", "", out var pass))
                    {
                        return;
                    }
                    _password = pass.Input;
                }
            }

            if (_password is null || _password.Length == 0)
            {
                throw new ArgumentException("Password should not be empty.");
            }

            if (string.IsNullOrEmpty(_user))
            {
                if (_passwordInputedFromPrompt)
                    _password.Dispose();
                throw new ArgumentException("UserName should not be empty.");
            }
        }

        protected override Dictionary<string, object> CreateSendData()
        {
            var sendData = new Dictionary<string, object>();
            if (_user is not null)
                sendData.Add("username", _user);
            if (_password is not null)
                sendData.Add("password", _password);
            if (!string.IsNullOrEmpty(FirstName))
                sendData.Add("first_name", FirstName);
            if (!string.IsNullOrEmpty(LastName))
                sendData.Add("last_name", LastName);
            if (!string.IsNullOrEmpty(Email))
                sendData.Add("email", Email);
            if (IsSuperUser)
                sendData.Add("is_superuser", IsSuperUser);
            if (IsSystemAuditor)
                sendData.Add("is_system_auditor", IsSystemAuditor);

            return sendData;
        }

        protected override void ProcessRecord()
        {
            try
            {
                GatherUserAndPassword();
            }
            catch (Exception e)
            {
                WriteError(new ErrorRecord(e, "Invalid UserName or Password", ErrorCategory.InvalidArgument, null));
                return;
            }

            if (TryCreate(out var result))
            {
                WriteObject(result, false);
            }

            if (_passwordInputedFromPrompt && _password is not null)
                _password.Dispose();
        }
    }

    [Cmdlet(VerbsData.Update, "User", SupportsShouldProcess = true)]
    [OutputType(typeof(User))]
    public class UpdateUserCommand : UpdateCommandBase<User>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.User])]
        public override ulong Id { get; set; }

        [Parameter()]
        public string UserName { get; set; } = string.Empty;

        [Parameter()]
        [AllowEmptyString]
        public string? FirstName { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? LastName { get; set; }

        [Parameter()]
        [AllowEmptyString]
        public string? Email { get; set; }

        [Parameter()]
        public bool? IsSuperUser { get; set; } = null;

        [Parameter()]
        public bool? IsSystemAuditor { get; set; } = null;

        [Parameter()]
        public SecureString? Password { get; set; }

        protected override Dictionary<string, object?> CreateSendData()
        {
            var sendData = new Dictionary<string, object?>();
            string dataDescription = string.Empty;
            if (!string.IsNullOrEmpty(UserName))
                sendData.Add("username", UserName);
            if (FirstName is not null)
                sendData.Add("first_name", FirstName);
            if (LastName is not null)
                sendData.Add("last_name", LastName);
            if (Email is not null)
                sendData.Add("email", Email);
            if (IsSuperUser is not null)
                sendData.Add("is_superuser", IsSuperUser);
            if (IsSystemAuditor is not null)
                sendData.Add("is_system_auditor", IsSystemAuditor);
            if (Password is not null)
            {
                var passwordString = Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(Password));
                Password.Dispose();
                if (!string.IsNullOrEmpty(passwordString))
                {
                    sendData.Add("password", "***"); // dummy
                    dataDescription = Json.Stringify(sendData, pretty: true);
                    sendData["password"] = passwordString;
                }
            }

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

    [Cmdlet(VerbsLifecycle.Register, "User", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class RegisterUserCommand : RegistrationCommandBase<User>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.User])]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Team,
                ResourceType.Role
        ])]
        public IResource To { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = To.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{To.Id}/users/",
                ResourceType.Team => $"{Team.PATH}{To.Id}/users/",
                ResourceType.Role => $"{Role.PATH}{To.Id}/users/",
                _ => throw new ArgumentException($"Invalid resource type: {To.Type}")
            };
            Register(path, Id, To);
        }
    }

    [Cmdlet(VerbsLifecycle.Unregister, "User", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]
    public class UnregisterUserCommand : RegistrationCommandBase<User>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.User])]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(AcceptableTypes = [
                ResourceType.Organization,
                ResourceType.Team,
                ResourceType.Role
        ])]
        public IResource From { get; set; } = new Resource(0, 0);

        protected override void ProcessRecord()
        {
            var path = From.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{From.Id}/users/",
                ResourceType.Team => $"{Team.PATH}{From.Id}/users/",
                ResourceType.Role => $"{Role.PATH}{From.Id}/users/",
                _ => throw new ArgumentException($"Invalid resource type: {From.Type}")
            };
            Unregister(path, Id, From);
        }
    }

    [Cmdlet(VerbsCommon.Remove, "User", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [OutputType(typeof(void))]
    public class RemoveUserCommand : RemoveCommandBase<User>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceIdTransformation(AcceptableTypes = [ResourceType.User])]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}