using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting;
using System.ComponentModel;
using Newtonsoft.Json.Serialization;
using PCTTools.Extensions;
using PCTTools.Model;

namespace PCTTools
{
    /// <summary>
    /// Assembly Catalog Generator
    /// </summary>
    public class AssemblyCatalog
    {
        /// <summary>
        /// Writer to log things
        /// </summary>
        private TextWriter writer;
        /// <summary>
        /// Add Trace for each Assembly
        /// </summary>
        private bool logAssembly;
        /// <summary>
        /// Add Trace for each Type
        /// </summary>
        private bool logTypes;

        /// <summary>
        /// True if an error append during scan
        /// </summary>
        public bool HasError => ScanExceptions.Any();

        /// <summary>
        /// List of all scan exceptions 
        /// </summary>
        public List<Exception> ScanExceptions { get; set; } = new List<Exception>();

        /// <summary>
        /// Each Type will contains documentation of Declared and Inherted members
        /// Default to false to return declared members only
        /// </summary>
        public bool WithInherited { get; set; }

        /// <summary>
        /// return only public member
        /// Default to false to return public and protected members
        /// </summary>
        public bool PublicOnly { get; set; }

        /// <summary>
        /// Use Openedge types instead of native dotnet types
        /// </summary>
        public bool UseOeTypes { get; set; }

        /// <summary>
        /// List of Types Documentation
        /// </summary>
        public List<TypeDocumentation> TypeDocumentations { get; } = new List<TypeDocumentation>();


        /// <summary>
        /// Define a Writer to log things
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="logAssembly"></param>
        /// <param name="logType"></param>
        public void SetWriter(TextWriter writer, bool logAssembly, bool logType)
        {
            this.writer = writer;
            this.logAssembly = logAssembly;
            this.logTypes = logType;
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
            string json = JsonConvert.SerializeObject(TypeDocumentations, Formatting.Indented, jsonSettings);

            // Write to file
            File.WriteAllText(path, json);
        }

        internal void ToJsonFileFull(string path)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            var jsonSettings = new JsonSerializerSettings()
            {

                ContractResolver = contractResolver
            };
            // Convert to Json
            string json = JsonConvert.SerializeObject(TypeDocumentations, Formatting.Indented, jsonSettings);

            // Write to file
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Generate documentation from each assembly in current AppDomain
        /// </summary>
        public void GenerateDocumentationFromAppDomain()
        {
            var appDomain = AppDomain.CurrentDomain;

            foreach (var assembly in appDomain.GetAssemblies())
            {
                try
                {

                    // Exclude this assembly
                    if (assembly == typeof(AssemblyCatalog).Assembly) continue;
                    // Exclude Newtonsoft.Json if embedded in PCTTools
                    if (assembly == typeof(JsonConvert).Assembly && assembly.CodeBase.EndsWith("PCTTools.dll")) continue;

                    GenerateDocumentationFromAssembly(assembly);
                }
                catch (Exception ex)
                {
                    ScanExceptions.Add(ex);
                    writer?.WriteLine("{0} - ERROR Reading Assembly {1} : {2}", DateTime.Now.ToString("s"), assembly.GetName().Name, ex.Message);
                    writer?.WriteLine("{0}", ex.StackTrace);
                }
            }
        }


        /// <summary>
        /// Generate documentation from an assembly
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        public void GenerateDocumentationFromAssembly(Assembly assembly)
        {
            if (logAssembly)
                writer?.WriteLine("{0} - Scan Assembly {1}", DateTime.Now.ToString("s"), assembly.GetName().Name);

            foreach (Type type in assembly.GetTypes())
            {
                GenerateDocumentationFromType(type);
            }
        }

        /// <summary>
        /// Generate documentation from type
        /// </summary>
        /// <param name="type">type to scan</param>
        public void GenerateDocumentationFromType(Type type)
        {
            GenerateDocumentationFromType(type, WithInherited);
        }

        /// <summary>
        /// Generate documentation from type
        /// </summary>
        /// <param name="type">type to scan</param>
        /// <param name="withInherited">include inherited member in doc</param>
        internal void GenerateDocumentationFromType(Type type, bool withInherited)
        {
            try
            {
                if (type.IsPublic)
                {
                    if (logTypes)
                        writer?.WriteLine("{0} - \tScan Type {1}", DateTime.Now.ToString("s"), type.FullName);

                    AddTypeToList(type, withInherited);
                }
            }
            catch (Exception ex)
            {
                ScanExceptions.Add(ex);
                writer?.WriteLine("{0} - ERROR Reading Type {1} : {2}", DateTime.Now.ToString("s"), type.FullName, ex.Message);
                writer?.WriteLine("{0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Add Type to List. Recursive for Nested Class.
        /// </summary>
        /// <param name="type">type to add</param>
        /// <param name="withInherited">include inherited member in doc</param>
        internal void AddTypeToList(Type type, bool withInherited)
        {
            var typeDocumentation = new TypeDocumentation
            {
                ShortName = type.GetFormattedName(),
                Name = type.GetFormattedFullName(),
                IsClass = type.IsClass,
                IsInterface = type.IsInterface,
                IsEnum = type.IsEnum,
                IsValueType = type.IsValueType,
                IsAbstract = type.IsAbstract,
                IsSealed = type.IsSealed,
                IsGeneric = type.IsGenericType,
                IsNested = type.IsNested,
                BaseTypes = GetBaseTypes(type),
                Constructors = GetConstructors(type),
                Methods = GetMethods(type, withInherited),
                Properties = GetProperties(type, withInherited),
                Events = GetEvents(type, withInherited),
                Fields = GetFields(type, withInherited),
                Obsolete = type.GetObsolete()
            };

            TypeDocumentations.Add(typeDocumentation);

            // scan public nested types
            foreach (var nested in type.GetNestedTypes().Where(t => t.IsNestedPublic))
            {
                AddTypeToList(nested, withInherited);
            }
        }

        /// <summary>
        /// Return all Base Types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal List<string> GetBaseTypes(Type type)
        {
            var baseTypes = new List<string>();

            var baseType = type.BaseType;
            while (baseType != null)
            {
                baseTypes.Add(baseType.FullName);

                baseType = baseType.BaseType;
            }
            return baseTypes;
        }

        /// <summary>
        /// Retrieve Methods Documentation
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="withInherited">include inherited member in doc</param>
        /// <returns></returns>
        internal List<MethodDocumentation> GetMethods(Type type, bool withInherited = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetMethods(bind)
                .Where(method => !method.IsSpecialName && method.IsMethodPublicOrProtected(PublicOnly))
                .Select(method => new MethodDocumentation
                {
                    Name = method.Name,
                    FormattedName = GetFormattedMethodName(method),
                    ReturnType = method.ReturnType.GetFormattedFullName(UseOeTypes),
                    Parameters = GetParameters(method),
                    Obsolete = method.GetObsolete(),
                    IsStatic = method.IsStatic,
                    IsPublic = method.IsPublic
                })
                .ToList();
        }

        /// <summary>
        /// Retrieve Parameters Documentation
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal List<ParameterDocumentation> GetParameters(MethodBase method)
        {
            return method.GetParameters()
                                    .Select(param => new ParameterDocumentation
                                    {
                                        Name = param.Name,
                                        Type = param.ParameterType.GetFormattedFullName(UseOeTypes)
                                    })
                                    .ToList();
        }

        /// <summary>
        /// Find if method is public or protected. ignore internal or private
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="publicOnly">ignore protected, public only</param>
        /// <returns></returns>
        public string GetFormattedMethodName(MethodBase method)
        {
            var parameters = method.GetParameters();
            var stringparameters = !parameters.Any() ?
                    string.Empty :
                    parameters
                        .Select(p => string.Format("{0}{1} {2}",
                            p.IsOut ? "OUTPUT " : string.Empty,
                            p.Name,
                            p.ParameterType.GetFormattedFullName(UseOeTypes)))
                        .Aggregate((x1, x2) => $"{x1}, {x2}");
            return string.Format("{0}({1})", method.Name, stringparameters);
        }

        /// <summary>
        /// Retrieve Constructors Documentation
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="withInherited">include inherited member in doc</param>
        /// <returns></returns>
        internal List<ConstructorDocumentation> GetConstructors(Type type, bool withInherited = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetConstructors(bind)
                .Where(method => method.IsMethodPublicOrProtected(PublicOnly))
                .Select(method => new ConstructorDocumentation
                {
                    Name = method.Name,
                    Parameters = GetParameters(method),
                    Obsolete = method.GetObsolete(),
                    IsStatic = method.IsStatic,
                    IsPublic = method.IsPublic
                })
                .ToList();
        }

        /// <summary>
        /// Retrieve Properties Documentation
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="withInherited">include inherited member in doc</param>
        /// <returns></returns>
        internal List<PropertyDocumentation> GetProperties(Type type, bool withInherited = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetProperties(bind)
                .Where(property => MemberInfoExtensions.IsMethodPublicOrProtected(property.GetMethod ?? property.SetMethod, PublicOnly))
                .Select(property => new PropertyDocumentation
                {
                    Name = property.Name,
                    Type = property.PropertyType.GetFormattedFullName(UseOeTypes),
                    CanRead = property.CanRead,
                    CanWrite = property.CanWrite,
                    Obsolete = property.GetObsolete(),
                    IsStatic = property.GetMethod?.IsStatic == true || property.SetMethod?.IsStatic == true,
                    IsPublic = property.GetMethod?.IsPublic ?? property.SetMethod?.IsPublic ?? false
                })
                .ToList();
        }

        /// <summary>
        /// Retrieve Events Documentation
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="withInherited">include inherited member in doc</param>
        /// <returns></returns>
        internal List<EventDocumentation> GetEvents(Type type, bool withInherited = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetEvents(bind)
                .Where(e => MemberInfoExtensions.IsMethodPublicOrProtected(e.AddMethod ?? e.RemoveMethod, PublicOnly))
                .Select(e => new EventDocumentation
                {
                    Name = e.Name,
                    EventType = e.EventHandlerType.GetFormattedFullName(UseOeTypes),
                    DelegateReturnType = e.EventHandlerType.GetMethod("Invoke").ReturnType.GetFormattedFullName(UseOeTypes),
                    DelegateParameters = GetParameters(e.EventHandlerType.GetMethod("Invoke")),
                    Obsolete = e.GetObsolete(),
                    IsStatic = e.AddMethod?.IsStatic == true && e.RemoveMethod?.IsStatic == true,
                    IsPublic = e.AddMethod?.IsPublic ?? e.RemoveMethod?.IsPublic ?? false
                })
                .ToList();
        }

        /// <summary>
        /// Retrieve Fields Documentation
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="withInherited">include inherited member in doc</param>
        /// <returns></returns>
        internal List<FieldDocumentation> GetFields(Type type, bool withInherited = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetFields(bind)
                .Where(field => !field.IsSpecialName && field.IsFieldPublicOrProtected(PublicOnly))
                .Select(field => new FieldDocumentation
                {
                    Name = field.Name,
                    Type = field.FieldType.GetFormattedFullName(UseOeTypes),
                    Obsolete = field.GetObsolete(),
                    IsStatic = field.IsStatic,
                    IsPublic = field.IsPublic
                })
                .ToList();
        }
    }
}