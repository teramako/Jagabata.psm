using Jagabata.Cmdlets.ArgumentTransformation;
using Jagabata.Cmdlets.Completer;
using Jagabata.Cmdlets.Utilities;
using Jagabata.Resources;
using System.Management.Automation;
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
        [Parameter(Mandatory = true, Position = 0, ValueFromRemainingArguments = true, ValueFromPipeline = true)]
        [ResourceIdTransformation(ResourceType.User)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.User)]
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

    [Cmdlet(VerbsCommon.Find, "User")]
    [OutputType(typeof(User))]
    public class FindUserCommand : FindCommandBase
    {
        [Parameter(ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.Organization, ResourceType.Team, ResourceType.Credential, ResourceType.Role
        )]
        [ResourceCompletions(
            ResourceType.Organization, ResourceType.Team, ResourceType.Credential, ResourceType.Role
        )]
        [Alias("associatedWith", "r")]
        public IResource? Resource { get; set; }

        [Parameter(Position = 1)]
        public string[]? UserName { get; set; }

        [Parameter(Position = 2)]
        public string[]? Email { get; set; }

        [Parameter()]
        [OrderByCompletion("id", "username", "first_name", "last_name", "email", "is_superuser", "last_login",
                           "enterprise_auth", "social_auth", "main_oauth2application", "activity_stream",
                           "roles", "profile")]
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
            var path = Resource?.Type switch
            {
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/users/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/users/",
                ResourceType.Credential => $"{Credential.PATH}{Resource.Id}/owner_users/",
                ResourceType.Role => $"{Role.PATH}{Resource.Id}/users/",
                _ => User.PATH
            };
            Find<User>(path);
        }
    }

    [Cmdlet(VerbsCommon.Find, "AccessList")]
    [OutputType(typeof(User))]
    public class FindAccessListCommand : FindCommandBase
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        [ResourceTransformation(
            ResourceType.InstanceGroup, ResourceType.Organization, ResourceType.User, ResourceType.Project,
            ResourceType.Team, ResourceType.Credential, ResourceType.Inventory, ResourceType.JobTemplate,
            ResourceType.WorkflowJobTemplate
        )]
        [ResourceCompletions(
            ResourceType.InstanceGroup, ResourceType.Organization, ResourceType.User, ResourceType.Project,
            ResourceType.Team, ResourceType.Credential, ResourceType.Inventory, ResourceType.JobTemplate,
            ResourceType.WorkflowJobTemplate
        )]
        [Alias("associatedWith", "r")]
        public IResource Resource { get; set; } = new Resource(0, 0);

        [Parameter()]
        [OrderByCompletion("id", "username", "first_name", "last_name", "email", "is_superuser", "last_login",
                           "enterprise_auth", "social_auth", "main_oauth2application", "activity_stream",
                           "roles", "profile")]
        public override string[] OrderBy { get; set; } = ["id"];

        protected override void BeginProcessing()
        {
            SetupCommonQuery();
        }
        protected override void ProcessRecord()
        {
            var path = Resource.Type switch
            {
                ResourceType.InstanceGroup => $"{InstanceGroup.PATH}{Resource.Id}/access_list/",
                ResourceType.Organization => $"{Organization.PATH}{Resource.Id}/access_list/",
                ResourceType.User => $"{User.PATH}{Resource.Id}/access_list/",
                ResourceType.Project => $"{Project.PATH}{Resource.Id}/access_list/",
                ResourceType.Team => $"{Team.PATH}{Resource.Id}/access_list/",
                ResourceType.Credential => $"{Credential.PATH}{Resource.Id}/access_list/",
                ResourceType.Inventory => $"{Inventory.PATH}{Resource.Id}/access_list/",
                ResourceType.JobTemplate => $"{JobTemplate.PATH}{Resource.Id}/access_list/",
                ResourceType.WorkflowJobTemplate => $"{WorkflowJobTemplate.PATH}{Resource.Id}/access_list/",
                _ => throw new ArgumentException($"Can't handle the type: {Resource?.Type}")
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
        private bool _passwordInputedFromPrompt;

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
        [ResourceIdTransformation(ResourceType.User)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.User)]
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
                sendData.Add("password", Password);
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
        [ResourceIdTransformation(ResourceType.User)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.User)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(ResourceType.Organization, ResourceType.Team, ResourceType.Role)]
        [ResourceCompletions(ResourceType.Organization, ResourceType.Team, ResourceType.Role)]
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
        [ResourceIdTransformation(ResourceType.User)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.User)]
        public ulong Id { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ResourceTransformation(ResourceType.Organization, ResourceType.Team, ResourceType.Role)]
        [ResourceCompletions(ResourceType.Organization, ResourceType.Team, ResourceType.Role)]
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
        [ResourceIdTransformation(ResourceType.User)]
        [ResourceCompletions(ResourceCompleteType.Id, ResourceType.User)]
        public ulong Id { get; set; }

        protected override void ProcessRecord()
        {
            TryDelete(Id);
        }
    }
}
