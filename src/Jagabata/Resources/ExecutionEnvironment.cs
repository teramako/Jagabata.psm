using System.Collections.Specialized;

namespace Jagabata.Resources
{
    public interface IExecutionEnvironment
    {
        /// <summary>
        /// Name of this execution environment.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this execution environment.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The organization used to determine access to this execution environment.
        /// </summary>
        ulong? Organization { get; }
        /// <summary>
        /// The full image location, including the container registory, image name, and version tag.
        /// </summary>
        string Image { get; }
        ulong? Credential { get; }
        /// <summary>
        /// Pull image before running?
        /// <list type="bullet">
        /// <item><term><c>""</c></term><description>----- (default)</description></item>
        /// <item><term><c>"always"</c></term><description>Always pull container before running</description></item>
        /// <item><term><c>"missing"</c></term><description>Only pull the image if not present before running</description></item>
        /// <item><term><c>"never"</c></term><description>Never pull container before running</description></item>
        /// </list>
        /// </summary>
        string Pull { get; }
    }

    public class ExecutionEnvironment(ulong id, ResourceType type, string url, RelatedDictionary related,
                                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified,
                                      string name, string description, ulong? organization, string image, bool managed,
                                      ulong? credential, string pull)
        : ResourceBase, IExecutionEnvironment
    {
        public const string PATH = "/api/v2/execution_environments/";

        /// <summary>
        /// Retrieve an Execution Environment.<br/>
        /// API Path: <c>/api/v2/execution_environments/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<ExecutionEnvironment> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<ExecutionEnvironment>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Execution Environments.<br/>
        /// API Path: <c>/api/v2/execution_environments/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ExecutionEnvironment> Find(NameValueCollection? query, bool getAll = false)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<ExecutionEnvironment>(PATH, query, getAll))
            {
                foreach (var exeEnv in result.Contents.Results)
                {
                    yield return exeEnv;
                }
            }
        }
        /// <summary>
        /// List Execution Environments for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/execution_environments/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <param name="getAll"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<ExecutionEnvironment> FindFromOrganization(ulong organizationId,
                                                                                        NameValueCollection? query = null,
                                                                                        bool getAll = false)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/execution_environments/";
            await foreach (var result in RestAPI.GetResultSetAsync<ExecutionEnvironment>(path, query, getAll))
            {
                foreach (var exeEnv in result.Contents.Results)
                {
                    yield return exeEnv;
                }
            }
        }

        public override ulong Id { get; } = id;
        public override ResourceType Type { get; } = type;
        public override string Url { get; } = url;
        public override RelatedDictionary Related { get; } = related;
        public override SummaryFieldsDictionary SummaryFields { get; } = summaryFields;
        public DateTime Created { get; } = created;
        public DateTime? Modified { get; } = modified;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public ulong? Organization { get; } = organization;
        public string Image { get; } = image;
        public bool Managed { get; } = managed;
        public ulong? Credential { get; } = credential;
        public string Pull { get; } = pull;

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description)
            {
                Metadata = {
                    ["Image"] = Image
                }
            };
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}
