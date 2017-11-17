using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using System.Web;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class FormStorage : IDisposable
    {
        private PageStorage pageStorage;
        private WorkflowStorage workflowStorage;
        private object formCacheSyncLock = new object();

        private ISqlHelper sqlHelper;

        //public FormStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public FormStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            pageStorage = new PageStorage(connection);
            workflowStorage = new WorkflowStorage(connection);
        }

        public List<Form> GetAllForms(bool archived)
        {
            //SQL 
            string sql = "SELECT * FROM UFforms where archived = @archived order by name ASC;";

            List<Form> l = new List<Form>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@archived",archived));
            while (rr.Read())
            {
                Guid id = rr.GetGuid("id");
                
                Form f = Form.CreateFromDataReader(rr);
                f.Pages.AddRange(pageStorage.GetAllPages(f));

                l.Add(f);
            }
            rr.Dispose();

            return l;
        }

        public List<Form> GetAllForms()
        {
            //SQL 
            string sql = "SELECT * FROM UFforms order by name ASC;";

            List<Form> l = new List<Form>();

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);
            while (rr.Read())
            {
                Guid id = rr.GetGuid("id");
               
                        Form f = Form.CreateFromDataReader(rr);
                        f.Pages.AddRange(pageStorage.GetAllPages(f));
                      

                l.Add(f);
            }
            rr.Dispose();

            return l;
        }

        public Form GetFormFromRecordId(Guid recordId)
        {
            string sql = @"SELECT Form AS FormId
                            FROM UFRecords WHERE Id = @id";

          
                return GetForm(sqlHelper.ExecuteScalar<Guid>(sql, sqlHelper.CreateParameter("@id", recordId)));            

        }
        public Form GetFormFromFieldId(Guid fieldId) {
            string sql = string.Empty;

            if (umbraco.GlobalSettings.DbDSN.ToLower().Contains("datalayer=mysql"))
            {
                sql = "SELECT form FROM UFPages where id = (select page from UFFieldsets where id = (select fieldset from UFFields where Id = @id))";

                 return GetForm(new Guid(sqlHelper.ExecuteScalar<string>(sql, sqlHelper.CreateParameter("@id", fieldId))));
               
            }
            else
            {
                sql = @"SELECT UFForms.Id AS FormId
                            FROM UFFields INNER JOIN
                                                  UFFieldsets ON UFFields.Fieldset = UFFieldsets.Id INNER JOIN
                                                  UFPages ON UFFieldsets.Page = UFPages.Id INNER JOIN
                                                  UFForms ON UFPages.Form = UFForms.Id
                            WHERE (UFFields.Id = @id)";

                return GetForm(sqlHelper.ExecuteScalar<Guid>(sql, sqlHelper.CreateParameter("@id", fieldId)));
            }
            
        }

        public Form GetForm(Guid id)
        {
            /*
            string sql = "SELECT TOP 1 * FROM UFforms where id = @id";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

            Form f = new Form();
            if (rr.Read())
            {
                f = Form.CreateFromDataReader(rr);
                f.Pages.AddRange(pageStorage.GetAllPages(f));
            }

            rr.Dispose();

            return f;
            */

           
                string sql = "SELECT * FROM UFforms where id = @id";
                IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

                Form f = new Form();
                if (rr.Read())
                {
                    f = Form.CreateFromDataReader(rr);
                    f.Pages.AddRange(pageStorage.GetAllPages(f));
                }

                rr.Dispose();

                return f;
           
            
        }

        public Form GetForm(string name)
        {
            Form form = GetAllForms().Find(
            delegate(Form f)
            {
                return f.Name == name;
            }
            );

            return form;

        }

  




        #region IDisposable Members

        public void Dispose()
        {
            sqlHelper.Dispose();
            pageStorage.Dispose();

            sqlHelper = null;
            pageStorage = null;
            formCacheSyncLock = null;
        }

        #endregion
    }
}
