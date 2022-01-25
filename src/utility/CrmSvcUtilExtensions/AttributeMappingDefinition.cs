using System.Collections.Generic;
using System.Linq;

namespace CrmSvcUtilExtensions
{
    public class AttributeMappingDefinition : LogicalNameMappingDefinition
    {
    }

    public class AttributeMappingDefinitionCollection : List<AttributeMappingDefinition>
    {
        public void Skip(string logicalName)
        {
            if (!this.Any(_ => _.LogicalName == logicalName))
            {
                Add(new AttributeMappingDefinition { LogicalName = logicalName, Skip = true });
            }
        }

        public void Skip(params string[] logicalNames)
        {
            foreach (var logicalName in logicalNames)
            {
                Add(new AttributeMappingDefinition { LogicalName = logicalName, Skip = true });
            }
        }
    }
}
