using NUnit.Framework;
using PCTTools.Extensions;
using PCTTools.Model.Enums;
using PCTTools.Sample.SAssemblyCatalog.Misc;
using PCTTools.Sample.SAssemblyCatalog.Nested;
using static PCTTools.Sample.SAssemblyCatalog.Misc.ClassDemo;

namespace PCTTools.Tests.TAssemblyCatalog
{

    [TestFixture()]
    public class MiscTest
    {
        [Test()]
        public void TypeTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromAssembly(typeof(NestedPublic).Assembly);
            Assert.That(pct.HasError, Is.False);

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(AbstractDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(true), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(true), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(ClassDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(false), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(true), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(ClassSealedDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(false), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(true), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(true), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(NestedDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(false), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(true), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(true), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(ClassGenericDemo<>).GetFormattedFullName()));

            Assert.That(doctype.IsAbstract, Is.EqualTo(false), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(true), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(true), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(InterfaceDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(true), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(false), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(InterfaceGenericDemo<>).GetFormattedFullName()));

            Assert.That(doctype.IsAbstract, Is.EqualTo(true), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(false), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(false), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(true), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(true), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(false), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(false), "IsValueType");

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EnumDemo).FullName));

            Assert.That(doctype.IsAbstract, Is.EqualTo(false), "IsAbstract");
            Assert.That(doctype.IsClass, Is.EqualTo(false), "IsClass");
            Assert.That(doctype.IsEnum, Is.EqualTo(true), "IsEnum");
            Assert.That(doctype.IsInterface, Is.EqualTo(false), "IsInterface");
            Assert.That(doctype.IsGeneric, Is.EqualTo(false), "IsGeneric");
            Assert.That(doctype.IsNested, Is.EqualTo(false), "IsNested");
            Assert.That(doctype.IsSealed, Is.EqualTo(true), "IsSealed");
            Assert.That(doctype.IsValueType, Is.EqualTo(true), "IsValueType");
        }

        [Test()]
        public void EnumTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(typeof(EnumDemo));
            Assert.That(pct.HasError, Is.False);

            var doctype = pct.TypeDocumentations.First();

            var field = doctype.Fields.First(f => f.Name == "A");
            Assert.That(field.IsPublic, Is.EqualTo(true));
            Assert.That(field.IsStatic, Is.EqualTo(true));

            field = doctype.Fields.First(f => f.Name == "B");
            Assert.That(field.IsPublic, Is.EqualTo(true));
            Assert.That(field.IsStatic, Is.EqualTo(true));

        }

        [Test()]
        public void ObsoleteTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromAssembly(typeof(EnumWithObsoleteDemo).Assembly);
            Assert.That(pct.HasError, Is.False);

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EnumWithObsoleteDemo).FullName));

            var field = doctype.Fields.First(f => f.Name == "C");
            Assert.That(field.Obsolete.Message, Is.EqualTo("Use B instead"));

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(ObsoleteClassDemo).FullName));

            Assert.That(doctype.Obsolete.Message, Is.EqualTo("Use ClassDemo instead"));
            Assert.That(doctype.Properties.First().Obsolete, Is.EqualTo(null));

            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(OtherClassDemo).FullName));

            var property = doctype.Properties.First(f => f.Name == "PropString");
            Assert.That(property.Obsolete.Message, Is.EqualTo("Obsolete")); // if no message, then "Obsolete"

            property = doctype.Properties.First(f => f.Name == "PropString2");
            Assert.That(property.Obsolete.Message, Is.EqualTo("Do Not Use"));
            Assert.That(property.Obsolete.IsError, Is.EqualTo(false));

            property = doctype.Properties.First(f => f.Name == "PropString3");
            Assert.That(property.Obsolete.Message, Is.EqualTo("Do Not Use"));
            Assert.That(property.Obsolete.IsError, Is.EqualTo(true));

            var event0 = doctype.Events.First(f => f.Name == "PropChanged");
            Assert.That(event0.Obsolete.Message, Is.EqualTo("Do Not Use"));

            var method = doctype.Methods.First(f => f.Name == "GetString");
            Assert.That(method.Obsolete.Message, Is.EqualTo("Do Not Use"));

            field = doctype.Fields.First(f => f.Name == "FieldString");
            Assert.That(field.Obsolete.Message, Is.EqualTo("Do Not Use"));

        }

        [Test()]
        public void StaticTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(typeof(StaticDemo));
            Assert.That(pct.HasError, Is.False);

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(StaticDemo).FullName));

            var field = doctype.Fields.First(f => f.Name == "FieldStaticString");
            Assert.That(field.IsStatic, Is.EqualTo(true));
            field = doctype.Fields.First(f => f.Name == "FieldString");
            Assert.That(field.IsStatic, Is.EqualTo(false));


            var property = doctype.Properties.First(f => f.Name == "PropStaticString");
            Assert.That(property.IsStatic, Is.EqualTo(true));
            property = doctype.Properties.First(f => f.Name == "PropString");
            Assert.That(property.IsStatic, Is.EqualTo(false));


            var event0 = doctype.Events.First(f => f.Name == "EventStatic");
            Assert.That(event0.IsStatic, Is.EqualTo(true));
            event0 = doctype.Events.First(f => f.Name == "Event");
            Assert.That(event0.IsStatic, Is.EqualTo(false));

            var method = doctype.Methods.First(f => f.Name == "GetStaticString");
            Assert.That(method.IsStatic, Is.EqualTo(true));
            method = doctype.Methods.First(f => f.Name == "GetString");
            Assert.That(method.IsStatic, Is.EqualTo(false));

        }

        [Test()]
        public void EventsTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(typeof(EventsDemo));
            Assert.That(pct.HasError, Is.False);
            Assert.That(pct.TypeDocumentations.Count, Is.EqualTo(4));

            var doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EventsDemo).FullName));
            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EventsDemo.DemoHandler).FullName));
            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EventHandler<AssemblyLoadEventArgs>).GetFormattedFullName()));
            doctype = pct.TypeDocumentations.First(t => t.Name.Equals(
                typeof(EventHandler).FullName));
        }

        [Test()]
        public void OutputParametersTest()
        {
            var pct = new AssemblyCatalog();

            pct.GenerateDocumentationFromType(typeof(MethodDemo));
            Assert.That(pct.HasError, Is.False);

            var method = pct.TypeDocumentations.First().Methods.First(m => m.Name == "GetMyInfo");
            Assert.That(method.IsStatic, Is.EqualTo(false));
            var parameter = method.Parameters.First(p => p.Name == "hello");
            Assert.That(parameter.Mode, Is.EqualTo(ParameterMode.INPUT));
            Assert.That(parameter.Type, Is.EqualTo("System.Int32"));
            parameter = method.Parameters.First(p => p.Name == "allow");
            Assert.That(parameter.Mode, Is.EqualTo(ParameterMode.INPUTOUTPUT));
            Assert.That(parameter.Type, Is.EqualTo("System.Boolean"));
            parameter = method.Parameters.First(p => p.Name == "name");
            Assert.That(parameter.Mode, Is.EqualTo(ParameterMode.OUTPUT));
            Assert.That(parameter.Type, Is.EqualTo("System.String"));
        }
    }
}