using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(RecordDelimiter = "~", FieldDelimiter = "*")]
	public class ProfessionalServiceLoop
	{
		public ProfessionalServiceLoop()
		{
			ProcedureCodeParts = new ProcedureCodeElements();
		}

		[Field]
		public string Prefix { get { return "SV1"; } }

		[SubRecord]
		public ProcedureCodeElements ProcedureCodeParts { get; private set; }

		[Field]
		public decimal LineItemChargeAmount { get; set; }

		[Field]
		public string UnitOfMeasure { get { return "UN"; } }

		[Field]
		public int ServiceUnitCount { get; set; }

		[Field]
		public int? FacilityCode { get; set; }

		[Field]
		public string CompositeCodePointer { get; set; }

		[Field(Delimiter = ":", Optional = true)]
		public int? DiagnosisCodePointer1 { get; set; }

		[Field(Delimiter = ":", Optional = true)]
		public int? DiagnosisCodePointer2 { get; set; }

		[Field(Delimiter = ":", Optional = true)]
		public int? DiagnosisCodePointer3 { get; set; }

		[Field(Optional = true)]
		public int? DiagnosisCodePointer4 { get; set; }

		[Field(Optional = true)]
		public char? EmergencyIndicator { get; set; }

		[Record(FieldDelimiter = ":")]
		public class ProcedureCodeElements
		{
			[Field]
			public string ProductOrServiceQualifier { get { return "HC"; } }

			[Field]
			public string ProcedureCode { get; set; }

			[Field(Optional = true)]
			public string Modifier1 { get; set; }

			[Field(Optional = true)]
			public string Modifier2 { get; set; }

			[Field(Optional = true)]
			public string Modifier3 { get; set; }

			[Field(Optional = true)]
			public string Modifier4 { get; set; }
		}
	}
}