using NUnit.Framework;

namespace ParserCombinatorsDemo.Persons
{
	[TestFixture]
	public class PersonParser_Test
	{
		[Test]
		public void Parse()
		{
			const string input = @"magician
[
     name      'Merlin'
     age   100500
]";

			var person = PersonParser.ParsePerson(input);

			Assert.That(person.Type, Is.EqualTo("magician"));
			Assert.That(person.Properties.Length, Is.EqualTo(2));
			Assert.That(person.Properties[0].Name, Is.EqualTo("name"));
			Assert.That(person.Properties[0].Value, Is.EqualTo("Merlin"));
			Assert.That(person.Properties[1].Name, Is.EqualTo("age"));
			Assert.That(person.Properties[1].Value, Is.EqualTo(100500));
		}
	}
}