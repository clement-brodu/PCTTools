using System.ComponentModel;

namespace PCTTools.Model
{
    public class ObsoleteDocumentation
    {
        public string Message { get; set; }
        [DefaultValue(false)]
        public bool IsError { get; set; }
    }
}