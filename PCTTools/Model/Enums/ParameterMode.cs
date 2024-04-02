using System.Runtime.Serialization;

namespace PCTTools.Model.Enums
{
    public enum ParameterMode
    {
        [EnumMember(Value = "I")]
        INPUT,
        [EnumMember(Value = "IO")]
        INPUTOUTPUT,
        [EnumMember(Value = "O")]
        OUTPUT,
    }
}
