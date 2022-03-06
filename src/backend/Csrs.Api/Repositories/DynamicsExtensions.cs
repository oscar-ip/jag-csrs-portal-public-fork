using Csrs.Api.Models;
using Csrs.Interfaces.Dynamics.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Rest;
using System.Net;

namespace Csrs.Interfaces.Dynamics
{
    public static class DynamicsExtensions
    {
        private const string ActiveStateCode = "statecode eq 0";

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

            bceid = GuidGuard(bceid);

            List<string> orderby = new List<string> { "ssg_bceid_last_update desc" };
            string filter = $"ssg_bceid_guid eq '{bceid}' and {ActiveStateCode}";
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
                var select = new List<string> { "ssg_csrsfileid", "ssg_filenumber" };

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

            bceid = GuidGuard(bceid);

            List<string> select = new List<string> { "ssg_csrspartyid" };
            List<string> orderby = new List<string> { "ssg_bceid_last_update desc" };
            string filter = $"ssg_bceid_guid eq '{bceid}' and {ActiveStateCode}"; 
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

            partyId = GuidGuard(partyId);

            var filter = $"(_ssg_recipient_value eq {partyId} or _ssg_payor_value eq {partyId}) and {ActiveStateCode}";
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

                var summary = new FileSummary
                {
                    FileId = Guid.Parse(file.SsgCsrsfileid),
                    FileNumber = file.SsgFilenumber,
                    UsersRole = role,
                    Status = FileStatus.Active
                };

                results.Add(summary);
            }

            return results;
        }

        public static async Task<MicrosoftDynamicsCRMssgCsrsfileCollection> GetFilesByParty(this IDynamicsClient dynamicsClient, string partyId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            partyId = GuidGuard(partyId);

            string filter = $"_ssg_payor_value eq {partyId} or _ssg_recipient_value eq {partyId}";
            List<string> select = new List<string> { "ssg_csrsfileid", "ssg_filenumber" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            var files = await dynamicsClient.Ssgcsrsfiles.GetAsync(select: select, orderby: orderby, filter: filter, expand: null, cancellationToken: cancellationToken);
            return files;
        }

        //This method should be promptly removed when fileid is available in AccountSummary and provided by frontend.
        public static async Task<MicrosoftDynamicsCRMssgCsrsfile> GetFileByFileId(this IDynamicsClient dynamicsClient, string fileId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            fileId = GuidGuard(fileId);

            string filter = $"ssg_csrsfileid eq {fileId}";
            List<string> select = new List<string> { "ssg_csrsfileid", "ssg_filenumber", "_ownerid_value", "_owninguser_value", "_owningteam_value" };
           
            return await dynamicsClient.Ssgcsrsfiles.GetByKeyAsync(fileId, select: select, expand: null, cancellationToken: cancellationToken);
        }
        public static async Task<MicrosoftDynamicsCRMssgCsrscommunicationmessageCollection> GetCommunicationMessagesByFile(this IDynamicsClient dynamicsClient, string fileId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            fileId = GuidGuard(fileId);

            string filter = $"_ssg_csrsfile_value eq {fileId} and statuscode eq 867670001";
            List<string> select = new List<string> { "_ssg_csrsfile_value", "ssg_sentreceiveddate", "ssg_csrsmessage", 
                                                    "ssg_csrsmessageattachment", "ssg_csrsmessageread", "ssg_csrsmessagesubject", 
                                                    "statuscode", "_ssg_toparty_value"};
            List<string> expand = new List<string> { "ssg_csrsFile($select=ssg_filenumber)" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            var messages = await dynamicsClient.Ssgcsrscommunicationmessages.GetAsync(select: select, expand: expand, orderby: orderby, filter: filter, cancellationToken: cancellationToken);
            return messages;
        }

        /// <summary>
        /// Returns parties with matching ssg_bceid_userid for input csrs account.
        /// </summary>
        /// <param name="dynamicsClient"></param>
        /// <param name="bceid"></param>
        /// <param name="referenceNumber"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>parties</returns>
        public static async Task<MicrosoftDynamicsCRMssgCsrspartyCollection> GetPartyByRefenceNumberAsync(this IDynamicsClient dynamicsClient, string referenceNumber, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(dynamicsClient);

            referenceNumber = Escape(referenceNumber);

            string filter = $"ssg_referencenumber eq '{referenceNumber}'";
            var  parties = await dynamicsClient.Ssgcsrsparties.GetAsync(filter: filter, cancellationToken: cancellationToken);
            return parties;
        }

        public static async Task<MicrosoftDynamicsCRMssgCsrsfileCollection> GetFileByPartyIdAndFileNumber(this IDynamicsClient dynamicsClient, string partyId, string fileNumber, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(dynamicsClient);

            partyId = GuidGuard(partyId);
            fileNumber = Escape(fileNumber);

            string filter = $"(_ssg_payor_value eq {partyId} or _ssg_recipient_value eq {partyId}) and ssg_filenumber eq '{fileNumber}'";
            List<string> select = new List<string> { "ssg_csrsfileid", "_ssg_recipient_value", "_ssg_payor_value" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            MicrosoftDynamicsCRMssgCsrsfileCollection files;
            try
            {
                files = await dynamicsClient.Ssgcsrsfiles.GetAsync(select: select, orderby: orderby, filter: filter, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }

            return files;
        }

        public static async Task<PartyRole> GetFileByPartyIdAndFileId(this IDynamicsClient dynamicsClient, string partyId, string fileId, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(dynamicsClient);

            partyId = GuidGuard(partyId);
            fileId = GuidGuard(fileId);

            string filter = $"(_ssg_payor_value eq {partyId} or _ssg_recipient_value eq {partyId}) and ssg_csrsfileid eq {fileId}";
            List<string> select = new List<string> { "ssg_csrsfileid", "_ssg_recipient_value", "_ssg_payor_value" };
            List<string> orderby = new List<string> { "modifiedon desc" };

            PartyRole role = PartyRole.Unknown;
            try
            {
                MicrosoftDynamicsCRMssgCsrsfileCollection files = await dynamicsClient.Ssgcsrsfiles.GetAsync(select: select, orderby: orderby, filter: filter, cancellationToken: cancellationToken);
                if (files is not null && files.Value.Count > 0)
                {
                    if (files.Value[0]._ssgRecipientValue == partyId)
                    {
                        role = PartyRole.Recipient;
                    }
                    else if (files.Value[0]._ssgPayorValue == partyId)
                    {
                        role = PartyRole.Payor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return role;
        }
        


public static async Task<MicrosoftDynamicsCRMssgCsrscommunicationmessage> GetCommunicationMessagesByPartyAndIdAsync(this IDynamicsClient dynamicsClient, string partyId, string messageId, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(dynamicsClient);

            partyId = GuidGuard(partyId);
            messageId = GuidGuard(messageId);

            var filter = $"(_ssg_toparty_value eq {partyId} or _ssg_fromparty_value eq {partyId}) and ssg_csrscommunicationmessageid eq {messageId}";
            var select = new List<string> { "ssg_csrscommunicationmessageid" , "ssg_csrsmessagesubject" };

            var messages = await dynamicsClient.Ssgcsrscommunicationmessages.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);

            if (messages.Value.Count == 0) return null;

            return messages.Value[0];

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

            id = GuidGuard(id);

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

            var filter = $"{ActiveStateCode}";
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

            var filter = $"{ActiveStateCode}";
            var select = new List<string> { "ssg_csrsbccourtlevelid", "ssg_courtlevellabel" };

            var values = await dynamicsClient.Ssgcsrsbccourtlevels.GetAsync(filter: filter, select: select, cancellationToken: cancellationToken);

            return values;
        }

        public static async Task<string?> GetSharepointDocumentLocationIdByRelatveUrlAsync(this IDynamicsClient dynamicsClient, string relativeUrl, CancellationToken cancellationToken)
        {

            relativeUrl = Escape(relativeUrl);
            
            var filter = $"relativeurl eq '{relativeUrl}'";
            var select = new List<string> { "sharepointdocumentlocationid" };

            var locations = await dynamicsClient.Sharepointdocumentlocations.GetAsync(top: 1, null, null, filter: filter, null, null, select: select, null, cancellationToken);

            if (locations.Value.Count == 0)
            {
                return null;
            }

            return locations.Value[0].Sharepointdocumentlocationid;
        }


        /// <summary>
        /// Validates the incoming entity is a valid <see cref="Guid"/> value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns the <see cref="Guid"/> formated with dashes, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.</returns>
        /// <exception cref="InvalidIdException"><paramref name="value"/> is null, empty or not a valid <see cref="Guid"/>.</exception>
        private static string GuidGuard(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new InvalidIdException("No id specified", value);
            }

            if (Guid.TryParse(value, out Guid guid))
            {
                return guid.ToString("d");
            }

            throw new InvalidIdException("Invalid id specified", value);
        }

        /// <summary>
        /// Escapes a string value used in a filter expression.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Escape(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            value = value.Replace("'", "''");
            return value;
        }


    }
}
