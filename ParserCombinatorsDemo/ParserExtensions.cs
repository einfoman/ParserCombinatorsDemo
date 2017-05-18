using System;
using Sprache;

namespace ParserCombinatorsDemo
{
	public static class ParserExtensions
	{
		public static Parser<T> SurroundByWhiteSpace<T>(this Parser<T> parser)
		{
			if (parser == null) throw new ArgumentNullException(nameof(parser));

			var containedParser = Parse.WhiteSpace.Except(NewLine).Many();
			return parser.Contained(containedParser, containedParser);
		}

		private static readonly Parser<string> NewLine = Parse
			.String(Environment.NewLine)
			.Text()
			.Named("new line");
	}
}