using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.CountMembers
{
    public class Class2
    {
        public Class2()
        {

        }

        public Class2(string propstring)
        {
            PropString = propstring;
        }

        public string PropString { get; set; }
        internal string InternalPropString { get; set; }
        protected string ProtectedPropString { get; set; }
        private string PrivatePropString { get; set; }

        public string VarString;
        internal string InternalVarString;
        protected string ProtectedVarString;
        private string PrivateVarString;

        public void GetMethodPublic()
        {
        }
        protected void GetMethodProtected()
        {
        }
        internal void GetMethodInternal()
        {
        }
        private void GetMethodPrivate()
        {
        }

        public event EventHandler EventPublic;
        protected event EventHandler EventProtected;
        internal event EventHandler EventInternal;
        private event EventHandler EventPrivate;

    }

    public class Class2B : Class2
    {
        public int PropInt { get; set; }
        internal int InternalPropInt { get; set; }
        protected int ProtectedPropInt { get; set; }
        private int PrivatePropInt { get; set; }

        public int VarInt;
        internal int InternalVarInt;
        protected int ProtectedVarInt;
        private int PrivateVarInt;

        public void GetMethodPublicB()
        {
        }
        protected void GetMethodProtectedB()
        {
        }
        internal void GetMethodInternalB()
        {
        }
        private void GetMethodPrivateB()
        {
        }

        public event EventHandler EventPublicB;
        protected event EventHandler EventProtectedB;
        internal event EventHandler EventInternalB;
        private event EventHandler EventPrivateB;
    }

    public class Class2C : Class2B
    {
        public Class2C()
        {

        }

        public Class2C(string propstring)
        {
            PropString = propstring;
        }

        protected Class2C(int propint)
        {

        }

        protected Class2C(bool propbool)
        {

        }

        public bool PropBool { get; set; }
        internal bool InternalPropBool { get; set; }
        protected bool ProtectedPropBool { get; set; }
        private bool PrivatePropBool { get; set; }

        public bool VarBool;
        internal bool InternalVarBool;
        protected bool ProtectedVarBool;
        private bool PrivateVarBool;
    }

    public enum Enum2
    {
        A,
        B,
        C
    }


}
