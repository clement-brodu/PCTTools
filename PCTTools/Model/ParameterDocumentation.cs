using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCTTools.Model.Enums;
using System.ComponentModel;

namespace PCTTools.Model
{
    public class ParameterDocumentation
    {
        [DefaultValue(ParameterMode.INPUT)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParameterMode Mode { get; set; } = ParameterMode.INPUT;
        public string Name { get; set; }
        public string Type { get; set; }
    }
}