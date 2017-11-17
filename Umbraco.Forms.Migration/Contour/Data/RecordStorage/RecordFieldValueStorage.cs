using System;
using System.Collections.Generic;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class RecordFieldValueStorage :IDisposable
    {
        private object formCacheSyncLock = new object();

        private ISqlHelper sqlHelper;

        public RecordFieldValueStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
        }

        public List<object> GetRecordFieldValues(RecordField rf)
        {
            string sql = string.Format("SELECT * FROM UFRecordData{0} where [Key] = @key ORDER BY Id ASC", rf.DataTypeAlias);
            List<object> l = new List<object>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@key", rf.Key));

            while (rr.Read())
            {
                switch(rf.DataType)
                {
                    case FieldDataType.String:
                    case FieldDataType.LongString:
                        l.Add(rr.GetString("Value"));
                        break;
                    case FieldDataType.Integer:
                        l.Add(rr.GetInt("Value"));
                        break;
                    case FieldDataType.DateTime:
                        l.Add(rr.GetDateTime("Value"));
                        break;
                    case FieldDataType.Bit:
                        l.Add(rr.GetBoolean("Value"));
                        break;
                }
            }

            rr.Dispose();

            return l;
        }

        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();

            sqlHelper = null;
            formCacheSyncLock = null;
        }

        #endregion
    }
}