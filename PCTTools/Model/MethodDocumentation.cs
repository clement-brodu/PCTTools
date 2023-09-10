using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace PCTTools.Model
{
    public class MethodDocumentation
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string FormattedName { get; set; }
        public string ReturnType { get; set; }
        public List<ParameterDocumentation> Parameters { get; set; } = new List<ParameterDocumentation>();
        public ObsoleteDocumentation Obsolete { get; set; }
        [DefaultValue(false)]
        public bool IsStatic { get; set; }
        [DefaultValue(true)]
        public bool IsPublic { get; set; }
    }
}