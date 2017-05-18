using System;

namespace ParserCombinatorsDemo.Persons
{
	public class Property
	{
		public Property(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			Name = name;
			Value = value;
		}

		public string Name { get; }

		public object Value { get; }
	}
}