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

    public partial class AddTranslationsResult
    {
        /// <summary>
        /// Initializes a new instance of the AddTranslationsResult class.
        /// </summary>
        public AddTranslationsResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AddTranslationsResult class.
        /// </summary>
        public AddTranslationsResult(IList<string> notAdded = default(IList<string>))
        {
            NotAdded = notAdded;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "notAdded")]
        public IList<string> NotAdded { get; set; }

    }
}
