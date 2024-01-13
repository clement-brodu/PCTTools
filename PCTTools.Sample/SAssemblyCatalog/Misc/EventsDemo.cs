using System;
using System.Collections.Generic;
using System.Text;

namespace PCTTools.Sample.SAssemblyCatalog.Misc
{
    public class EventsDemo
    {
        public delegate string DemoHandler(object sender, int e);

        public event EventHandler Event1;
        public event EventHandler<AssemblyLoadEventArgs> Event2;
        public event DemoHandler Event3;
    }
}
