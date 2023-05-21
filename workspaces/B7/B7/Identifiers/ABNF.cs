namespace B7.Identifiers;

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
 */

internal class ABNF
{
    static readonly char[] ALPHA = new char[] {
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
    static readonly char[] DIGIT = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    static readonly char[] HEXDIG = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'A', 'B', 'C', 'D', 'E', 'F' }.Concat(DIGIT);
    static readonly char CR = '\r';
    static readonly char LF = '\n';
    static readonly char SP = ' ';
    static readonly char DQUOTE = '"';
    static readonly char hyphen = '-';
    static readonly char period = '.';
    static readonly char underscore = '_';
    static readonly char tilde = '~';
    static readonly char pct = '%';

    static readonly char[] alphanum = ALPHA.Concat(DIGIT);
    static readonly char[] ldh = alphanum.Add(hyphen);
    static readonly char[] unreserved = alphanum.Concat(new[] { hyphen, period, underscore, tilde });

    static readonly char[] gendelims = new char[] { ':', '/', '?', '#', '[', ']', '@' };
    static readonly char[] subdelims = new[] { '!', '$', '&', '(', ')', '+', ',', ';', '=' };
    static readonly char[] reserved = gendelims.Concat(subdelims);

    static readonly char[] defaultPChars = unreserved.Concat(subdelims).Concat(new char[] { ':', '@' });

    public static bool isAlphaNum(char chr)
    {
        return alphanum.Contains(chr);
    }

    public static bool isLDH(char chr)
    {
        return ldh.Contains(chr);
    }

    public static bool isHex(char chr)
    {
        return HEXDIG.Contains(chr);
    }

    public static bool IsPctEncoded(String input)
    {
        return input.Length == 3 && input[0] == pct && isHex(input[1]) && isHex(input[2]);
    }

    public static bool isPChar(string str, int index)
    {
        return isPChar(str, index, Array.Empty<char>());
    }

    public static bool isPChar(string str, int index, char additionalAllowedChar)
    {
        char[] additionalAllowedChars = new char[1] { additionalAllowedChar };
        return isPChar(str, index, additionalAllowedChars);
    }

    public static bool isPChar(string str, int index, string additionalAllowedChars)
    {
        char[] aac = additionalAllowedChars.ToArray<char>();
        return isPChar(str, index, aac);
    }

    public static bool isPChar(string str, int index, char[] additionalAllowedChars)
    {
        if (str.Length < index + 1)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        /*
         * pchar       = unreserved / pct-encoded / sub-delims / ":" / "@"
         *      
         * unreserved  = ALPHA / DIGIT / "-" / "." / "_" / "~"
         * pct-encoded = "%" HEXDIG HEXDIG
         * sub-delims  = "!" / "$" / "&" / "'" / "(" / ")" / "*" / "+" / "," / ";" / "="
         */

        if (str[index] == '%')
        {
            return IsPctEncoded(str.Substring(index, 3));
        }

        // 		allAllowedChars	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-._~!$&'()+,;=:@System.Char[]"	string

        additionalAllowedChars ??= Array.Empty<char>();
        char[] allowedChars = defaultPChars.Concat(additionalAllowedChars);

        char toTest = str[index];

        return allowedChars.Contains(toTest);
    }

    public static bool isDigit(char chr)
    {
        return DIGIT.Contains(chr);
    }

    public static bool isAllAscii(string candidate)
    {
        return candidate.All(char.IsAscii);
    }

    public static bool isFragment(string candidate)
    {
        // fragment      = *( pchar / "/" / "?" )         

        if (candidate == null) { return false; }
        char[] additionalChars = new char[2] { '/', '?' };

        for (int index = 0; index < candidate.Length; index++)
        {
            if (!isPChar(candidate, index, additionalChars))
            {
                return false;
            }
        }
        return true;
    }
}

