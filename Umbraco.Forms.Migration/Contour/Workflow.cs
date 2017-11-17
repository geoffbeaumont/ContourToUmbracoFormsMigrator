using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration.Enums;


namespace Umbraco.Forms.Migration
{
    public class Workflow : IWorkflow
    {
        #region IWorkflow Members

        public static Workflow Create()
        {
            Workflow wf = new Workflow();
            wf.Settings = new Dictionary<string, string>();
          
            return wf;
        }

        public static Workflow CreateFromDataReader(IRecordsReader reader)
        {
            Workflow wf = new Workflow();
            wf.Id = reader.GetGuid("id");
            wf.Name = reader.GetString("name");
            wf.Form = reader.GetGuid("form");
            wf.SortOrder = reader.GetInt("sortorder");
            wf.Active = reader.GetBoolean("active");

            if (!reader.IsNull("executesOn"))
                wf.ExecutesOn = (FormState)System.Enum.ToObject( typeof(FormState), reader.GetInt("executesOn"));

            //if the type is null or does not exist in the collection, we should not initialize the workflow
            if (!reader.IsNull("type") && Umbraco.Forms.Core.Providers.WorkflowTypeProviderCollection.Instance.ContainsProvider(reader.GetGuid("type")))
                wf.Type = Umbraco.Forms.Core.Providers.WorkflowTypeProviderCollection.Instance.GetProviderInstance(reader.GetGuid("type"));
            else
                wf.Active = false;

            wf.Settings = new Dictionary<string, string>();
            
            return wf;
        }


        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Form { get; set; }

        public Boolean Active { get; set; }

        public Core.WorkflowType Type { get; set; }
        public FormState ExecutesOn { get; set; }

        public Dictionary<string, string> Settings { get; set; }

        public int SortOrder { get; set; }

        #endregion
    }
}
