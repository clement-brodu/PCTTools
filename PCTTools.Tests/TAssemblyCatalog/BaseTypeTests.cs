using NUnit.Framework;
using PCTTools.Sample.SAssemblyCatalog.Misc;

namespace PCTTools.Tests.TAssemblyCatalog
{

    [TestFixture()]
    public class BaseTypeTests
    {
        [Test()]
        public void InterfaceBaseTypesTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromAssembly(typeof(IParentInterface).Assembly);
            Assert.That(pct.HasError, Is.False);

            var childMultiParentInterface = pct.TypeDocumentations.First(x => x.IsInterface && x.ShortName == nameof(IChildMultiParentInterface));
            Assert.That(childMultiParentInterface.BaseTypes.Count, Is.EqualTo(2), $"{nameof(IChildMultiParentInterface)} doit avoir 2 parents");
            Assert.That(childMultiParentInterface.BaseTypes, Does.Contain(typeof(IParentInterface).FullName));
            Assert.That(childMultiParentInterface.BaseTypes, Does.Contain(typeof(IParentBInterface).FullName));

            var childParentInterface = pct.TypeDocumentations.First(x => x.IsInterface && x.ShortName == nameof(IChildInterface));
            Assert.That(childParentInterface.BaseTypes.Count, Is.EqualTo(1), $"{nameof(IChildInterface)} doit avoir 1 parent");
            Assert.That(childParentInterface.BaseTypes, Does.Contain(typeof(IParentInterface).FullName));

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(IParentInterface).FullName));

            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IParentInterface doit être une interface");
            Assert.That(doctype.BaseTypes, Is.Not.Null, "BaseTypes ne doit pas être null");
            Assert.That(doctype.BaseTypes, Is.Empty, "Une interface sans parent ne doit avoir aucun BaseType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(IChildInterface).FullName));

            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IChildInterface doit être une interface");
            Assert.That(doctype.BaseTypes, Is.Not.Null, "BaseTypes ne doit pas être null");
        }
    }
}