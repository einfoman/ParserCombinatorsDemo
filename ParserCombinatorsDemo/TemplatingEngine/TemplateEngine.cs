using System.Linq;
using System.Text;
using Sprache;

namespace ParserCombinatorsDemo.TemplatingEngine
{
	public static class TemplateParser
	{
		public interface IToken
	{
		string FormatToken(ParserContext context);
	}

	public class ParameterToken : IToken
	{
		private readonly string name;

		public ParameterToken(string name)
		{
			this.name = name;
		}

		public string FormatToken(ParserContext context)
		{
			var result = $"{{{context.Index}}}";
			context.AddParameter(name);
			return result;
		}
	}

	public class TextToken : IToken
	{
		private readonly string value;

		public TextToken(string value)
		{
			this.value = value;
		}

		public string FormatToken(ParserContext context)
		{
			return value;
		}
	}

		private static readonly Parser<string> identifier = Parse
			.LetterOrDigit
			.Or(Parse.Char('_'))
			.AtLeastOnce()
			.Token()
			.Text();

		private static readonly Parser<IToken> text =
			from text in Parse.AnyChar.Except(Parse.Chars('{', '}'))
				.Many()
				.Text()
			select new TextToken(text);

		private static readonly Parser<IToken> parameterToken =
			from token in identifier.Contained(
				Parse.String("{{"),
				Parse.String("}}"))
			select new ParameterToken(token);

		private static readonly Parser<IToken[]> parser =
			from tokens in text.Or(parameterToken).Many().End()
			select tokens.ToArray();

		public static ParsedTemplate ParseTemplate(string template)
		{
			var tokens = parser.Parse(template);
			var result = new StringBuilder();
			var parserParameters = new ParserContext();
			foreach (var token in tokens)
			{
				result.Append(token.FormatToken(parserParameters));
			}

			return new ParsedTemplate(result.ToString(), parserParameters.Parameters.ToArray());
		}
	}
}