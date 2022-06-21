using System.Diagnostics;
using DewIt.Model.DataTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace DewIt.Model.Tests.DataTypes;

public class URNTests
{
    [SetUp]
    public void Setup()
    {
    }

    private const string Scheme = "urn";

    private static string ResolveString(string NID, string NSS, string? RComponent, string? QComponent,
        string? FComponent)
    {
        var urnString = $"{Scheme}:{NID}:{NSS}";
        if (!string.IsNullOrEmpty(RComponent)) urnString += $"?+{RComponent}";
        if (!string.IsNullOrEmpty(QComponent)) urnString += $"?={QComponent}";
        if (!string.IsNullOrEmpty(FComponent)) urnString += $"#{FComponent}";
        return urnString;
    }

    [TestCase("example", "a123,z456", null, null, null)]
    [TestCase("example", "a123,z456", null, null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", "789")]
    [TestCase("example", "a123,z456", "abc", null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", null)]
    [TestCase("example", "a123,z456", "abc", "xyz", null)]
    [TestCase("example", "a123,z456", "abc", null, null)]
    [TestCase("example", "a123,z456", "abc", "xyz", "789")]
    public void Ctor_urnString_WorksForPassingArgs(string NID, string NSS, string? RComponent, string? QComponent,
        string? FComponent)
    {
        var urnString = ResolveString(NID, NSS, RComponent, QComponent, FComponent);

        var urn = new URN(urnString);

        Assert.Multiple(() =>
        {
            Assert.That(urn.AssignedName, Is.EqualTo($"{Scheme}:{NID}:{NSS}"));
            Assert.That(urn.Scheme, Is.EqualTo(Scheme));
            Assert.That(urn.NID, Is.EqualTo(NID));
            Assert.That(urn.NSS, Is.EqualTo(NSS));
            Assert.That(urn.RComponent, Is.EqualTo(RComponent ?? ""));
            Assert.That(urn.QComponent, Is.EqualTo(QComponent ?? ""));
            Assert.That(urn.FComponent, Is.EqualTo(FComponent ?? ""));

            Assert.That(urn.Urn, Is.EqualTo(urnString));
        });
    }

    [Test]
    public void Ctor_urnString_ThrowsForWrongScheme()
    {
        const string urnString = "https://www.example.com";

        var ex = Assert.Throws<FormatException>(() =>
        {
            var _ = new URN(urnString);
        });
        Assert.That(ex?.Message, Is.EqualTo("URN scheme must be 'urn'."));
    }

    [TestCase("urn://www.example.com")]
    public void Ctor_urnString_ThrowsForBadNIDFormatting(string urnString)
    {
        var ex = Assert.Throws<FormatException>(() =>
        {
            var x = new URN(urnString);
            Console.WriteLine(x);
        });
        Assert.That(ex?.Message, Is.EqualTo("URN is invalid."));
    }

    [TestCase("urn:example:a123,z456", "urn:example:a123,z456")]
    [TestCase("urn:example:a123,z456", "URN:example:a123,z456")]
    [TestCase("urn:example:a123,z456", "urn:EXAMPLE:a123,z456")]
    [TestCase("urn:example:a123,z456?+abc", "urn:EXAMPLE:a123,z456?=xyz")]
    [TestCase("urn:example:a123,z456?+abc", "urn:EXAMPLE:a123,z456#789")]
    [TestCase("urn:example:a123,z456?=xyz", "urn:EXAMPLE:a123,z456#789")]
    [TestCase("urn:example:a123%2Cz456", "URN:EXAMPLE:a123%2cz456")]
    public void opEquality(string urn1, string urn2)
    {
        var a = new URN(urn1);
        var b = new URN(urn2);
        Assert.Multiple(() =>
        {
            Assert.That(a == b, Is.True);
            Assert.That(a != b, Is.False);
        });
    }

    [TestCase("urn:example:a123,z456/foo", "urn:example:a123,z456/bar")]
    [TestCase("urn:example:a123,z456/foo", "URN:example:a123,z456/baz")]
    [TestCase("urn:example:a123,z456/bar", "urn:example:a123,z456/baz")]
    [TestCase("urn:example:A123,z456", "urn:example:a123,Z456")]
    [TestCase("urn:example:a123,z456", "urn:example:%D0%B0123,z456")]
    public void opInequality(string urn1, string urn2)
    {
        var a = new URN(urn1);
        var b = new URN(urn2);
        Assert.Multiple(() =>
        {
            Assert.That(a != b, Is.True);
            Assert.That(a == b, Is.False);
        });
    }

    [Test]
    public void opEquality_RefEquals()
    {
        const string urn1 = "urn:example:a123,z456";
        var a = new URN(urn1);
        // ReSharper disable once InlineTemporaryVariable
        var b = a;
        Assert.Multiple(() =>
        {
#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
            Assert.That(a == b, Is.True);
            Assert.That(a != b, Is.False);
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
        });
    }

    [Test]
    public void opEquality_null()
    {
        var a = new URN("urn:example:a123,z456");
        object b = null!;
        Assert.Multiple(() =>
        {
#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
            Assert.That(a == (URN)b, Is.False);
            Assert.That((URN)b == a, Is.False);
            Assert.That(a != (URN)b, Is.True);
            Assert.That((URN)b != a, Is.True);
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
        });
    }

    [Test]
    public void Equals_RefEquals()
    {
        const string urn1 = "urn:example:a123,z456";
        var a = new URN(urn1);
        // ReSharper disable once InlineTemporaryVariable
        var b = a;

#pragma warning disable NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
        Assert.That(a.Equals(b), Is.True);
#pragma warning restore NUnit2010 // Use EqualConstraint for better assertion messages in case of failure
    }

    [Test]
    public void Equals_WrongType()
    {
        const string urn1 = "urn:example:a123,z456";
        var a = new URN(urn1);
        var b = new List<string>{urn1};
        
        Assert.That(a.Equals(b), Is.False);
    }

    [Test]
    public void WithRComponent_Works()
    {
        var urn = new URN("urn:example:a123,z456");
        const string component = "r!";

        var result = urn.WithRComponent(component);

        Assert.That(result.RComponent, Is.EqualTo(component));
    }

    [Test]
    public void WithQComponent_Works()
    {
        var urn = new URN("urn:example:a123,z456");
        const string component = "q!";

        var result = urn.WithQComponent(component);

        Assert.That(result.QComponent, Is.EqualTo(component));
    }

    [Test]
    public void WithFComponent_Works()
    {
        var urn = new URN("urn:example:a123,z456");
        const string component = "f!";

        var result = urn.WithFComponent(component);

        Assert.That(result.FComponent, Is.EqualTo(component));
    }

    [TestCase("example", "a123,z456", null, null, null)]
    [TestCase("example", "a123,z456", null, null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", "789")]
    [TestCase("example", "a123,z456", "abc", null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", null)]
    [TestCase("example", "a123,z456", "abc", "xyz", null)]
    [TestCase("example", "a123,z456", "abc", null, null)]
    [TestCase("example", "a123,z456", "abc", "xyz", "789")]
    public void Types_AreSerializableToJSON(string NID, string NSS, string? RComponent, string? QComponent,
        string? FComponent)
    {
        var urnString = ResolveString(NID, NSS, RComponent, QComponent, FComponent);
        var urn = new URN(urnString);
        var options = new JsonSerializerOptions
            { WriteIndented = true, PropertyNameCaseInsensitive = true, Converters = { new JsonURNConverter() } };

        string jsonString = JsonSerializer.Serialize(urn, options);
        var deserialized = JsonSerializer.Deserialize<URN>(jsonString, options);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized!.AssignedName, Is.EqualTo($"{Scheme}:{NID}:{NSS}"));
            Assert.That(deserialized.Scheme, Is.EqualTo(Scheme));
            Assert.That(deserialized.NID, Is.EqualTo(NID));
            Assert.That(deserialized.NSS, Is.EqualTo(NSS));
            Assert.That(deserialized.RComponent, Is.EqualTo(RComponent ?? ""));
            Assert.That(deserialized.QComponent, Is.EqualTo(QComponent ?? ""));
            Assert.That(deserialized.FComponent, Is.EqualTo(FComponent ?? ""));
            Assert.That(deserialized.Urn, Is.EqualTo(urnString));
        });
    }

    [TestCase("example", "a123,z456", null, null, null)]
    [TestCase("example", "a123,z456", null, null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", "789")]
    [TestCase("example", "a123,z456", "abc", null, "789")]
    [TestCase("example", "a123,z456", null, "xyz", null)]
    [TestCase("example", "a123,z456", "abc", "xyz", null)]
    [TestCase("example", "a123,z456", "abc", null, null)]
    [TestCase("example", "a123,z456", "abc", "xyz", "789")]
    public void Types_AreSerializableToXML(string NID, string NSS, string? RComponent, string? QComponent,
        string? FComponent)
    {
        var urnString = ResolveString(NID, NSS, RComponent, QComponent, FComponent);
        var urn = new URN(urnString);
        var serializer = new XmlSerializer(typeof(URN));
        var deserializer = new XmlSerializer(typeof(URN));
        var stream = new MemoryStream();
        
        serializer.Serialize(stream, urn);
        
        stream.Seek(0L, SeekOrigin.Begin); // rewind the stream
        
        var deserialized = (URN)deserializer.Deserialize(stream)!;
        stream.Close();

        Assert.Multiple(() =>
        {
            Assert.That(deserialized, Is.Not.Null);
            Assert.That(deserialized.AssignedName, Is.EqualTo($"{Scheme}:{NID}:{NSS}"));
            Assert.That(deserialized.Scheme, Is.EqualTo(Scheme));
            Assert.That(deserialized.NID, Is.EqualTo(NID));
            Assert.That(deserialized.NSS, Is.EqualTo(NSS));
            Assert.That(deserialized.RComponent, Is.EqualTo(RComponent ?? ""));
            Assert.That(deserialized.QComponent, Is.EqualTo(QComponent ?? ""));
            Assert.That(deserialized.FComponent, Is.EqualTo(FComponent ?? ""));
            Assert.That(deserialized.Urn, Is.EqualTo(urnString));
        });
    }
}