using System;
using System.Collections.Generic;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class RecordStorage : IDisposable
    {
        private RecordFieldStorage recordFieldStorage;
        private object formCacheSyncLock = new object();

        private ISqlHelper sqlHelper;
        
        public RecordStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            recordFieldStorage = new RecordFieldStorage(connection);
        }

        public List<Record> GetAllRecords(Form form)
        {
            string sql = "SELECT * FROM UFRecords where form = @form ORDER BY created ASC";
            List<Record> l = new List<Record>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@form", form.Id));

            while (rr.Read())
            {
                Record r = Record.CreateFromDataReader(rr);
                r.RecordFields = recordFieldStorage.GetAllRecordFields(r, form);
                l.Add(r);
            }

            rr.Dispose();

            return l;
        }

        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();
            recordFieldStorage.Dispose();

            sqlHelper = null;
            formCacheSyncLock = null;
        }

        #endregion
    }
}