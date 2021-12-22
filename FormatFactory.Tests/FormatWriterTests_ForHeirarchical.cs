using System;
using System.Collections.Generic;
using Company.Entities.Hierarchical;
using Irvin.FormatFactory;
using NUnit.Framework;

namespace TestProject
{
	[TestFixture]
	public class FormatWriterTests_ForHierarchical
	{
		 [Test]
		 public void WritesOutHierarchicalElements_IfSomeInTheChainAreNull()
		 {
			 InterchangeControlLoop mainLoop = new InterchangeControlLoop();
			 mainLoop.InterchangeSenderID = 615156491;
			 mainLoop.InterchangeDate = new DateTime(2008, 1, 25);
			 mainLoop.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
			 mainLoop.InterchangeControlNumber = 4;
			 mainLoop.UsageIndicator = 'T';

			 mainLoop.FunctionalGroup = null;

			 mainLoop.Trailer = new InterchangeControlTrailer();
			 mainLoop.Trailer.FunctionalGroupCount = 1;
			 mainLoop.Trailer.ControlNumber = 4;

			 FormatOptions writerOptions = new FormatOptions();
			 writerOptions.RecordDelimiter = "~" + Environment.NewLine;
			 writerOptions.FieldDelimiter = "*";
			 string actual = FormatWriter.Instance.WriteSingle(mainLoop, writerOptions);

			 Assert.AreEqual(
				 "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + Environment.NewLine +
				 "IEA*1*000000004~" + Environment.NewLine,
				 actual);
		 }

		 [Test]
		 public void WritesOutHierarchicalElements_ForMultipleNesting()
		 {
			 InterchangeControlLoop mainLoop = new InterchangeControlLoop();
			 mainLoop.InterchangeSenderID = 615156491;
			 mainLoop.InterchangeDate = new DateTime(2008, 1, 25);
			 mainLoop.InterchangeTime = DateTime.Now.Date.AddHours(7).AddMinutes(38);
			 mainLoop.InterchangeControlNumber = 4;
			 mainLoop.UsageIndicator = 'T';

			 mainLoop.FunctionalGroup = new FunctionalGroupHeader();
			 mainLoop.FunctionalGroup.Samuel = 4;
			 mainLoop.FunctionalGroup.TransactionSets = new List<TransactionSet>();
			 mainLoop.FunctionalGroup.TransactionSets.Add(new TransactionSet
				 {
					 PlanType = "OOOOH"
				 });
			 
			 mainLoop.Trailer = new InterchangeControlTrailer();
			 mainLoop.Trailer.FunctionalGroupCount = 1;
			 mainLoop.Trailer.ControlNumber = 4;

			 FormatOptions writerOptions = new FormatOptions();
			 writerOptions.RecordDelimiter = "~" + Environment.NewLine;
			 writerOptions.FieldDelimiter = "*";
			 string actual = FormatWriter.Instance.WriteSingle(mainLoop, writerOptions);

			 Assert.AreEqual(
				 "ISA*00*          *00*          *30*615156491      *ZZ*EYEMED         *080125*0738*^*00501*000000004*0*T*:~" + Environment.NewLine +
				 "4~" + Environment.NewLine +
				 "OOOOH~" + Environment.NewLine +
				 "IEA*1*000000004~" + Environment.NewLine,
				 actual);
		 }
	}
}