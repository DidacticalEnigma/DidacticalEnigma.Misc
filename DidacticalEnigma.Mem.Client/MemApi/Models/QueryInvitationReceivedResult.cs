// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DidacticalEnigma.Mem.Client.MemApi.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class QueryInvitationReceivedResult
    {
        /// <summary>
        /// Initializes a new instance of the QueryInvitationReceivedResult
        /// class.
        /// </summary>
        public QueryInvitationReceivedResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryInvitationReceivedResult
        /// class.
        /// </summary>
        public QueryInvitationReceivedResult(string projectName = default(string), string invitingUser = default(string))
        {
            ProjectName = projectName;
            InvitingUser = invitingUser;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "invitingUser")]
        public string InvitingUser { get; set; }

    }
}
