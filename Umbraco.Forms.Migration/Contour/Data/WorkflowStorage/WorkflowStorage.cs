using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration.Enums;


namespace Umbraco.Forms.Migration.Data.Storage
{
    public class WorkflowStorage : IDisposable
    {
        private ISqlHelper sqlHelper;
        private SettingsStorage settings;

        //public WorkflowStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public WorkflowStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connection)
        {
            sqlHelper = connection;
            settings = new SettingsStorage(connection);
        }

        public List<Workflow> GetActiveWorkFlows()
        {
            string sql = "SELECT * from UFworkflows where Active = 1 ORDER by Name ASC";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);
            List<Workflow> l = new List<Workflow>();

            while (rr.Read())
            {
                Workflow wf = Workflow.CreateFromDataReader(rr);
                wf.Settings = settings.GetSettings(wf.Id);

                //if the type is missing, it will deactivate, the type will be missing if 3rd party dll is gone
                //this way we avoid non-existing workflows from trying to execute
                //the UI should reflect it this. 
                if(wf.Active)
                    l.Add(wf);
            }
            rr.Dispose();

            return l;
        }

        public List<Workflow> GetAllWorkFlows()
        {
            string sql = "SELECT * from UFworkflows ORDER by Name ASC";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);
            List<Workflow> l = new List<Workflow>();

            while (rr.Read())
            {
                Workflow wf = Workflow.CreateFromDataReader(rr);
                wf.Settings = settings.GetSettings(wf.Id);
                l.Add(wf);
            }
            rr.Dispose();
        
            return l;
        }

        public List<Workflow> GetActiveWorkFlows(Form form, FormState state)
        {
            string sql = "SELECT * from UFworkflows where form = @form AND executesOn = @state AND active = 1 ORDER by SortOrder ASC";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, 
                sqlHelper.CreateParameter("@form", form.Id),
                sqlHelper.CreateParameter("@state", state));

            List<Workflow> l = new List<Workflow>();

            while (rr.Read())
            {
                Workflow wf = Workflow.CreateFromDataReader(rr);
                wf.Settings = settings.GetSettings(wf.Id);

                if (wf.Active)
                    l.Add(wf);
            }
            rr.Dispose();

            return l;
        }

        public List<Workflow> GetAllWorkFlows(Form form)
        {
            string sql = "SELECT * from UFworkflows where form = @form ORDER by Name ASC";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@form", form.Id));

            List<Workflow> l = new List<Workflow>();

            while (rr.Read())
            {
                Workflow wf = Workflow.CreateFromDataReader(rr);
                wf.Settings = settings.GetSettings(wf.Id);
                l.Add(wf);
            }
            rr.Dispose();

            return l;
        }

        

        public Workflow GetWorkflow(Guid id)
        {
            string sql = "SELECT * from UFworkflows where id = @id";
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));
            
            Workflow wf = new Workflow();
            
            if (rr.Read())
            {
                
                wf = Workflow.CreateFromDataReader(rr);
                wf.Settings = settings.GetSettings(wf.Id);
            }
            rr.Dispose();
            return wf;
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
