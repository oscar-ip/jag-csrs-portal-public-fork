using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics;
using Csrs.Interfaces.Dynamics.Models;

namespace Csrs.Api.Repositories
{
    public static class DynamicsExtensions
    {
        private const int Active = 1;

        /// <summary>
        /// Returns all the parties with matching ssg_bceid_userid, ordered by ssg_bceid_last_update descending.
        /// </summary>
        /// <param name="dynamicsClient"></param>
        /// <param name="bceid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MicrosoftDynamicsCRMssgCsrspartyCollection> GetPartyByBCeIdAsync(this IDynamicsClient dynamicsClient, string bceid, CancellationToken cancellationToken)
        {
            List<string> orderby = new List<string> { "ssg_bceid_last_update desc" };
            string filter = $"ssg_bceid_guid eq '{bceid}' and statuscode eq {Active}";
            var parties = await dynamicsClient.Ssgcsrsparties.GetAsync(filter: filter, orderby: orderby, cancellationToken: cancellationToken);
            return parties;
        }

        public static async Task<string> GetPartyIdByBCeIdAsync(this IDynamicsClient dynamicsClient, string bceid, CancellationToken cancellationToken)
        {
            List<string> select = new List<string> { "ssg_csrspartyid" };
            List<string> orderby = new List<string> { "ssg_bceid_last_update desc" };
            string filter = $"ssg_bceid_guid eq '{bceid}' and statuscode eq {Active}";
            try
            {
                var parties = await dynamicsClient.Ssgcsrsparties.GetAsync(filter: filter, orderby: orderby, cancellationToken: cancellationToken);

                if (parties is not null && parties.Value is not null && parties.Value.Count > 0)
                {
                    var party = parties.Value[0];
                    return party.SsgCsrspartyid;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static async Task<List<FileSummary>> GetFileSummaryByPartyAsync(this IDynamicsClient dynamicsClient, string partyId, CancellationToken cancellationToken)
        {
            var filter = $"(_ssg_recipient_value eq {partyId} or _ssg_payor_value eq {partyId}) and statuscode eq {Active}";
            var orderby = new List<string> { "createdon" };
            var select = new List<string> { "ssg_filenumber", "_ssg_recipient_value", "_ssg_payor_value" };

            var files = await dynamicsClient.Ssgcsrsfiles.GetAsync(filter: filter, orderby: orderby, select: select, cancellationToken: cancellationToken);

            var results = new List<FileSummary>();

            foreach (var file in files.Value)
            {
                PartyRole role = PartyRole.Unknown;

                if (file._ssgRecipientValue == partyId)
                {
                    role = PartyRole.Recipient;
                }
                else if (file._ssgPayorValue == partyId)
                {
                    role = PartyRole.Payor;
                }

                results.Add(new FileSummary {  FileId = file.SsgFilenumber, UsersRole = role, Status = FileStatus.Active });
            }

            return results;
        }
    }
}
