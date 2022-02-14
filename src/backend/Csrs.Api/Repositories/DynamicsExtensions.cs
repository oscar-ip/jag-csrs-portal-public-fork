using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Rest;
using System.Net;

namespace Csrs.Interfaces.Dynamics
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
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            List<string> orderby = new List<string> { "ssg_bceid_last_update desc" };
            string filter = $"ssg_bceid_guid eq '{bceid}' and statuscode eq {Active}";
            var parties = await dynamicsClient.Ssgcsrsparties.GetAsync(filter: filter, orderby: orderby, cancellationToken: cancellationToken);
            return parties;
        }

        public static async Task<bool> FileExistsAsync(this IDynamicsClient dynamicsClient, string id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            try
            {
                List<string> select = new() { "ssg_csrsfileid" };
                _ = await dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(id, select: select, cancellationToken: cancellationToken);
                return true;
            }
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public static async Task<MicrosoftDynamicsCRMssgCsrsfile> GetFileByPartyAndId(this IDynamicsClient dynamicsClient, string partyId, string fileId, CancellationToken cancellationToken)
        {
            
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            if (string.IsNullOrEmpty(partyId) || string.IsNullOrEmpty(fileId))
            {
                return null;
            }

            try
            {
                
                var filter = $"(_ssg_recipient_value eq {partyId} or _ssg_payor_value eq {partyId}) and ssg_csrsfileid eq {fileId}";
                var select = new List<string> { "ssg_csrsfileid" };

                var files = await dynamicsClient.Ssgcsrsfiles.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);

                if (files.Value.Count == 0) return null;

                return files.Value[0];

            }
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public static async Task<string> GetPartyIdByBCeIdAsync(this IDynamicsClient dynamicsClient, string bceid, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

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
            ArgumentNullException.ThrowIfNull(dynamicsClient);

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

        public static async Task<MicrosoftDynamicsCRMssgCsrsfileCollection> GetFilesByParty(this IDynamicsClient dynamicsClient, string partyId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            string filter = $"_ssg_payor_value eq {partyId} or _ssg_recipient_value eq {partyId}";
            List<string> select = new List<string> { "ssg_csrsfileid" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            try
            {
                return await dynamicsClient.Ssgcsrsfiles.GetAsync(select: select, orderby: orderby, filter: filter, expand: null, cancellationToken: cancellationToken);
            }
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

        }
        public static async Task<MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection> GetCommunicationMessagesByFile(this IDynamicsClient dynamicsClient, string fileId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            string filter = $"_ssg_csrsfile_value eq {fileId}";
            List<string> select = new List<string> { "_ssg_csrsfile_value", "ssg_sentreceiveddate", "ssg_csrsmessage", "ssg_csrsmessageattachment", "ssg_csrsmessageread", "ssg_csrsmessagesubject", "statuscode", "_ssg_toparty_value" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            var messages = await dynamicsClient.Ssgcsrscommunicationmessages.GetAsync(select: select, orderby: orderby, filter: filter, cancellationToken: cancellationToken);

            return messages;

        }

        public static async Task<PicklistOptionSetMetadata> GetPicklistOptionSetMetadataAsync(
            this IDynamicsClient dynamicsClient,
            string entityName,
            string attributeName,
            IMemoryCache cache,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);
            ArgumentNullException.ThrowIfNull(entityName);
            ArgumentNullException.ThrowIfNull(attributeName);
            ArgumentNullException.ThrowIfNull(cache);

            string cacheKey = $"{entityName}-{attributeName}-Picklist";

            if (!cache.TryGetValue(cacheKey, out PicklistOptionSetMetadata metadata))
            {
                metadata = await dynamicsClient.GetPicklistOptionSetMetadataAsync(entityName, attributeName, cancellationToken);
                if (metadata is not null && metadata.Value is not null && metadata.Value.Count != 0)
                {
                    cache.Set(cacheKey, metadata, TimeSpan.FromHours(1));
                }
            }

            if (metadata is null)
            {
                metadata = new PicklistOptionSetMetadata();
            }

            if (metadata.Value is null)
            {
                metadata.Value = new List<OptionSetMetadata>();
            }

            return metadata;
        }

        public static async Task<MicrosoftDynamicsCRMssgCsrsfile> GetFileForSharePointDocumentLocation(this IDynamicsClient dynamicsClient, string id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            // get only the required fields for working with Sharepoint
            List<string> select = new List<string> { "ssg_csrsfileid" };
            List<string> expand = new List<string> { "ssg_csrsfile_SharePointDocumentLocations" };

            var entity = await dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(id, select: select, expand: expand, cancellationToken: cancellationToken);

            return entity;
        }

        /// <summary>
        /// Gets the list of active court locations. Returns only the
        /// <see cref="MicrosoftDynamicsCRMssgIjssbccourtlocation.SsgIjssbccourtlocationid"/> and
        /// <see cref="MicrosoftDynamicsCRMssgIjssbccourtlocation.SsgBccourtlocationname"/> properties.
        /// </summary>
        /// <param name="dynamicsClient"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MicrosoftDynamicsCRMssgIjssbccourtlocationCollection> GetCourtLocationsAsync(this IDynamicsClient dynamicsClient, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            var filter = $"statuscode eq {Active}";
            var select = new List<string> { "ssg_ijssbccourtlocationid", "ssg_bccourtlocationname" };

            var values = await dynamicsClient.Ssgijssbccourtlocations.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);

            return values;
        }

        /// <summary>
        /// Gets the list of active court levels. Returns only the
        /// <see cref="MicrosoftDynamicsCRMssgCsrsbccourtlevel.SsgCsrsbccourtlevelid"/> and
        /// <see cref="MicrosoftDynamicsCRMssgCsrsbccourtlevel.SsgCourtlevellabel"/> properties.
        /// </summary>
        /// <param name="dynamicsClient"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<MicrosoftDynamicsCRMssgCsrsbccourtlevelCollection> GetCourtLevelsAsync(this IDynamicsClient dynamicsClient, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            var filter = $"statuscode eq {Active}";
            var select = new List<string> { "ssg_csrsbccourtlevelid", "ssg_courtlevellabel" };

            var values = await dynamicsClient.Ssgcsrsbccourtlevels.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);

            return values;
        }

        public static async Task<string> GetSharepointDocumentLocationIdByRelatveUrl(this IDynamicsClient dynamicsClient, string relativeUrl, CancellationToken cancellationToken)
        {
            var filter = $"relativeurl eq '{relativeUrl}'";
            var select = new List<string> { "sharepointdocumentlocationid" };

            try { 
                var locations = await dynamicsClient.Sharepointdocumentlocations.GetAsync(top: 1, null, null, filter: filter, null, null, select: select, null, cancellationToken);

                if (locations.Value.Count == 0) return null;

                return locations.Value[0].Sharepointdocumentlocationid;
            } 
            catch (HttpOperationException exception) when (exception.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

        }

    }
}
