using System;
using Company.Entities.Inherited;
using Irvin.FormatFactory;
using NUnit.Framework;

namespace TestProject
{
	[TestFixture]
	public class FormatWriterTests_ForInherited
	{
		[Test]
		public void Write_OutputsTheDesiredResult_ForInterchangeControlLoop()
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
		public void Write_OutputsTheDesiredResult_ForInterchangeControlTrailer()
		{
			InterchangeControlTrailer element = new InterchangeControlTrailer();
			element.FunctionalGroupCount = 1;
			element.ControlNumber = 4;

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("IEA*1*000000004~" + Environment.NewLine, actual);
		}

		[Test]
		public void Write_OutputsTheDesiredResult_ForFunctionalGroupTrailer()
		{
			FunctionalGroupTrailer element = new FunctionalGroupTrailer();
			element.TransactionSetCount = 1;
			element.ControlNumber = "40001";

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("GE*1*40001~" + Environment.NewLine, actual);
		}

		[Test]
		public void Write_OutputsTheDesiredResult_ForTransactionSegmentTrailer()
		{
			TransactionSegmentTrailer element = new TransactionSegmentTrailer();
			element.SegmentCount = 41;
			element.ControlNumber = 1;

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("SE*41*0001~" + Environment.NewLine, actual);
		}

		[Test]
		public void Write_OutputsTheDesiredResult_ForSalesTaxSegment()
		{
			SalesTaxSegment element = new SalesTaxSegment();
			element.MonetaryAmount = 3.50m;

			string actual = FormatWriter.Instance.WriteSingle(element);

			Assert.AreEqual("AMT*T*3.5~" + Environment.NewLine, actual);
		}
	}
}