using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MvcSiteMapProvider.Text
{
    public static class StringExtensions
    {
        // C# keywords: http://msdn.microsoft.com/en-us/library/x53a06bb(v=vs.71).aspx
        private static string[] keywords = new[]
        {
            "abstract", "event", "new", "struct",
            "as", "explicit", "null", "switch",
            "base", "extern", "object", "this",
            "bool", "false", "operator", "throw",
            "breal", "finally", "out", "true",
            "byte", "fixed", "override", "try",
            "case", "float", "params", "typeof",
            "catch", "for", "private", "uint",
            "char", "foreach", "protected", "ulong",
            "checked", "goto", "public", "unchekeced",
            "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort",
            "continue", "in", "return", "using",
            "decimal", "int", "sbyte", "virtual",
            "default", "interface", "sealed", "volatile",
            "delegate", "internal", "short", "void",
            "do", "is", "sizeof", "while",
            "double", "lock", "stackalloc",
            "else", "long", "static",
            "enum", "namespace", "string"
        };

        // definition of a valid C# identifier: http://msdn.microsoft.com/en-us/library/aa664670(v=vs.71).aspx
        private const string formattingCharacter = @"\p{Cf}";
        private const string connectingCharacter = @"\p{Pc}";
        private const string decimalDigitCharacter = @"\p{Nd}";
        private const string combiningCharacter = @"\p{Mn}|\p{Mc}";
        private const string letterCharacter = @"\p{Lu}|\p{Ll}|\p{Lt}|\p{Lm}|\p{Lo}|\p{Nl}";
        private const string identifierPartCharacter = letterCharacter + "|" +
            decimalDigitCharacter + "|" +
            connectingCharacter + "|" +
            combiningCharacter + "|" +
            formattingCharacter;
        private const string identifierPartCharacters = "(" + identifierPartCharacter + ")+";
        private const string identifierStartCharacter = "(" + letterCharacter + "|_)";
        private const string identifierOrKeyword = identifierStartCharacter + "(" +
            identifierPartCharacters + ")*";
        private static Regex validIdentifierRegex = new Regex("^" + identifierOrKeyword + "$", RegexOptions.Compiled);


        /// <summary>
        /// Determines if a string matches a valid C# identifier according to the C# language specification (including Unicode support).
        /// </summary>
        /// <param name="identifier">The identifier being analyzed.</param>
        /// <returns><b>true</b> if the identifier is valid, otherwise <b>false</b>.</returns>
        /// <remarks>Source: https://gist.github.com/LordDawnhunter/5245476 </remarks>
        public static bool IsValidIdentifier(this string identifier)
        {
            if (string.IsNullOrEmpty(identifier)) return false;
            var normalizedIdentifier = identifier.Normalize();

            // 1. check that the identifier match the validIdentifer regular expression and it's not a C# keyword
            if (validIdentifierRegex.IsMatch(normalizedIdentifier) && !keywords.Contains(normalizedIdentifier))
            {
                return true;
            }

            // 2. check if the identifier starts with @
            if (normalizedIdentifier.StartsWith("@") && validIdentifierRegex.IsMatch(normalizedIdentifier.Substring(1)))
            {
                return true;
            }

            // 3. it's not a valid identifier
            return false;
        }
    }
}
