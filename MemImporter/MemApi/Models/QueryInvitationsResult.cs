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

    public partial class QueryInvitationsResult
    {
        /// <summary>
        /// Initializes a new instance of the QueryInvitationsResult class.
        /// </summary>
        public QueryInvitationsResult()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the QueryInvitationsResult class.
        /// </summary>
        public QueryInvitationsResult(IList<QueryInvitationSentResult> invitationsPending = default(IList<QueryInvitationSentResult>), IList<QueryInvitationReceivedResult> invitationsReceived = default(IList<QueryInvitationReceivedResult>))
        {
            InvitationsPending = invitationsPending;
            InvitationsReceived = invitationsReceived;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "invitationsPending")]
        public IList<QueryInvitationSentResult> InvitationsPending { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "invitationsReceived")]
        public IList<QueryInvitationReceivedResult> InvitationsReceived { get; set; }

    }
}
