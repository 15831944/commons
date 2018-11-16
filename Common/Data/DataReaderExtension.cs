namespace System.Data
{
    public static class DataReaderExtension
    {
        public static DbValue GetDbValue(this IDataReader reader, string name)
        {
            int ordinal = reader.GetOrdinal(name);
            return reader.GetDbValue(ordinal);
        }

        public static DbValue GetDbValue(this IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return new DbValue();
            }
            return new DbValue(reader.GetValue(ordinal));
        }

        public static DataTable FillDataTable(this IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                DataColumn column = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                dataTable.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow dataRow = dataTable.NewRow();
                for (int j = 0; j < fieldCount; j++)
                {
                    object value = reader[j];
                    dataRow[j] = value;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public static DataSet FillDataSet(this IDataReader reader)
        {
            DataSet dataSet = new DataSet();
            DataTable table = reader.FillDataTable();
            dataSet.Tables.Add(table);
            while (reader.NextResult())
            {
                table = reader.FillDataTable();
                dataSet.Tables.Add(table);
            }
            return dataSet;
        }
    }
}