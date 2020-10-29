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

    public partial class QueryResult
    {
        /// <summary>
        /// Initializes a new instance of the QueryResult class.
        /// </summary>
        public QueryResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryResult class.
        /// </summary>
        public QueryResult(IList<QueryTranslationResult> translations = default(IList<QueryTranslationResult>))
        {
            Translations = translations;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "translations")]
        public IList<QueryTranslationResult> Translations { get; private set; }

    }
}
