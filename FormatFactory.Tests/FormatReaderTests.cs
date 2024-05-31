using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Company.Entities;
using Company.Entities.Malformed;
using Irvin.FormatFactory;
using NUnit.Framework;
using InvalidDataException = Irvin.FormatFactory.InvalidDataException;

namespace TestProject
{
    [TestFixture]
    public class FormatReaderTests
    {
        private const string NON_DATA_FILE = @"c:\windows\system32\drivers\etc\hosts";

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
        
        [TestCase(typeof(Person3))]
        public void Read_ReadsContent_ForCsvWithMixedQuoting(Type recordType)
        {
            string content =
                "\"Given Name\",\"Family Name\",\"Age\",\"State/Province\",\"Created\",\"Balance (USD)\"" + Environment.NewLine +
                "\"Fred\",\"Flinstone\",\"25\",\"UT\",'1/5/2017',\"159.36\"" + Environment.NewLine +
                "\"Anna-Marie\",\"Sadler\",\"55\",\"AB\",'2/9/2016 15:33:01',\"-45.02\"" + Environment.NewLine +
                "\"Michael\",\"Van Dusen\",\"2\",\"CA\",'7/6/2016 3:00 PM',\"34\"" + Environment.NewLine;
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

        [TestCase(typeof(Person), "State,Province")]
        [TestCase(typeof(Person2), "State,Province")]
        [TestCase(typeof(Person3), "State/Province")]
        [TestCase(typeof(Person4), "State/Province")]
        public void Read_ReadsEscapedHeaders_ForCsv_HeaderQuotingOnly(Type recordType, string header)
        {
            string content =
               $"Given Name,Family Name,Age,\"{header}\",Created,Balance (USD)" + Environment.NewLine +
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

        [TestCase(typeof(Person), "State,Province")]
        [TestCase(typeof(Person2), "State,Province")]
        [TestCase(typeof(Person3), "State/Province")]
        [TestCase(typeof(Person4), "State/Province")]
        public void Read_ReadsEscapedHeaders_ForCsvMinimalQuoting(Type recordType, string header)
        {
            string content =
               $"Given Name,Family Name,Age,\"{header}\",Created,Balance (USD)" + Environment.NewLine +
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
            IEnumerable<IPerson> persons;
            
            if (recordType == typeof(Person))
            {
                persons = FormatReader.Default.Read<Person>(content, options);
            }
            else if(recordType == typeof(Person2))
            {
                persons = FormatReader.Default.Read<Person2>(content);
            }
            else if(recordType == typeof(Person3))
            {
                persons = FormatReader.Default.Read<Person3>(content, options);
            }
            else if(recordType == typeof(Person4))
            {
                persons = FormatReader.Default.Read<Person4>(content, options);
            }
            else
            {
                throw new NotSupportedException();
            }

            return persons.ToList();
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
        public void Read_ThrowsException_TryingtoParseBareCsvFormat_ForComprehnsiveAttribute()
        {
            string input = "toad,\"gargalong\"" + Environment.NewLine + "\"zupa, toscana\",fletermouse";

            try
            {
                FormatReader.Default.Read<Statement2>(input);
            }
            catch (InvalidDataException actualException)
            {
                Assert.AreEqual("The header 'toad' was not recognized.", actualException.Message);
            }
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
        public void Read_ThrowsException_IfCharDecoratedInappropriately()
        {
            try
            {
                FormatReader.Instance.ReadFromFile<BadE>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("The field 'Gorzo' has an invalid length (chars are only 1 character long)", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfFieldAndSubRecordsCombined()
        {
            try
            {
                FormatReader.Instance.ReadFromFile<ExampleA>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("A member cannot be a field and a sub-record (violator: 'Name')", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfInStrictMode_AndMemberNotDecorated()
        {
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.UseStrictMode = true;

            try
            {
                FormatReader.Default.ReadFromFile<Order>(NON_DATA_FILE, readerOptions);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("In strict mode, all members must be decorated.", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfTryingToWriteNonRecord()
        {
            try
            {
                FormatReader.Instance.ReadFromFile<string>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("Only types decorated as Records can be used.", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfFieldAndChildrenCombined()
        {
            try
            {
                FormatReader.Default.ReadFromFile<ExampleB>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("A member cannot be a field and a child element (violator: 'Name')", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfFieldAndSubRecordCombined()
        {
            try
            {
                FormatReader.Instance.ReadFromFile<ExampleC>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("A member cannot be a sub-record and a child element (violator: 'Name')", actualException.Message);
            }
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
        public void Read_Throws_IfOutputInIncorrectOrder_WhenOrderSpecified()
        {
            string content = "Total,OrderNumber,Ordered,ShipName\r\n36,ABCD1,12/31/2007,Tom Hardy 1\r\n";

            try
            {
                FormatReader.Default.ReadSingle<Order3>(content);
            }
            catch (InvalidDataException actualException)
            {
                Assert.AreEqual("The columns are not in the correct order.", actualException.Message);
            }
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
        public void Read_ThrowsException_IfDataCannotBeMapped()
        {
            string input = "tom,dean,he's\\, very\\, cool,$1\\,234.45|~~~45;True|";
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.AllowDelimitersAsEscapedContent = false;

            try
            {
                FormatReader.Default.ReadSingle<Master>(input, readerOptions);
            }
            catch (InvalidDataException actualException)
            {
                Assert.AreEqual(@"The value ' very\' cannot be converted to a Decimal (field 'Money').", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfSetToNotAllowTooLongValues()
        {
            try
            {
                FormatReader.Default.ReadSingle<Thing>("1234567890123");
            }
            catch (InvalidDataException actualException)
            {
                Assert.AreEqual("The value of 'Goober' cannot exceed 10 characters.", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_IfWidthsNotDecoratedCorrectly()
        {
            try
            {
                FormatReader.Instance.ReadFromFile<BadD>(NON_DATA_FILE);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("The maximum length of a field cannot be less than the minimum width.", actualException.Message);
            }
        }

        [Test]
        public void Read_ThrowsException_ForUnParseableParentChildLists()
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

            try
            {
                FormatReader.Default.Read<SimpleHeader>(input);
            }
            catch (InvalidUsageException actualException)
            {
                Assert.AreEqual("Two or more child lists of the same type is not parseable.", actualException.Message);
            }
        }
    }
}