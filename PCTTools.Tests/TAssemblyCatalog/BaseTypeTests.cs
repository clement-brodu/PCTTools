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
            Assert.That(childMultiParentInterface.BaseTypes.Count, Is.EqualTo(2), $"{nameof(IChildMultiParentInterface)} should have 2 parents");
            Assert.That(childMultiParentInterface.BaseTypes, Does.Contain(typeof(IParentInterface).FullName));
            Assert.That(childMultiParentInterface.BaseTypes, Does.Contain(typeof(IParentBInterface).FullName));

            var childParentInterface = pct.TypeDocumentations.First(x => x.IsInterface && x.ShortName == nameof(IChildInterface));
            Assert.That(childParentInterface.BaseTypes.Count, Is.EqualTo(1), $"{nameof(IChildInterface)} should have 1 parent");
            Assert.That(childParentInterface.BaseTypes, Does.Contain(typeof(IParentInterface).FullName));

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(IParentInterface).FullName));

            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IParentInterface should be an interface");
            Assert.That(doctype.BaseTypes, Is.Not.Null, "BaseTypes should not be null");
            Assert.That(doctype.BaseTypes, Is.Empty, "An interface with no parent should have no BaseTypes");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(IChildInterface).FullName));

            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IChildInterface should be an interface");
            Assert.That(doctype.BaseTypes, Is.Not.Null, "BaseTypes should not be null");
        }
    }
}