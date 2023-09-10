using System.Collections.Generic;
using System.ComponentModel;

namespace PCTTools.Model
{
    public class EventDocumentation
    {
        public string Name { get; set; }
        public string EventType { get; set; }
        public string DelegateReturnType { get; set; }
        public List<ParameterDocumentation> DelegateParameters { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        [DefaultValue(false)]
        public bool IsStatic { get; set; }
        [DefaultValue(true)]
        public bool IsPublic { get; set; }
    }
}