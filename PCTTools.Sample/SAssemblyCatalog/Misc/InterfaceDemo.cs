namespace PCTTools.Sample.SAssemblyCatalog.Misc
{
    public interface IParentInterface
    {
        string ParentProp { get; set; }
    }

    public interface IParentBInterface
    {
        string ParentProp { get; set; }
    }

    public interface IChildInterface : IParentInterface
    {
        string ChildProp { get; set; }
    }

    public interface IChildMultiParentInterface : IParentInterface, IParentBInterface
    {
        string ChildProp { get; set; }
    }
}