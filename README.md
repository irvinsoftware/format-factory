# FormatFactory 1.0.0

_**The best tool for building and parsing flat file formats (e.g., EDI, CSV, NATCHA, etc.)**_

**FormatFactory** uses attribute decoration to translate objects to and from a file format. The two key attributes are `Record` and `Field`. `Record` is used to decorate the class, and represents the row / line / record level. `Field` is used to decorate fields and properties, and represents the column / field level. You then use either `FormatReader` and/or `FormatWriter` to read and/or write data to streams and/or files as needed.

```
[Record(FieldDelimiter = ",")]
public class Stats
{
   [Field(Order = 1)]
   public string Name { get; set; }

   [Field(Order = 2)]
   public int FigureA;

   [Field(Order = 3)]
   public int FigureB;

   [Field(Order = 4)]
   public int FigureC { get { return 0; } }

   [Field(Order = 5)]
   public int FigureD;
}

FormatWriter.Instance.Write("file.csv", listOfStatsObjects);
```

Both class fields and properties (including read-only ones) can be used as file fields. You can explicitly specify the order of the fields: this is not required, but the output order where `Order` is not specified is not guaranteed. (No two elements may have the same order.) You can also use the `WriteSingle()` method if you are not working with a list.

Additional details about `Record`:

- property **UseStrictMode** - Strict Mode means properties & fields not decorated will cause an error.
- property **FieldDelimiter** - you can specify this delimiter here instead of on each field.
- property **UseTrailingDelimiter** - some formats require the field delimiter to appear the end of the line, in addition to the record delimiter.
- property **IncludeHeaders** - a header row will be printed/expected (useful for formats like CSV)

Additional details about `Field`:

- property **IsFixedWidth** - Specifies whether the field must have always have the same width (as specified by property **MaximumLength**).
  - You can also use the attribute `FixedWidthField` to be more explicit in this scenario.
- property **AllowTruncation** - if the field has a maximum length, and the assigned value exceeds it, this property controls whether the value is truncated, or an exception is thrown
- property **Delimiter** - the delimiter can be different for each field if needed/desired
- property **Format** - specifies a string format for the field value
- property **Alignment**
- property **PaddingCharacter** - when a value is shorter than the specified width, additional padding can be added. The default is a space.
- property **Name** - the name that is used in the header. If this is not specified, and you are using headers, it will default to the name of the property or field.

### Complex Formatting

Sometimes a file does not have a consistent format throughout - the delimiters and shape of the records may change from section to section. An example might be an EDI file that has different delimiters for some fields, or a file that combines comma- and tab-delimited rows. For these scenarios, there is `ChildElement` and `SubRecord`.

`ChildElement` is used when a record has accompanying child record(s) that have different shapes from the parent record and/or from each other.

```
[Record]
public class Batch
{
   [ChildElement]
   public BatchHeader BatchHeader { get; set; }
   
   [ChildElement]
   public List<EntryDetail> EntryDetails { get; set; }
   
   [ChildElement]
   public BatchControl BatchControl { get; set; }
}
```

*BatchHeader*, *EntryDetail*, and *BatchControl* are each decorated as `Record` and have very different shapes. In this way heterogenous and heirarchical data can be represented.

`SubRecord` is used when a group of fields have a different format than the rest of the record. For example:

```
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
   public ProcedureCodeElements ProcedureCodeParts { get; }
   
   [Field]
   public decimal LineItemChargeAmount { get; set; }
}

[Record(FieldDelimiter = ":")]
public class ProcedureCodeElements
{
   [Field]
   public string ProductOrServiceQualifier { get { return "HC"; } }

   [Field]
   public string ProcedureCode { get; set; }
}
```

If *LineItemChargeAmount* is 89.99 and *ProcedureCode* is set to "ABCD", this would produce the text `SV1*HC:ABCD*89.99~`. (Again, *ProcedureCodeElements* must be decorated as a `Record`.)
