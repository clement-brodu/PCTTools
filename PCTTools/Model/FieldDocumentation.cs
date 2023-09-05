namespace PCTTools.Model
{
    public class FieldDocumentation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
    }
}