using System.Data;

namespace Irvin.FormatFactory.Internal.Member
{
    internal class DataTableInfo : IMemberContainer
    {
        public DataTableInfo(DataTable table)
        {
            Table = table;
        }

        private DataTable Table { get; }

        public string Name => Table.TableName;

        public object CreateNew()
        {
            return Table.NewRow();
        }
    }
}