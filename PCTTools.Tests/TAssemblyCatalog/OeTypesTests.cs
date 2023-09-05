using NUnit.Framework;
using PCTTools.Sample.SAssemblyCatalog.Nested;
using PCTTools.Sample.SAssemblyCatalog.OeTypes;

namespace PCTTools.Tests.TAssemblyCatalog
{

    [TestFixture()]
    public class OeTypesTests
    {
        [Test()]
        public void OeTypesTest()
        {
            var pct = new AssemblyCatalog();
            pct.UseOeTypes = true;
            pct.GenerateDocumentationFromType(typeof(Class1));

            var doctype = pct.TypeDocumentations.First();
            var property = doctype.Properties.First(p => p.Name == "PropString");
            Assert.That(property.Type, Is.EqualTo("CHARACTER"));
            property = doctype.Properties.First(p => p.Name == "PropInt32");
            Assert.That(property.Type, Is.EqualTo("INTEGER"));
            property = doctype.Properties.First(p => p.Name == "PropBool");
            Assert.That(property.Type, Is.EqualTo("LOGICAL"));
            property = doctype.Properties.First(p => p.Name == "PropStringArray");
            Assert.That(property.Type, Is.EqualTo("System.String[]"));
        }
    }
}