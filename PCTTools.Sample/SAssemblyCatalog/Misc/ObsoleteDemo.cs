using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Misc
{
    [Obsolete("Use ClassDemo instead")]
    public class ObsoleteClassDemo
    {
        public string PropString { get; set; }

    }

    public class OtherClassDemo
    {
        [Obsolete()]
        public string PropString { get; set; }

        [Obsolete("Do Not Use")]
        public string PropString2 { get; set; }

        [Obsolete("Do Not Use", true)]
        public string PropString3 { get; set; }

        [Obsolete("Do Not Use")]
        public event EventHandler PropChanged;

        [Obsolete("Do Not Use")]
        public string FieldString;

        [Obsolete("Do Not Use")]
        public void GetString()
        {

        }

    }

    public enum EnumWithObsoleteDemo
    {
        A = 1,
        B = 2,
        [Obsolete("Use B instead")]
        C = 3
    }
}