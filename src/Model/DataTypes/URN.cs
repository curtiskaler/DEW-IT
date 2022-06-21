using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

// https://datatracker.ietf.org/doc/html/rfc8141
// Uniform Resource Names (URNs)

namespace DewIt.Model.DataTypes
{
    [Serializable]
    [XmlRoot("URN", IsNullable = true)]
    public class URN : Uri, ISerializable
    {
        private const RegexOptions URNRegexOptions = RegexOptions.Singleline | RegexOptions.CultureInvariant;

        private static readonly Regex URNRegex =
            new(
                "\\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\\/]|%[0-9a-f]{2})+)(?:\\?\\+(?<rcomponent>.*?))?(?:\\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\\z",
                URNRegexOptions);

        [XmlIgnore, IgnoreDataMember] public string NID { get; protected set; }

        [XmlIgnore, IgnoreDataMember] public string NSS { get; protected set; }

        [XmlIgnore, IgnoreDataMember] public string RComponent { get; protected set; }

        [XmlIgnore, IgnoreDataMember] public string QComponent { get; protected set; }

        [XmlIgnore, IgnoreDataMember] public string FComponent { get; protected set; }


        public string AssignedName => Scheme + ":" + NID + ":" + NSS;

        [DataMember(IsRequired = true), XmlAttribute()]
        public string Urn
        {
            get => AssignedName +
                       (string.IsNullOrWhiteSpace(RComponent)
                           ? ""
                           : "?+" + RComponent) +
                       (string.IsNullOrWhiteSpace(QComponent)
                           ? ""
                           : "?=" + QComponent) +
                       (string.IsNullOrWhiteSpace(FComponent) ? "" : "#" + FComponent);
            set
            {
                var that = new URN(value);
                NID = that.NID;
                NSS = that.NSS;
                RComponent = that.RComponent;
                QComponent = that.QComponent;
                FComponent = that.FComponent;
            }
        }

        /// <summary>
        /// Attempts to create a URN object from the given URN string.
        /// </summary>
        /// <param name="urnString"></param>
        /// <exception cref="FormatException"></exception>
        public URN(string urnString) : base(urnString, UriKind.Absolute)
        {
            if (!string.Equals(Scheme, nameof(Urn), StringComparison.InvariantCultureIgnoreCase))
            {
                throw new FormatException($"URN scheme must be 'urn'.");
            }

            var match = URNRegex.Match(AbsoluteUri);
            if (!match.Success) throw new FormatException("URN is invalid.");
            NID = match.Groups["nid"].Value;
            NSS = match.Groups["nss"].Value;
            RComponent = match.Groups["rcomponent"].Value;
            QComponent = match.Groups["qcomponent"].Value;
            FComponent = match.Groups["fcomponent"].Value;
        }

        /// <summary> Deserialization ctor. </summary>
        protected URN() : base("urn:deserialization:ctor", UriKind.Absolute)
        {
            NID ??= "";
            NSS ??= "";
            RComponent ??= "";
            QComponent ??= "";
            FComponent ??= "";
        }

        /// <summary> Serialization/deserialization constructor. </summary>
        protected URN(SerializationInfo info, StreamingContext streamingContext) : base(info.GetString(nameof(Urn)) ??
            string.Empty)
        {
            if (string.IsNullOrWhiteSpace(NID))
                throw new InvalidOperationException("Cannot deserialize a URN without a NID.");

            if (string.IsNullOrWhiteSpace(NSS))
                throw new InvalidOperationException("Cannot deserialize a URN without a NSS.");

            RComponent ??= "";
            QComponent ??= "";
            FComponent ??= "";
        }


        public URN WithRComponent(string rComponent)
        {
            RComponent = rComponent;
            return this;
        }

        public URN WithQComponent(string qComponent)
        {
            QComponent = qComponent;
            return this;
        }

        public URN WithFComponent(string fComponent)
        {
            FComponent = fComponent;
            return this;
        }

        public override string ToString() => Urn;


        #region Equality

        public override bool Equals(object? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is not URN u) return false;
            if (!string.Equals(Scheme, u.Scheme, StringComparison.InvariantCultureIgnoreCase)) return false;
            if (!string.Equals(NID, u.NID, StringComparison.InvariantCultureIgnoreCase)) return false;

            var thisNSS = NSS.CapitalizePCTEncoding();
            var otherNSS = u.NSS.CapitalizePCTEncoding();
            if (!string.Equals(thisNSS, otherNSS, StringComparison.InvariantCulture)) return false;

            return true;
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(URN? u1, URN? u2)
        {
            if (ReferenceEquals(u1, u2)) return true;
            if (u1 is null || u2 is null) return false;
            return u1.Equals(u2);
        }

        public static bool operator !=(URN? u1, URN? u2)
        {
            if (ReferenceEquals(u1, u2)) return false;
            if (u1 is null || u2 is null) return true;
            return !u1.Equals(u2);
        }

        #endregion

        #region Serialization

        protected new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Urn), Urn);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            GetObjectData(info, context);
        }


        #endregion
    }

    public class JsonURNConverter : JsonConverter<URN>
    {
        private const string urnKey = "urn";

        public override URN? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            URN? urn = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return urn;
                }

                // Get the key.
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                if (propertyName != urnKey) throw new JsonException("Unable to convert URN.");

                // Get the value.
                reader.Read();
                var value = reader.GetString()!;
                urn = new URN(value);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, URN value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(urnKey) ?? urnKey);
            writer.WriteStringValue(string.Format(value.Urn, CultureInfo.InvariantCulture));
            writer.WriteEndObject();
        }
    }

    internal static class URNExtensions
    {
        internal static string CapitalizePCTEncoding(this string str)
        {
            for (var index = 0; index < str.Length; index++)
            {
                var chr = str[index];
                if (chr != '%') continue;
                var sub = str.Substring(index, 3);
                var rep = sub.ToUpperInvariant();
                str = str.Replace(sub, rep);
            }

            return str;
        }
    }
}