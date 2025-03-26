using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jagabata.Resources;

namespace Jagabata
{
    public class ApiConfig
    {

        [JsonConstructor]
        public ApiConfig(Uri origin, SecureString token, DateTime? lastSaved, string? lang) : this(origin, token, lang)
        {
            LastSaved = lastSaved;
        }
        public ApiConfig(Uri uri, SecureString token, string? lang = null)
        {
            Origin = new Uri($"{uri.Scheme}://{uri.Authority}");
            Token = token;
            if (!string.IsNullOrEmpty(lang))
            {
                Lang = lang;
            }
        }
        public ApiConfig()
        {
        }
        ~ApiConfig()
        {
            Token?.Dispose();
        }
        /// <summary>
        /// The URL of AWX.<br/>
        /// Should be `<c>scheme</c>://<c>domain</c>[:<c>port</c>]`
        /// </summary>
        [JsonPropertyName("origin")]
        public Uri Origin { get; private set; } = new Uri("http://localhost");
        /// <summary>
        /// Personal Access Token for OAuth.<br/>
        /// This token is assigned to the <c>Authorization</c> HTTP request header
        /// </summary>
        [JsonPropertyName("token")]
        public SecureString? Token { get; private set; }
        [JsonPropertyName("last_saved")]
        public DateTime? LastSaved { get; private set; }
        /// <summary>
        /// For the first directive of <c>Accept-Language</c> HTTP request header
        /// </summary>
        [JsonPropertyName("lang")]
        public string? Lang { get; private set; }
        /// <summary>
        /// Save to or Loaded file
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public FileInfo? File { get; set; }


        private static ApiConfig? _instance;

        /// <summary>
        /// Current config
        /// </summary>
        public static ApiConfig Instance => _instance is not null ? _instance : Load();
        /// <summary>
        /// Save the config to <paramref name="fileInfo"/> or
        /// default config path (<see cref="DefaultConfigPath"/>)
        /// </summary>
        /// <param name="fileInfo"></param>
        public void Save(FileInfo? fileInfo = null)
        {
            if (fileInfo is null)
            {
                File ??= new FileInfo(DefaultConfigPath);
                fileInfo = File;
            }
            else
            {
                File = fileInfo;
            }
            LastSaved = DateTime.UtcNow;
            using var fs = fileInfo.OpenWrite();
            JsonSerializer.Serialize(fs, this, Json.DeserializeOptions);
        }
        public static ApiConfig Load(ApiConfig config)
        {
            _instance = config;
            return config;
        }
        /// <summary>
        /// Load config from specified file
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns><see cref="ApiConfig"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public static ApiConfig Load(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                return Load(new ApiConfig());
            }
            using var fs = fileInfo.OpenRead();
            var config = JsonSerializer.Deserialize<ApiConfig>(fs, Json.DeserializeOptions)
                ?? throw new InvalidDataException($"Could not load config.");
            config.File = fileInfo;
            return Load(config);
        }
        /// <summary>
        /// Load config file from default file (<see cref="DefaultConfigPath"/>)
        /// </summary>
        /// <returns><see cref="ApiConfig"/></returns>
        public static ApiConfig Load()
        {
            return Load(new FileInfo(DefaultConfigPath));
        }
        /// <summary>
        /// Default config file
        /// <list type="bullet">
        /// <item><term>Windows</term><description><c>%USEFPROIFLE%\.ansible_psm_config.json</c></description></item>
        /// <item><term>Linux, MacOS</term><description><c>$HOME/.ansible_psm_config.json</c></description></item>
        /// </list>
        /// </summary>
        public static string DefaultConfigPath
        {
            get
            {
                var envPath = Environment.GetEnvironmentVariable(ENV_CONFIG);
                if (!string.IsNullOrEmpty(envPath) && System.IO.File.Exists(envPath))
                {
                    return envPath;
                }
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        return Path.Join(Environment.GetEnvironmentVariable("HOME") ?? string.Empty, DEFULT_CONFIG_NAME);
                    case PlatformID.Win32NT:
                        return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), DEFULT_CONFIG_NAME);
                    default:
                        throw new NotSupportedException($"Not supported: {Environment.OSVersion.Platform}");

                }
            }
        }

        private const string ENV_CONFIG = "ANSIBLE_API_CONFIG";
        private const string DEFULT_CONFIG_NAME = ".ansible_api_config.json";

        private User? _user;
        private ulong? _userId;
        private string? _userName;
        internal User LoadUser(bool save = false, bool force = false)
        {
            if (force || _user is null)
            {
                var task = User.GetMe();
                task.Wait();
                _user = task.Result;
            }
            _userId = _user.Id;
            _userName = _user.Username;
            if (save && File is not null)
                Save();

            return _user;
        }
        public ulong? UserId
        {
            get
            {
                if (_userId is null)
                    LoadUser(save: true);
                return _userId;
            }
            init => _userId = value;
        }
        public string? UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                    LoadUser(save: true);
                return _userName;
            }
            init => _userName = value;
        }

        internal string? GetTokenString()
        {
            if (Token is null)
                return null;

            var str = System.Runtime.InteropServices.Marshal.PtrToStringUni(
                    System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(Token));
            return str;
        }
    }
}
