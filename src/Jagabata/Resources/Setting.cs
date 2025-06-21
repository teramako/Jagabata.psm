using System.Runtime.CompilerServices;

namespace Jagabata.Resources
{
    /// <summary>
    /// <code>
    /// /api/v2/settings/
    /// </code>
    /// </summary>
    public class Setting(string url, string slug, string name)
    {
        public string Url { get; } = url;
        public string Slug { get; } = slug;
        public string Name { get; } = name;

        public const string PATH = "/api/v2/settings/";

        /// <summary>
        /// List setting slugs
        /// <para>
        /// Implement API: <c>/api/v2/settings/</c>
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Array of <see cref="Setting"/></returns>
        public static async Task<Setting[]> ListSlugsAsync(CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<ResultSet<Setting>>(PATH, cancellationToken: ct);
            return apiResult.Contents.Results;
        }

        /// <inheritdoc cref="ListSlugsAsync(CancellationToken)"/>
        public static Setting[] ListSlugs()
        {
            return ListSlugsAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get a Setting
        /// <para>
        /// Implement API: <c>/api/v2/settings/<paramref name="categorySlug"/>/</c>
        /// </para>
        /// </summary>
        /// <param name="categorySlug">Category slug</param>
        /// <param name="ct">Cancellation token</param>
        public static async Task<Dictionary<string, object?>> GetAsync(string categorySlug, CancellationToken ct = default)
        {
            var apiResult = await RestAPI.GetAsync<Dictionary<string, object?>>($"{PATH}{categorySlug}/", cancellationToken: ct);
            return apiResult.Contents;
        }

        /// <inheritdoc cref="GetAsync(string, CancellationToken)"/>
        public static Dictionary<string, object?> Get(string categorySlug)
        {
            return GetAsync(categorySlug).GetAwaiter().GetResult();
        }
    }
}

