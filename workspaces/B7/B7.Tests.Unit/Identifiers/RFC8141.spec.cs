using B7.Identifiers;

namespace B7.Tests.Unit;

public class RFC8141Spec
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase("urn:example:1/406/47452/2", "example", "1/406/47452/2", "", "", "")]
    [TestCase("urn:example:a123#z456", "example", "a123", "", "", "z456")]
    [TestCase("urn:example:a123?+z456", "example", "a123", "z456", "", "")]
    [TestCase("urn:example:a123?=z456", "example", "a123", "", "z456", "")]
    [TestCase("urn:example:a123?+b456?=z456#c789", "example", "a123", "b456", "z456", "c789")]
    public void ResolveUrn_golden_path(String candidate, string nid, string nss, string r, string q, string f)
    {
        Urn urn = RFC8141.ResolveUrn(candidate);

        Assert.That(urn.NID, Is.EqualTo(nid));
        Assert.That(urn.NSS, Is.EqualTo(nss));
        Assert.That(urn.RComponent, Is.EqualTo(r));
        Assert.That(urn.QComponent, Is.EqualTo(q));
        Assert.That(urn.FComponent, Is.EqualTo(f));
        Assert.That(urn.AssignedName, Is.EqualTo("urn:" + nid + ":" + nss));
        Assert.That(urn.NameString, Is.EqualTo(candidate));
    }

    [Test]
    public void ResolveUrn_bad_scheme()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = RFC8141.ResolveUrn("pdq:example:1/406/47452/2");
        });
        Assert.That(ex.Message, Is.EqualTo("String cannot be parsed into a URN."));
    }

    [TestCase("URN:%32example%32:a123,z456")]
    [TestCase("URN:example-:a123,z456")]
    [TestCase("URN:example-way-too-long-nananananananananananananananana-batmaaaan:a123,z456")]
    [TestCase("URN:!example:a123,z456")]
    [TestCase("URN:example!:a123,z456")]
    public void ResolveUrn_bad_NID(string candidate)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = RFC8141.ResolveUrn(candidate);
        });
    }

    [TestCase("urn:example://a123,z456", "Invalid NSS: must start with a pchar.")] // must start with pchar
    [TestCase("urn:example:a123?z456", "String cannot be parsed into a URN.")]
    [TestCase("urn:example:a123[z456]", "String cannot be parsed into a URN.")]
    [TestCase("urn:example:a123`z456", "String cannot be parsed into a URN.")]
    public void ResolveUrn_bad_NSS(string candidate, string reason)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            Urn urn = RFC8141.ResolveUrn(candidate);
        });
        Assert.That(ex.Message, Is.EqualTo(reason));
    }


    [TestCase("example-%32", "Invalid NID: must contain only alphanumeric characters or hyphen.")]
    [TestCase("examp!e", "Invalid NID: must contain only alphanumeric characters or hyphen.")]
    [TestCase("example-way-too-long-nananananananananananananananana-batmaaaan", "Invalid NID: must be between 3 and 32 characters.")]
    [TestCase("-example", "Invalid NID: first and last characters of NID must be alphanumeric.")]
    [TestCase("example-", "Invalid NID: first and last characters of NID must be alphanumeric.")]
    public void ValidateNID_Throws(string candidate, string reason)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            RFC8141.ValidateNID(candidate);
        });
        Assert.That(ex.Message, Is.EqualTo(reason));
    }


    [TestCase("/a123,z456", "Invalid NSS: must start with a pchar.")] // must start with pchar
    [TestCase("a123?z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    [TestCase("a123#z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    [TestCase("a123[z456]", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    [TestCase("a123`z456", "Invalid NSS: all characters following the first must be either a pchar or '/'.")]
    public void ValidateNSS_Throws(string candidate, string reason)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        {
            RFC8141.ValidateNSS(candidate);
        });
        Assert.That(ex.Message, Is.EqualTo(reason));
    }



}
