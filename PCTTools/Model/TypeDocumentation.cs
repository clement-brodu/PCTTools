using System.Collections.Generic;

namespace PCTTools.Model
{
    /// <summary>
    /// Documentation POJO for type
    /// </summary>
    public class TypeDocumentation
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string AssemblyQualifiedName { get; set; }
        public bool IsClass { get; set; }
        public bool IsInterface { get; set; }
        public bool IsEnum { get; set; }
        public bool IsValueType { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsNested { get; set; }
        public List<ConstructorDocumentation> Constructors { get; set; }
        public List<MethodDocumentation> Methods { get; set; }
        public List<PropertyDocumentation> Properties { get; set; }
        public List<EventDocumentation> Events { get; set; }
        public List<FieldDocumentation> Fields { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
    }
}