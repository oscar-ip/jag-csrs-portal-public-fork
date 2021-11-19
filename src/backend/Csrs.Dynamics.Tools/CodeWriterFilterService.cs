
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Csrs.Dynamics.Tools
{
    public class CodeWriterFilterService : ICodeWriterFilterService
    {
        private HashSet<string> _entityTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private ICodeWriterFilterService DefaultService { get; set; }

        public CodeWriterFilterService(ICodeWriterFilterService defaultService)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}

            DefaultService = defaultService;

            _entityTypes.Add("ssg_csrsparty");
            _entityTypes.Add("ssg_csrsfile");
            _entityTypes.Add("ssg_feedbackform");
            _entityTypes.Add("ssg_message");
        }

        bool ICodeWriterFilterService.GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateAttribute(attributeMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if (GenerateEntity(entityMetadata))
            {
                return this.DefaultService.GenerateEntity(entityMetadata, services);
            }
            return false;

        }

        private bool GenerateEntity(EntityMetadata entityMetadata)
        {
            return _entityTypes.Contains(entityMetadata.LogicalName) || entityMetadata.LogicalName.StartsWith("ssg_");
        }

        bool ICodeWriterFilterService.GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateOption(optionMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateOptionSet(optionSetMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata,
        IServiceProvider services)
        {
            return this.DefaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateServiceContext(IServiceProvider services)
        {
            return this.DefaultService.GenerateServiceContext(services);
        }

        public bool GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public bool GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public bool GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public bool GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public bool GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata, IServiceProvider services)
        {
            throw new NotImplementedException();
        }

        public bool GenerateServiceContext(IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}
