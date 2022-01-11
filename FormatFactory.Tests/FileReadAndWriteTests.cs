using System;
using System.Collections.Generic;
using System.IO;
using Company.Entities;
using Company.Nacha.NachaElements;
using Irvin.FormatFactory;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class FileReadAndWriteTests
    {
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
        
        [TestCase(true)]
        [TestCase(false)]
        public void Write_WritesFileWithEscapedValues_IfQuoting(bool headers)
        {
            var salesRecords = new List<SaleDetail>();
            salesRecords.Add(new SaleDetail
            {
                WeekStart = new DateTime(2021,12,5),
                ChannelName = "TV",
                RawSKU = 753366.ToString(),
                RawDescription = "TrueControlWeed/KnifeKit", 
                ConvertedSKUNumber = 8001550,
                SoldAmount = 447,
                SoldQuantity = 15,
                OnHandQuantity = 15,
                OnHandValue = 1439
            });
            salesRecords.Add(new SaleDetail
            {
                WeekStart = new DateTime(2021,12,5),
                ChannelName = "TV",
                RawSKU = 701884.ToString(),
                RawDescription = "Pressable Mat 16\" x 20\"", 
                ConvertedSKUNumber = 2005398,
                SoldAmount = 425,
                SoldQuantity = 8,
                OnHandQuantity = 55,
                OnHandValue = null
            });
			
            FormatOptions options = new FormatOptions();
            options.IncludeHeaders = headers;
            using (Stream fileStream = File.Open(_temporaryFilename, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                FormatWriter.Instance.Write(fileStream, salesRecords, options);
            }
			
            StringAssert.EndsWith("12/05/2021 00:00:00,TV,753366,,\"TrueControlWeed/KnifeKit\",8001550,0,447,15,15,1439,False\r\n" +
                                  "12/05/2021 00:00:00,TV,701884,,\"Pressable Mat 16\"\" x 20\"\"\",2005398,0,425,8,55,,False\r\n",
                File.ReadAllText(_temporaryFilename));
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
        public void Read_DoesNotRunOffTheEndOfString_IfContentAllPadding()
        {
            FileHeaderRecord record = NachaFileTestHelper.GetFileHeaderRecord();
            record.ImmediateDestination = "          ";

            TestWriteReadCycle(record);
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
        public void Read_ParsesEnumValue_AccordingToSpecifier()
        {
            Thing element = new Thing();
            element.Goober = "ABCD";
            element.CompareKind = StringComparison.Ordinal;
            element.C2 = StringComparison.CurrentCultureIgnoreCase;
            element.C3 = StringComparison.InvariantCulture;

            TestWriteReadCycle(element);
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