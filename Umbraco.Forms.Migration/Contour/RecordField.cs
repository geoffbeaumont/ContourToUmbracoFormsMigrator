using System;
using System.Collections.Generic;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration
{
    public class RecordField
    {
        public RecordField()
        {

        }

        public FieldDataType DataType { get; set; }
        public string DataTypeAlias { get; set; }
        public Field Field { get; set; }
        public Guid FieldId { get; set; }
        public Guid Key { get; set; }
        public Guid Record { get; set; }
        public List<object> Values { get; set; }

        public static RecordField CreateFromDataReader(IRecordsReader reader)
        {
            RecordField recordField   = new RecordField();
            recordField.Key           = reader.GetGuid("Key");
            recordField.FieldId       = reader.GetGuid("Field");
            recordField.Record        = reader.GetGuid("Record");
            recordField.DataTypeAlias = reader.GetString("DataType");
            recordField.DataType      = (FieldDataType)Enum.Parse(typeof(FieldDataType), recordField.DataTypeAlias);
            recordField.Values        = new List<object>();
            return recordField;
        }
    }
}