using System;
using System.Collections.Generic;
using Company.Nacha.NachaElements;
using Company.Nacha.NachaElements.Enum;
using Company.Nacha.ResponseElements;
using Batch = Company.Nacha.ResponseElements.Batch;
using BatchControl = Company.Nacha.ResponseElements.BatchControl;
using BatchHeader = Company.Nacha.ResponseElements.BatchHeader;
using EntryDetail = Company.Nacha.ResponseElements.EntryDetail;
using FileControl = Company.Nacha.ResponseElements.FileControl;
using FileHeaderRecord = Company.Nacha.ResponseElements.FileHeaderRecord;

namespace TestProject
{
    public static class ResponseFileTestHelper
    {
        public static ResponseFile GetResponseFile(int i = 1)
        {
            ResponseFile file = new ResponseFile();
            file.Batches = new List<Batch>();
            file.FileHeaderRecord = GetFileHeaderRecord(i);
            file.Batches.Add(GetBatch(i));
            file.Batches.Add(GetBatch(i+1));
            file.Batches.Add(GetBatch(i+2));
            file.Batches.Add(GetBatch(i+3));
            file.FileControl = GetFileControl(i);
            return file;
        }

        private static FileHeaderRecord GetFileHeaderRecord(int i)
        {
            FileHeaderRecord record = new FileHeaderRecord();
            record.BlockingFactor = i;
            record.FileCreationDateAndTime = new DateTime(DateTime.Now.Year, 4, i, 7, i, 0);
            record.FormatCode = ((char)(i+65)).ToString();
            record.ImmediateDestination = "IMD" + i;
            record.ImmediateDestinationName = "IMDN" + i;
            record.ImmediateOrigin = i + 10;
            record.ImmediateOrginName = "ORG " + i;
            record.PriorityCode = i.ToString();
            record.RecordSize = i;
            record.FileIdModifier = ((char)(i + 65)).ToString();
            record.ReferenceCode = "REF" + i;
            return record;
        }

        private static FileControl GetFileControl(int i)
        {
            FileControl item = new FileControl();
            item.BatchCount = i * 4;
            item.BlockCount = i * 6;
            item.EntryCount = i * 2;
            item.EntryHash = i * 1000;
            item.TotalCreditAmmount = i * 5;
            item.TotalDebitAmmount = i * 3;
            return item;
        }

        private static Batch GetBatch(int i)
        {
            Batch batch = new Batch();
            batch.BatchHeader = GetBatchHeader(i);
            batch.EntryDetails = new List<Entries>();
            batch.EntryDetails.Add(GetEntry(i));
            batch.EntryDetails.Add(GetEntry(i+1));
            batch.EntryDetails.Add(GetEntry(i+2));
            batch.EntryDetails.Add(GetEntry(i+3));
            batch.BatchControl = GetBatchControl(i);
            return batch;
        }

        private static Entries GetEntry(int i)
        {
            Entries entries = new Entries();
            entries.Addenda = GetAddenda(i);
            entries.EntryDetail = GetEntryDetail(i);
            return entries;
        }

        private static Addenda GetAddenda(int i)
        {
            Addenda item = new Addenda();
            item.AddendaInfo = i.ToString() + "ALKSJDF";
            item.AddendaType = AddendaTypeCode.Returns;
            item.CurrentTraceNumber = i;
            item.DateOfDeath = new DateTime(2017, 1, i);
            item.OriginalDfiRoutingNumber = (i * 12).ToString();
            item.OriginalTraceNumberA = i * 1000;
            item.OriginalTraceNumberB = i * 15;
            item.ReturnReason = ReturnReasonCode.AuthorizationRevokedByCustomer;
            return item;
        }

        private static EntryDetail GetEntryDetail(int i)
        {
            EntryDetail entryDetail = new EntryDetail();
            entryDetail.AddendaRecordIndicator = Math.Min(i, 9);
            entryDetail.Amount = i * 7;
            entryDetail.DiscretionaryData = i.ToString();
            entryDetail.IdNumber = "000" + i;
            entryDetail.Name = "zzz" + i;
            entryDetail.ReceivingDfiAccountNumber = string.Format("{0}{1}{2}", i, i+1, i+2);
            entryDetail.ReceivingDfiRoutingNumber = string.Format("{0}{1}{2}", i+3, i+4, i+5);
            entryDetail.TraceNumberA = i * 10000;
            entryDetail.TraceNumberB = i * 15;
            entryDetail.TransactionCode = TransactionCode.SavingsAccountCredit;
            return entryDetail;
        }

        private static BatchHeader GetBatchHeader(int i)
        {
            BatchHeader batchHeader = new BatchHeader();
            batchHeader.BatchNumber = i * 3;
            batchHeader.CompanyDescriptiveDate = new DateTime(2000+i, 3, 7);
            batchHeader.CompanyEntryDescription = "aa43" + i;
            batchHeader.CompanyId = "A" + i;
            batchHeader.CompanyName = "COMPANY" + i;
            batchHeader.DiscretionaryData = i.ToString();
            batchHeader.EffectiveEntryDate = new DateTime(2016, 5, i);
            batchHeader.OriginatingFinancialInstitutionId = (i * 7).ToString();
            batchHeader.OriginatorStatusCode = "A";
            batchHeader.ServiceClassCode = ServiceClassCode.AchMixedDebitsAndCredits;
            batchHeader.SettlementDate = String.Empty;
            batchHeader.StandardEntryClassCode = StandardEntryClass.Ccd;
            return batchHeader;
        }

        private static BatchControl GetBatchControl(int i)
        {
            BatchControl item = new BatchControl();
            item.BatchNumber = i * 10;
            item.CompanyId = "A" + i;
            item.EntryCount = i * 15;
            item.EntryHash = i * 1573;
            item.OriginatingDfiId = i * 10;
            item.ServiceClassCode = ServiceClassCode.AchCreditsOnly;
            item.TotalCreditAmmount = i * 15;
            item.TotalDebitAmmount = i * 13;
            return item;
        }
    }
}