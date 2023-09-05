using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using PCTTools.Sample.SAssemblyCatalog.CountMembers;

namespace PCTTools.Tests.TAssemblyCatalog
{
    [TestFixture(typeof(Class2), 2, 2, 2, 2, 2, false)]
    [TestFixture(typeof(Class2B), 2, 2, 2, 2, 1, false)]
    [TestFixture(typeof(Class2B), 10, 4, 4, 4, 1, true)]
    [TestFixture(typeof(Class2C), 0, 2, 0, 2, 4, false)]
    [TestFixture(typeof(Class2C), 10, 6, 4, 6, 4, true)]
    [TestFixture(typeof(Enum2), 0, 0, 0, 3, 0, false)]
    [TestFixture(typeof(Enum2), 12, 0, 0, 3, 0, true)]
    public class CountMembersTest
    {
        private readonly Type typeToTest;
        private readonly int methodsCount;
        private readonly int propertiesCount;
        private readonly int eventsCount;
        private readonly int fieldsCount;
        private readonly int constructorsCount;
        private readonly bool withInherits;

        public CountMembersTest(Type typeToTest, int methodsCount, int propertiesCount, int eventsCount, int fieldsCount, int constructorsCount, bool withInherits)
        {
            this.typeToTest = typeToTest;
            this.methodsCount = methodsCount;
            this.propertiesCount = propertiesCount;
            this.eventsCount = eventsCount;
            this.fieldsCount = fieldsCount;
            this.constructorsCount = constructorsCount;
            this.withInherits = withInherits;
        }

        [Test()]
        public void CountElementsTest()
        {
            var pct = new AssemblyCatalog();
            pct.GenerateDocumentationFromType(typeToTest, withInherits);

            var typedoc = pct.TypeDocumentations.First(t => t.Name == typeToTest.Name);

            Assert.That(typedoc.Constructors.Count, Is.EqualTo(constructorsCount), "Incorrect number of constructor");
            Assert.That(typedoc.Properties.Count, Is.EqualTo(propertiesCount), "Incorrect number of properties");
            Assert.That(typedoc.Events.Count, Is.EqualTo(eventsCount), "Incorrect number of events");
            Assert.That(typedoc.Fields.Count, Is.EqualTo(fieldsCount), "Incorrect number of fields");
            Assert.That(typedoc.Methods.Count, Is.EqualTo(methodsCount), "Incorrect number of methods");
        }

        [Test()]
        public void CountElementsPublicTest()
        {
            var pct = new AssemblyCatalog();
            pct.PublicOnly = true;
            pct.GenerateDocumentationFromType(typeToTest, withInherits);

            var typedoc = pct.TypeDocumentations.First(t => t.Name == typeToTest.Name);
            var systemObjectMethodsCount = withInherits ? 6 : 0; // remove 6 public and protected methods of System.Object
            var systemObjectPublicMethodsCount = withInherits ? 4 : 0; // add 4 public methods of System.Object
            var methodsPublicCount = (methodsCount - systemObjectMethodsCount) / 2 + systemObjectPublicMethodsCount;
            var enumMethodsCount = methodsCount - (withInherits ? 2 : 0);
            if (typeToTest == typeof(Enum2)) // no protected fields on enum2
                Assert.That(typedoc.Methods.Count, Is.EqualTo(enumMethodsCount), "Incorrect number of methods");
            else
                Assert.That(typedoc.Methods.Count, Is.EqualTo(methodsPublicCount), "Incorrect number of methods");
            Assert.That(typedoc.Properties.Count, Is.EqualTo(propertiesCount / 2), "Incorrect number of properties");
            Assert.That(typedoc.Events.Count, Is.EqualTo(eventsCount / 2), "Incorrect number of events");
            if (typeToTest == typeof(Enum2)) // no protected fields on enum2
                Assert.That(typedoc.Fields.Count, Is.EqualTo(fieldsCount), "Incorrect number of fields");
            else
                Assert.That(typedoc.Fields.Count, Is.EqualTo(fieldsCount / 2), "Incorrect number of fields");
            if (typeToTest == typeof(Class2C)) // no protected contructor on other types
                Assert.That(typedoc.Constructors.Count, Is.EqualTo(constructorsCount / 2), "Incorrect number of constructor");
            else
                Assert.That(typedoc.Constructors.Count, Is.EqualTo(constructorsCount), "Incorrect number of constructor");
        }
    }
}