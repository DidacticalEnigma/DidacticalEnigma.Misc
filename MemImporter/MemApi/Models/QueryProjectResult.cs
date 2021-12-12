// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace MagicTranslatorProjectMemImporter.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class QueryProjectResult
    {
        /// <summary>
        /// Initializes a new instance of the QueryProjectResult class.
        /// </summary>
        public QueryProjectResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryProjectResult class.
        /// </summary>
        public QueryProjectResult(string name = default(string), string owner = default(string), bool? canContribute = default(bool?))
        {
            Name = name;
            Owner = owner;
            CanContribute = canContribute;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "canContribute")]
        public bool? CanContribute { get; set; }

    }
}
