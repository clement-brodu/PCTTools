using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Misc
{
    public abstract class AbstractDemo
    {
        public string PropString { get; set; }


    }




    public class ClassDemo
    {
        public string PropString { get; set; }

        public class NestedDemo
        {
            public string PropString { get; set; }
        }
    }

    public sealed class ClassSealedDemo
    {
        public string PropString { get; set; }
    }

    public class ClassGenericDemo<T>
    {
        public string PropString { get; set; }
    }

    public interface InterfaceDemo
    {
        string PropString { get; set; }
    }
    public interface InterfaceGenericDemo<T>
    {
        string PropString { get; set; }
    }

    public enum EnumDemo
    {
        A = 1,
        B = 2
    }
}
