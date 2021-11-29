using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace CrmSvcUtilExtensions
{
    public class MappingDefinition
    {
        static MappingDefinition()
        {
            Current = LoadMappingDefinitions();
        }

        public MappingDefinition()
        {
            Entities = new List<EntityMappingDefinition>();
            Attributes = new AttributeMappingDefinitionCollection();
        }

        public string Prefix { get; set; }

        public List<EntityMappingDefinition> Entities { get; set; }
        public AttributeMappingDefinitionCollection Attributes { get; set; }

        public static MappingDefinition Current { get; private set; }

        public string RemovePrefix(string name)
        {
            if (!string.IsNullOrEmpty(Prefix))
            {
                if (name.StartsWith(Prefix))
                {
                    return name.Substring(Prefix.Length);
                }
            }

            return name;
        }

        public bool Generate(EntityMetadata entityMetadata)
        {
            EntityMappingDefinition entityMapping = GetEntityMapping(entityMetadata);

            if (entityMapping != null)
            {
                return !entityMapping.Skip;
            }

            return false; // do not generate
        }

        public EntityMappingDefinition GetEntityMapping(EntityMetadata entityMetadata)
        {
            return Entities.SingleOrDefault(_ => _.LogicalName == entityMetadata.LogicalName);
        }

        public AttributeMappingDefinition GetAttributeMapping(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata)
        {
            var entityMapping = GetEntityMapping(entityMetadata);
            if (entityMapping != null)
            {
                return entityMapping.Attributes.SingleOrDefault(_ => _.LogicalName == attributeMetadata.LogicalName);
            }

            // didnt find a specific entity attribute mapping, is it a global mapping
            return Attributes.SingleOrDefault(_ => _.LogicalName == attributeMetadata.LogicalName);
        }

        private static MappingDefinition LoadMappingDefinitions()
        {
            // TODO: load from json
            MappingDefinition mappings = new MappingDefinition();
            mappings.Prefix = "ssg_";

            mappings.Attributes.Add(new AttributeMappingDefinition { LogicalName = "statecode", Name = "StateCode" });
            mappings.Attributes.Add(new AttributeMappingDefinition { LogicalName = "statuscode", Name = "StatusCode" });

            // skip any of these fields if we dont need them
            mappings.Attributes.Skip("createdby", "createdbyname", "createdon", "createdonbehalfby", "createdonbehalfbyname","createdbyyominame","createdonbehalfbyyominame", "overriddencreatedon");
            mappings.Attributes.Skip("modifiedby","modifiedbyname", "modifiedon", "modifiedonbehalfby", "modifiedbyyominame","modifiedonbehalfbyyominame");
            mappings.Attributes.Skip("ownerid", "owneridname", "owneridtype", "owneridyominame", "owningbusinessunit", "owningteam", "owninguser");
            mappings.Attributes.Skip("importsequencenumber", "versionnumber", "utcconversiontimezonecode", "timezoneruleversionnumber");

            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrschild",
                Name = "SSG_CsrsChild",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrschildid", Name =  "ChildId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_childisadependent", Name =  "ChildIsADependent" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_firstname", Name =  "FirstName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_middlename", Name =  "MiddleName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_lastname", Name =  "LastName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_dateofbirth", Name =  "DateOfBirth" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_childsfather", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_childsmother", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_fullname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_triggerrecalcrelationship", Skip = true },
                }
            });

            // add the entity mappings
            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrsparty",
                Name = "SSG_CsrsParty",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "ssg_areapostalcode", Name = "PostalCode" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_bceid_displayname", Name = "BCeIDDisplayName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_bceid_guid", Name = "BCeIDGuid" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_bceid_last_update", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_bceid_userid", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_cellphone", Name = "CellPhone" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_city", Name = "City" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsoptoutedocuments", Name = "OptOutElectronicDocuments" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsoptoutedocumentsname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrspartyid", Name = "PartyId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_dateofbirth", Name = "DateOfBirth" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_email", Name = "Email" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_firstname", Name = "FirstName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_fullname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_gender", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_gendername", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_homephone", Name = "HomePhone" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_identity", Name = "Identity" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_identityname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_identityotherdetails", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeassistance", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeassistancename", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_lastname", Name = "LastName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_middlename", Name = "MiddleName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_partygender", Name = "Gender" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_partygendername", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_portalaccess", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_portalaccessname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_preferredcontactmethod", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_preferredcontactmethodname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_preferredname", Name = "PreferredName" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_provinceterritory", Name = "Province" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_provinceterritoryname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_referral", Name = "Referral" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_referralname", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_stagingfilenumber", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_stagingid", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_street1", Name = "AddressStreet1" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_street2", Name = "AddressStreet2" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_workphone", Name = "WorkPhone" },
                    new AttributeMappingDefinition() { LogicalName = "statecode", Name = "StateCode" },
                    new AttributeMappingDefinition() { LogicalName = "statecodename", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "statuscode", Name = "StatusCode" },
                    new AttributeMappingDefinition() { LogicalName = "statuscodename", Skip = true },
                }
            });

            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrsfile",
                Name = "SSG_CsrsFile",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsfileid", Name =  "CsrsFileId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_filenumber", Name =  "FileNumber" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_autonumber", Name =  "AutoNumber" },
                    // skipped
                    new AttributeMappingDefinition() { LogicalName = "ssg_recipientschildsupportonorder", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_recipientschildsupportonorder_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_recipientsincomeonorder", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_recipientsincomeonorder_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeonorder", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeonorder_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear1", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear1_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear2", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear2_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear3", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_incomeyear3_base", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_offsetchildsupportamountonorder", Skip =  true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_offsetchildsupportamountonorder_base", Skip =  true },
                }
            });

            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrsfeedback",
                Name = "SSG_CsrsFeedback",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsfeedbackid", Name =  "FeedbackId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsfeedbackmessage", Name =  "Message" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsfeedbacksubject", Name =  "Subject" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsparty", Name =  "Party" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_name", Name =  "Name" },
                }
            });

            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrsportalmessage",
                Name = "SSG_CsrsPortalMessage",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsfile", Name =  "File" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsmessageattachment", Name =  "HasAttachment" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsmessagedate", Name =  "Date" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsmessagesubject", Name =  "Subject" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsportalmessageid", Name =  "MessageId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrsmessageread", Name =  "Read" },                    
                    new AttributeMappingDefinition() { LogicalName = "ssg_name", Name =  "Name" },
                }
            });

            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_csrsbccourtlevel",
                Name = "SSG_CsrsBCCourtLevel",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    new AttributeMappingDefinition() { LogicalName = "csrsbccourtlevelId", Name =  "BCCourtLevelId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_courtlevellabel", Name =  "CourtLevel" },
                }
            });


            mappings.Entities.Add(new EntityMappingDefinition
            {
                LogicalName = "ssg_ijssbccourtlocation",
                Name = "SSG_IJSSBCCourtlocation",
                Attributes = new AttributeMappingDefinitionCollection
                {
                    // ijssbccourtlocationId
                    new AttributeMappingDefinition() { LogicalName = "ssg_ijssbccourtlocationid", Name =  "CourtLocationId" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_bccourtlocationname", Name =  "Name" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_street1", Name =  "AddressStreet1" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_street2", Name =  "AddressStreet2" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_city", Name =  "City" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_provinceterritory", Name =  "Province" },
                    new AttributeMappingDefinition() { LogicalName = "ssg_courtofficeemail", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_courtofficefax", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_courtofficephone", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_courtofficephoneextension", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_csrscourtlevelfilter", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_datamigrationcount", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_filenumber", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_postalcode", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_stagingid", Skip = true },
                    new AttributeMappingDefinition() { LogicalName = "ssg_accessdbid", Skip = true },
                }
            });

            return mappings;
        }

    }
}
