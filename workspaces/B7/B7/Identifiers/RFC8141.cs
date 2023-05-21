using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace B7.Identifiers;

// TODO: i18n

/*
 * ALPHA (%41-%5A and %61-%7A), DIGIT (%30-%39), 
 * hyphen (%2D), period (%2E), underscore (%5F), or tilde (%7E)
 *    alphanum      = ALPHA / DIGIT 
 *    pchar         = unreserved / pct-encoded / sub-delims / ":" / "@"
 *    fragment      = *( pchar / "/" / "?" )
 *    HEXDIG        = ABCDEFabcdef1234567890
 *    pct-encoded   = "%" HEXDIG HEXDIG
 *    
 *    unreserved    = ALPHA / DIGIT / "-" / "." / "_" / "~"
 *    sub-delims    = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
 *    
 *    namestring    = assigned-name
 *                    [ rq-components ]
 *                    [ "#" f-component ]
 *    assigned-name = "urn" ":" NID ":" NSS
 *    NID = (alphanum)0 * 30(ldh)(alphanum)
 *    ldh           = alphanum / "-"
 *    NSS           = pchar* (pchar / "/")
 *    rq-components = [ "?+" r-component]
 *                    [ "?=" q-component]
 *    r-component   = pchar* (pchar / "/" / "?" )
 *    q-component   = pchar* (pchar / "/" / "?" )
 *    f-component   = fragment
*/

public abstract class RFC8141
{
    private static readonly RegexOptions URNRegexOptions = RegexOptions.Singleline | RegexOptions.CultureInvariant;
    private static readonly Regex URNRegex =
        new(
            "\\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\\/]|%[0-9a-f]{2})+)(?:\\?\\+(?<rcomponent>.*?))?(?:\\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\\z",
            URNRegexOptions);

    public const string URNScheme = "urn";

    public static Urn ResolveUrn(string urnString)
    {
        if (string.IsNullOrWhiteSpace(urnString))
        {
            throw new ArgumentNullException(nameof(urnString));
        }

        // this regex is not quite right, and returns false positives
        // (at least in the NSS)
        MatchCollection matches = URNRegex.Matches(urnString);
        if (matches.Count != 1 || matches[0].Groups.Count < 6)
        {
            throw new ArgumentException("String cannot be parsed into a URN.");
        }
        GroupCollection groups = matches[0].Groups;

        string NID = groups["nid"].ToString();
        string NSS = groups["nss"].ToString();
        string RComponent = groups["rcomponent"].ToString();
        string QComponent = groups["qcomponent"].ToString();
        string FComponent = groups["fcomponent"].ToString();

        return new Urn(NID, NSS, RComponent, QComponent, FComponent);
    }

    public static string ResolveAssignedName(string nid, string nss)
    {
        if (string.IsNullOrWhiteSpace(nid)) throw new ArgumentNullException(nameof(nid));
        if (string.IsNullOrWhiteSpace(nss)) throw new ArgumentNullException(nameof(nss));

        ValidateNID(nid);
        ValidateNSS(nss);

        return $"{URNScheme}:{nid}:{NormalizePercentEncoding(nss)}";
    }

    public static string ResolveNameString(string nid, string nss, string rComponent = null, string qComponent = null, string fComponent = null)
    {
        string assignedName = ResolveAssignedName(nid, nss);
        return ResolveNameString(assignedName, rComponent, qComponent, fComponent);
    }

    public static string ResolveNameString(string assignedName, string rComponent = null, string qComponent = null, string fComponent = null)
    {
        if (string.IsNullOrWhiteSpace(assignedName)) throw new ArgumentNullException(nameof(assignedName));

        rComponent = rComponent ?? "";
        qComponent = qComponent ?? "";
        fComponent = fComponent ?? "";

        ValidateComponent(rComponent, "r-component");
        ValidateComponent(qComponent, "q-component");
        ValidateComponent(fComponent, "f-component");

        string nameString = assignedName;
        if (!string.IsNullOrWhiteSpace(rComponent))
        {
            nameString = $"{nameString}?+{rComponent}";
        }
        if (!string.IsNullOrWhiteSpace(qComponent))
        {
            nameString = $"{nameString}?={qComponent}";
        }
        if (!string.IsNullOrWhiteSpace(fComponent))
        {
            nameString = $"{nameString}#{fComponent}";
        }
        return nameString;
    }

    public static void ValidateUrn(Urn urn)
    {
        // Most relaxed validation rules
        ValidateNID(urn.NID);
        ValidateNSS(urn.NSS);
        ValidateComponent(urn.RComponent, "r-component");
        ValidateComponent(urn.QComponent, "q-component");
        ValidateComponent(urn.FComponent, "f-component");
        ValidateFragment(urn.FComponent);
    }

    public static void ValidateNID(string nid)
    {
        if (nid == null)
        {
            throw new ArgumentNullException(nameof(nid));
        }


        /*
         * From RFC-8141 (https://datatracker.ietf.org/doc/rfc8141/)
         * 
         * ALPHA         = ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz
         * DigitNonZero          = "1" / "2" / "3" / "4" / "5"
         *                             / "6" / "7" / "8" / "9"
         * DIGIT                 = "0" / DigitNonZero
         * alphanum      = ALPHA / DIGIT
         * NID           = (alphanum) 0*30(ldh) (alphanum)
         * ldh           = alphanum / "-"
         *  
         *    NIDs are case insensitive (e.g., "ISBN" and "isbn" are equivalent).
         *    
         *    Characters outside the ASCII range [RFC20] are not permitted in NIDs,
         *    and no encoding mechanism for such characters is supported.
         *
         * first and last chars must be alphanum
         * overall length must be 3-32
         * chars 2-31 must be alphanum or hyphem
         *
         * if it starts with 'urn-', then it's an informal namespace, and:
         * InformalNamespaceName = "urn-" Number
         * Number = DigitNonZero 0 * Digit
         */


        if (nid.Length < 3 || nid.Length > 32)
        {
            throw new ArgumentException("Invalid NID: must be between 3 and 32 characters.");
        }

        if (!(ABNF.isAlphaNum(nid.First()) && ABNF.isAlphaNum(nid.Last())))
        {
            throw new ArgumentException("Invalid NID: first and last characters of NID must be alphanumeric.");
        }

        string midChars = nid.Substring(1, nid.Length - 2);
        if (!midChars.All(it => ABNF.isLDH(it)))
        {
            throw new ArgumentException("Invalid NID: must contain only alphanumeric characters or hyphen.");
        }

        if (nid.StartsWith("urn-"))
        {
            // formal namespaces MUST NOT start with 'urn-'
            // this is an informal namespace.

            // NID = "urn-" DigitNonZero 0 * Digit

            // MUST NOT start with zero
            string firstDigit = nid.Substring(4, 1);
            if (firstDigit == "0")
            {
                throw new ArgumentException("Invalid NID: informal namespace number must not start with zero ('0').");
            }

            // MUST be all digits
            string trailingDigits = nid.Substring(4, nid.Length - 4);
            if (trailingDigits.Any(it => !ABNF.isDigit(it)))
            {
                throw new ArgumentException("Invalid NID: namespaces starting with 'urn-' must end in numberic digits.");
            }
        }
    }

    public static void ValidateNSS(string nss)
    {
        if (nss == null)
        {
            throw new ArgumentNullException(nameof(nss));
        }

        /* Does not validate NSS generated under RFC-2141, where certain characters
         * are disallowed which became allowed in RFC-8141. 
         * 
         * If you want to use characters outside the ASCII range, then
         * you must create a separate parser to translate those names
         * into the acceptable character set (e.g., unicode characters must be
         * percent-encoded.)
         */

        /*
         * From RFC-8141 (https://datatracker.ietf.org/doc/rfc8141/)
         * 
         * NSS           = pchar *(pchar / "/")
         *          
         * pchar         = unreserved / pct-encoded / sub-delims / ":" / "@"
         *      
         * unreserved    = ALPHA / DIGIT / "-" / "." / "_" / "~"
         * pct-encoded   = "%" HEXDIG HEXDIG
         * sub-delims    = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
         */

        // must be at least one character long
        // first character must be a pchar
        // all following characters must be either pchar or /
        // For consistency, URI producers and normalizers should use uppercase hexadecimal digits for all percent-encodings.

        // must be at least one character long


        // first character must be a pchar
        if (!ABNF.isPChar(nss, 0))
        {
            throw new ArgumentException("Invalid NSS: must start with a pchar.");
        }

        // all following characters must be either pchar or /
        for (int index = 1; index < nss.Length; index++)
        {
            if (!ABNF.isPChar(nss, index, '/'))
            {
                throw new ArgumentException("Invalid NSS: all characters following the first must be either a pchar or '/'.");
            }
        }
    }

    public static void ValidateComponent(string candidate, string componentName)
    {
        if (string.IsNullOrWhiteSpace(componentName)) throw new ArgumentNullException(nameof(componentName));
        if (string.IsNullOrEmpty(candidate)) { return; }

        // Note that characters outside the ASCII range[RFC20] MUST be percent-encoded
        // using the method defined in Section 2.1 of the generic URI
        // specification[RFC3986].
        if (!ABNF.isAllAscii(candidate))
        {
            throw new ArgumentException($"Invalid {componentName}: characters outside the ASCII range[RFC20] MUST be percent-encoded.");
        }
    }

    public static void ValidateFragment(string candidate)
    {
        if (string.IsNullOrEmpty(candidate)) { return; }
        // fragment = *(pchar / "/" / "?")
        if (!ABNF.isFragment(candidate))
        {
            throw new ArgumentException("""Invalid f-component: must match *( pchar / "/" / "?" )""");
        }
    }

    public static string NormalizePercentEncoding(string str)
    {
        // For consistency, URI producers and normalizers should use uppercase hexadecimal digits for all percent-encodings.
        // iterate through the string to find any % signs

        string result = "";
        int index = 0;
        do
        {
            if (str[index] != '%')
            {
                result += str[index];
                index += 1;
                continue;
            }

            // found a %
            string pct = str.Substring(index, 3);
            result += pct.ToUpperInvariant();
            index += 3;

        } while (index < str.Length);

        return result;
    }
}
