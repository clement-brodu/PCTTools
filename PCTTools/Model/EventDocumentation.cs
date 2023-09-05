namespace PCTTools.Model
{
    public class EventDocumentation
    {
        public string Name { get; set; }
        public string EventType { get; set; }
        public ObsoleteDocumentation Obsolete { get; set; }
        public bool IsStatic { get; set; }
        public bool IsPublic { get; set; }
    }
}