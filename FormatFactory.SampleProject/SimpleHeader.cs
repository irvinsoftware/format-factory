using System;
using System.Collections.Generic;
using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(RecordDelimiter="|", FieldDelimiter = ",")]
	public class SimpleHeader
	{
		public SimpleHeader()
		{
			Parents = new List<SimpleFooter>();
			Children = new List<SimpleFooter>();
		}

		[Field(1)]
		public string FamilyName { get; set; }

		[Field(2, FieldFormat.AmericanStyleLongYearUndelimitedDate)]
		public DateTime MarriageDate { get; set; }

		[ChildElement(IndentCharacters = "\t\t")]
		public IEnumerable<SimpleFooter> Parents { get; set; }

		[ChildElement(IndentCharacters = "\t\t")]
		public List<SimpleFooter> Children { get; set; }
	}

	[Record(RecordDelimiter = "|", FieldDelimiter = ",")]
	public class SimpleFooter
	{
		[Field]
		public string FirstName { get; set; }

		[Field]
		public string LastName { get; set; }

		[Field("0.00")]
		public int Age { get; set; }
	}
}