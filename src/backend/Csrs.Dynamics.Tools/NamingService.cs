
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;

namespace Csrs.Tools
{
    public class NamingService : INamingService
    {
        private Dictionary<string, string> _nameForEntities = new Dictionary<string, string>();
        public NamingService(INamingService defaultService)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}

            //_nameForEntities.Add("ssg_csrsparty", "Party");
            //_nameForEntities.Add("ssg_csrsfile", "File");

            DefaultService = defaultService;
        }

        public INamingService DefaultService { get; }

        public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return DefaultService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
        }

        public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            var defaultName = DefaultService.GetNameForEntity(entityMetadata, services);
            if (_nameForEntities.TryGetValue(defaultName, out string entityName))
            {
                return entityName;
            }

            return defaultName;
        }

        public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services)
        {
            return DefaultService.GetNameForEntitySet(entityMetadata, services);
        }

        public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return DefaultService.GetNameForMessagePair(messagePair, services);
        }

        public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services)
        {
            return DefaultService.GetNameForOption(optionSetMetadata, optionMetadata, services);
        }

        public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            return DefaultService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
        }

        public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services)
        {
            return DefaultService.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
        }

        public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services)
        {
            return DefaultService.GetNameForRequestField(request, requestField, services);
        }

        public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services)
        {
            return DefaultService.GetNameForResponseField(response, responseField, services);
        }

        public string GetNameForServiceContext(IServiceProvider services)
        {
            return DefaultService.GetNameForServiceContext(services);
        }
    }
}
