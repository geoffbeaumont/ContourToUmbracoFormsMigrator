using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class FieldSetStorage : IDisposable
    {
        private FieldStorage storage;
        private FieldConditionStorage condstorage;
        private ISqlHelper sqlHelper;

        //public FieldSetStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public FieldSetStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            storage = new FieldStorage(connection);
            condstorage = new FieldConditionStorage(connection);
        }

        public List<FieldSet> GetAllFieldSets(Page page)
        {
            string sql = "SELECT * FROM UFfieldsets where page = @id ORDER BY sortorder ASC";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", page.Id));

            List<FieldSet> l = new List<FieldSet>();

            while (rr.Read())
            {
                FieldSet fs = FieldSet.CreateFromDataReader(rr);

                fs.Fields.AddRange(storage.GetAllFields(fs));

                l.Add(fs);
            }


            rr.Dispose();

            return l;
        }

        public FieldSet GetFieldSet(Guid id)
        {
            string sql = "SELECT * FROM UFfieldsets where id = @id";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));
            FieldSet fs = new FieldSet();
            if (rr.Read())
            {
                fs = FieldSet.CreateFromDataReader(rr);
                fs.Fields.AddRange( storage.GetAllFields(fs) );
                return fs;
            }

            rr.Dispose();

            return fs;
        }

       

        #region IDisposable Members

        public void Dispose()
        {
            storage.Dispose();
            condstorage.Dispose();
            sqlHelper.Dispose();

            storage = null;
            condstorage = null;
            sqlHelper = null;
        }

        #endregion
    }
}
