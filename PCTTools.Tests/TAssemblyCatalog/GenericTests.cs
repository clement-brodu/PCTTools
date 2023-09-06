using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PCTTools.Sample;
using PCTTools.Extensions;
using PCTTools.Sample.SAssemblyCatalog.Generic;

namespace PCTTools.Tests.TAssemblyCatalog
{

    [TestFixture()]
    public class GenericTests
    {
        [Test()]
        public void Generic1Test()
        {
            var type = typeof(GenericDemo<>);
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(type);

            var typedoc = pct.TypeDocumentations.First();
            Assert.That(typedoc.Name, Is.EqualTo("GenericDemo<T>"));

            var method = typedoc.Methods.First(m => m.Name == "Hello");
            Assert.That(method.ReturnType, Is.EqualTo("T"));

            method = typedoc.Methods.First(m => m.Name == "HelloList");
            Assert.That(method.ReturnType, Is.EqualTo("System.Collections.Generic.List<T>"));

            method = typedoc.Methods.First(m => m.Name == "HelloList2");
            Assert.That(method.ReturnType, Is.EqualTo("System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.Int32, T>>"));

            var property = typedoc.Properties.First(m => m.Name == "Value");
            Assert.That(property.Type, Is.EqualTo("T"));

            property = typedoc.Properties.First(m => m.Name == "ValueList");
            Assert.That(property.Type, Is.EqualTo("System.Collections.Generic.List<T>"));
        }

        [Test()]
        public void Generic2Test()
        {
            var type = typeof(GenericDemo2<int, Dictionary<string, GenericDemo<int>>>).GetGenericTypeDefinition();
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(type);

            var typedoc = pct.TypeDocumentations.First();
            Assert.That(typedoc.Name, Is.EqualTo("GenericDemo2<T, U>"));

            var method = typedoc.Methods.First(m => m.Name == "Hello");
            Assert.That(method.ReturnType, Is.EqualTo("T"));

            var property = typedoc.Properties.First(m => m.Name == "Value");
            Assert.That(property.Type, Is.EqualTo("T"));

        }

        [Test()]
        public void Generic3Test()
        {
            var type = typeof(GenericDemo2<int, Dictionary<string, GenericDemo<int>>>);
            Assert.That(type.GetFormattedFullName(),
                Is.EqualTo("PCTTools.Sample.SAssemblyCatalog.Generic.GenericDemo2<System.Int32, System.Collections.Generic.Dictionary<System.String, PCTTools.Sample.SAssemblyCatalog.Generic.GenericDemo<System.Int32>>>"));

        }

        [Test()]
        public void Generic4Test()
        {
            var type = typeof(GenericDemo2<int, Dictionary<string, GenericDemo<int>>>);
            Assert.That(type.GetFormattedFullName(true), // same test with UseOETypes
                Is.EqualTo("PCTTools.Sample.SAssemblyCatalog.Generic.GenericDemo2<System.Int32, System.Collections.Generic.Dictionary<System.String, PCTTools.Sample.SAssemblyCatalog.Generic.GenericDemo<System.Int32>>>"));

        }

        [Test()]
        public void Generic5Test()
        {
            var type = typeof(System.Collections.ObjectModel.ReadOnlyDictionary<int, int>).GetGenericTypeDefinition();
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(type);

            var typedoc = pct.TypeDocumentations.First();
            var property = typedoc.Properties.First(p => p.Name == "Keys");
            Assert.That(property.Type, Is.EqualTo("System.Collections.ObjectModel.KeyCollection"));

        }
    }
}