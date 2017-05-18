namespace ParserCombinatorsDemo.TemplatingEngine
{
	public class ParsedTemplate
	{
		public ParsedTemplate(string format, string[] parameters)
		{
			Format = format;
			Parameters = parameters;
		}

		public string Format { get; }
		public string[] Parameters { get; }
	}
}