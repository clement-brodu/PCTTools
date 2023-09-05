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
            var namespac = typeof(NestedPublic).Namespace;

            var doctypes = pct.TypeDocumentations.Where(t => t.FullName.Contains(".Nested."));
            // Should only get NestedPublic and NestedPublic+NeastedPublicSubPublic
            Assert.That(doctypes.Count, Is.EqualTo(2));
        }
    }
}