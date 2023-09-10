using System.ComponentModel;

namespace PCTTools.Model
{
    public class FieldDocumentation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        [DefaultValue(false)]
        public bool IsStatic { get; set; }
        [DefaultValue(true)]
        public bool IsPublic { get; set; }
    }
}