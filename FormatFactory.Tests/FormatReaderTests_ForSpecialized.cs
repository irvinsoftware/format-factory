using System;
using System.Collections.Generic;
using System.Text;
using Company.Entities.Hierarchical;
using Company.Entities.Inherited;
using Irvin.FormatFactory;
using Company.Nacha.NachaElements;
using Company.Nacha.ResponseElements;
using NUnit.Framework;
using InterchangeControlLoop = Company.Entities.Hierarchical.InterchangeControlLoop;
using InterchangeControlTrailer = Company.Entities.Hierarchical.InterchangeControlTrailer;

namespace TestProject
{
    [TestFixture]
    public class FormatReaderTests_ForSpecialized
    {
        [Test]
        public void Read_ReadsHierarchicalElements_IfSomeInTheChainAreNull()
        {
            string input = "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + 
                            Environment.NewLine + 
                            "IEA*1*000000004~" + Environment.NewLine;
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.RecordDelimiter = "~" + Environment.NewLine;
            readerOptions.FieldDelimiter = "*";

            InterchangeControlLoop expected = new InterchangeControlLoop();
            expected.InterchangeSenderID = 615156491;
            expected.InterchangeDate = new DateTime(2008, 1, 25);
            expected.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            expected.InterchangeControlNumber = 4;
            expected.UsageIndicator = 'T';
            expected.FunctionalGroup = null;
            expected.Trailer = new InterchangeControlTrailer();
            expected.Trailer.FunctionalGroupCount = 1;
            expected.Trailer.ControlNumber = 4;
            
            InterchangeControlLoop actual = FormatReader.Default.ReadSingle<InterchangeControlLoop>(input, readerOptions);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadHierarchicalElements_ForMultipleNesting()
        {
            string input =
                "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" +
                Environment.NewLine +
                "4~" + Environment.NewLine +
                "OOOOH~" + Environment.NewLine +
                "IEA*1*000000004~" + Environment.NewLine;
            FormatOptions readerOptions = new FormatOptions();
            readerOptions.RecordDelimiter = "~" + Environment.NewLine;
            readerOptions.FieldDelimiter = "*";

            InterchangeControlLoop expected = new InterchangeControlLoop();
            expected.InterchangeSenderID = 615156491;
            expected.InterchangeDate = new DateTime(2008, 1, 25);
            expected.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            expected.InterchangeControlNumber = 4;
            expected.UsageIndicator = 'T';
            expected.FunctionalGroup = new FunctionalGroupHeader();
            expected.FunctionalGroup.Samuel = 4;
            expected.FunctionalGroup.TransactionSets = new List<TransactionSet>();
            expected.FunctionalGroup.TransactionSets.Add(new TransactionSet
            {
                PlanType = "OOOOH"
            });
            expected.Trailer = new InterchangeControlTrailer();
            expected.Trailer.FunctionalGroupCount = 1;
            expected.Trailer.ControlNumber = 4;

            InterchangeControlLoop actual = FormatReader.Default.ReadSingle<InterchangeControlLoop>(input, readerOptions);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInheritedInterchangeControlLoop()
        {
            string input = "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + Environment.NewLine;

            Company.Entities.Inherited.InterchangeControlLoop expected = new Company.Entities.Inherited.InterchangeControlLoop();
            expected.InterchangeSenderID = 615156491;
            expected.InterchangeDate = new DateTime(2008, 1, 25);
            expected.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            expected.InterchangeControlNumber = 4;
            expected.UsageIndicator = 'T';

            Company.Entities.Inherited.InterchangeControlLoop actual =
                FormatReader.Default.ReadSingle<Company.Entities.Inherited.InterchangeControlLoop>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInheritedInterchangeControlTrailer()
        {
            string input = "IEA*1*000000004~" + Environment.NewLine;

            Company.Entities.Inherited.InterchangeControlTrailer expected = new Company.Entities.Inherited.InterchangeControlTrailer();
            expected.FunctionalGroupCount = 1;
            expected.ControlNumber = 4;

            Company.Entities.Inherited.InterchangeControlTrailer actual =
                FormatReader.Default.ReadSingle<Company.Entities.Inherited.InterchangeControlTrailer>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInheritedFunctionalGroupTrailer()
        {
            string input = "GE*1*40001~" + Environment.NewLine;

            FunctionalGroupTrailer expected = new FunctionalGroupTrailer();
            expected.TransactionSetCount = 1;
            expected.ControlNumber = "40001";

            FunctionalGroupTrailer actual = FormatReader.Default.ReadSingle<FunctionalGroupTrailer>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsTheDesiredResult_ForInheritedTransactionSegmentTrailer()
        {
            string input = "SE*41*0001~" + Environment.NewLine;

            TransactionSegmentTrailer expected = new TransactionSegmentTrailer();
            expected.SegmentCount = 41;
            expected.ControlNumber = 1;

            TransactionSegmentTrailer actual = FormatReader.Default.ReadSingle<TransactionSegmentTrailer>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInheritedSalesTaxSegment()
        {
            string input = "AMT*T*3.5~" + Environment.NewLine;

            SalesTaxSegment expected = new SalesTaxSegment();
            expected.MonetaryAmount = 3.50m;

            SalesTaxSegment actual = FormatReader.Default.ReadSingle<SalesTaxSegment>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInterchangeControlLoop()
        {
            string input = "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + Environment.NewLine;

            Company.Entities.MostExplicit.InterchangeControlLoop expected = new Company.Entities.MostExplicit.InterchangeControlLoop();
            expected.InterchangeSenderID = 615156491;
            expected.InterchangeDate = new DateTime(2008, 1, 25);
            expected.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            expected.InterchangeControlNumber = 4;
            expected.UsageIndicator = 'T';

            Company.Entities.MostExplicit.InterchangeControlLoop actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.InterchangeControlLoop>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForFieldThatHasTheRightMaxLengthAlready()
        {
            string input = "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*123456789*0*T*:~" + Environment.NewLine;

            Company.Entities.MostExplicit.InterchangeControlLoop expected = new Company.Entities.MostExplicit.InterchangeControlLoop();
            expected.InterchangeSenderID = 615156491;
            expected.InterchangeDate = new DateTime(2008, 1, 25);
            expected.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
            expected.InterchangeControlNumber = 123456789;
            expected.UsageIndicator = 'T';

            Company.Entities.MostExplicit.InterchangeControlLoop actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.InterchangeControlLoop>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForInterchangeControlTrailer()
        {
            string input = "IEA*1*000000004~" + Environment.NewLine;

            Company.Entities.MostExplicit.InterchangeControlTrailer expected = new Company.Entities.MostExplicit.InterchangeControlTrailer();
            expected.FunctionalGroupCount = 1;
            expected.ControlNumber = 4;

            Company.Entities.MostExplicit.InterchangeControlTrailer actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.InterchangeControlTrailer>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForFunctionalGroupTrailer()
        {
            string input = "GE*1*40001~" + Environment.NewLine;

            Company.Entities.MostExplicit.FunctionalGroupTrailer expected = new Company.Entities.MostExplicit.FunctionalGroupTrailer();
            expected.TransactionSetCount = 1;
            expected.ControlNumber = "40001";

            Company.Entities.MostExplicit.FunctionalGroupTrailer actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.FunctionalGroupTrailer>(input);
            
            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForTransactionSegmentTrailer()
        {
            string input = "SE*41*0001~" + Environment.NewLine;

            Company.Entities.MostExplicit.TransactionSegmentTrailer expected = new Company.Entities.MostExplicit.TransactionSegmentTrailer();
            expected.SegmentCount = 41;
            expected.ControlNumber = 1;

            Company.Entities.MostExplicit.TransactionSegmentTrailer actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.TransactionSegmentTrailer>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_ReadsDesiredResult_ForSalesTaxSegment()
        {
            string input = "AMT*T*3.5~" + Environment.NewLine;

            Company.Entities.MostExplicit.SalesTaxSegment expected = new Company.Entities.MostExplicit.SalesTaxSegment();
            expected.MonetaryAmount = 3.50m;

            Company.Entities.MostExplicit.SalesTaxSegment actual = FormatReader.Default.ReadSingle<Company.Entities.MostExplicit.SalesTaxSegment>(input);

            ObjectAssert.ObjectsEquivalent(expected, actual);
        }

        [Test]
        public void Read_CanReadComplexHierarchy()
        {
            List<NachaFile> items = new List<NachaFile>();
            items.Add(NachaFileTestHelper.GetNachaFile(2));
            items.Add(NachaFileTestHelper.GetNachaFile(3));
            items.Add(NachaFileTestHelper.GetNachaFile(10));
            items.Add(NachaFileTestHelper.GetNachaFile(11));
            items.Add(NachaFileTestHelper.GetNachaFile(15));
            string content = FormatWriter.Instance.Write(items);

            List<NachaFile> actual = FormatReader.Default.Read<NachaFile>(content);

            ObjectAssert.ListsEqual(items, actual);
        }

        [Test]
        public void Read_CanReadComplexHierarchy_MoreDeeplyNested()
        {
            List<ResponseFile> items = new List<ResponseFile>();
            items.Add(ResponseFileTestHelper.GetResponseFile(2));
            items.Add(ResponseFileTestHelper.GetResponseFile(3));
            items.Add(ResponseFileTestHelper.GetResponseFile(10));
            items.Add(ResponseFileTestHelper.GetResponseFile(11));
            items.Add(ResponseFileTestHelper.GetResponseFile(15));
            string content = FormatWriter.Instance.Write(items);

            List<ResponseFile> actual = FormatReader.Default.Read<ResponseFile>(content);

            ObjectAssert.ListsEqual(items, actual);
        }

        [Test]
        public void Read_ThrowsProperException_WhenDataCannotMap_ForComplexHeirarchy()
        {
            string sampleFile =
@"1011xxxx22268 0420003141502050616M094101EX RCVD RETURNS        5/3 OH CINCINTI FTCI           
5200Payer Inc                           1471034885CCDHCCLAIMPMT      1502040001041000120000001
6210410001244224711694       00000045071700828852     Example 1               1242071750000309
799R0200000122222000700000004100012                                            242071750000309
820000000200041000120000000000000000000045071340922268                         041000120000001
5200Customer Inc                        1471034885CCDHCCLAIMPMT      1502040001041000120000002
6210410001244224793204       00000041121053353896     Example 2               1242071750000315
799R0200000122222000300000004100012                                            242071750000315
820000000200041000120000000000000000000041121340922268                         041000120000002
5200VendorInc                           1471034885CCDHCCLAIMPMT      1502040001041000120000003
6210410001244224793204       00000083681053353896     Example 3               1242071750000317
799R0200000122222002600000004100012                                            242071750000317
820000000200041000120000000000000000000083681340922268                         041000120000003
9000001000003000000200084000620000000692817000000007050                                       ";

            try
            {
                FormatReader.Default.Read<ResponseFile>(sampleFile);
            }
            catch (InvalidDataException actualException)
            {
                Assert.AreEqual("The value 'R02' cannot be converted to a ReturnReasonCode (field 'ReturnReason').", actualException.Message);
            }
        }

        [Test]
        public void Read_CanReadComplexHierarchy_FromExpandedLiveFile()
        {
            string sampleFile =
@"101 242071758 4710348851703091006A094101FIFTH THIRD BANK       SAMPLE COMPANY1 LLC            
5225SAMPLE COMPANY1                     1471034885PPDMNTRANSACT170309170310   1242071750000001
6270420003181234567890123456700000000003              1Test Customer Name Ne  0242071750000001
6270420003181234567890123456700000000004              1Test Customer Name Ne  0242071750000002
6270420003181234567890123456700000001005              1Test Customer Name Ne  0242071750000003
6270420003187777777788888888800000001006              Test Deb and Cred       0242071750000004
6270420003189899999990000000000000001007              Test Deb and Cred 2     0242071750000005
62704200031812345678901234567000000420012             1Test Customer Name Ne  0242071750000006
62704200031812345678901234567000000422513             1Test Customer Name Ne  0242071750000007
62704200031898999999900000000000000012518             Test Deb and Cred 2     0242071750000008
62704200031898999999900000000000003252519             Test Deb and Cred 2     0242071750000009
62704200031898999999900000000000004135020             Test Deb and Cred 2     0242071750000010
62704200031898999999900000000000033000021             Test Deb and Cred 2     0242071750000011
62704200031898999999900000000000025002222             Test Deb and Cred 2     0242071750000012
627042000318741852963        000002067037             Kevin Appleton          0242071750000013
822500001300546004030000006834170000000000001471034885                         242071750000001
5200SAMPLE COMPANY1                     1471034885CCDMNTRANSACT170309170310   1242071750000001
627042000318123456           000000235030             Minnie Mouse2           0242071750000014
627042000318123456           000000235031             Minnie Mouse2           0242071750000015
627042000318123456           000000235032             Minnie Mouse2           0242071750000016
627042000318123456           000000235033             Minnie Mouse2           0242071750000017
627042000318123456           000000235034             Minnie Mouse2           0242071750000018
627042000318123456           000000235035             Minnie Mouse2           0242071750000019
627042000318123456           000000235036             Minnie Mouse2           0242071750000020
820000000700294002170000000094000000000070501471034885                         242071750000001
9000001000003000000200084000620000000692817000000007050                                       ";

            List<NachaFile> actual = FormatReader.Default.Read<NachaFile>(sampleFile);

            Assert.NotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual[0].Batches.Count);
            Assert.AreEqual(13, actual[0].Batches[0].EntryDetails.Count);
            Assert.AreEqual(7, actual[0].Batches[1].EntryDetails.Count);
            Assert.AreEqual(1, actual[0].FileControl.BatchCount);
            Assert.AreEqual(3, actual[0].FileControl.BlockCount);
            Assert.AreEqual("7050", actual[0].FileControl.TotalCreditAmmount);
            Assert.AreEqual("SAMPLE COMPANY1 LLC", actual[0].FileHeaderRecord.ImmediateOrginName);
            Assert.AreEqual(1, actual[0].Batches[0].BatchHeader.BatchNumber);
        }

        [Test]
        public void Read_CanReadComplexHierarchy_FromExpandedLiveFile_WithLeadingAndTrailingSpaces()
        {
            string sampleFile =
@"


101 242071758 4710348851703091006A094101FIFTH THIRD BANK       SAMPLE COMPANY1 LLC            
5225SAMPLE COMPANY1                     1471034885PPDMNTRANSACT170309170310   1242071750000001
6270420003181234567890123456700000000003              1Test Customer Name Ne  0242071750000001
6270420003181234567890123456700000000004              1Test Customer Name Ne  0242071750000002
6270420003181234567890123456700000001005              1Test Customer Name Ne  0242071750000003
6270420003187777777788888888800000001006              Test Deb and Cred       0242071750000004
6270420003189899999990000000000000001007              Test Deb and Cred 2     0242071750000005
62704200031812345678901234567000000420012             1Test Customer Name Ne  0242071750000006
62704200031812345678901234567000000422513             1Test Customer Name Ne  0242071750000007
62704200031898999999900000000000000012518             Test Deb and Cred 2     0242071750000008
62704200031898999999900000000000003252519             Test Deb and Cred 2     0242071750000009
62704200031898999999900000000000004135020             Test Deb and Cred 2     0242071750000010
62704200031898999999900000000000033000021             Test Deb and Cred 2     0242071750000011
62704200031898999999900000000000025002222             Test Deb and Cred 2     0242071750000012
627042000318741852963        000002067037             Kevin Appleton          0242071750000013
822500001300546004030000006834170000000000001471034885                         242071750000001
5200SAMPLE COMPANY1                     1471034885CCDMNTRANSACT170309170310   1242071750000001
627042000318123456           000000235030             Minnie Mouse2           0242071750000014
627042000318123456           000000235031             Minnie Mouse2           0242071750000015
627042000318123456           000000235032             Minnie Mouse2           0242071750000016
627042000318123456           000000235033             Minnie Mouse2           0242071750000017
627042000318123456           000000235034             Minnie Mouse2           0242071750000018
627042000318123456           000000235035             Minnie Mouse2           0242071750000019
627042000318123456           000000235036             Minnie Mouse2           0242071750000020
820000000700294002170000000094000000000070501471034885                         242071750000001
9000001000003000000200084000620000000692817000000007050                                       
";

            List<NachaFile> actual = FormatReader.Default.Read<NachaFile>(sampleFile);

            Assert.NotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual[0].Batches.Count);
            Assert.AreEqual(13, actual[0].Batches[0].EntryDetails.Count);
            Assert.AreEqual(7, actual[0].Batches[1].EntryDetails.Count);
            Assert.AreEqual(1, actual[0].FileControl.BatchCount);
            Assert.AreEqual(3, actual[0].FileControl.BlockCount);
            Assert.AreEqual("7050", actual[0].FileControl.TotalCreditAmmount);
            Assert.AreEqual("SAMPLE COMPANY1 LLC", actual[0].FileHeaderRecord.ImmediateOrginName);
            Assert.AreEqual(1, actual[0].Batches[0].BatchHeader.BatchNumber);
        }

        [Test]
        public void Read_CanReadComplexHierarchy_FromExpandedLiveFile_SimpleLineEndings()
        {
            string sampleFile =
@"101 242071758 4710348851703091006A094101FIFTH THIRD BANK       SAMPLE COMPANY1 LLC            
5225SAMPLE COMPANY1                     1471034885PPDMNTRANSACT170309170310   1242071750000001
6270420003181234567890123456700000000003              1Test Customer Name Ne  0242071750000001
6270420003181234567890123456700000000004              1Test Customer Name Ne  0242071750000002
6270420003181234567890123456700000001005              1Test Customer Name Ne  0242071750000003
6270420003187777777788888888800000001006              Test Deb and Cred       0242071750000004
6270420003189899999990000000000000001007              Test Deb and Cred 2     0242071750000005
62704200031812345678901234567000000420012             1Test Customer Name Ne  0242071750000006
62704200031812345678901234567000000422513             1Test Customer Name Ne  0242071750000007
62704200031898999999900000000000000012518             Test Deb and Cred 2     0242071750000008
62704200031898999999900000000000003252519             Test Deb and Cred 2     0242071750000009
62704200031898999999900000000000004135020             Test Deb and Cred 2     0242071750000010
62704200031898999999900000000000033000021             Test Deb and Cred 2     0242071750000011
62704200031898999999900000000000025002222             Test Deb and Cred 2     0242071750000012
627042000318741852963        000002067037             Kevin Appleton          0242071750000013
822500001300546004030000006834170000000000001471034885                         242071750000001
5200SAMPLE COMPANY1                     1471034885CCDMNTRANSACT170309170310   1242071750000001
627042000318123456           000000235030             Minnie Mouse2           0242071750000014
627042000318123456           000000235031             Minnie Mouse2           0242071750000015
627042000318123456           000000235032             Minnie Mouse2           0242071750000016
627042000318123456           000000235033             Minnie Mouse2           0242071750000017
627042000318123456           000000235034             Minnie Mouse2           0242071750000018
627042000318123456           000000235035             Minnie Mouse2           0242071750000019
627042000318123456           000000235036             Minnie Mouse2           0242071750000020
820000000700294002170000000094000000000070501471034885                         242071750000001
9000001000003000000200084000620000000692817000000007050                                       ";
            sampleFile = sampleFile.Replace("\r", "");

            List<NachaFile> actual = FormatReader.Default.Read<NachaFile>(sampleFile);

            Assert.NotNull(actual);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual[0].Batches.Count);
            Assert.AreEqual(13, actual[0].Batches[0].EntryDetails.Count);
            Assert.AreEqual(7, actual[0].Batches[1].EntryDetails.Count);
            Assert.AreEqual(1, actual[0].FileControl.BatchCount);
            Assert.AreEqual(3, actual[0].FileControl.BlockCount);
            Assert.AreEqual("7050", actual[0].FileControl.TotalCreditAmmount);
            Assert.AreEqual("SAMPLE COMPANY1 LLC", actual[0].FileHeaderRecord.ImmediateOrginName);
            Assert.AreEqual(1, actual[0].Batches[0].BatchHeader.BatchNumber);
        }
    }
}