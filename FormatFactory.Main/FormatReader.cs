using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using Irvin.FormatFactory.Internal;
using Irvin.Parser;

namespace Irvin.FormatFactory
{
    public class FormatReader : IFormatReader
    {
        static FormatReader()
        {
            Default = new FormatReader();
        }

        public T ReadSingle<T>(string content, FormatOptions readOptions = null)
        {
            return Parse<T>(readOptions, () => content).First();
        }

        public T ReadSingle<T>(Stream stream, FormatOptions readOptions = null)
        {
            return Read<T>(stream, readOptions).First();
        }

        public DataTable ReadTable(string content, FormatOptions readOptions)
        {
            FormatParser parser = new FormatParser(readOptions);
            TokenCollection tokens = parser.Parse(content, SettingsFactory.GetCompareOption(readOptions));

            DataTable dataTable = InitializeDataTable(tokens, readOptions);

            ContentReader reader = new ContentReader(readOptions);
            List<DataRow> rows = reader.ParseRecords<DataRow>(tokens, SettingsFactory.GetSettingsForDataTable(dataTable, readOptions));
            foreach (DataRow row in rows)
            {
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static DataTable InitializeDataTable(TokenCollection tokens, IRecordOptions readOptions)
        {
            DataTable dataTable = new DataTable();

            tokens.SetCheckpoint();

            int columnIncrement = 0;
            ReadOnlyCollection<Token> headerTokens = tokens.MoveUntil(x => x.Content == readOptions.RecordDelimiter);
            foreach (Token token in headerTokens.Where(x => !x.IsDelimiter))
            {
                columnIncrement++;
                string columnName = !readOptions.IncludeHeaders ? $"Column{columnIncrement}" : token.Content;
                dataTable.Columns.Add(columnName, typeof(string));
            }

            if (readOptions.IncludeHeaders)
            {
                tokens.MoveNext();
            }
            else
            {
                tokens.Rewind();
            }

            return dataTable;
        }

        public List<T> ReadFromFile<T>(string filename, FormatOptions readOptions = null)
        {
            return Parse<T>(readOptions, () => File.ReadAllText(filename));
        }

        public List<T> Read<T>(string content, FormatOptions readOptions = null)
        {
            return Parse<T>(readOptions, () => content);
        }

        public List<T> Read<T>(Stream stream, FormatOptions readOptions = null)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return Parse<T>(readOptions, () => reader.ReadToEnd());
            }
        }

        private List<T> Parse<T>(FormatOptions readOptions, Func<string> contentGetter)
        {
            string content = contentGetter();
            content = content.TrimStart(Environment.NewLine.ToCharArray());

            ContentReader contentReader = new ContentReader(readOptions);
            return contentReader.Parse<T>(content);
        }

        public static FormatReader Default { get; }
        public static IFormatReader Instance => Default;
    }
}