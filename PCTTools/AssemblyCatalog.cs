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

        public List<TypeDocumentation> TypeDocumentations { get; set; } = new List<TypeDocumentation>();

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
                NullValueHandling = NullValueHandling.Ignore,
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
                GenerateDocumentationFromAssembly(assembly);
            }
        }

        /// <summary>
        /// Generate documentation from an assembly
        /// </summary>
        /// <param name="assembly">assembly to scan</param>
        public void GenerateDocumentationFromAssembly(Assembly assembly)
        {
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
            if (type.IsPublic)
            {
                AddTypeToList(type, withInherited);
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
                Name = type.GetFormattedName(),
                FullName = type.GetFormattedFullName(),
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                IsClass = type.IsClass,
                IsInterface = type.IsInterface,
                IsEnum = type.IsEnum,
                IsValueType = type.IsValueType,
                IsAbstract = type.IsAbstract,
                IsSealed = type.IsSealed,
                IsGeneric = type.IsGenericType,
                IsNested = type.IsNested,
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
                    ReturnType = method.ReturnType.GetFormattedFullName(UseOeTypes),
                    Parameters = method.GetParameters()
                        .Select(param => new ParameterDocumentation
                        {
                            Name = param.Name,
                            Type = param.ParameterType.GetFormattedFullName(UseOeTypes)
                        })
                        .ToList(),
                    Obsolete = method.GetObsolete(),
                    IsStatic = method.IsStatic,
                    IsPublic = method.IsPublic
                })
                .ToList();
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
                    Parameters = method.GetParameters()
                        .Select(param => new ParameterDocumentation
                        {
                            Name = param.Name,
                            Type = param.ParameterType.GetFormattedFullName(UseOeTypes)
                        })
                        .ToList(),
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
                    EventType = e.EventHandlerType?.GetFormattedFullName(UseOeTypes),
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