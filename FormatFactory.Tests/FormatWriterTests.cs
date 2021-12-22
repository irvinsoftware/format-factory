using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Company.Entities;
using Company.Entities.Malformed;
using Irvin.FormatFactory;
using NUnit.Framework;
using InvalidDataException = Irvin.FormatFactory.InvalidDataException;

namespace TestProject
{
	[TestFixture]
	public class FormatWriterTests
	{
	    [Test]
	    public void Write_WritesOutCsvContentWithEscapedHeaders()
	    {
	        List<Person> items = new List<Person>();
            items.Add(new Person
            {
                FirstName = "Fred",
                LastName = "Flinstone",
                Age = 25,
                State = "UT",
                EnteredDate = new DateTime(2017, 1, 5),
                Amount = 159.36F
            });
            items.Add(new Person
            {
                FirstName = "Anna-Marie",
                LastName = "Sadler",
                Age = 55,
                State = "AB",
                EnteredDate = new DateTime(2016, 2, 19, 15, 33, 1),
                Amount = -45.02F
            });
            items.Add(new Person
            {
                FirstName = "Michael",
                LastName = "Van Dusen",
                Age = 2,
                State = "CA",
                EnteredDate = new DateTime(2016, 7, 6, 9, 0, 0),
                Amount = 34F
            });
            FormatOptions options = new FormatOptions
            {
                EscapeKind = EscapeKind.DoubleQuote
            };

	        StringBuilder builder = new StringBuilder();
	        FormatWriter.Instance.Write(builder, items, options);

            string expected =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine +
                "Fred,Flinstone,25,UT,01/05/2017 00:00:00,159.36" + Environment.NewLine +
                "Anna-Marie,Sadler,55,AB,02/19/2016 15:33:01,-45.02" + Environment.NewLine +
                "Michael,Van Dusen,2,CA,07/06/2016 09:00:00,34" + Environment.NewLine;
            Assert.AreEqual(expected, builder.ToString());
        }

        [Test]
        public void Write_WritesOutCsvContentWithEscapedHeaders_ForComprehensiveAttribute()
        {
            List<Person2> items = new List<Person2>();
            items.Add(new Person2
            {
                FirstName = "Fred",
                LastName = "Flinstone",
                Age = 25,
                State = "UT",
                EnteredDate = new DateTime(2017, 1, 5),
                Amount = 159.36F
            });
            items.Add(new Person2
            {
                FirstName = "Anna-Marie",
                LastName = "Sadler",
                Age = 55,
                State = "AB",
                EnteredDate = new DateTime(2016, 2, 19, 15, 33, 1),
                Amount = -45.02F
            });
            items.Add(new Person2
            {
                FirstName = "Michael",
                LastName = "Van Dusen",
                Age = 2,
                State = "CA",
                EnteredDate = new DateTime(2016, 7, 6, 9, 0, 0),
                Amount = 34F
            });

            StringBuilder builder = new StringBuilder();
            FormatWriter.Default.Write(builder, items);

            string expected =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine.Last() +
                "Fred,Flinstone,25,UT,01/05/2017 00:00:00,159.36" + Environment.NewLine.Last() +
                "Anna-Marie,Sadler,55,AB,02/19/2016 15:33:01,-45.02" + Environment.NewLine.Last() +
                "Michael,Van Dusen,2,CA,07/06/2016 09:00:00,34" + Environment.NewLine.Last();
            Assert.AreEqual(expected, builder.ToString());
        }

        [Test]
        public void Write_WritesOutCsvContentWithEscapedValues()
        {
            List<Person> items = new List<Person>();
            items.Add(new Person
            {
                FirstName = "Fred",
                LastName = "Flinstone",
                Age = 25,
                State = "UT",
                EnteredDate = new DateTime(2017, 1, 5),
                Amount = 159.36F
            });
            items.Add(new Person
            {
                FirstName = "Anna,Marie",
                LastName = "Sadler",
                Age = 55,
                State = "AB",
                EnteredDate = new DateTime(2016, 2, 9, 15, 33, 1),
                Amount = 1245.02F
            });
            items.Add(new Person
            {
                FirstName = "Michael",
                LastName = "Van Dusen",
                Age = 2,
                State = "Q,",
                EnteredDate = new DateTime(2016, 7, 6, 9, 0, 0),
                Amount = 34F
            });
            FormatOptions options = new FormatOptions
            {
                EscapeKind = EscapeKind.DoubleQuote
            };

            StringBuilder builder = new StringBuilder();
            FormatWriter.Instance.Write(builder, items, options);

            string expected =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine +
                "Fred,Flinstone,25,UT,01/05/2017 00:00:00,159.36" + Environment.NewLine +
                "\"Anna,Marie\",Sadler,55,AB,02/09/2016 15:33:01,1245.02" + Environment.NewLine +
                "Michael,Van Dusen,2,\"Q,\",07/06/2016 09:00:00,34" + Environment.NewLine;
            Assert.AreEqual(expected, builder.ToString());
        }

        [Test]
        public void Write_WritesOutCsvContentWithEscapedValues_ForComprehensiveAttribute()
        {
            List<Person2> items = new List<Person2>();
            items.Add(new Person2
            {
                FirstName = "Fred",
                LastName = "Flinstone",
                Age = 25,
                State = "UT",
                EnteredDate = new DateTime(2017, 1, 5),
                Amount = 159.36F
            });
            items.Add(new Person2
            {
                FirstName = "Anna,Marie",
                LastName = "Sadler",
                Age = 55,
                State = "AB",
                EnteredDate = new DateTime(2016, 2, 9, 15, 33, 1),
                Amount = 1245.02F
            });
            items.Add(new Person2
            {
                FirstName = "Michael",
                LastName = "Van Dusen",
                Age = 2,
                State = "Q,",
                EnteredDate = new DateTime(2016, 7, 6, 9, 0, 0),
                Amount = 34F
            });

            StringBuilder builder = new StringBuilder();
            FormatWriter.Default.Write(builder, items);

            string expected =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine.Last() +
                "Fred,Flinstone,25,UT,01/05/2017 00:00:00,159.36" + Environment.NewLine.Last() +
                "\"Anna,Marie\",Sadler,55,AB,02/09/2016 15:33:01,1245.02" + Environment.NewLine.Last() +
                "Michael,Van Dusen,2,\"Q,\",07/06/2016 09:00:00,34" + Environment.NewLine.Last();
            Assert.AreEqual(expected, builder.ToString());
        }

        [Test]
	    public void Write_WritesOutEnumNameByDefault()
	    {
	        Thing element = new Thing();
	        element.Goober = "ABCD";
            element.CompareKind = StringComparison.Ordinal;

	        string actual = FormatWriter.Instance.WriteSingle(element);

            StringAssert.StartsWith("ABCD;Ordinal", actual);
	    }

        [Test]
        public void Write_WritesOutEnumValueWhenSpecified()
        {
            Thing element = new Thing();
            element.Goober = "ABCD";
            element.CompareKind = StringComparison.Ordinal;
            element.C2 = StringComparison.CurrentCultureIgnoreCase;

            string actual = FormatWriter.Default.WriteSingle(element);

            StringAssert.StartsWith("ABCD;Ordinal;1", actual);
        }

        [Test]
        public void Write_WritesOutEnumNameWhenSpecified()
        {
            Thing element = new Thing();
            element.Goober = "ABCD";
            element.CompareKind = StringComparison.Ordinal;
            element.C2 = StringComparison.CurrentCultureIgnoreCase;
            element.C3 = StringComparison.InvariantCulture;

            string actual = FormatWriter.Instance.WriteSingle(element);

            Assert.AreEqual("ABCD;Ordinal;1;InvariantCulture\r\n", actual);
        }

        [Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "The field 'Gorzo' has an invalid length (chars are only 1 character long)")]
		public void Write_ThrowsException_IfCharDecoratedInappropriately()
		{
			BadE element = new BadE();

			FormatWriter.Default.WriteSingle(element);
		}

		[Test]
		public void Write_WritesOutValuesForMinMaxLengthFields_RegardlessOfFixedWidthSettings()
		{
			GoodD element = new GoodD();
			element.Gorzo = "fa";
			element.JackieChan = "la";

			StringBuilder container = new StringBuilder();
			TextWriter writer = new StringWriter(container);
			FormatWriter.Instance.Write(writer, new List<GoodD> {element});

			Assert.AreEqual("fazz,lazz,False|", container.ToString());
		}

		[Test]
		public void Write_WritesOutValuesForMinMaxLengthFields_RegardlessOfFixedWidthSettings_ForValueThatsBetweenMinimumAndMaximum()
		{
			GoodD element = new GoodD();
			element.Gorzo = "fafetnu";
			element.JackieChan = "la";

			StringBuilder container = new StringBuilder();
			TextWriter writer = new StringWriter(container);
			FormatWriter.Default.Write(writer, new List<GoodD> { element });

			Assert.AreEqual("fafetnu,lazz,False|", container.ToString());
		}

		[Test]
		public void Write_WritesOutValuesForMinMaxLengthFields_ForFixedMinWidthOnly()
		{
			GoodD element = new GoodD();
			element.Gorzo = "fafetnu";
			element.JackieChan = "la";
			element.WWF = true;

			StringBuilder container = new StringBuilder();
			TextWriter writer = new StringWriter(container);
			FormatWriter.Instance.Write(writer, new List<GoodD> { element });

			Assert.AreEqual("fafetnu,lazz,True |", container.ToString());
		}

		[Test]
		public void Write_WritesCorrectly_ForMixOfAccess()
		{
			GoodE element = new GoodE();
			element.Gorzo = "fafetnu";

			StringBuilder container = new StringBuilder();
			TextWriter writer = new StringWriter(container);
			FormatWriter.Default.Write(writer, new List<GoodE> { element });

			Assert.AreEqual("fafetnu,lazz,True |", container.ToString());
		}

		[Test]
		public void Write_OutputsTheDesiredResult_ForFixedWidthAttributeFields()
		{
			InterchangeControlLoop element = new InterchangeControlLoop();
			element.InterchangeSenderID = 615156491;
			element.InterchangeDate = new DateTime(2008, 1, 25);
			element.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
			element.InterchangeControlNumber = 4;
			element.UsageIndicator = 'T';

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + Environment.NewLine, actual);
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a field and a sub-record (violator: 'Name')")]
		public void Write_ThrowsException_IfFieldAndSubRecordsCombined()
		{
			List<ExampleA> elements = new List<ExampleA>
				{
					new ExampleA(),
					new ExampleA()
				};

			FormatWriter.Default.Write(elements);
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "In strict mode, all members must be decorated.")]
		public void Write_ThrowsException_IfInStrictMode_AndMemberNotDecorated()
		{
			FormatOptions writerOptions = new FormatOptions();
			writerOptions.UseStrictMode = true;

			StringBuilder container = new StringBuilder();
			FormatWriter.Instance.WriteSingle(container, new Order(), writerOptions);
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "Only types decorated as Records can be used.")]
		public void Write_ThrowsException_IfTryingToWriteNonRecord()
		{
			StringBuilder container = new StringBuilder();

			FormatWriter.Default.Write(container, new List<string> {"hi"});
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a field and a child element (violator: 'Name')")]
		public void Write_ThrowsException_IfFieldAndChildrenCombined()
		{
			List<ExampleB> elements = new List<ExampleB>
				{
					new ExampleB(),
					new ExampleB()
				};

			FormatWriter.Instance.Write(elements);
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a sub-record and a child element (violator: 'Name')")]
		public void Write_ThrowsException_IfFieldAndSubRecordCombined()
		{
		    List<ExampleC> elements = new List<ExampleC>
				{
					new ExampleC(),
					new ExampleC()
				};

		    FormatWriter.Default.Write(elements);
		}

		[Test]
		public void Write_WritesOutRecordInCorrectOrder_WithMixOfFieldsAndProperties()
		{
			Stats element = new Stats();
			element.Name = "greg";
			element.FigureA = 34;
			element.FigureB = 45;
			element.FigureD = 99;

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("greg,34,45,0,99,0000" + Environment.NewLine, actual);
		}

		 [Test]
		 public void Write_WritesOutRecordWithWeirdRules()
		 {
			 ProfessionalServiceLoop element = new ProfessionalServiceLoop();
			 element.ProcedureCodeParts.ProcedureCode = "S0500";
			 element.LineItemChargeAmount = 89.25m;
			 element.ServiceUnitCount = 1;
			 element.DiagnosisCodePointer1 = 1;
			 element.DiagnosisCodePointer2 = 1;

			 string actual = FormatWriter.Default.WriteSingle(element);

			 Assert.AreEqual("SV1*HC:S0500*89.25*UN*1***1:1~", actual);
		 }

		[Test]
		public void Write_WritesOutBareCsvRecord()
		{
			Order subject = OrderTestHelper.GetOrder();

			string actual = FormatWriter.Instance.WriteSingle(subject);

			Assert.AreEqual("ABCD1,Tom Hardy 1,12/31/2007,36" + Environment.NewLine, actual);
		}

        [Test]
        public void Write_WritesOutStandardCsvRecord_WithHeaderSpecifiedByOptions()
        {
            Order subject = OrderTestHelper.GetOrder();
            FormatOptions options = new FormatOptions
            {
                IncludeHeaders = true,
                FieldDelimiter = ","
            };

            string actual = FormatWriter.Default.WriteSingle(subject, options);

            Assert.AreEqual("OrderNumber,ShippingName,Ordered,OrderTotal\r\nABCD1,Tom Hardy 1,12/31/2007,36" + Environment.NewLine, actual);
        }

        [Test]
        public void Write_WritesOutStandardCsvRecord_WritesHeaders_WhenSpecifiedOnRecord()
        {
            Order2 subject = OrderTestHelper.GetOrder2();

            string actual = FormatWriter.Instance.WriteSingle(subject);

            Assert.AreEqual("OrderNumber,ShipName,Ordered,Total\r\nABCD1,Tom Hardy 1,12/31/2007,36\r\n", actual);
        }

        [Test]
        public void Write_WritesOutStandardCsvRecord_WritesHeaders_ForComprehensiveAttribute()
        {
            OrderC subject = OrderTestHelper.GetOrderC();

            string actual = FormatWriter.Default.WriteSingle(subject);

            Assert.AreEqual("OrderNumber,ShippingName,Ordered,OrderTotal\nABCD1,Tom Hardy 1,12/31/2007,36\n", actual);
        }

        [Test]
        public void Write_WritesOutStandardCsvRecord_WritesHeaders_WhenSpecifiedOnRecordAndOptions()
        {
            Order2 subject = OrderTestHelper.GetOrder2();
            FormatOptions options = new FormatOptions
            {
                IncludeHeaders = true,
                FieldDelimiter = ","
            };

            string actual = FormatWriter.Instance.WriteSingle(subject, options);

            Assert.AreEqual("OrderNumber,ShipName,Ordered,Total\r\nABCD1,Tom Hardy 1,12/31/2007,36\r\n", actual);
        }

        [Test]
        public void Write_WritesOutStandardCsvRecord_StillWritesHeaders_IfSupressedByOptions()
        {
            Order2 subject = OrderTestHelper.GetOrder2();
            FormatOptions options = new FormatOptions
            {
                IncludeHeaders = false,
                FieldDelimiter = ","
            };

            string actual = FormatWriter.Default.WriteSingle(subject, options);

            Assert.AreEqual("OrderNumber,ShipName,Ordered,Total\r\nABCD1,Tom Hardy 1,12/31/2007,36\r\n", actual);
        }

        [Test]
		public void Write_OutputsRecordsToStreamProperly()
		{
			List<Order> list = new List<Order>
				{
					OrderTestHelper.GetOrder(),
					OrderTestHelper.GetOrder(2)
				};

			MemoryStream stream = new MemoryStream();
			FormatWriter.Instance.Write(stream, list);

			Assert.AreNotEqual(0, stream.Position);
			stream.Position = 0;
			StreamReader r = new StreamReader(stream);
			string actual = r.ReadToEnd();
			Assert.AreEqual("ABCD1,Tom Hardy 1,12/31/2007,36" + Environment.NewLine + 
							"ABCD2,Tom Hardy 2,01/01/2008,37" + Environment.NewLine, 
							actual);
		}

		[TestCase("0001", 1)]
		[TestCase("0012", 12)]
		[TestCase("0123", 123)]
		[TestCase("1234", 1234)]
		[TestCase("123456", 123456)]
		public void Write_OutputsRecordWithMinimumWidth(string expected, int value)
		{
			Stats item = new Stats();
			item.Id = value;

			MemoryStream stream = new MemoryStream();
			FormatWriter.Default.WriteSingle(stream, item);

			Assert.AreNotEqual(0, stream.Position);
			stream.Position = 0;
			StreamReader r = new StreamReader(stream);
			string actual = r.ReadToEnd();
			Assert.AreEqual(",0,0,0,0," + expected + Environment.NewLine, actual);
		}

		[Test]
		public void Write_WritesOutPipeDelimitedRecord()
		{
			LegacyOrder subject = new LegacyOrder();
			subject.OrderNumber = "ABCD";
			subject.OrderTotal = 35m;
			subject.Ordered = new DateTime(2007, 12, 31);
			subject.ShippingName = "Tom Hardy";
			subject.IsActive = true;

			string actual = FormatWriter.Instance.WriteSingle(subject);

			Assert.AreEqual("ABCD|Tom Hardy|12/31/2007|35" + Environment.NewLine, actual);
		}

		[Test]
		public void Write_CanEscapeProperly()
		{
			Master element = new Master();
			element.FirstName = "tom";
			element.LastName = "dean";
			element.Description = "he's, very, cool";
			element.Money = 1234.45m;
			element.Detail = new Detail();

			FormatOptions writerOptions = new FormatOptions();
			writerOptions.AllowDelimitersAsEscapedContent = true;
			writerOptions.EscapeKind = EscapeKind.Backslash;

			string actual = FormatWriter.Default.WriteSingle(element, writerOptions);

			Assert.AreEqual("tom,dean,he's\\, very\\, cool,$1\\,234.45|~~~45;True|", actual);
		}

		[Test]
		public void Write_CanEscapeProperly_ForTransformation()
		{
			Master element = new Master();
			element.FirstName = "tom";
			element.LastName = "dean";
			element.Description = "he's, very, cool";
			element.Money = 1234.45m;
			element.Detail = new Detail();

			FormatOptions writerOptions = new FormatOptions();
			writerOptions.AllowDelimitersAsEscapedContent = true;
			writerOptions.EscapeKind = EscapeKind.Transform;
			writerOptions.TransformEscapeCharacter = '_';

			string actual = FormatWriter.Instance.WriteSingle(element, writerOptions);

			Assert.AreEqual("tom,dean,he's_ very_ cool,$1_234.45|~~~45;True|", actual);
		}

		[Test]
		[ExpectedException(typeof(InvalidDataException), ExpectedMessage = "The field delimiter (',') was found in the content for field 'Description' for element #1.")]
		public void Write_ThrowsException_IfEscapingNotAllowed()
		{
			Master element = new Master();
			element.FirstName = "tom";
			element.LastName = "dean";
			element.Description = "he's, very, cool";
			element.Money = 1234.45m;
			element.Detail = new Detail();

			FormatOptions writerOptions = new FormatOptions();
			writerOptions.AllowDelimitersAsEscapedContent = false;

			FormatWriter.Default.WriteSingle(element, writerOptions);
		}

		[Test]
		[ExpectedException(typeof(InvalidDataException), ExpectedMessage = "The value of 'Goober' cannot exceed 10 characters.")]
		public void Write_ThrowsException_IfSetToNotAllowTooLongValues()
		{
			Thing element = new Thing();
			element.Goober = "1234567890123";
			FormatOptions writerOptions = new FormatOptions();

			FormatWriter.Instance.WriteSingle(element, writerOptions);
		}

		[Test]
		[ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "The maximum length of a field cannot be less than the minimum width.")]
		public void Write_ThrowsException_IfWidthsNotDecoratedCorrectly()
		{
			BadD element = new BadD();
			element.Gorzo = "9";

			TextWriter writer = new StringWriter(new StringBuilder());
			FormatWriter.Default.WriteSingle(writer, element);
		}

		[Test]
		public void Write_Writes_ForParentChildLists()
		{
			List<SimpleHeader> elements = new List<SimpleHeader>();
			elements.Add(new SimpleHeader
				{
					FamilyName = "Hardy",
					MarriageDate = new DateTime(1983, 2, 9),
					Parents = new List<SimpleFooter>
						{
							new SimpleFooter { FirstName = "Tom", LastName = "Hardy", Age = 35},
							new SimpleFooter { FirstName = "Sheryl", LastName = "Hardy", Age = 33},
						}
				});
			elements[0].Children.Add(new SimpleFooter { FirstName = "Laurel", LastName = "Hardy", Age = 4});
			elements[0].Children.Add(new SimpleFooter { FirstName = "Tom Jr.", LastName = "Hardy", Age = 8 });

			string actual = FormatWriter.Instance.Write(elements);

			string expected = "Hardy,02091983|		Tom,Hardy,35.00|		Sheryl,Hardy,33.00|		Laurel,Hardy,4.00|		Tom Jr.,Hardy,8.00|";
			Assert.AreEqual(expected, actual);
		}

	    [Test]
	    public void Write_WritesAllFixedWidth()
	    {
	        FixItFelix item = new FixItFelix();
	        item.ID = 290348;
	        item.Name = "Ralph Nader";
	        item.Amount = 23.3m;

	        string actual = FormatWriter.Default.WriteSingle(item);

            Assert.AreEqual("0000290348Ralph Nader                   23.3      \r\n", actual);
	    }
	}
}