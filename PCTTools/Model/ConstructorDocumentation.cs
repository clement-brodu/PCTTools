using System.Collections.Generic;

namespace PCTTools.Model
{
    public class ConstructorDocumentation
    {
        public string Name { get; set; }
        public List<ParameterDocumentation> Parameters { get; set; } = new List<ParameterDocumentation>();
        public ObsoleteDocumentation Obsolete { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
    }
}