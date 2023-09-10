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
            // Generation of demo file
            GenerateDemoFile();

            var pct = new AssemblyCatalog();
            pct.SetWriter(Console.Out, true, true);
            pct.UseOeTypes = true;
            pct.PublicOnly = true;
            //pct.WithInherited = true;
            //pct.GenerateDocumentationFromType(typeof(Class1));
            //pct.GenerateDocumentationFromAssembly(typeof(Class1).Assembly);
            pct.GenerateDocumentationFromAppDomain();
            var outputFile = Path.GetFullPath(@"PCTToolsApp-catalog.json");
            Console.WriteLine(outputFile);
            pct.ToJsonFile(outputFile);

            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }


        static void GenerateDemoFile()
        {
            var pct = new AssemblyCatalog();
            pct.UseOeTypes = true;
            pct.SetWriter(Console.Out, true, true);
            pct.GenerateDocumentationFromType(typeof(DemoAssemblyCatalog));
            var asmloc = typeof(DemoAssemblyCatalog).Assembly.Location;
            if (asmloc.EndsWith("PCTToolsApp\\bin\\Debug\\net461\\PCTToolsApp.exe"))
            {
                pct.ToJsonFile(Path.Combine(Path.GetDirectoryName(asmloc), "..\\..\\..\\..\\DemoAssemblyCatalog.json"));
                pct.ToJsonFileFull(Path.Combine(Path.GetDirectoryName(asmloc), "..\\..\\..\\..\\DemoAssemblyCatalog-full.json"));
            }
        }
    }
}