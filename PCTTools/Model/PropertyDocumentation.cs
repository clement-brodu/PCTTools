namespace PCTTools.Model
{
    public class PropertyDocumentation
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
    }
}