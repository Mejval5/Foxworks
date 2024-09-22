using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Foxworks.Utils
{
    public static class StringUtils
    {
	    /// <summary>
	    ///     Returns the case spaced string of the string passed as parameter.
	    ///     Regex found on:https://stackoverflow.com/questions/21326963/splitting-camelcase-with-regex
	    /// </summary>
	    /// <param name="str"></param>
	    /// <returns></returns>
	    public static string ToCaseSpacedString(this string str)
        {
            return Regex.Replace(str, @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})", " ").Trim();
        }

	    /// <summary>
	    ///     Returns the type's name as case spaced string.
	    /// </summary>
	    /// <param name="type"></param>
	    /// <returns></returns>
	    public static string ToCaseSpacedString(this Type type)
        {
            return type.Name.ToCaseSpacedString();
        }

	    /// <summary>
	    ///     Converts the first letter of the string to lower case
	    /// </summary>
	    /// <param name="str"></param>
	    /// <returns></returns>
	    public static string ToCamelCaseString(this string str)
        {
            return str.Length > 0 ? char.ToLowerInvariant(str[0]) + str[1..] : string.Empty;
        }

	    /// <summary>
	    ///     Returns name of enum in snake case
	    /// </summary>
	    public static string ToSnakeCase(this Enum enumeration)
        {
            return enumeration?.ToString() == null ? null : enumeration.ToString().ToSnakeCase();
        }

	    /// <summary>
	    ///     Converts string to snake case
	    /// </summary>
	    public static string ToSnakeCase(this string str)
	    {
		    return str == null ? null : Regex.Replace(str, "(?<!^)[A-Z0-9]", match => $"_{match.Value}").ToLower();
	    }

	    /// <summary>
	    ///     Check if the given string is a valid email address
	    /// </summary>
	    /// <param name="email"></param>
	    /// <returns></returns>
	    public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Attempt to convert the string into an email address
                MailAddress _ = new(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

	    /// <summary>
	    ///     Generates a random document ID.
	    ///     This is useful when you want to generate a random ID for a document.
	    ///     Generally, it is better to use a GUID for this purpose.
	    /// </summary>
	    /// <returns></returns>
	    public static string GetRandomDocumentID()
        {
            const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int charAmount = 20;
            string myString = "";
            for (int i = 0; i < charAmount; i++)
            {
                myString += glyphs[Random.Range(0, glyphs.Length)];
            }

            return myString;
        }
	    
		public static string JoinIgnoringNullAndEmpty(string separator, IEnumerable<string> values)
		{
			return string.Join(separator, values.Where(s => !string.IsNullOrEmpty(s)));
		}

		/// <summary>
		/// Formats a variable name into a more readable form by removing common prefixes
		/// and inserting spaces before capital letters.
		/// </summary>
		/// <param name="name">The original variable name.</param>
		/// <returns>A nicified version of the variable name.</returns>
		public static string NicifyVariableName(string name)
		{
			// Remove common prefixes: m_, _, k
			string cleanedName = Regex.Replace(name, @"^(m_|_|k)(?=[A-Z])", "");

			// Insert spaces before capital letters, but not at the beginning
			string nicifiedName = Regex.Replace(cleanedName, "(?<!^)([A-Z])", " $1");

			return nicifiedName;
		}

		/// <summary>
		/// Escape line breaks in a string to be able to store it in a single line.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string EscapeLineBreaks(this string text)
		{
			return text.Replace("\n", "{n}").Replace("\r", "{r}");
		}

		/// <summary>
		/// Get original multiline format of a line break escaped string.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string UnescapeLineBreaks(this string text)
		{
			return text.Replace("{n}", "\n").Replace("{r}", "\r");
		}
    }
}