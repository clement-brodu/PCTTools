using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCTToolsApp
{
    public class DemoAssemblyCatalog : StreamWriter
    {
        public DemoAssemblyCatalog(string path, bool append) : base(path, append)
        {
        }

        public string MyField;
        public static string MyStaticField;

        public string MyProperty { get; set; }

        protected string MyProtectedProperty { get; set; }

        [Obsolete("Use MyProperty instead")]
        public string MyObsoletProperty { get; set; }

        public List<int> MyList { get; set; }

        public static string StaticProperty { get; set; }

        [Obsolete("Use other GetMyProperty instead")]
        public string GetMyProperty()
        {
            return MyProperty;
        }

        public string GetMyProperty(string name, string othername)
        {
            return MyProperty;
        }

        public string DoSomething(List<int> list, Dictionary<int, string> dico)
        {
            return string.Empty;
        }

        public static DateTime GetMyDate(bool isToday, int currentYear)
        {
            return DateTime.Now;
        }

        public static bool TryGetMyDate(bool isToday, out DateTime dateTime)
        {
            dateTime = DateTime.Now;
            return true;
        }

        public delegate void DemoDelegate(string s, int i);

        public event DemoDelegate CustomEvent;
        public event CancelEventHandler Validating;
        public static event CancelEventHandler StaticValidating;
    }
}
