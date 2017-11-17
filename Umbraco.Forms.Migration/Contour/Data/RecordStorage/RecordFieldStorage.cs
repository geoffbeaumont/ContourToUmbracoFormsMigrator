using System;
using System.Collections.Generic;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class RecordFieldStorage : IDisposable
    {
        private RecordFieldValueStorage recordFieldValueStorage;
        private object formCacheSyncLock = new object();

        private ISqlHelper sqlHelper;

        public RecordFieldStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            recordFieldValueStorage = new RecordFieldValueStorage(connection);
        }


        public Dictionary<Guid, RecordField> GetAllRecordFields(Record record, Form form)
        {
            string sql = "SELECT * FROM UFRecordFields where Record = @record";
            Dictionary<Guid, RecordField> l = new Dictionary<Guid, RecordField>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@record", record.Id));

            while (rr.Read())
            {
                RecordField r = RecordField.CreateFromDataReader(rr);
                r.Values = recordFieldValueStorage.GetRecordFieldValues(r);
                l.Add(r.FieldId, r);
            }

            rr.Dispose();

            return l;

        }


        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();
            recordFieldValueStorage.Dispose();

            sqlHelper = null;
            formCacheSyncLock = null;
        }

        #endregion
    }
}