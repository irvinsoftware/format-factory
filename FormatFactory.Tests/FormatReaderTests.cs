using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Company.Entities;
using Company.Entities.Malformed;
using Irvin.FormatFactory;
using Company.Nacha.NachaElements;
using NUnit.Framework;
using InvalidDataException = Irvin.FormatFactory.InvalidDataException;

namespace TestProject
{
    [TestFixture]
    public class FormatReaderTests
    {
        private const string NON_DATA_FILE = @"c:\windows\system32\drivers\etc\hosts";
        private string _temporaryFilename;

        [TestFixtureSetUp]
        public void RunFirstOnce()
        {
            _temporaryFilename = Path.GetTempFileName();
        }

        [SetUp]
        public void RunBeforeEachTest()
        {
            if (File.Exists(_temporaryFilename))
            {
                File.Delete(_temporaryFilename);
            }
        }

        [TestCase(typeof(Person))]
        [TestCase(typeof(Person2))]
        public void Read_ReadsEscapedHeaders_ForCsvWithUniversalQuoting(Type recordType)
        {
            string content =
                "\"Given Name\",\"Family Name\",\"Age\",\"State,Province\",\"Created\",\"Balance (USD)\"" + Environment.NewLine +
                "\"Fred\",\"Flinstone\",\"25\",\"UT\",\"1/5/2017\",\"159.36\"" + Environment.NewLine +
                "\"Anna-Marie\",\"Sadler\",\"55\",\"AB\",\"2/9/2016 15:33:01\",\"-45.02\"" + Environment.NewLine +
                "\"Michael\",\"Van Dusen\",\"2\",\"CA\",\"7/6/2016 3:00 PM\",\"34\"" + Environment.NewLine;
            FormatOptions options = new FormatOptions {EscapeKind = EscapeKind.DoubleQuote};

            List<IPerson> actual = ParseForPersons(recordType, content, options);

            Assert.AreEqual(3, actual.Count);

            Assert.AreEqual("Fred", actual[0].FirstName);
            Assert.AreEqual("Flinstone", actual[0].LastName);
            Assert.AreEqual(25, actual[0].Age);
            Assert.AreEqual("UT", actual[0].State);
            Assert.AreEqual(new DateTime(2017, 1, 5), actual[0].EnteredDate);
            Assert.AreEqual(159.36F, actual[0].Amount);

            Assert.AreEqual("Anna-Marie", actual[1].FirstName);
            Assert.AreEqual("Sadler", actual[1].LastName);
            Assert.AreEqual(55, actual[1].Age);
            Assert.AreEqual("AB", actual[1].State);
            Assert.AreEqual(new DateTime(2016, 2, 9, 15, 33, 1), actual[1].EnteredDate);
            Assert.AreEqual(-45.02F, actual[1].Amount);

            Assert.AreEqual("Michael", actual[2].FirstName);
            Assert.AreEqual("Van Dusen", actual[2].LastName);
            Assert.AreEqual(2, actual[2].Age);
            Assert.AreEqual("CA", actual[2].State);
            Assert.AreEqual(new DateTime(2016, 7, 6, 15, 0, 0), actual[2].EnteredDate);
            Assert.AreEqual(34F, actual[2].Amount);
        }

        [TestCase(typeof(Person))]
        [TestCase(typeof(Person2))]
        public void Read_ReadsEscapedHeaders_ForCsv_HeaderQuotingOnly(Type recordType)
        {
            string content =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine +
                "Fred,Flinstone,25,UT,1/5/2017,159.36" + Environment.NewLine +
                "Anna-Marie,Sadler,55,AB,2/9/2016 15:33:01,-45.02" + Environment.NewLine +
                "Michael,Van Dusen,2,CA,7/6/2016 3:00 PM,34" + Environment.NewLine;
            FormatOptions options = new FormatOptions { EscapeKind = EscapeKind.DoubleQuote };

            List<IPerson> actual = ParseForPersons(recordType, content, options);

            Assert.AreEqual(3, actual.Count);

            Assert.AreEqual("Fred", actual[0].FirstName);
            Assert.AreEqual("Flinstone", actual[0].LastName);
            Assert.AreEqual(25, actual[0].Age);
            Assert.AreEqual("UT", actual[0].State);
            Assert.AreEqual(new DateTime(2017, 1, 5), actual[0].EnteredDate);
            Assert.AreEqual(159.36F, actual[0].Amount);

            Assert.AreEqual("Anna-Marie", actual[1].FirstName);
            Assert.AreEqual("Sadler", actual[1].LastName);
            Assert.AreEqual(55, actual[1].Age);
            Assert.AreEqual("AB", actual[1].State);
            Assert.AreEqual(new DateTime(2016, 2, 9, 15, 33, 1), actual[1].EnteredDate);
            Assert.AreEqual(-45.02F, actual[1].Amount);

            Assert.AreEqual("Michael", actual[2].FirstName);
            Assert.AreEqual("Van Dusen", actual[2].LastName);
            Assert.AreEqual(2, actual[2].Age);
            Assert.AreEqual("CA", actual[2].State);
            Assert.AreEqual(new DateTime(2016, 7, 6, 15, 0, 0), actual[2].EnteredDate);
            Assert.AreEqual(34F, actual[2].Amount);
        }

        [TestCase(typeof(Person))]
        [TestCase(typeof(Person2))]
        public void Read_ReadsEscapedHeaders_ForCsvMinimalQuoting(Type recordType)
        {
            string content =
                "Given Name,Family Name,Age,\"State,Province\",Created,Balance (USD)" + Environment.NewLine +
                "Fred,Flinstone,25,UT,1/5/2017,159.36" + Environment.NewLine +
                "Anna-Marie,Sadler,55,AB,2/9/2016 15:33:01,\"1,245.02\"" + Environment.NewLine +
                "Michael,Van Dusen,2,\"Q,\",7/6/2016 3:00 PM,34" + Environment.NewLine;
            FormatOptions options = new FormatOptions { EscapeKind = EscapeKind.DoubleQuote };

            List<IPerson> actual = ParseForPersons(recordType, content, options);

            Assert.AreEqual(3, actual.Count);

            Assert.AreEqual("Fred", actual[0].FirstName);
            Assert.AreEqual("Flinstone", actual[0].LastName);
            Assert.AreEqual(25, actual[0].Age);
            Assert.AreEqual("UT", actual[0].State);
            Assert.AreEqual(new DateTime(2017, 1, 5), actual[0].EnteredDate);
            Assert.AreEqual(159.36F, actual[0].Amount);

            Assert.AreEqual("Anna-Marie", actual[1].FirstName);
            Assert.AreEqual("Sadler", actual[1].LastName);
            Assert.AreEqual(55, actual[1].Age);
            Assert.AreEqual("AB", actual[1].State);
            Assert.AreEqual(new DateTime(2016, 2, 9, 15, 33, 1), actual[1].EnteredDate);
            Assert.AreEqual(1245.02F, actual[1].Amount);

            Assert.AreEqual("Michael", actual[2].FirstName);
            Assert.AreEqual("Van Dusen", actual[2].LastName);
            Assert.AreEqual(2, actual[2].Age);
            Assert.AreEqual("Q,", actual[2].State);
            Assert.AreEqual(new DateTime(2016, 7, 6, 15, 0, 0), actual[2].EnteredDate);
            Assert.AreEqual(34F, actual[2].Amount);
        }

        private static List<IPerson> ParseForPersons(Type recordType, string content, FormatOptions options)
        {
            if (recordType == typeof(Person))
            {
                List<Person> persons = FormatReader.Default.Read<Person>(content, options);
                return persons.Cast<IPerson>().ToList();
            }
            else
            {
                List<Person2> persons = FormatReader.Default.Read<Person2>(content);
                return persons.Cast<IPerson>().ToList();
            }
        }

        [Test]
        public void Read_ParsesEnumValue_AccordingToSpecifier()
        {
            Thing element = new Thing();
            element.Goober = "ABCD";
            element.CompareKind = StringComparison.Ordinal;
            element.C2 = StringComparison.CurrentCultureIgnoreCase;
            element.C3 = StringComparison.InvariantCulture;

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_CanParseBareCsvFormat()
        {
            string input = "toad,\"gargalong\"" + Environment.NewLine + "\"zupa, toscana\",fletermouse";
            FormatOptions options = new FormatOptions();
            options.EscapeKind = EscapeKind.DoubleQuote;

            List<Statement> actual = FormatReader.Instance.Read<Statement>(input, options);
            
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("toad", actual[0].Name);
            Assert.AreEqual("gargalong", actual[0].Description);
            Assert.AreEqual("zupa, toscana", actual[1].Name);
            Assert.AreEqual("fletermouse", actual[1].Description);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "The header 'toad' was not recognized.")]
        public void Read_ThrowsException_TryingtoParseBareCsvFormat_ForComprehnsiveAttribute()
        {
            string input = "toad,\"gargalong\"" + Environment.NewLine + "\"zupa, toscana\",fletermouse";

            FormatReader.Default.Read<Statement2>(input);
        }

        [Test]
        public void Read_CanParseSimpleCsvContent_ForComprehensiveAttribute()
        {
            string input = "Name,Description\r\ntoad,\"gargalong\"" + Environment.NewLine + "\"zupa, toscana\",fletermouse";

            List<Statement2> actual = FormatReader.Instance.Read<Statement2>(input);

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("toad", actual[0].Name);
            Assert.AreEqual("gargalong", actual[0].Description);
            Assert.AreEqual("zupa, toscana", actual[1].Name);
            Assert.AreEqual("fletermouse", actual[1].Description);
        }

        [Test]
        public void Read_CanHandleRepeatedEscapeCharacter()
        {
            FormatOptions options = new FormatOptions { EscapeKind = EscapeKind.Repeat };
            Statement expected = new Statement();
            expected.Name = "me";
            expected.Description = "quick, kill it";

            Statement actual;
            using (MemoryStream stream = new MemoryStream())
            {
                FormatWriter.Instance.WriteSingle(stream, expected, options);
                stream.Position = 0;
                actual = FormatReader.Default.ReadSingle<Statement>(stream, options);
            }

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "The field 'Gorzo' has an invalid length (chars are only 1 character long)")]
        public void Read_ThrowsException_IfCharDecoratedInappropriately()
        {
            FormatReader.Instance.ReadFromFile<BadE>(NON_DATA_FILE);
        }

        [Test]
        public void Read_ReadsBackValuesForMinMaxLengthFields_RegardlessOfFixedWidthSettings()
        {
            GoodD element = new GoodD();
            element.Gorzo = "fa";
            element.JackieChan = "la";

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_ReadsValuesForMinMaxLengthFields_RegardlessOfFixedWidthSettings_ForValueThatsBetweenMinimumAndMaximum()
        {
            GoodD element = new GoodD();
            element.Gorzo = "fafetnu";
            element.JackieChan = "la";

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_ReadsValuesForMinMaxLengthFields_ForFixedMinWidthOnly()
		{
			GoodD element = new GoodD();
			element.Gorzo = "fafetnu";
			element.JackieChan = "la";
			element.WWF = true;

            TestWriteReadCycle(element);
		}

        [Test]
        public void Read_ReadsCorrectly_ForMixOfAccess()
        {
            GoodE element = new GoodE();
            element.Gorzo = "fafetnu";

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForFixedWidthAttributeFields()
        {
            InterchangeControlLoop element = new InterchangeControlLoop();
            element.InterchangeSenderID = 615156491;
            element.InterchangeDate = new DateTime(2008, 1, 25);
            element.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            element.InterchangeControlNumber = 4;
            element.UsageIndicator = 'T';

            TestWriteReadCycle(element);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a field and a sub-record (violator: 'Name')")]
        public void Read_ThrowsException_IfFieldAndSubRecordsCombined()
        {
            FormatReader.Instance.ReadFromFile<ExampleA>(NON_DATA_FILE);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "In strict mode, all members must be decorated.")]
        public void Read_ThrowsException_IfInStrictMode_AndMemberNotDecorated()
        {
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.UseStrictMode = true;

            FormatReader.Default.ReadFromFile<Order>(NON_DATA_FILE, readerOptions);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "Only types decorated as Records can be used.")]
        public void Read_ThrowsException_IfTryingToWriteNonRecord()
        {
            FormatReader.Instance.ReadFromFile<string>(NON_DATA_FILE);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a field and a child element (violator: 'Name')")]
        public void Read_ThrowsException_IfFieldAndChildrenCombined()
        {
            FormatReader.Default.ReadFromFile<ExampleB>(NON_DATA_FILE);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "A member cannot be a sub-record and a child element (violator: 'Name')")]
        public void Read_ThrowsException_IfFieldAndSubRecordCombined()
        {
            FormatReader.Instance.ReadFromFile<ExampleC>(NON_DATA_FILE);
        }

        [Test]
        public void Read_ReadsRecordsInCorrectOrder_WithMixOfFieldsAndProperties()
        {
            Stats element = new Stats();
            element.Name = "greg";
            element.FigureA = 34;
            element.FigureB = 45;
            element.FigureD = 99;

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_ReadsRecordWithWeirdRules()
        {
            ProfessionalServiceLoop element = new ProfessionalServiceLoop();
            element.ProcedureCodeParts.ProcedureCode = "S0500";
            element.LineItemChargeAmount = 89.25m;
            element.ServiceUnitCount = 1;
            element.DiagnosisCodePointer1 = 1;
            element.DiagnosisCodePointer2 = 1;

            TestWriteReadCycle(element);
        }

        [Test]
        public void Read_ParsesDataCorrectly_IfNamesInUnusualOrder_WhenNoSpecifiedOrder()
        {
            string content = "Total,OrderNumber,Ordered,ShipName\r\n36,ABCD1,12/31/2007,Tom Hardy 1\r\n";

            Order2 actual = FormatReader.Default.ReadSingle<Order2>(content);

            Assert.AreEqual("ABCD1", actual.OrderNumber);
            Assert.AreEqual("Tom Hardy 1", actual.ShippingName);
            Assert.AreEqual(new DateTime(2007,12,31), actual.Ordered);
            Assert.AreEqual(36m, actual.OrderTotal);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "The columns are not in the correct order.")]
        public void Read_Throws_IfOutputInIncorrectOrder_WhenOrderSpecified()
        {
            string content = "Total,OrderNumber,Ordered,ShipName\r\n36,ABCD1,12/31/2007,Tom Hardy 1\r\n";

            FormatReader.Default.ReadSingle<Order3>(content);
        }

        [Test]
        public void Read_ReadsStandardCsvRecord()
        {
            Order subject = OrderTestHelper.GetOrder();
            subject.IsActive = false;

            TestWriteReadCycle(subject);
        }

        [Test]
        public void Read_ReadsRecordWithOddDecorationOrder()
        {
            OrderQ subject = OrderTestHelper.GetOrderQ();

            TestWriteReadCycle(subject);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Read_ReadsHeaderlessCsvRecord_RegardlessOfHeaderSetting(bool headers)
        {
            Order subject = OrderTestHelper.GetOrder();
            subject.IsActive = false;
            List<Order> source = new List<Order> {subject};
            FormatWriter.Instance.Write(_temporaryFilename, source);
            FormatOptions options = new FormatOptions
            {
                FieldDelimiter = ",",
                IncludeHeaders = headers
            };

            List<Order> actual = FormatReader.Default.ReadFromFile<Order>(_temporaryFilename, options);

            ObjectAssert.ListsEqual(source, actual);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Read_ReadsCsvRecord_RegardlessOfHeaderSetting_ForComprehensiveAttribute(bool headers)
        {
            OrderB subject = OrderTestHelper.GetOrderB();
            subject.IsActive = false;
            List<OrderB> source = new List<OrderB> { subject };
            FormatWriter.Instance.Write(_temporaryFilename, source);
            FormatOptions options = new FormatOptions
            {
                FieldDelimiter = ",",
                IncludeHeaders = headers
            };

            List<OrderB> actual = FormatReader.Instance.ReadFromFile<OrderB>(_temporaryFilename, options);

            ObjectAssert.ListsEqual(source, actual);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Read_ReadsCsvRecord_RegardlessOfHeaderSetting_ForComprehensiveAttribute_UnusualOrder(bool headers)
        {
            OrderC subject = OrderTestHelper.GetOrderC();
            subject.IsActive = false;
            List<OrderC> source = new List<OrderC> { subject };
            FormatWriter.Instance.Write(_temporaryFilename, source);
            FormatOptions options = new FormatOptions
            {
                FieldDelimiter = ",",
                IncludeHeaders = headers
            };

            List<OrderC> actual = FormatReader.Default.ReadFromFile<OrderC>(_temporaryFilename, options);

            ObjectAssert.ListsEqual(source, actual);
        }

        [Test]
        public void Read_ReadsCsvCorrectly_ForOddDecorationOrder()
        {
            string content = "OrderNumber,ShippingName,Ordered,OrderTotal\r\n5,bob,11/11/2017,19.99";

            OrderC actual = FormatReader.Default.ReadSingle<OrderC>(content);

            Assert.AreEqual("5", actual.OrderNumber);
            Assert.AreEqual("bob", actual.ShippingName);
            Assert.AreEqual(new DateTime(2017, 11, 11).Date, actual.Ordered.Date);
            Assert.AreEqual(19.99m, actual.OrderTotal);
        }

        [Test]
        public void Read_ReadsStandardCsvRecord_WithHeaders()
        {
            Order2 subject = OrderTestHelper.GetOrder2();
            subject.IsActive = false;

            TestWriteReadCycle(subject);
        }

        [Test]
        public void Read_ReadsStandardCsvRecord_WithHeaders_OrderSpecified()
        {
            Order3 subject = OrderTestHelper.GetOrder3();
            subject.IsActive = false;

            TestWriteReadCycle(subject);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Read_ReadsCsvRecord_RegardlessOfHeaderSetting(bool headers)
        {
            Order2 subject = OrderTestHelper.GetOrder2();
            subject.IsActive = false;
            List<Order2> source = new List<Order2> { subject };
            FormatWriter.Instance.Write(_temporaryFilename, source);
            FormatOptions options = new FormatOptions
            {
                FieldDelimiter = ",",
                IncludeHeaders = headers
            };

            List<Order2> actual = FormatReader.Instance.ReadFromFile<Order2>(_temporaryFilename, options);

            ObjectAssert.ListsEqual(source, actual);
        }

        [Test]
        public void Read_ReadsRecords()
        {
            List<Order> list = new List<Order>
				{
					OrderTestHelper.GetOrder(),
					OrderTestHelper.GetOrder(2)
				};
            list.ForEach(x => x.IsActive = false);

            TestWriteReadCycle(list);
        }

        [TestCase(1)]
        [TestCase(12)]
        [TestCase(123)]
        [TestCase(1234)]
        [TestCase(123456)]
        public void Reads_ReadsRecordWithMinimumWidth(int value)
        {
            Stats item = new Stats();
            item.Id = value;

            TestWriteReadCycle(item);
        }

        [Test]
        public void Read_ReadsPipeDelimitedRecord()
        {
            LegacyOrder subject = new LegacyOrder();
            subject.OrderNumber = "ABCD";
            subject.OrderTotal = 35m;
            subject.Ordered = new DateTime(2007, 12, 31);
            subject.ShippingName = "Tom Hardy";

            TestWriteReadCycle(subject);
        }

        [Test]
        public void Read_CanEscapeProperly()
        {
            string input = "tom,dean,he's\\, very\\, cool,$1\\,234.45|~~~45;True|";
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.AllowDelimitersAsEscapedContent = true;
            readerOptions.EscapeKind = EscapeKind.Backslash;

            Master element = new Master();
            element.FirstName = "tom";
            element.LastName = "dean";
            element.Description = "he's, very, cool";
            element.Money = 1234.45m;
            element.Detail = new Detail();
            
            Master actual = FormatReader.Default.ReadSingle<Master>(input, readerOptions);

            ObjectAssert.ObjectsEquivalent(element, actual);
        }

        [Test]
        public void Read_CanEscapeProperly_ForTransformation()
        {
            string input = "tom,dean,he's_ very_ cool,$1_234.45|~~~45;True|";
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.AllowDelimitersAsEscapedContent = true;
            readerOptions.EscapeKind = EscapeKind.Transform;
            readerOptions.TransformEscapeCharacter = '_';

            Master element = new Master();
            element.FirstName = "tom";
            element.LastName = "dean";
            element.Description = "he's, very, cool";
            element.Money = 1234.45m;
            element.Detail = new Detail();

            Master actual = FormatReader.Instance.ReadSingle<Master>(input, readerOptions);

            ObjectAssert.ObjectsEquivalent(element, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException), ExpectedMessage = @"The value ' very\' cannot be converted to a Decimal (field 'Money').")]
        public void Read_ThrowsException_IfDataCannotBeMapped()
        {
            string input = "tom,dean,he's\\, very\\, cool,$1\\,234.45|~~~45;True|";
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.AllowDelimitersAsEscapedContent = false;

            FormatReader.Default.ReadSingle<Master>(input, readerOptions);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException), ExpectedMessage = "The value of 'Goober' cannot exceed 10 characters.")]
        public void Read_ThrowsException_IfSetToNotAllowTooLongValues()
        {
            FormatReader.Default.ReadSingle<Thing>("1234567890123");
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "The maximum length of a field cannot be less than the minimum width.")]
        public void Read_ThrowsException_IfWidthsNotDecoratedCorrectly()
        {
            FormatReader.Instance.ReadFromFile<BadD>(NON_DATA_FILE);
        }

        [Test]
        [ExpectedException(typeof(InvalidUsageException), ExpectedMessage = "Two or more child lists of the same type is not parseable.")]
        public void Read_ThrowsException_ForUnparseableParentChildLists()
        {
            string input = "Hardy,02091983|		Tom,Hardy,35.00|		Sheryl,Hardy,33.00|		Laurel,Hardy,4.00|		Tom Jr.,Hardy,8.00|";

            List<SimpleHeader> expected = new List<SimpleHeader>();
            expected.Add(new SimpleHeader
            {
                FamilyName = "Hardy",
                MarriageDate = new DateTime(1983, 2, 9),
                Parents = new List<SimpleFooter>
						{
							new SimpleFooter { FirstName = "Tom", LastName = "Hardy", Age = 35},
							new SimpleFooter { FirstName = "Sheryl", LastName = "Hardy", Age = 33},
						}
            });
            expected[0].Children.Add(new SimpleFooter { FirstName = "Laurel", LastName = "Hardy", Age = 4 });
            expected[0].Children.Add(new SimpleFooter { FirstName = "Tom Jr.", LastName = "Hardy", Age = 8 });

            FormatReader.Default.Read<SimpleHeader>(input);
        }

        [Test]
        public void Read_DoesNotRunOffTheEndOfString_IfContentAllPadding()
        {
            FileHeaderRecord record = NachaFileTestHelper.GetFileHeaderRecord();
            record.ImmediateDestination = "          ";
            Debug.Assert(record.ImmediateDestination.Length == 10);

            TestWriteReadCycle(record);
        }
        
        private void TestWriteReadCycle<T>(T element)
            where T : class
        {
            TestWriteReadCycle(new List<T> {element});
        }

        private void TestWriteReadCycle<T>(List<T> source)
            where T : class
        {
            FormatWriter.Instance.Write(_temporaryFilename, source);

            List<T> actual = FormatReader.Default.ReadFromFile<T>(_temporaryFilename);

            ObjectAssert.ListsEqual(source, actual);
        }
    }
}