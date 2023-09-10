using System.ComponentModel;

namespace PCTTools.Model
{
    public class PropertyDocumentation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        [DefaultValue(true)]
        public bool CanRead { get; set; }
        [DefaultValue(true)]
        public bool CanWrite { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        [DefaultValue(false)]
        public bool IsStatic { get; set; }
        [DefaultValue(true)]
        public bool IsPublic { get; set; }
    }
}