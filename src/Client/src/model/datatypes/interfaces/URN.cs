namespace DewIt.Client.model.enumerations
{
    internal class DummURN
    {
        DummURN()
        {
            var thing = new WorkbookUrn("bob", "alice");
        }
    }

    public class WorkbookUrn : URN
    {
        public override string NamespaceIdentifier => "workbook";

        public WorkbookUrn(string fullString)
        {

        }

        public WorkbookUrn(string entityType, string id)
        {

        }
    }

    public abstract class URN
    {
        public static string Scheme => "urn";
        public abstract string NamespaceIdentifier { get; }
        public string NamespaceString { get; init; }

        public string QComponent { get; init; }
        public string RComponent { get; init; }
        public string Fragment { get; init; }


        public string AssignedName => Scheme + ":" + NamespaceIdentifier + ":" + NamespaceString;

        public string NameString
        {
            get
            {
                return AssignedName + RQComponents + FComponent;
            }
        }

        private string RQComponents
        {
            get
            {
                string result = "";
                if (!string.IsNullOrEmpty(RComponent))
                {
                    result += "?+" + RComponent;
                }
                if (!string.IsNullOrEmpty(QComponent))
                {
                    result += "?=" + QComponent;
                }
                return result;
            }
        }

        private string FComponent
        {
            get
            {
                if (string.IsNullOrEmpty(Fragment))
                    return "";
                return "#" + Fragment;
            }
        }
    }

    /// <summary> Augmented Backus-Naur form. </summary>
    static class ABNF
    {
        // https://en.wikipedia.org/wiki/Augmented_Backus%E2%80%93Naur_form

        /// <summary> Lower-case ASCII letters (a-z) </summary>
        public static bool isLOW_ALPHA(this char chr)
        {
            return (chr >= '\x61' && chr <= '\x7A');
        }
        
        /// <summary> Upper-case ASCII letters (A-Z) </summary>
        public static bool isUP_ALPHA(this char chr)
        {
            return (chr >= '\x41' && chr <= '\x5A') ;
        }

        /// <summary> Upper- and lower-case ASCII letters (A-Z, a-z) </summary>
        public static bool isALPHA(this char chr) => chr.isLOW_ALPHA() || chr.isUP_ALPHA();
        
        /// <summary> Decimal digits (0-9) </summary>
        public static bool isDIGIT(this char chr) => (chr >= '\x30' && chr <= '\x39');

        /// <summary> Hexadecimal digits (0-9, A-F, a-f) </summary>
        public static bool isHEXDIG(this char chr) => chr.isDIGIT() || "abcdefABCDEF".Contains(chr);

        /// <summary> Double quote </summary>
        public static bool isDQUOTE(this char chr) => chr == '\x22';

        /// <summary> Carriage return </summary>
        public static bool isCR(this char chr) => chr == '\x0D';

        /// <summary> Line feed </summary>
        public static bool isLF(this char chr) => chr == '\x0A';

        /// <summary> Internet-standard newline </summary>
        public static bool isCRLF(this string str) => str == "\x0D\x0A";

        /// <summary> Space </summary>
        public static bool isSP(this char chr) => chr == '\x20';

        /// <summary> Horizontal tab </summary>
        public static bool isHTAB(this char chr) => chr == '\x09';

        /// <summary> Space or horizontal tab </summary>
        public static bool isWSP(this char chr) => (chr.isSP() || chr.isHTAB());

        /// <summary> 
        /// Linear white space (past newline) 
        /// NOTE: this should be used with caution, as it may permit lines that cause interoperability problems.
        /// </summary>
        public static bool isLWSP(this string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            var chars = str.ToCharArray();
            for (var index = 0; index < chars.Length; index++)
            {
                char chr = chars[index];
                if (chr.isCR())
                {
                    // ensure the next two chars are LF followed by WSP
                    if (chars.Length < index + 3) return false;
                    var nextOne = chars[index + 1];
                    var nextTwo = chars[index + 2];
                    if (!(nextOne.isLF() && nextTwo.isWSP())) return false;
                }
                else
                {
                    // ensure it's WSP
                    if (!chr.isWSP()) return false;
                }
            }

            return true;
        }

        /// <summary> Visible (printing) characters </summary>
        public static bool isVCHAR(this char chr) => (chr >= '\x21' && chr <= '\x7E');

        /// <summary> Any ASCII character, excluding NUL </summary>
        public static bool isCHAR(this char chr) => (chr >= '\x01' && chr <= '\x7F');

        /// <summary> 8 bits of data </summary>
        public static bool isOCTET(this char chr) => (chr >= '\x00' && chr <= '\xFF');

        /// <summary> Controls </summary>
        public static bool isCTL(this char chr) => (chr >= '\x00' && chr <= '\x1F') || (chr == '\x7F');

        /// <summary> Controls </summary>
        public static bool isBIT(this char chr) => (chr == '0' || chr == '1');
    }
    static class ABNF_URI_RULES
    {
        // https://datatracker.ietf.org/doc/html/rfc2396

        public static bool isPCHAR(this char chr)
        {
            // should also include isPCT_ENCODED, but that ONLY handles strings, not chars
            return chr.isUNRESERVED() || chr.isSUB_DELIM() || chr == ':' || chr == '@';
        }

        public static bool isIP_V6_ADDRESS(this string str)
        {
            /*
                   IPv6address =               6( h16 ":" ) ls32
                  /                       "::" 5( h16 ":" ) ls32
                  / [               h16 ] "::" 4( h16 ":" ) ls32
                  / [ *1( h16 ":" ) h16 ] "::" 3( h16 ":" ) ls32
                  / [ *2( h16 ":" ) h16 ] "::" 2( h16 ":" ) ls32
                  / [ *3( h16 ":" ) h16 ] "::"    h16 ":"   ls32
                  / [ *4( h16 ":" ) h16 ] "::"              ls32
                  / [ *5( h16 ":" ) h16 ] "::"              h16
                  / [ *6( h16 ":" ) h16 ] "::"

            ... jeez.
            
            IPV6 is 8 blocks of 1-4 HEXDIGs, separated by colons.
            
            Leading zeros in each 16-bit field are suppressed, but 
            each group must retain at least one digit.
            
            The longest sequence of consecutive all-zero fields is 
            replaced with two colons (::). If the address contains 
            multiple runs of all-zero fields of the same size, to 
            prevent ambiguities, it is the leftmost that is compressed. 

            The above table is basically explaining the suppression rules. 
            
            ... jeez.  ALSO:

            During the transition of the Internet from IPv4 to IPv6, 
            it is typical to operate in a mixed addressing environment. 
            For such use cases, a special notation has been introduced, 
            which expresses IPv4-mapped and IPv4-compatible IPv6 addresses 
            by writing the least-significant 32 bits of an address in the 
            familiar IPv4 dot-decimal notation, whereas the 96 most-significant 
            bits are written in IPv6 format. For example, the IPv4-mapped 
            IPv6 address ::ffff:c000:0280 is written as ::ffff:192.0.2.128, 
            thus expressing clearly the original IPv4 address that was 
            mapped to IPv6.

            ... JEEZ.  

             */

            if (string.IsNullOrEmpty(str)) return false;
            if (str == "::") return true;




            throw new Exception('not implemented yet');
        }
        public static bool isLS32(this string str)
        {
            // ( h16 ":" h16 ) / IPv4address
            if (string.IsNullOrEmpty(str)) return false;
            
            if (str.isIP_V4_ADDRESS()) return true;

            if (!str.Contains(':')) return false;
            if (str.First() == ':' || str.Last() == ':') return false;
            var parts = str.Split(':');
            return parts.All(isH16);
        }
        public static bool isH16(this string str)
        {
            // 1*4HEXDIG
            if (string.IsNullOrEmpty(str)) return false;
            if (str.Length < 1 || str.Length > 4) return false;
            return str.All(ABNF.isHEXDIG);
        }
        public static bool isIP_V4_ADDRESS(this string str)
        {
            // dec-octet "." dec-octet "." dec-octet "." dec-octet
            if (string.IsNullOrEmpty(str)) return false;
            if (str.First() == '.' || str.Last() == '.') return false;
            var parts = str.Split('.');
            return parts.All(isDEC_OCTET);
        }

        public static bool isDEC_OCTET(this string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (str.Length > 3) return false;

            // converting to byte has different behavior based on compiler
            // don't risk it.

            if (!str.All(ABNF.isDIGIT)) return false;
            
            if (str.Length == 1)
            {
                if (!ABNF.isDIGIT(str[0])) return false;
            } 
            
            if (str.Length == 2)
            {
                // %x31-39 DIGIT         ; 10-99
                if (str[0] < '\x31' || str[0] > '\x39') return false;
                if (!ABNF.isDIGIT(str[1])) return false;
            }

            if (str.Length == 3)
            {
                // "1" 2DIGIT            ; 100-199
                if (str[0] == '1')
                {
                    if (!ABNF.isDIGIT(str[1])) return false;
                    if (!ABNF.isDIGIT(str[2])) return false;
                } else if (str[0] == '2')
                {
                    // "2" %x30-34 DIGIT     ; 200-249
                    // "25" % x30 - 35; 250 - 255
                    if (str[1] < '\x30' || str[1] > '\x35') return false;
                    if (str[1] == '\x35')
                    {
                        // "25" % x30 - 35; 250 - 255
                        if (str[2] < '\x30' || str[2] > '\x35') return false;
                    } else
                    {
                        // "2" %x30-34 DIGIT     ; 200-249
                        if (!ABNF.isDIGIT(str[2])) return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isREG_NAME(this string str)
        {
            // *(unreserved / pct-encoded / sub-delims)
            if (str == null) return false;

            for (var index = 0; index < str.Length; index++)
            {
                var chr = str[index];
                if (!(chr.isUNRESERVED() || chr.isSUB_DELIM()))
                {
                    // if it's also not a pct-encoded, then bork
                    if (chr != '%') return false;
                    if (str.Length < index + 3) return false;
                    var candidate = str.Substring(index, 3);
                    if (!candidate.isPCT_ENCODED()) return false;
                }
            }
            return true;
        }

        public static bool isPATH(this string str)
        {
            return str.isPATH_ABEMPTY() || // begins with "/" or is empty
                str.isPATH_ABSOLUTE() || // begins with "/" but not "//"
                str.isPATH_NOSCHEME() || // begins with a non-colon segment
                str.isPATH_ROOTLESS() || // begins with a segment
                str.isPATH_EMPTY(); // zero characters
        }
        public static bool isPATH_ABEMPTY(this string str)
        {
            // begins with "/" or is empty
            // any number of ( "/" segment )
            if (str == null) return false;
            if (str == "" || str =="/") return true;
            if (str.Last() == '/') return false;
            if (str[0] != '/') return false;
            var parts = str.Split('/');
            return parts.All(isSEGMENT);
        }
        public static bool isPATH_ABSOLUTE(this string str)
        {
            // begins with "/" but not "//"
            //  "/" [ segment-nz *( "/" segment ) ]
            // at least one ( "/" segment )
            if (string.IsNullOrEmpty(str)) return false;
            if (str[0] != '/') return false;
            if (str.Length == 1) return true;

            if (str[1] == '/') return false;
            if (str.Last() == '/') return false;

            var parts = str.Split('/');
            if (parts.Length == 0) return false;
            return parts.All(isSEGMENT);
        }
        public static bool isPATH_NOSCHEME(this string str)
        {
            // begins with a non-colon segment
            // segment-nz-nc *( "/" segment )
            // segment-nz-nc followed by any number of ("/" segment)

            if (string.IsNullOrEmpty(str)) return false;

            if (str[0] == '/') return false;
            if (str.Last() == '/') return false;
            var parts = str.Split('/');
            if (parts.Length == 0) return false;
            if (!parts[0].isSEGMENT_NZ_NC()) return false;
            
            var lastParts = parts.Skip(1);
            return lastParts.All(isSEGMENT);
        }
        public static bool isPATH_ROOTLESS(this string str)
        {
            // begins with a segment
            // segment-nz *( "/" segment )
            // segment-nz followed by any number of ("/" segment)

            if (string.IsNullOrEmpty(str)) return false;

            if (str[0] == '/') return false;
            if (str.Last() == '/') return false;
            
            var parts = str.Split('/');
            if (parts.Length == 0) return false;
            if (!parts[0].isSEGMENT_NZ()) return false;

            var lastParts = parts.Skip(1);
            return lastParts.All(isSEGMENT);
        }
        public static bool isPATH_EMPTY(this string str)
        {
            // 0<pchar>
            // an empty string
            return (str == "");
        }

        public static bool isSEGMENT(this string str)
        {
            // any number of pchars, even zero
            if (str == null) return false;

            for (var index = 0; index < str.Length; index++)
            {
                char chr = str[index];
                if (chr.isPCHAR()) continue;
                if (chr == '%')
                {
                    if (str.Length < index + 2) return false;
                    string sub = str.Substring(index, 3);
                    if (!sub.isPCT_ENCODED()) return false;
                }
            }
            return true;
        }
        public static bool isSEGMENT_NZ(this string str)
        {
            // At least ONE pchar
            if (string.IsNullOrEmpty(str)) return false;
            return str.isSEGMENT();
        }
        public static bool isSEGMENT_NZ_NC(this string str)
        {
            // non-zero-length segment without any colon
            return str.isSEGMENT_NZ() && !str.Contains(':');
        }

        public static bool isFRAGMENT(this string str)
        {
            if (str == null) return false;
            
            for (var index = 0; index<str.Length; index++)
            {
                char chr = str[index];
                if (chr == '/' || chr == '?' || chr.isPCHAR()) continue;
                if (chr == '%')
                {
                    if (str.Length < index + 2) return false;
                    string sub = str.Substring(index, 3);
                    if (!sub.isPCT_ENCODED()) return false;
                }
            }
            return true;
        }
        public static bool isQUERY(this string str)
        {
            if (str == null) return false;

            for (var index = 0; index < str.Length; index++)
            {
                char chr = str[index];
                if (chr == '/' || chr == '?' || chr.isPCHAR()) continue;
                if (chr == '%')
                {
                    if (str.Length < index + 2) return false;
                    string sub = str.Substring(index, 3);
                    if (!sub.isPCT_ENCODED()) return false;
                }
            }
            return true;
        }

        public static bool isPCT_ENCODED(this string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (str.Length % 3 != 0) return false;

            if (str.Length == 3)
            {
                if (str[0] != '%') return false;
                if (!str[1].isHEXDIG()) return false;
                return str[2].isHEXDIG();
            }

            var parts = str.splitInParts(3);
            return parts.All(isPCT_ENCODED);
        }

        public static bool isUNRESERVED(this char chr)
        {
            return chr.isALPHA() || chr.isDIGIT() || "-._~".Contains(chr);
        }
        public static bool isRESERVED(this char chr) => chr.isGEN_DELIM() || chr.isSUB_DELIM();
        public static bool isGEN_DELIM(this char chr)
        {
            return ":/?#[]@".Contains(chr);
        }
        public static bool isSUB_DELIM(this char chr)
        {
            return "!$&\'()*+,;=".Contains(chr);
        }

        private static IEnumerable<string> splitInParts(this string s, int partLength)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", nameof(partLength));

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }
    }
    public static class URNExtensions
    {
        

        public static void ValidateURN(this URN urn)
        {
            if (urn == null) return;
            urn.ValidateNID();
            urn.ValidateNSS();
        }

        private static void ValidateNID(this URN urn)
        {
            if (urn == null) return;
        }
        private static void ValidateNSS(this URN urn)
        {
            if (urn == null) return;
        }

        public static string ToString(this URN urn)
        {
            return urn.ToString();
        }
    }
}