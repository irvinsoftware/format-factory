namespace Company.Nacha.NachaElements.Enum
{
	public enum RecordTypeCode
	{
		FileHeaderRecord = 1,
		BatchHeaderRecord = 5,
		EntryDetailRecord = 6,
		EntryDetailAddendaRecord = 7,
		BatchControlRecord = 8,
		FileControlRecord = 9
	}
}