namespace Jagabata.Resources
{
    public interface ITeam
    {
        /// <summary>
        /// Name of this team.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Optional description of this team.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The organization ID to which this team belongs.
        /// </summary>
        ulong Organization { get; }
    }

    public class Team(ulong id, ResourceType type, string url, RelatedDictionary related,
                      SummaryFieldsDictionary summaryFields, DateTime created, DateTime? modified, string name,
                      string description, ulong organization)
        : ResourceBase, ITeam
    {
        public const string PATH = "/api/v2/teams/";

        /// <summary>
        /// Retrieve a Team.<br/>
        /// API Path: <c>/api/v2/teams/<paramref name="id"/>/</c>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Team> Get(ulong id)
        {
            var apiResult = await RestAPI.GetAsync<Team>($"{PATH}{id}/");
            return apiResult.Contents;
        }
        /// <summary>
        /// List Teams.<br/>
        /// API Path: <c>/api/v2/teams/</c>
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> Find(HttpQuery? query = null)
        {
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(PATH, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
                }
            }
        }
        /// <summary>
        /// List Teams for an Organization.<br/>
        /// API Path: <c>/api/v2/organizations/<paramref name="organizationId"/>/teams/</c>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> FindFromOrganization(ulong organizationId,
                                                                        HttpQuery? query = null)
        {
            var path = $"{Resources.Organization.PATH}{organizationId}/teams/";
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(path, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
                }
            }
        }
        /// <summary>
        /// List Teams for a User.<br/>
        /// API Path: <c>/api/v2/users/<paramref name="userId"/>/teams/</c>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> FindFromUser(ulong userId,
                                                                HttpQuery? query = null)
        {
            var path = $"{User.PATH}{userId}/teams/";
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(path, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
                }
            }
        }
        /// <summary>
        /// List Teams for a Project.<br/>
        /// API Path: <c>/api/v2/projects/<paramref name="projectId"/>/teams/</c>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> FindFromProject(ulong projectId,
                                                                   HttpQuery? query = null)
        {
            var path = $"{Project.PATH}{projectId}/teams/";
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(path, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
                }
            }
        }
        /// <summary>
        /// List owner Teams for a Credential.<br/>
        /// API Path: <c>/api/v2/credentials/<paramref name="credentialId"/>/owner_teams/</c>
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> FindOwnerFromCredential(ulong credentialId,
                                                                           HttpQuery? query = null)
        {
            var path = $"{Credential.PATH}{credentialId}/owner_teams/";
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(path, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
                }
            }
        }
        /// <summary>
        /// List Teams for a Role.<br/>
        /// API Path: <c>/api/v2/roles/<paramref name="credentialId"/>/teams/</c>
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Team> FindFromRole(ulong credentialId,
                                                                HttpQuery? query = null)
        {
            var path = $"{Role.PATH}{credentialId}/teams/";
            await foreach (var result in RestAPI.GetResultSetAsync<Team>(path, query))
            {
                foreach (var team in result.Contents.Results)
                {
                    yield return team;
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
        public ulong Organization { get; } = organization;

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/teams/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="pageSize">Max number of activity streams to retrieve</param>.
        public ActivityStream[] GetRecentActivityStream(ushort pageSize = 20)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", string.Empty, "-timestamp", pageSize)];
        }

        /// <summary>
        /// Get the recent activity stream for this resource
        /// <para>
        /// Implement API: <c>/api/v2/teams/{id}/activity_stream/</c>
        /// </para>
        /// </summary>
        /// <param name="query">Full customized queries (filtering, sorting and paging)</param>.
        public ActivityStream[] GetRecentActivityStream(HttpQuery query)
        {
            return [.. GetResultsByRelatedKey<ActivityStream>("activity_stream", query)];
        }

        protected override CacheItem GetCacheItem()
        {
            return new CacheItem(Type, Id, Name, Description);
        }

        public override string ToString()
        {
            return $"{Type}:{Id}:{Name}";
        }
    }
}

