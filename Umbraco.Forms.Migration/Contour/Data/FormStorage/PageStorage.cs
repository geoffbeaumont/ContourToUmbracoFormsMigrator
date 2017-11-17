using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class PageStorage : IDisposable
    {
        private FieldSetStorage storage;
        private ISqlHelper sqlHelper;

        //public PageStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public PageStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            storage = new FieldSetStorage(connection);
        }

        public List<Page> GetAllPages(Form form)
        {
            string sql = "SELECT * FROM UFpages where form = @form ORDER BY sortorder ASC";
            List<Page> l = new List<Page>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@form", form.Id));

            while (rr.Read())
            {
                Page p = Page.CreateFromDataReader(rr);
                p.FieldSets.AddRange(storage.GetAllFieldSets(p));
                l.Add(p);
            }

            rr.Dispose();

            return l;
        }

        public Page GetPage(Guid id)
        {
            string sql = "SELECT * from UFpages where id = @id";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

            Page p = new Page();
            if (rr.Read())
            {
                p = Page.CreateFromDataReader(rr);
                p.FieldSets.AddRange(storage.GetAllFieldSets(p));
            }

            rr.Dispose();

            return p;
        }

       

        #region IDisposable Members

        public void Dispose()
        {
            storage.Dispose();
            sqlHelper.Dispose();

            storage = null;
            sqlHelper = null;
        }

        #endregion
    }
}
