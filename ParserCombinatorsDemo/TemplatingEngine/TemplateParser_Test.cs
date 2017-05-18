using System;
using NUnit.Framework;

namespace ParserCombinatorsDemo.TemplatingEngine
{
	[TestFixture]
	public class TemplateParser_Test
	{
		[Test]
		public void Parse()
		{
			const string template = "{{Caption}} №{{Number}} от {{Date}}";

			var actual = TemplateParser.ParseTemplate(template);

			Console.WriteLine(actual.Format);
			Assert.That(actual.Format, Is.EqualTo("{0} №{1} от {2}"));
			Assert.That(actual.Parameters, Is.EqualTo(new[] {"Caption", "Number", "Date"}));
		}
	}
}