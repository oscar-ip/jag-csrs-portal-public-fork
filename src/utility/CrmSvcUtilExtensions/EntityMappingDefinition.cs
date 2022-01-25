namespace CrmSvcUtilExtensions
{
    public class EntityMappingDefinition : LogicalNameMappingDefinition
    {
        public EntityMappingDefinition()
        {
            Attributes = new AttributeMappingDefinitionCollection();
        }

        public AttributeMappingDefinitionCollection Attributes { get; set; }
    }
}
