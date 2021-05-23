// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace MagicTranslatorProjectMemImporter.MemApi
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for DidacticalEnigmaMem.
    /// </summary>
    public static partial class DidacticalEnigmaMemExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            public static object AddProject(this IDidacticalEnigmaMem operations, string projectName = default(string))
            {
                return operations.AddProjectAsync(projectName).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddProjectAsync(this IDidacticalEnigmaMem operations, string projectName = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddProjectWithHttpMessagesAsync(projectName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            public static object DeleteProject(this IDidacticalEnigmaMem operations, string projectName = default(string))
            {
                return operations.DeleteProjectAsync(projectName).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteProjectAsync(this IDidacticalEnigmaMem operations, string projectName = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteProjectWithHttpMessagesAsync(projectName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='body'>
            /// </param>
            public static object AddTranslations(this IDidacticalEnigmaMem operations, string projectName = default(string), AddTranslationsParams body = default(AddTranslationsParams))
            {
                return operations.AddTranslationsAsync(projectName, body).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddTranslationsAsync(this IDidacticalEnigmaMem operations, string projectName = default(string), AddTranslationsParams body = default(AddTranslationsParams), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddTranslationsWithHttpMessagesAsync(projectName, body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='correlationId'>
            /// </param>
            /// <param name='query'>
            /// </param>
            public static QueryResult Query(this IDidacticalEnigmaMem operations, string projectName = default(string), string correlationId = default(string), string query = default(string))
            {
                return operations.QueryAsync(projectName, correlationId, query).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='correlationId'>
            /// </param>
            /// <param name='query'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<QueryResult> QueryAsync(this IDidacticalEnigmaMem operations, string projectName = default(string), string correlationId = default(string), string query = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.QueryWithHttpMessagesAsync(projectName, correlationId, query, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='correlationId'>
            /// </param>
            public static object DeleteTranslation(this IDidacticalEnigmaMem operations, string projectName = default(string), string correlationId = default(string))
            {
                return operations.DeleteTranslationAsync(projectName, correlationId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='projectName'>
            /// </param>
            /// <param name='correlationId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteTranslationAsync(this IDidacticalEnigmaMem operations, string projectName = default(string), string correlationId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteTranslationWithHttpMessagesAsync(projectName, correlationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            public static object AddContexts(this IDidacticalEnigmaMem operations, AddContextsParams body = default(AddContextsParams))
            {
                return operations.AddContextsAsync(body).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddContextsAsync(this IDidacticalEnigmaMem operations, AddContextsParams body = default(AddContextsParams), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddContextsWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static QueryContextResult GetContext(this IDidacticalEnigmaMem operations, System.Guid? id = default(System.Guid?))
            {
                return operations.GetContextAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<QueryContextResult> GetContextAsync(this IDidacticalEnigmaMem operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetContextWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static object DeleteContext(this IDidacticalEnigmaMem operations, System.Guid? id = default(System.Guid?))
            {
                return operations.DeleteContextAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteContextAsync(this IDidacticalEnigmaMem operations, System.Guid? id = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteContextWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
