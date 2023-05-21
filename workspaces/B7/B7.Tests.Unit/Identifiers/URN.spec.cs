using B7.Identifiers;

namespace B7.Tests.Unit;

public class UrnSpec
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void URN_Golden_Path()
    {
        Urn urn = new Urn("abc", "123");

        Assert.That(urn.AssignedName, Is.EqualTo("urn:abc:123"));
    }

    [Test]
    public void URN_Capitalizes_Percent_Encoded_Characters()
    {
        // %3F === ?

        Urn urn = new Urn("abc", "123-%3f-4c6");

        Assert.That(urn.AssignedName, Is.EqualTo("urn:abc:123-%3F-4c6"));
    }

    [Test]
    public void URN_Can_Encapsulate_Hierarchy_with_slash()
    {
        Urn urn = new Urn("example", "1/406/47452/2");

        Assert.That(urn.AssignedName, Is.EqualTo("urn:example:1/406/47452/2"));
    }


    [Test]
    public void URN_ctor_string_golden_path()
    {
        Urn urn = new Urn("urn:example:1/406/47452/2");

        Assert.That(urn.AssignedName, Is.EqualTo("urn:example:1/406/47452/2"));
    }

    [Test]
    public void URN_equivalence()
    {
        // basic
        Urn urn1 = new Urn("urn:example:a123,z456");
        Urn urn2 = new Urn("urn:example:a123,z456");
        Assert.That(urn1, Is.EqualTo(urn1));
        Assert.That(urn1, Is.EqualTo(urn2));

        // case-insensitivity of scheme and nid
        Urn urn3 = new Urn("URN:example:a123,z456");
        Urn urn4 = new Urn("urn:EXAMPLE:a123,z456");
        Assert.That(urn2, Is.EqualTo(urn3));
        Assert.That(urn3, Is.EqualTo(urn4));

        // components are not considered in URN-equivalence
        Urn urn5 = new Urn("urn:EXAMPLE:a123,z456?+abc");
        Urn urn6 = new Urn("urn:EXAMPLE:a123,z456?=xyz");
        Urn urn7 = new Urn("urn:EXAMPLE:a123,z456#789");
        Assert.That(urn4, Is.EqualTo(urn5));
        Assert.That(urn5, Is.EqualTo(urn6));
        Assert.That(urn6, Is.EqualTo(urn7));

        // slashes are meaningful in nss
        Urn urn8 = new Urn("urn:EXAMPLE:a123,z456/foo");
        Urn urn9 = new Urn("urn:EXAMPLE:a123,z456/bar");
        Urn urn10 = new Urn("urn:EXAMPLE:a123,z456/baz");
        Assert.That(urn8, Is.Not.EqualTo(urn1));
        Assert.That(urn8, Is.Not.EqualTo(urn9));
        Assert.That(urn8, Is.Not.EqualTo(urn10));
        Assert.That(urn9, Is.Not.EqualTo(urn1));
        Assert.That(urn9, Is.Not.EqualTo(urn10));

        // case-insensitivity in NSS for pct-encoded
        Urn urn11 = new Urn("urn:example:a123%2Cz456");
        Urn urn12 = new Urn("urn:EXAMPLE:a123%2cz456");
        Assert.That(urn11, Is.EqualTo(urn12));

        // case-sensitivity in the NSS for non-pct-encoded
        Urn urn13 = new Urn("urn:example:A123,z456");
        Urn urn14 = new Urn("urn:example:a123,Z456");
        Assert.That(urn13, Is.Not.EqualTo(urn14));

        // cyrillic
        Urn urn15 = new Urn("urn:example:%D0%B0123,z456");
        Assert.That(urn15, Is.Not.EqualTo(urn1));
    }

    [Test]
    public void ctor_throws_bad_scheme()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = new Urn("pdq:example:1/406/47452/2");
        });
        Assert.That(ex.Message, Is.EqualTo("String cannot be parsed into a URN."));
    }

    [TestCase("URN:%32example%32:a123,z456")]
    [TestCase("URN:example-:a123,z456")]
    [TestCase("URN:example-way-too-long-nananananananananananananananana-batmaaaan:a123,z456")]
    [TestCase("URN:!example:a123,z456")]
    [TestCase("URN:example!:a123,z456")]
    public void ctor_throws_bad_NID(string candidate)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = new Urn(candidate);
        });
    }

    [TestCase("urn:example://a123,z456", "Invalid NSS: must start with a pchar.")] // must start with pchar
    [TestCase("urn:example:a123?z456", "String cannot be parsed into a URN.")]
    [TestCase("urn:example:a123[z456]", "String cannot be parsed into a URN.")]
    [TestCase("urn:example:a123`z456", "String cannot be parsed into a URN.")]
    public void ctor_throws_bad_NSS(string candidate, string reason)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = new Urn(candidate);
        });
        Assert.That(ex.Message, Is.EqualTo(reason));
    }


    //[TestCase("example-%32", "Invalid NID: must contain only alphanumeric characters or hyphen.")]
    //[TestCase("examp!e", "Invalid NID: must contain only alphanumeric characters or hyphen.")]
    //[TestCase("example-way-too-long-nananananananananananananananana-batmaaaan", "Invalid NID: must be between 2 and 32 characters.")]
    //[TestCase("-example", "Invalid NID: first and last characters of NID must be alphanumeric.")]
    //[TestCase("example-", "Invalid NID: first and last characters of NID must be alphanumeric.")]
    //public void ctor_bad_NID_Throws(string candidate, string reason)
    //{
    //    var ex = Assert.Throws<ArgumentException>(() =>
    //    {
    //        Urn urn = new Urn(candidate);
    //    });
    //    Assert.That(ex.Message, Is.EqualTo(reason));
    //}


    //[TestCase("/a123,z456", "Invalid NSS: must start with a pchar.")] // must start with pchar
    //[TestCase("a123?z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    //[TestCase("a123#z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    //[TestCase("a123[z456]", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    //[TestCase("a123`z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    //public void ValidateNSS_Throws(string candidate, string reason)
    //{
    //    var ex = Assert.Throws<ArgumentException>(() =>
    //    {
    //        Urn urn = new Urn(candidate);
    //    });
    //    Assert.That(ex.Message, Is.EqualTo(reason));
    //}


    [TestCase("urn:example:a123#z456", "example", "a123", "", "", "z456")]
    [TestCase("urn:example:a123?+z456", "example", "a123", "z456", "", "")]
    [TestCase("urn:example:a123?=z456", "example", "a123", "", "z456", "")]
    [TestCase("urn:example:a123?+b456?=z456#c789", "example", "a123", "b456", "z456", "c789")]
    public void ctor_parses(String candidate, string nid, string nss, string r, string q, string f)
    {
        Urn urn = new Urn(candidate);

        Assert.That(urn.NID, Is.EqualTo(nid));
        Assert.That(urn.NSS, Is.EqualTo(nss));
        Assert.That(urn.RComponent, Is.EqualTo(r));
        Assert.That(urn.QComponent, Is.EqualTo(q));
        Assert.That(urn.FComponent, Is.EqualTo(f));
        Assert.That(urn.AssignedName, Is.EqualTo("urn:" + nid + ":" + nss));
        Assert.That(urn.NameString, Is.EqualTo(candidate));
    }
}
