using System.ComponentModel;

namespace PCTTools.Model
{
    public class ParameterDocumentation
    {
        [DefaultValue(false)]
        public bool IsOut { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}