using System.Globalization;

namespace GoodToCode.Analytics.Abstractions
{
    public static class StringExtensions
    {
        public static bool IsValidIdentifier(this string item)
        {
            var returnValue = false;
            var firstChar = true;
            foreach (var character in item)
            {
                switch (char.GetUnicodeCategory(character))
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.OtherLetter:
                        returnValue = true;
                        break;
                    case UnicodeCategory.LetterNumber:
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.ConnectorPunctuation:
                    case UnicodeCategory.Format:
                        returnValue = !firstChar;
                        break;
                    default:
                        returnValue = false;
                        break;
                }
                if (!returnValue) break;
                firstChar = false;
            }

            return returnValue;
        }

        public static string ToIdentifier(this string item)
        {
            return item.Replace(" ", "").Replace("-", "_").Replace(".", "");
        }
    }
}