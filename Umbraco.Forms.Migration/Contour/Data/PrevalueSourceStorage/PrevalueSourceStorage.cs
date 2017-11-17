using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;


namespace Umbraco.Forms.Migration.Data.Storage
{
    public class PrevalueSourceStorage : IDisposable
    {
        private ISqlHelper sqlHelper;

        private SettingsStorage settings;

        //public PrevalueSourceStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public PrevalueSourceStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection) {
            sqlHelper = connection;
            settings = new SettingsStorage(connection);    
        }

        public List<FieldPreValueSource> GetAllPrevalueSources()
        {
            List<FieldPreValueSource> l = new List<FieldPreValueSource>();

            string sql = "SELECT * From UFPrevalueSources ORDER BY name ASC";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);

            while (rr.Read())
            {
                FieldPreValueSource pv = FieldPreValueSource.CreateFromDataReader(rr);
                pv.Settings = settings.GetSettings(pv.Id);
                l.Add(pv);
            }

            rr.Dispose();
            return l;
        }

        public FieldPreValueSource GetPrevalueSource(Guid id)
        {
            //check for the default guid so we don't try to look for a provider which are not there..
            //this could be considered a hack, but not everything can follow a generic model. 
            FieldPreValueSource pv = new FieldPreValueSource();
            if (id == FieldPreValueSource.GetDefaultProvider().Id)
            {
                pv = FieldPreValueSource.GetDefaultProvider();
            }
            else
            {
                string sql = "SELECT * From UFPrevalueSources where id = @id";

                IRecordsReader rr = sqlHelper.ExecuteReader(sql,
                    sqlHelper.CreateParameter("@id", id));


                if (rr.Read())
                {
                    pv = FieldPreValueSource.CreateFromDataReader(rr);
                    pv.Settings = settings.GetSettings(pv.Id);

                    rr.Dispose();

                    return pv;
                }

                rr.Dispose();
            }

            return pv;
        }

       

        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();
            sqlHelper = null;

            settings.Dispose();
            settings = null;
        }

        #endregion
    }
}
