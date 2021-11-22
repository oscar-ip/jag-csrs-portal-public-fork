
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrmSvcUtilExtensions
{

    public class NamingService : INamingService
    {
        private MappingDefinition _mappings = MappingDefinition.Current;
        public INamingService _namingService;

        public NamingService(INamingService defaultService)
        {
            _namingService = defaultService;
        }

        public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            // entity level attributes
            var attributeMapping = _mappings.GetAttributeMapping(entityMetadata, attributeMetadata);
            if (attributeMapping != null)
            {
                return attributeMapping.Name;
            }

            // global attribute mappings like Status ans StateCode
            attributeMapping = _mappings.Attributes.Where(_ => _.LogicalName == attributeMetadata.LogicalName && !_.Skip).SingleOrDefault();
            if (attributeMapping != null)
            {
                return attributeMapping.Name;
            }

            string name = _namingService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
            name = _mappings.RemovePrefix(name);
            return name;
        }

        public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            var entityMapping = _mappings.GetEntityMapping(entityMetadata);
            if (entityMapping != null)
            {
                return entityMapping.Name;
            }

            string name = _namingService.GetNameForEntity(entityMetadata, services);
            name = _mappings.RemovePrefix(name);
            return name;
        }

        public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services)
        {
            return _namingService.GetNameForEntitySet(entityMetadata, services);
        }

        public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return _namingService.GetNameForMessagePair(messagePair, services);
        }

        public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services)
        {
            var name = _namingService.GetNameForOption(optionSetMetadata, optionMetadata, services);
            name = _mappings.RemovePrefix(name);
            return name;
        }

        public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            var name = _namingService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
            name = _mappings.RemovePrefix(name);
            return name;
        }

        public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services)
        {
            var name = _namingService.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
            name = _mappings.RemovePrefix(name);
            return name;
        }

        public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services)
        {
            var defaultName = _namingService.GetNameForRequestField(request, requestField, services);
            return defaultName;
        }

        public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services)
        {
            var defaultName = _namingService.GetNameForResponseField(response, responseField, services);
            return defaultName;
        }

        public string GetNameForServiceContext(IServiceProvider services)
        {
            var defaultName = _namingService.GetNameForServiceContext(services);
            return defaultName;
        }
    }
}
