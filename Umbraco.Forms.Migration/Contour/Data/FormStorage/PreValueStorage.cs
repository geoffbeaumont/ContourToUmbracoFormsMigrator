using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration;
using Umbraco.Forms.Migration.DataLayer;


namespace Umbraco.Forms.Migration.Data.Storage
{
    public class PreValueStorage : IDisposable
    {
        private ISqlHelper sqlHelper;
        //public PreValueStorage()
        //{
        //    sqlHelper = Helper.SqlHelper;
        //}

        public PreValueStorage(ISqlHelper connection)
        {
            sqlHelper = connection;
        }

        public List<PreValue> GetAllPreValues(Field field)
        {
            List<PreValue> l = new List<PreValue>();
            string sql = "SELECT * From UFprevalues where field = @field";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql,
                sqlHelper.CreateParameter("@field", field.Id));

            while (rr.Read())
            {
                PreValue pv = PreValue.CreateFromDataReader(rr);
                l.Add(pv);
            }

            rr.Dispose();
            return l;
        }

        public PreValue GetPreValue(Guid id)
        {
            string sql = "SELECT * From UFprevalues where id = @id";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql,
                sqlHelper.CreateParameter("@id", id));

            PreValue pv = new PreValue();
            if (rr.Read())
            {
                pv = PreValue.CreateFromDataReader(rr);
            
                return pv;
            }
            rr.Dispose();
            return pv;
        }

        public PreValue InsertPreValue(Field field, string value)
        {
            PreValue pv = new PreValue();
            pv.Field = field.Id;
            pv.Value = value;


            return InsertPreValue(pv);
        }

        public PreValue InsertPreValue(PreValue prevalue)
        {
            prevalue.Id = Guid.NewGuid();


            string sql = "INSERT INTO UFprevalues(id, field, value, sortorder) VALUES(@id, @field, @value, @sortorder);";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", prevalue.Id),
                sqlHelper.CreateParameter("@field", prevalue.Field),
                sqlHelper.CreateParameter("@value", prevalue.Value),
             sqlHelper.CreateParameter("@sortorder", prevalue.SortOrder));

            return prevalue;
        }

        public bool DeletePreValue(PreValue prevalue)
        {
            string sql = "DELETE from UFprevalues where id = @id";
            sqlHelper.ExecuteNonQuery(sql, sqlHelper.CreateParameter("@id", prevalue.Id));

            return true;
        }


        public PreValue UpdatePreValue(PreValue prevalue)
        {

            string sql = "UPDATE UFprevalues SET field = @field, value = @value, sortorder = @sortorder where id = @id;";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@field", prevalue.Field),
                sqlHelper.CreateParameter("@value", prevalue.Value),
                sqlHelper.CreateParameter("@id", prevalue.Id),
                sqlHelper.CreateParameter("@sortorder", prevalue.SortOrder));

            return prevalue;
        }

        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();
            sqlHelper = null;
        }

        #endregion
    }
}
