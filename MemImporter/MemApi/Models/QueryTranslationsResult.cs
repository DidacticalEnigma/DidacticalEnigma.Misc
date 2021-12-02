// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace MagicTranslatorProjectMemImporter.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class QueryTranslationsResult
    {
        /// <summary>
        /// Initializes a new instance of the QueryTranslationsResult class.
        /// </summary>
        public QueryTranslationsResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryTranslationsResult class.
        /// </summary>
        public QueryTranslationsResult(IList<QueryTranslationResult> translations = default(IList<QueryTranslationResult>), string paginationToken = default(string), System.DateTime? queryTime = default(System.DateTime?))
        {
            Translations = translations;
            PaginationToken = paginationToken;
            QueryTime = queryTime;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "translations")]
        public IList<QueryTranslationResult> Translations { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "paginationToken")]
        public string PaginationToken { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "queryTime")]
        public System.DateTime? QueryTime { get; set; }

    }
}