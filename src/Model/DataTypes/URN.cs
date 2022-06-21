using System.Runtime.Serialization;
using System.Text.RegularExpressions;

// https://datatracker.ietf.org/doc/html/rfc8141
// Uniform Resource Names (URNs)


namespace DewIt.Model.DataTypes
{
    [Serializable]
    public class URN : Uri
    {
        public const string URNScheme = "urn";
        private const RegexOptions URNRegexOptions = RegexOptions.Singleline | RegexOptions.CultureInvariant;
        private static readonly Regex URNRegex = new("\\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\\/]|%[0-9a-f]{2})+)(?:\\?\\+(?<rcomponent>.*?))?(?:\\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\\z", URNRegexOptions);

        public string? NID { get; init; }
        public string? NSS { get; init; }
        public string RComponent { get; private set; }
        public string QComponent { get; private set; }
        public string FComponent { get; private set; }
        

        public string AssignedName => Scheme + ":" + NID + ":" + NSS;

        // public string NameString => AssignedName + RQComponents + FComponent;

        /// <summary>
        /// Attempts to create a URN object from the given URN string.
        /// </summary>
        /// <param name="urnString"></param>
        /// <exception cref="FormatException"></exception>
        public URN(string urnString) : base(urnString, UriKind.Absolute)
        {
            if (Scheme != URNScheme) throw new FormatException($"URN scheme must be '{URNScheme}'.");
            var match = URNRegex.Match(AbsoluteUri);

            // This regex percent-encodes '%'... so the underlying goodies will be double-encoded

            if (!match.Success) throw new FormatException("URN's NID is invalid.");
            NID = match.Groups["nid"].Value;
            NSS = match.Groups["nss"].Value;
            RComponent = match.Groups["rcomponent"].Value;
            QComponent = match.Groups["qcomponent"].Value;
            FComponent = match.Groups["fcomponent"].Value;

            if (ValidationErrors.Length>0) throw new FormatException("URN is invalid.", ValidationErrors[0]);
        }

        public FormatException[] ValidationErrors
        {
            get
            {
                var errors = new List<FormatException>();
                errors.AddRange(GetNSSValidationErrors(NSS));
                return errors.ToArray();
            }
        }

        /// <summary> Serialization/deserialization constructor. </summary>
        protected URN(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(
            serializationInfo, streamingContext)
        {
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
        
        public override bool Equals(object? other)
        {
            if (ReferenceEquals(other, this)) return true;
            return
                other is URN u &&
                string.Equals(NID, u.NID, StringComparison.InvariantCultureIgnoreCase) &&
                NSSEquals(NSS, u.NSS);
        }

        private static List<FormatException> GetNSSValidationErrors(string? nss)
        {
            var errors = new List<FormatException>();

            if (nss == null)
            {
                errors.Add(new FormatException("NSS must not be null."));
                return errors;
            }

            // check the nss for invalid '%' (one that isn't pct-encoding)
            for (var index = 0; index < nss.Length; index++)
            {
                var chr = nss[index];
                if (chr != '%') continue;

                const string msg =
                    "The '%' character can only be used for percent-encoding, and must be followed by two hex digits.";
                
                if (nss.Length < index + 3)
                {
                    errors.Add(new FormatException(msg));
                    continue;
                }

                var candidate = nss.Substring(index + 1, 2);
                if (candidate.Any(c =>
                        c is (< '\x30' or > '\x39') and (< '\x41' or > '\x46') and (< '\x61' or > '\x66')))
                    errors.Add(new FormatException(msg));
            }
            return errors;
        }
        private static bool NSSEquals(string? a, string? b)
        {
            if (a is null || b is null) return false;
            if (a.Length != b.Length) return false;

            // NSS           = pchar *(pchar / "/")
            // pchar does not include %, so it's safe to assume that % means pct-encoded
            CapitalizePCTEncoding(ref a);
            CapitalizePCTEncoding(ref b);

            return string.Equals(a, b, StringComparison.Ordinal);
        }

        private static void CapitalizePCTEncoding(ref string str)
        {
            for (var index = 0; index < str.Length; index++)
            {
                var chr = str[index];
                if (chr != '%') continue;

                if (str.Length < index + 3) return;
                var sub = str.Substring(index, 3);
                var rep = sub.ToUpperInvariant();
                str = str.Replace(sub, rep);
            }
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
        
        
    }
}
