using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PCTTools.Extensions;
using PCTTools.Model;
using PCTTools.Model.Enums;
using PCTTools.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Formatting = Newtonsoft.Json.Formatting;

namespace PCTTools
{
    /// <summary>
    /// Assembly Catalog Generator
    /// </summary>
    public class AssemblyCatalog
    {
        private static readonly ConcurrentBag<string> missingAssemblies = new();

        private readonly ConcurrentDictionary<string, Assembly> embededAssemblies = new();

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
        /// Add Trace for each Type Error
        /// </summary>
        private bool logTypesError = true;

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
        /// <param name="logTypes"></param>
        public void SetWriter(TextWriter writer, bool logAssembly, bool logTypes)
        {
            this.writer = writer;
            this.logAssembly = logAssembly;
            this.logTypes = logTypes;
        }
        /// <summary>
        /// Define a Writer to log things
        /// </summary>
        /// <param name="writer"></param>
        public void SetWriter(TextWriter writer)
        {
            this.writer = writer;
            this.logAssembly = true;
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

            var result = new TypeDocumentationFile()
            {
                Classes = TypeDocumentations
            };

            // Convert to Json
            string json = JsonConvert.SerializeObject(result, Formatting.Indented, jsonSettings);

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

            var result = new TypeDocumentationFile()
            {
                Classes = TypeDocumentations
            };

            // Convert to Json
            string json = JsonConvert.SerializeObject(result, Formatting.Indented, jsonSettings);

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
                bool flowControl = ShouldSkipAssembly(assembly);
                if (!flowControl)
                {
                    continue;
                }

                flowControl = LoadEmbeddedAssemblies(assembly);
                if (!flowControl)
                {
                    continue;
                }
            }

            foreach (var assembly in appDomain.GetAssemblies())
            {
                bool flowControl = ShouldSkipAssembly(assembly);
                if (!flowControl)
                {
                    continue;
                }

                GenerateDocumentationFromAssembly(assembly);
            }
        }

        /// <summary>
        /// Determines whether the specified assembly should be skipped during processing.
        /// </summary>
        /// <remarks>This method applies specific exclusion rules for certain assemblies, such as those
        /// embedded in PCTTools or the assembly containing the <see cref="AssemblyCatalog"/> type.</remarks>
        /// <param name="assembly">The assembly to evaluate.</param>
        /// <returns><see langword="true"/> if the assembly should be skipped; otherwise, <see langword="false"/>.</returns>
        private static bool ShouldSkipAssembly(Assembly assembly)
        {
            // Exclude this assembly
            if (assembly == typeof(AssemblyCatalog).Assembly) return false;

            // Exclude Newtonsoft.Json if embedded in PCTTools
            if (assembly == typeof(JsonConvert).Assembly && assembly.CodeBase.EndsWith("PCTTools.dll")) return false;
            // Exclude GitVersion.MsBuild if embedded in PCTTools
            if (assembly == typeof(GitVersionInformation).Assembly && assembly.CodeBase.EndsWith("PCTTools.dll")) return false;
            return true;
        }

        private void LogInfo(string format, params string[] args)
        {
            writer?.WriteLine("{0} {1} {2}", DateTime.Now.ToString("s"), "INFO ", string.Format(format, args));
        }
        private void LogError(string format, params string[] args)
        {
            writer?.WriteLine("{0} {1} {2}", DateTime.Now.ToString("s"), "ERROR", string.Format(format, args));
        }

        /// <summary>
        /// Loads embedded assemblies from the specified assembly.
        /// </summary>
        /// <remarks>This method identifies and loads embedded assemblies contained within the provided
        /// assembly.  It recursively processes any embedded assemblies to load their dependencies as well.  Assemblies
        /// with resource names containing ".native." are excluded from loading.</remarks>
        /// <param name="assembly">The assembly to inspect for embedded assemblies.</param>
        /// <returns><see langword="true"/> if the operation completes successfully; otherwise, <see langword="false"/>.</returns>
        private bool LoadEmbeddedAssemblies(Assembly assembly)
        {
            if (assembly.IsDynamic) return false;
            try
            {
                var resources = assembly.GetManifestResourceNames()
                        .Where(resource => resource.ToLower().Contains(".dll") && !resource.ToLower().Contains(".native."))
                        .ToList();
                if (resources.Any())
                    LogInfo("Load Embedded Assemblies : \"{0}\"", assembly.GetName().Name);
                foreach (var res in resources)
                {
                    var stream = EmbeddedAssemblyHelper.LoadStream(assembly, res);
                    var embeddedAssembly = Assembly.Load(EmbeddedAssemblyHelper.ReadStream(stream));
                    LoadEmbeddedAssemblies(embeddedAssembly);
                }
            }
            catch (Exception ex)
            {
                LogError(GetException(ex));
            }

            return true;
        }

        /// <summary>
        /// Retrieves a string representation of the specified exception.
        /// </summary>
        /// <param name="ex">The exception to process. Must not be <see langword="null"/>.</param>
        /// <returns>The message of the exception if it is a <see cref="FileNotFoundException"/> or  <see
        /// cref="ReflectionTypeLoadException"/>; otherwise, the full string representation of the exception.</returns>
        private static string GetException(Exception ex)
        {
            if (ex is FileNotFoundException || ex is ReflectionTypeLoadException)
            {
                return ex.Message;
            }

            return ex.ToString();
        }

        /// <summary>
        /// Generate documentation from an assembly
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        public void GenerateDocumentationFromAssembly(Assembly assembly)
        {
            try
            {
                if (logAssembly)
                    LogInfo("Scan Assembly \"{0}\"", assembly.GetName().Name);

                foreach (Type type in assembly.GetTypes())
                {
                    GenerateDocumentationFromType(type);
                }
            }
            catch (ReflectionTypeLoadException loadEx)
            {
                try
                {
                    ScanExceptions.Add(loadEx);
                    LogError("Error Reading Assembly \"{0}\" : {1}", assembly.GetName().Name, GetException(loadEx));
                    var listMessage = new List<string>();
                    foreach (Exception ex in loadEx.LoaderExceptions)
                    {
                        if (!listMessage.Contains(ex.Message))
                        {
                            listMessage.Add(ex.Message);
                            LogError("    - LoaderExceptions : {0}", GetException(ex));
                        }
                    }

                    // Load Types that we can 
                    if (loadEx.Types != null)
                    {
                        logTypesError = false;
                        foreach (Type type in loadEx.Types)
                        {
                            if (type is null) continue;

                            GenerateDocumentationFromType(type);
                        }
                    }
                    LogError("    => partial load : {0} types loaded", loadEx.Types.Count(t => t != null).ToString());

                }
                catch (Exception ex)
                {
                    ScanExceptions.Add(ex);
                    LogError("Error Reading Assembly \"{0}\" : {1}", assembly.GetName().Name, GetException(ex));
                }
                finally
                {
                    logTypesError = true;
                }
            }
            catch (Exception ex)
            {
                ScanExceptions.Add(ex);
                LogError("Error Reading Assembly \"{0}\" : {1}", assembly.GetName().Name, GetException(ex));
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
                        LogInfo("\tScan Type \"{0}\"", type.FullName);

                    AddTypeToList(type, withInherited);
                }
            }
            catch (Exception ex)
            {
                ScanExceptions.Add(ex);
                if (logTypesError) // Do not log error if type if part of ReflectionTypeLoadException
                    LogError("Error Reading Type \"{0}\" : {1}", type.FullName, GetException(ex));
            }
        }

        /// <summary>
        /// Add Type to List. Recursive for Nested Class.
        /// </summary>
        /// <param name="type">type to add</param>
        /// <param name="withInherited">include inherited member in doc</param>
        internal void AddTypeToList(Type type, bool withInherited)
        {
            var isDelegate = typeof(System.Delegate).IsAssignableFrom(type);
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
                Methods = GetMethods(type, withInherited, isDelegate),
                Obsolete = type.GetObsolete()
            };

            if (!isDelegate)
            {
                // useless information for delegate
                typeDocumentation.Constructors = GetConstructors(type);
                typeDocumentation.Properties = GetProperties(type, withInherited);
                typeDocumentation.Fields = GetFields(type, withInherited);
            }


            TypeDocumentations.Add(typeDocumentation);

            // scan public nested types
            foreach (var nested in type.GetNestedTypes().Where(t => t.IsNestedPublic))
            {
                AddTypeToList(nested, withInherited);
            }

            // scan events after add to list
            typeDocumentation.Events = GetEvents(type, withInherited);
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
        internal List<MethodDocumentation> GetMethods(Type type, bool withInherited = false, bool invokeOnly = false)
        {
            var bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            if (!withInherited)
            {
                bind |= BindingFlags.DeclaredOnly;
            }
            return type.GetMethods(bind)
                .Where(method => !method.IsSpecialName
                                && method.IsMethodPublicOrProtected(PublicOnly)
                                && (!invokeOnly || method.Name.Equals("Invoke")))
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
                                        Type = param.ParameterType.GetFormattedFullName(UseOeTypes),
                                        Mode = GetParameterMode(param)
                                    })
                                    .ToList();
        }

        /// <summary>
        /// Return the parameter mode
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        internal ParameterMode GetParameterMode(ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType.Name.EndsWith("&"))
            {
                if (parameterInfo.IsOut)
                    return ParameterMode.OUTPUT;
                else
                    return ParameterMode.INPUTOUTPUT;
            }
            return ParameterMode.INPUT;
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
                .Select(e =>
                {
                    //TypeDocumentations
                    var eventType = e.EventHandlerType.GetFormattedFullName(UseOeTypes);
                    if (!TypeDocumentations.Any(t => t.Name.Equals(eventType)))
                    {
                        AddTypeToList(e.EventHandlerType, withInherited);
                    }
                    return new EventDocumentation
                    {
                        Name = e.Name,
                        EventType = eventType,
                        Obsolete = e.GetObsolete(),
                        IsStatic = e.AddMethod?.IsStatic == true && e.RemoveMethod?.IsStatic == true,
                        IsPublic = e.AddMethod?.IsPublic ?? e.RemoveMethod?.IsPublic ?? false
                    };
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