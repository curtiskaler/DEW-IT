using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace B7.Identifiers;

/// <summary>
/// A Uniform Resource Name (URN) is a Uniform Resource Identifier (URI)
/// [RFC3986] that is assigned under the "urn" URI scheme and a
/// particular URN namespace, with the intent that the URN will be a
/// persistent, location-independent resource identifier.
/// <para></para>
/// See RFC8141 (<see href="https://datatracker.ietf.org/doc/rfc8141/"/>).
/// </summary>
[Serializable]
[XmlRoot("URN", IsNullable = true)]
public class Urn : Uri, IEquatable<Urn>
{
    public const string URNScheme = "urn";
    public const string AttributeName = "namestring";

    /// <summary>
    /// Namespace Identifier
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string NID { get; internal set; }

    /// <summary>
    /// Namespace Specific String
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string NSS { get; internal set; }

    /// <summary>
    /// The r-component is intended for passing parameters to URN resolution
    /// services and interpreted by those services.  
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string RComponent { get; internal set; }

    /// <summary>
    /// The q-component is intended for passing parameters to either the
    /// named resource or a system that can supply the requested service, for
    /// interpretation by that resource or system.
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string QComponent { get; internal set; }

    /// <summary>
    /// The f-component is intended to be interpreted by the client as a
    /// specification for a location within, or region of, the named
    /// resource.It distinguishes the constituent parts of a resource named
    /// by a URN.For a URN that resolves to one or more locators that can
    /// be dereferenced to a representation, or where the URN resolver
    /// directly returns a representation of the resource, the semantics of
    /// an f-component are defined by the media type of the representation.
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string FComponent { get; internal set; }

    /// <summary> The full name of the URN. </summary>
    [DataMember(Name = AttributeName, IsRequired = true), XmlAttribute(AttributeName)]
    public string NameString { get; internal set; }

    /// <summary>
    /// The combination of the "urn:" scheme, the NID, and the namespace
    /// specific string (NSS).  An "assigned-name" is consequently a substring
    /// of a URN if that URN contains any additional components.
    /// <br/>
    /// See <see href="https://datatracker.ietf.org/doc/rfc8141/"/>, Section 2.
    /// </summary>
    [XmlIgnore, IgnoreDataMember] public string AssignedName { get; internal set; }

    /// <summary>
    /// Initializes an instance of the <see cref="Urn"/> class from the given urn string.
    /// </summary>
    /// <param name="urnString">The <see cref="Urn"/> in string form.</param>
    public Urn(string urnString) : this(RFC8141.ResolveUrn(urnString))
    {
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Urn"/> class from another <see cref="Urn"/>.
    /// </summary>
    /// <param name="urn">Another <see cref="Urn"/> that may or may not be valid.</param>
    public Urn([NotNull] Urn urn) : this(urn.NID, urn.NSS, urn.RComponent, urn.QComponent, urn.FComponent)
    {
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Urn"/> class.
    /// </summary>
    /// <param name="nid">The namespace identifier.</param>
    /// <param name="nss">The namespace specific string.</param>
    public Urn(string nid, string nss) : this(nid, nss, null, null, null)
    {
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Urn"/> class.
    /// </summary>
    /// <param name="nid">The namespace identifier.</param>
    /// <param name="nss">The namespace specific string.</param>
    /// <param name="rComponent">The r-component.</param>
    /// <param name="qComponent">The q-component.</param>
    /// <param name="fComponent">The f-component.</param>
    public Urn(string nid, string nss, string rComponent, string qComponent, string fComponent) : base(RFC8141.ResolveNameString(nid, nss, rComponent, qComponent, fComponent), UriKind.Absolute)
    {
        if (string.IsNullOrWhiteSpace(nid)) throw new ArgumentNullException(nameof(NID));
        if (string.IsNullOrWhiteSpace(nss)) throw new ArgumentNullException(nameof(NSS));

        NID = nid;
        NSS = RFC8141.NormalizePercentEncoding(nss);
        RComponent = rComponent ?? "";
        QComponent = qComponent ?? "";
        FComponent = fComponent ?? "";
        AssignedName = RFC8141.ResolveAssignedName(NID, NSS);
        NameString = RFC8141.ResolveNameString(AssignedName, RComponent, QComponent, FComponent);

        RFC8141.ValidateUrn(this);
    }

    /// <summary> Deserialization ctor. </summary>
    protected Urn() : base("urn:deserialization:ctor", UriKind.Absolute)
    {
        NID ??= "";
        NSS ??= "";
        RComponent ??= "";
        QComponent ??= "";
        FComponent ??= "";
    }

    public override string ToString() => NameString;

    #region IEquatable

    public bool Equals(Urn other)
    {
        /**
         * Two URNs are URN-equivalent if their assigned-name portions are 
         * octet-by-octet equal after applying case normalization (as specified
         * in Section 6.2.2.1 of [RFC3986]) to the following conclasss:
         * 
         * 1.  the URI scheme "urn", by conversion to lower case
         * 
         * 2.  the NID, by conversion to lower case
         * 
         * 3.  any percent-encoded characters in the NSS (that is, all character
         * triplets that match the <pct-encoding> production found in
         * Section 2.1 of the base URI specification [RFC3986]), by
         * conversion to upper case for the digits A-F.
         */

        return string.Equals(NID, other.NID, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(NSS, other.NSS, StringComparison.Ordinal);

        throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
        return
                obj is Urn u &&
                string.Equals(NID, u.NID, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(NSS, u.NSS, StringComparison.Ordinal);
    }

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(Urn left, Urn right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Urn left, Urn right)
    {
        return !(left == right);
    }

    #endregion IEquatable
}
