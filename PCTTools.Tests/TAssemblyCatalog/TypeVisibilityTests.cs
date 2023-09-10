using NUnit.Framework;
using PCTTools.Sample.SAssemblyCatalog.Nested;

namespace PCTTools.Tests.TAssemblyCatalog
{

    [TestFixture()]
    public class TypeVisibilityTests
    {
        [Test()]
        public void NestedTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromAssembly(typeof(NestedPublic).Assembly);
            Assert.IsFalse(pct.HasError);

            var doctypes = pct.TypeDocumentations.Where(t => t.Name.Contains(".Nested."));
            // Should only get NestedPublic and NestedPublic+NeastedPublicSubPublic
            Assert.That(doctypes.Count, Is.EqualTo(2));
        }
    }
}