using System;
using System.Data;

namespace Irvin.FormatFactory.Internal.Member
{
    internal class DataColumnWrapper : IMemberInfo
    {
        private readonly DataColumn _columnInfo;

        public DataColumnWrapper(DataColumn columnInfo)
        {
            _columnInfo = columnInfo;
        }

        public bool Equals(IMemberInfo other)
        {
            DataColumnWrapper otherMember = other as DataColumnWrapper;

            if (otherMember == null)
            {
                return false;
            }

            return _columnInfo.Namespace == otherMember._columnInfo.Namespace &&
                   _columnInfo.ColumnName == otherMember._columnInfo.ColumnName &&
                   _columnInfo.Table == otherMember._columnInfo.Table;
        }

        public string Name => _columnInfo.ColumnName;
        public Type MemberType => _columnInfo.DataType;
        public IMemberContainer Container => new DataTableInfo(_columnInfo.Table);

        public bool SetValue(object target, object value)
        {
            DataRow currentRow = FindCurrentRow(target);
            currentRow[_columnInfo.ColumnName] = value;
            return true;
        }

        public object GetValue(object source)
        {
            DataRow currentRow = FindCurrentRow(source);
            return currentRow[_columnInfo.ColumnName];
        }

        private static DataRow FindCurrentRow(object row)
        {
            DataRow actualRow = row as DataRow;

            if (actualRow == null)
            {
                throw new InvalidOperationException("This is not a valid data row.");
            }

            return actualRow;
        }
    }
}