using System.Collections.Generic;

namespace ParserCombinatorsDemo.Persons
{
	public class Person
	{
		public Person(string type, Property[] properties)
		{
			Type = type;
			Properties = properties;
		}

		public string Type { get; }
		public Property[] Properties { get; }
	}
}