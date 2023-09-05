using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Nested
{
    class NestedPrivate
    {
        public string PropString { get; set; }
        internal string InternalPropString { get; set; }
        protected string ProtectedPropString { get; set; }
        private string PrivatePropString { get; set; }

        public string VarString;
        internal string InternalVarString;
        protected string ProtectedVarString;
        private string PrivateVarString;

        private class NestedPrivateSubPrivate
        {
            public int PropInt { get; set; }
            internal int InternalPropInt { get; set; }
            protected int ProtectedPropInt { get; set; }
            private int PrivatePropInt { get; set; }

            public int VarInt;
            internal int InternalVarInt;
            protected int ProtectedVarInt;
            private int PrivateVarInt;
        }
        internal class NestedPrivateSubInternal
        {
            public int PropInt { get; set; }
            internal int InternalPropInt { get; set; }
            protected int ProtectedPropInt { get; set; }
            private int PrivatePropInt { get; set; }

            public int VarInt;
            internal int InternalVarInt;
            protected int ProtectedVarInt;
            private int PrivateVarInt;
        }

        protected class NestedPrivateSubProtected
        {
            public int PropInt { get; set; }
            internal int InternalPropInt { get; set; }
            protected int ProtectedPropInt { get; set; }
            private int PrivatePropInt { get; set; }

            public int VarInt;
            internal int InternalVarInt;
            protected int ProtectedVarInt;
            private int PrivateVarInt;
        }

        public class NestedPrivateSubPublic
        {
            public int PropInt { get; set; }
            internal int InternalPropInt { get; set; }
            protected int ProtectedPropInt { get; set; }
            private int PrivatePropInt { get; set; }

            public int VarInt;
            internal int InternalVarInt;
            protected int ProtectedVarInt;
            private int PrivateVarInt;
        }
    }
}
