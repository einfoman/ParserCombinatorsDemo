using System;
using System.Linq;
using Sprache;

namespace ParserCombinatorsDemo.Authentication
{
	public class Challenge
	{
		public Challenge(string scheme, Parameter[] parameters)
		{
			Scheme = scheme;
			Parameters = parameters;
		}

		public string Scheme { get; }
		public Parameter[] Parameters { get; }
	}

	public class Parameter
	{
		public Parameter(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public string Value { get; }
	}

	/// <summary>
	/// The WWW-Authenticate header is described in detail in RFC-2617. The grammar looks like this, in what the RFC calls "augmented Backus-Naur Form" (see RFC 2616 §2.1)
	/// </summary>
	public static class Grammar
	{
		/**
		 * separators     = "(" | ")" | "<" | ">" | "@"
                          | "," | ";" | ":" | "\" | <">
                          | "/" | "[" | "]" | "?" | "="
                          | "{" | "}" | SP | HT
		 */
		private static readonly Parser<char> separatorChar =
			Parse.Chars("()<>@,;:\\\"/[]?={} \t");

		/**
		 * CTL char
		 * */
		private static readonly Parser<char> controlChar =
			Parse.Char(Char.IsControl, "Control character");


		// Parsing tokens

		/*
		 * token          = 1*<any CHAR except CTLs or separators>
		 */
		private static readonly Parser<char> tokenChar =
			Parse.AnyChar
				.Except(separatorChar)
				.Except(controlChar);

		private static readonly Parser<string> token =
			tokenChar.AtLeastOnce().Text();

		// Parsing quoted strings

		private static readonly Parser<char> doubleQuote = Parse.Char('"');
		private static readonly Parser<char> backslash = Parse.Char('\\');

		/*
		 * <any TEXT except <">>
		 */
		private static readonly Parser<char> qdText =
			Parse.AnyChar.Except(doubleQuote);

		/*
		 * quoted-pair    = "\" CHAR
		 */
		private static readonly Parser<char> quotedPair =
			from _ in backslash
			from c in Parse.AnyChar
			select c;

		/*
		 * quoted-string  = ( <"> *(qdtext | quoted-pair ) <"> )
		 */
		private static readonly Parser<string> quotedString =
			from open in doubleQuote
			from text in quotedPair.Or(qdText).Many().Text()
			from close in doubleQuote
			select text;

		/*
		 * auth-param     = token "=" ( token | quoted-string )
		 */
		private static readonly Parser<string> parameterValue =
			token.Or(quotedString);

		private static readonly Parser<char> equalSign = Parse.Char('=');

		/*
		 * auth-param     = token "=" ( token | quoted-string )
		 */
		private static readonly Parser<Parameter> parameter =
			from name in token
			from _ in equalSign
			from value in parameterValue
			select new Parameter(name, value);

		private static readonly Parser<char> comma = Parse.Char(',');

		private static readonly Parser<char> listDelimiter =
			from leading in Parse.WhiteSpace.Many()
			from c in comma
			from trailing in Parse.WhiteSpace.Or(comma).Many()
			select c;

		private static readonly Parser<Parameter[]> parameters =
			from p in parameter.DelimitedBy(listDelimiter)
			select p.ToArray();

		// Parsing the challenge

		/*
		 * challenge      = auth-scheme 1*SP 1#auth-param
		 */
		private static readonly Parser<Challenge> challenge =
			from scheme in token
			from _ in Parse.WhiteSpace.AtLeastOnce()
			from parameters in parameters
			select new Challenge(scheme, parameters);

		public static Challenge ParseChallenge(string input)
		{
			return challenge.Parse(input);
		}
	}
}