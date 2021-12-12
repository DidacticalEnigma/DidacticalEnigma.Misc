// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace MagicTranslatorProjectMemImporter.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RejectInvitationParams
    {
        /// <summary>
        /// Initializes a new instance of the RejectInvitationParams class.
        /// </summary>
        public RejectInvitationParams()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RejectInvitationParams class.
        /// </summary>
        public RejectInvitationParams(string invitingUserName = default(string), string projectName = default(string))
        {
            InvitingUserName = invitingUserName;
            ProjectName = projectName;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "invitingUserName")]
        public string InvitingUserName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

    }
}
