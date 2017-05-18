using System;
using NUnit.Framework;
using Sprache;

namespace ParserCombinatorsDemo.Authentication
{
	[TestFixture]
	public class Challenge_Parse_Test
	{
		[Test]
		public void Parse_Basic_Correct_ShouldBe_Ok()
		{
			const string authHeader = "Basic realm=\"FooCorp\"";

			var actual = Grammar.ParseChallenge(authHeader);
			Console.WriteLine($"Scheme: {actual.Scheme}");
			Console.WriteLine($"Parameters:");
			foreach (var p in actual.Parameters)
			{
				Console.WriteLine($"- {p.Name} = {p.Value}");
			}

			Assert.That(actual.Scheme, Is.EqualTo("Basic"));
			Assert.That(actual.Parameters.Length, Is.EqualTo(1));
			Assert.That(actual.Parameters[0].Name, Is.EqualTo("realm"));
			Assert.That(actual.Parameters[0].Value, Is.EqualTo("FooCorp"));
		}

		[Test]
		public void Parse_Bearer_Correct_ShouldBe_Ok()
		{
			const string authHeader = "Bearer realm=\"FooCorp\", error=invalid_token, error_description=\"The access token has expired\"";

			var actual = Grammar.ParseChallenge(authHeader);
			Console.WriteLine($"Scheme: {actual.Scheme}");
			Console.WriteLine($"Parameters:");
			foreach (var p in actual.Parameters)
			{
				Console.WriteLine($"- {p.Name} = {p.Value}");
			}

			Assert.That(actual.Scheme, Is.EqualTo("Bearer"));
			Assert.That(actual.Parameters.Length, Is.EqualTo(3));
			Assert.That(actual.Parameters[0].Name, Is.EqualTo("realm"));
			Assert.That(actual.Parameters[0].Value, Is.EqualTo("FooCorp"));
			Assert.That(actual.Parameters[1].Name, Is.EqualTo("error"));
			Assert.That(actual.Parameters[1].Value, Is.EqualTo("invalid_token"));
			Assert.That(actual.Parameters[2].Name, Is.EqualTo("error_description"));
			Assert.That(actual.Parameters[2].Value, Is.EqualTo("The access token has expired"));
		}

		[Test]
		public void Parse_Bearer_Incorrect_ShouldBe_Throw_Exception()
		{
			const string authHeader = "Bearerrealm=\"FooCorp\", error=invalid_token, error_description=\"The access token has expired\"";

			var actual = Assert.Throws<ParseException>(() => Grammar.ParseChallenge(authHeader));

			Console.WriteLine(actual.Message);
		}
	}
}