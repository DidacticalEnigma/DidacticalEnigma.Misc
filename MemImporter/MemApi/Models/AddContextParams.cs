// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace MagicTranslatorProjectMemImporter.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class AddContextParams
    {
        /// <summary>
        /// Initializes a new instance of the AddContextParams class.
        /// </summary>
        public AddContextParams()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AddContextParams class.
        /// </summary>
        public AddContextParams(System.Guid? id = default(System.Guid?), byte[] content = default(byte[]), string mediaType = default(string), string text = default(string))
        {
            Id = id;
            Content = content;
            MediaType = mediaType;
            Text = text;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public byte[] Content { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mediaType")]
        public string MediaType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

    }
}