using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Misc
{
    public class StaticDemo
    {
        static StaticDemo()
        {

        }

        public StaticDemo(string name)
        {

        }

        public static string PropStaticString { get; set; }
        public string PropString { get; set; }

        public static event EventHandler EventStatic;
        public event EventHandler Event;

        public static string FieldStaticString;
        public string FieldString;

        public static string GetStaticString()
        {
            return FieldStaticString;
        }
        public string GetString()
        {
            return FieldString;
        }


    }
}
