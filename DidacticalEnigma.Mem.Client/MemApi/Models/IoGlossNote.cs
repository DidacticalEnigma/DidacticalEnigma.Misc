// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DidacticalEnigma.Mem.Client.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class IoGlossNote
    {
        /// <summary>
        /// Initializes a new instance of the IoGlossNote class.
        /// </summary>
        public IoGlossNote()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IoGlossNote class.
        /// </summary>
        public IoGlossNote(string foreign = default(string), string explanation = default(string))
        {
            Foreign = foreign;
            Explanation = explanation;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "foreign")]
        public string Foreign { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "explanation")]
        public string Explanation { get; set; }

    }
}
