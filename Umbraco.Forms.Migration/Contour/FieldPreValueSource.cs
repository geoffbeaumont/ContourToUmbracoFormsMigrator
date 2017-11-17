using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration.Data;

namespace Umbraco.Forms.Migration
{

    public class FieldPreValueSource : IFieldPreValueSource
    {
        public static FieldPreValueSource GetDefaultProvider()
        {
            FieldPreValueSource f = new FieldPreValueSource();
            f.Name = "Default Provider";
            f.Id = new Guid("9FFD217D-C8DF-4a59-9C9A-690A6F389DC4");
            f.Settings = new Dictionary<string, string>();
            f.Type = (Umbraco.Forms.Core.FieldPreValueSourceType)Activator.CreateInstance(Umbraco.Forms.Core.Providers.FieldPreValueSourceTypeProviderCollection.Instance.GetProvider( new Guid("cc9f9b2a-a746-11de-9e17-681b56d89593") ).GetType());
            return f;
        }

        public static FieldPreValueSource Create()
        {
            FieldPreValueSource f = new FieldPreValueSource();
            f.Settings = new Dictionary<string, string>();
            return f;
        }

        public static FieldPreValueSource CreateFromDataReader(IRecordsReader rr)
        {
            FieldPreValueSource f = Create();
            f.Name = rr.GetString("name");
            f.Id = rr.GetGuid("id");

            if (!rr.IsNull("type")) {
                Guid g = rr.GetGuid("type");

                if (g != Guid.Empty && Umbraco.Forms.Core.Providers.FieldPreValueSourceTypeProviderCollection.Instance.ContainsProvider(g)) 
                    f.Type = Umbraco.Forms.Core.Providers.FieldPreValueSourceTypeProviderCollection.Instance.GetProvider(g);
            }

            
            return f;
        }

        public Boolean IsInUse()
        {
            ISqlHelper sqlHelper = Helper.SqlHelper;

            string sql = "Select 1 from UFfields where PreValueProvider = @id;";

             IRecordsReader rr = sqlHelper.ExecuteReader(sql,
                    sqlHelper.CreateParameter("@id", Id));


             bool inuse = false;

             if (rr.Read())
             {
                 inuse = true;

             }

             rr.Dispose();

             return inuse;
        }
            



        public Umbraco.Forms.Core.FieldPreValueSourceType Type { get; set; }
        public  Dictionary<string, string> Settings { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

}
