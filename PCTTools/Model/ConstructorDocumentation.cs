using System.Collections.Generic;
using System.ComponentModel;

namespace PCTTools.Model
{
    public class ConstructorDocumentation
    {
        public string Name { get; set; }
        public List<ParameterDocumentation> Parameters { get; set; } = new List<ParameterDocumentation>();
        public ObsoleteDocumentation Obsolete { get; set; }
        [DefaultValue(false)]
        public bool IsStatic { get; set; }
        [DefaultValue(true)]
        public bool IsPublic { get; set; }
    }
}