// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DidacticalEnigma.Mem.Client.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CancelInvitationParams
    {
        /// <summary>
        /// Initializes a new instance of the CancelInvitationParams class.
        /// </summary>
        public CancelInvitationParams()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CancelInvitationParams class.
        /// </summary>
        public CancelInvitationParams(string invitedUserName = default(string), string projectName = default(string))
        {
            InvitedUserName = invitedUserName;
            ProjectName = projectName;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "invitedUserName")]
        public string InvitedUserName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

    }
}
