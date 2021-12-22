using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Irvin.FormatFactory;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class DataTableTests
    {
        [Test]
        [SuppressMessage("ReSharper", "MethodTooLong")]
        public void ReadTable_ReturnsStringTypedDataset_WhenNoColumnsWereSpecified()
        {
            string input = "5023|Billy Jean|Michael Jackson|1985|FALSE|256.78\n" +
                           "2390|Over the Rainbow|Dorothy|1937|TRUE|759.00\n" +
                           "3333|Turns Out|Garage Band #145|2011|TRUE|392\n";
            FormatOptions readOptions = new FormatOptions
            {
                RecordDelimiter = "\n", FieldDelimiter = "|", IncludeHeaders = false
            };

            DataTable actual = FormatReader.Default.ReadTable(input, readOptions);

            Assert.AreEqual(6, actual.Columns.Count);
            Assert.AreEqual("Column1", actual.Columns[0].ColumnName);
            Assert.AreEqual("Column2", actual.Columns[1].ColumnName);
            Assert.AreEqual("Column3", actual.Columns[2].ColumnName);
            Assert.AreEqual("Column4", actual.Columns[3].ColumnName);
            Assert.AreEqual("Column5", actual.Columns[4].ColumnName);
            Assert.AreEqual("Column6", actual.Columns[5].ColumnName);
            Assert.True(actual.Columns.Cast<DataColumn>().All(x => x.DataType == typeof(string)));
            Assert.AreEqual(3, actual.Rows.Count);

            Assert.AreEqual("5023", actual.Rows[0]["Column1"]);
            Assert.AreEqual("Billy Jean", actual.Rows[0]["Column2"]);
            Assert.AreEqual("Michael Jackson", actual.Rows[0]["Column3"]);
            Assert.AreEqual("1985", actual.Rows[0]["Column4"]);
            Assert.AreEqual("FALSE", actual.Rows[0]["Column5"]);
            Assert.AreEqual("256.78", actual.Rows[0]["Column6"]);

            Assert.AreEqual("2390", actual.Rows[1]["Column1"]);
            Assert.AreEqual("Over the Rainbow", actual.Rows[1]["Column2"]);
            Assert.AreEqual("Dorothy", actual.Rows[1]["Column3"]);
            Assert.AreEqual("1937", actual.Rows[1]["Column4"]);
            Assert.AreEqual("TRUE", actual.Rows[1]["Column5"]);
            Assert.AreEqual("759.00", actual.Rows[1]["Column6"]);

            Assert.AreEqual("3333", actual.Rows[2]["Column1"]);
            Assert.AreEqual("Turns Out", actual.Rows[2]["Column2"]);
            Assert.AreEqual("Garage Band #145", actual.Rows[2]["Column3"]);
            Assert.AreEqual("2011", actual.Rows[2]["Column4"]);
            Assert.AreEqual("TRUE", actual.Rows[2]["Column5"]);
            Assert.AreEqual("392", actual.Rows[2]["Column6"]);
        }

        [Test]
        public void Write_WritesDataTable()
        {
            DataTable dataTable = GetSampleDataTable();

            StringBuilder container = new StringBuilder();
            FormatWriter.Instance.Write(container, dataTable, FormatOptions.DefaultCsvSettings);

            Assert.AreEqual("ID,First Name,LastName,IsActive\n290384,John,Doe,False\n", container.ToString());
        }

        [Test]
        public void ReadWriteTest()
        {
            DataTable inputTable = GetSampleDataTable();
            string tableContent = FormatWriter.Instance.Write(inputTable, FormatOptions.DefaultCsvSettings);

            DataTable outputTable = FormatReader.Default.ReadTable(tableContent, FormatOptions.DefaultCsvSettings);

            Assert.AreEqual(inputTable.Columns.Count, outputTable.Columns.Count);
            foreach (DataColumn expectedColumn in inputTable.Columns)
            {
                Assert.True(outputTable.Columns.Contains(expectedColumn.ColumnName));
                DataColumn actualColumn = outputTable.Columns[expectedColumn.ColumnName];
                Assert.AreEqual(typeof(string), actualColumn.DataType);
            }
            Assert.AreEqual(inputTable.Rows.Count, outputTable.Rows.Count);
            foreach (DataColumn expectedColumn in inputTable.Columns)
            {
                for (int i = 0; i < inputTable.Rows.Count; i++)
                {
                    DataRow expectedRow = inputTable.Rows[i];
                    Assert.AreEqual(expectedRow[expectedColumn.ColumnName].ToString(), outputTable.Rows[i][expectedColumn.ColumnName]);
                }
            }
        }

        private static DataTable GetSampleDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("First Name", typeof(string));
            dataTable.Columns.Add("LastName", typeof(string));
            dataTable.Columns.Add("IsActive", typeof(bool));
            DataRow row = dataTable.NewRow();
            row["ID"] = 290384;
            row["First Name"] = "John";
            row["LastName"] = "Doe";
            row["IsActive"] = false;
            dataTable.Rows.Add(row);
            return dataTable;
        }
    }
}