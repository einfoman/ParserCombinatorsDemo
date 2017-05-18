using System.Collections.Generic;

namespace ParserCombinatorsDemo.TemplatingEngine
{
	public class ParserContext
	{
		private readonly List<string> parameters = new List<string>();

		public void AddParameter(string name) => parameters.Add(name);

		public int Index => parameters.Count;

		public IReadOnlyCollection<string> Parameters => parameters.AsReadOnly();
	}
}