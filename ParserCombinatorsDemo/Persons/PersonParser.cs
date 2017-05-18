using System.Linq;
using Sprache;

namespace ParserCombinatorsDemo.Persons
{
	public static class PersonParser
	{
		private static readonly Parser<string> identifier =
			Parse.LetterOrDigit
			.AtLeastOnce()
			.Token()
			.Text();

		private static readonly Parser<object> stringValue =
			from openQuote in Parse.Char('\'')
			from value in Parse.LetterOrDigit.AtLeastOnce().Text()
			from closedQuote in Parse.Char('\'')
			select value;
		private static readonly Parser<object> numericValue =
			from value in Parse.Number
			select (object)int.Parse(value);

		private static readonly Parser<Property> property =
			from name in identifier.SurroundByWhiteSpace()
			from value in stringValue.Or(numericValue).SurroundByWhiteSpace()
			select new Property(name, value);

		private static readonly Parser<Person> person =
			from type in identifier
			from lbracket in Parse.Char('[')
			from properties in property.AtLeastOnce()
			from _ in Parse.LineEnd
			from rbracket in Parse.Char(']').End()
			select new Person(type, properties.ToArray());

		public static Person ParsePerson(string input)
		{
			return person.Parse(input);
		}
	}
}