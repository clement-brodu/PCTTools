using PCTTools;
using PCTTools.Sample.SAssemblyCatalog.OeTypes;
using System;
using System.Diagnostics;

namespace PCTToolsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var pct = new AssemblyCatalog();
            pct.UseOeTypes = true;
            pct.PublicOnly = true;
            //pct.WithInherited = true;
            //pct.AddTypeFromAssembly(typeof(Class6));
            pct.GenerateDocumentationFromAssembly(typeof(Class1).Assembly);
            //pct.GetDocumentationFromAppDomain();
            pct.ToJsonFile(@"D:\PCTToolsApp.json");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}