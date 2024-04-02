using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PCTTools.Extensions;
using PCTTools.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PCTTools
{
    public class ListingInterfaces
    {
        /// <summary>
        /// Writer to log things
        /// </summary>
        private TextWriter writer;

        /// <summary>
        /// True if an error append during scan
        /// </summary>
        public bool HasError => ScanExceptions.Any();

        /// <summary>
        /// List of all scan exceptions 
        /// </summary>
        public List<Exception> ScanExceptions { get; set; } = new List<Exception>();



        /// <summary>
        /// List of Types Documentation
        /// </summary>
        public List<InterfacesList> InterfacesListResult { get; } = new List<InterfacesList>();

        /// <summary>
        /// List of Types Documentation
        /// </summary>
        public Dictionary<string, List<string>> UsageResult { get; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// Define a Writer to log things
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="logAssembly"></param>
        /// <param name="logTypes"></param>
        public void SetWriter(TextWriter writer, bool logAssembly, bool logTypes)
        {
            this.writer = writer;
        }
        /// <summary>
        /// Define a Writer to log things
        /// </summary>
        /// <param name="writer"></param>
        public void SetWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Generate documentation from an assembly
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        public void GenerateDocumentationFromAssembly(Assembly assembly)
        {

            writer?.WriteLine("{0} - Scan Assembly {1}", DateTime.Now.ToString("s"), assembly.GetName().Name);

            foreach (Type type in assembly.GetTypes())
            {
                GenerateInterfacesListFromType(type);
            }

            //foreach (var classe in InterfacesListResult)
            //{
            //    foreach (var inter in classe.Interfaces)
            //    {
            //        if (!UsageResult.ContainsKey(inter.GetFormattedSimple()))
            //        {
            //            UsageResult.Add(inter.GetFormattedSimple(), new List<string>());
            //        }

            //        var list = UsageResult[inter.GetFormattedSimple()];
            //        if (!list.Contains(classe.TypeClasse.GetFormattedSimple()))
            //        {
            //            UsageResult[inter.GetFormattedSimple()].Add(classe.TypeClasse.GetFormattedSimple());
            //        }
            //    }
            //}


            // Générer les unions
            Console.WriteLine($"Unions");

            Dictionary<string, List<string>> UsageResult2 = new Dictionary<string, List<string>>();
            foreach (var usage in UsageResult)
            {
                if (usage.Key.StartsWith("IPnv", StringComparison.OrdinalIgnoreCase) /*|| usage.Key.StartsWith("INotify", StringComparison.OrdinalIgnoreCase)*/)
                    UsageResult2.Add(usage.Key, usage.Value);
            }



            Unions = GenerateUnions(UsageResult2);

            // Afficher le résultat
            foreach (var union in Unions)
            {
                Console.WriteLine($"{{ \"composants\": [{string.Join(", ", union.Composants.Select(c => $"\"{c}\""))}], \"obj\": [{string.Join(", ", union.Obj.Select(o => $"\"{o}\""))}] }}");
            }
        }
        public List<Union> Unions { get; set; }

        static List<Union> GenerateUnions(Dictionary<string, List<string>> components)
        {
            var unions = new List<Union>();

            for (int r = 1; r <= components.Count; r++)
            {
                foreach (var combination in GetCombinations(components.Keys.ToList(), r))
                {
                    Console.WriteLine(string.Join(", ", combination));
                    var objects = new HashSet<string>();
                    var componentNames = combination.ToList();

                    foreach (var compName in componentNames)
                    {
                        objects.UnionWith(components[compName]);
                    }

                    unions.Add(new Union { Composants = componentNames, Obj = objects.OrderBy(o => o).ToList() });
                }
            }

            return unions;
        }

        static IEnumerable<IEnumerable<string>> GetCombinations(List<string> keys, int r)
        {
            if (r == 0)
            {
                yield return Enumerable.Empty<string>();
            }
            else
            {
                var keysCount = keys.Count;
                for (int i = 0; i < keysCount; i++)
                {
                    var key = keys[i];
                    var subCombinations = GetCombinations(keys.Skip(i + 1).ToList(), r - 1);
                    foreach (var subCombination in subCombinations)
                    {
                        yield return new[] { key }.Concat(subCombination);
                    }
                }
            }
        }

        public class Union
        {
            public List<string> Composants { get; set; }
            public List<string> Obj { get; set; }
        }

        static IEnumerable<Dictionary<string, List<string>>> GetCombinations(Dictionary<string, List<string>> components, List<string> remainingKeys, int r)
        {
            if (r == 1)
            {
                foreach (var key in remainingKeys)
                {
                    yield return new Dictionary<string, List<string>> { { key, components[key] } };
                }
            }
            else
            {
                for (int i = 0; i < remainingKeys.Count; i++)
                {
                    var firstKey = remainingKeys[i];
                    var nextKeys = remainingKeys.Skip(i + 1).ToList();
                    var firstValue = components[firstKey];

                    foreach (var combination in GetCombinations(components, nextKeys, r - 1))
                    {
                        combination.Add(firstKey, firstValue);
                        yield return combination;
                    }
                }
            }
        }



        private void GenerateInterfacesListFromType(Type type)
        {
            writer?.WriteLine("{0} - Scan Type {1}", DateTime.Now.ToString("s"), type.FullName);
            Dictionary<string, Type> dico = new Dictionary<string, Type>();
            // récupérer toutes les interfaces de façon récusives
            GetInterfaceRecusif(type, dico);

            if (dico.Count > 0)
            {
                var interfacesList = new InterfacesList()
                {
                    TypeClasse = type,
                    Interfaces = dico.Values.ToList()
                };
                InterfacesListResult.Add(interfacesList);
            }

        }

        private void GetInterfaceRecusif(Type type, Dictionary<string, Type> dico)
        {

            var interfaces = type.GetInterfaces().Where(t => t.GetFormattedFullName().StartsWith("Proginov.PnvUserControls") || t.GetFormattedFullName().StartsWith("System.ComponentModel.INotifyPropertyChanged")).ToList();
            if (interfaces.Any())
            {

                foreach (var iface in interfaces)
                {
                    if (!dico.ContainsKey(iface.GetFormattedFullName()))
                    {
                        dico.Add(iface.GetFormattedFullName(), iface);
                    }
                    GetInterfaceRecusif(iface, dico);
                }
            }
        }

        /// <summary>
        /// Write documentation in Json file
        /// </summary>
        /// <param name="path">output json file</param>
        public void ToJsonFile(string path)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = contractResolver
            };
            // Convert to Json
            string json = JsonConvert.SerializeObject(Unions, Formatting.Indented, jsonSettings);

            // Write to file
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Write documentation in Json file
        /// </summary>
        /// <param name="path">output json file</param>
        public void ToCsvFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            var list = InterfacesListResult.OrderByDescending(t => t.Count).ToList();
            for (int i = 0; i < list.First().Count; i++)
            {
                if (i == 0)
                    sb.Append("Name").Append(";");
                if (i == 1)
                    sb.Append("Count").Append(";");
                if (i > 1)
                    sb.Append(i).Append(";");
            }
            sb.Append(Environment.NewLine);
            list = list.OrderBy(list => list.TypeClasse.GetFormattedFullName()).ToList();
            foreach (var item in list)
            {
                sb.Append(item.TypeClasse.GetFormattedFullName()).Append(";");
                sb.Append(item.Count).Append(";");
                foreach (var inter in item.Interfaces)
                {
                    sb.Append(inter.GetFormattedFullName().Replace("Proginov.PnvUserControls.", "").Replace("System.ComponentModel.", "")).Append(";");
                }
                sb.Append(Environment.NewLine);
            }

            // Write to file
            File.WriteAllText(path, sb.ToString());
        }
        /// <summary>
        /// Write documentation in Json file
        /// </summary>
        /// <param name="path">output json file</param>
        public void ToCsvFile2(string path)
        {
            StringBuilder sb = new StringBuilder();
            var list = UsageResult.OrderByDescending(t => t.Value.Count).ToList();
            for (int i = 0; i < list.First().Value.Count; i++)
            {
                if (i == 0)
                    sb.Append("Name").Append(";");
                else
                    sb.Append(i).Append(";");
            }
            sb.Append(Environment.NewLine);
            list = list.OrderBy(list => list.Key).ToList();
            foreach (var item in list)
            {
                sb.Append(item.Key).Append(";");
                foreach (var inter in item.Value)
                {
                    sb.Append(inter).Append(";");
                }
                sb.Append(Environment.NewLine);
            }

            // Write to file
            File.WriteAllText(path, sb.ToString());
        }

    }
}
