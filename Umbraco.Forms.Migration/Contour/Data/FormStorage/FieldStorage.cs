using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class FieldStorage : IDisposable
    {
        private ISqlHelper sqlHelper;
        

        private PrevalueSourceStorage prevalueSourceStorage;
        private SettingsStorage settings;
        private FieldConditionStorage conditionStorage;

        //public FieldStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public FieldStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connect){
            sqlHelper = connect;
            prevalueSourceStorage = new PrevalueSourceStorage(connect);
            settings = new SettingsStorage(connect);
            conditionStorage = new FieldConditionStorage(connect);
        }

        public List<Field> GetAllFields(Form form) {
            List<Field> l = new List<Field>();

            string sql = @"SELECT UFfields.*, UFpages.form AS form, UFfieldsets.sortorder as FieldsetIndex, UFpages.sortorder as PageIndex
                            From UFfields 
                            INNER JOIN UFfieldsets ON UFfieldsets.id = fieldset
                            INNER JOIN UFpages ON UFpages.id = UFfieldsets.page
                            where UFpages.form = @form 
                            ORDER by UfPages.SortOrder ASC, UFFieldsets.sortorder ASC, UFfields.sortOrder ASC
                            ";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql,
                sqlHelper.CreateParameter("@form", form.Id));

            while (rr.Read()) {
                Field f = Field.CreateFromDataReader(rr);

                if (!rr.IsNull("prevalueProvider"))
                    f.PreValueSource = prevalueSourceStorage.GetPrevalueSource(rr.GetGuid("prevalueProvider"));


                f.Settings = settings.GetSettingsAsList(f.Id);

                //if (f.FieldType.HasSettings())
                //    f.FieldType.LoadSettings(f.Settings);

                f.Condition = conditionStorage.GetFieldCondition(f);
                if (f.Condition == null)
                {
                    f.Condition = new FieldCondition();
                    f.Condition.Enabled = false;
                    f.Condition.ActionType = FieldConditionActionType.Show;
                    f.Condition.LogicType = FieldConditionLogicType.All;
                }
                l.Add(f);

            }

            rr.Dispose();

            return l;
        }

        public List<Field> GetAllFields(FieldSet fieldset)
        {
            List<Field> l = new List<Field>();

            string sql = @"SELECT UFfields.*, UFpages.form AS form, UFfieldsets.sortorder as FieldsetIndex, UFpages.sortorder as PageIndex
                            From UFfields 
                            INNER JOIN UFfieldsets ON UFfieldsets.id = fieldset
                            INNER JOIN UFpages ON UFpages.id = UFfieldsets.page
                            where fieldset = @fieldset ORDER by UFfields.sortOrder ASC";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@fieldset", fieldset.Id));

            while (rr.Read())
            {
                Field f = Field.CreateFromDataReader(rr);

                //if (!rr.IsNull("prevalueProvider")) {
                //    f.PreValueSource = prevalueSourceStorage.GetPrevalueSource(rr.GetGuid("prevalueProvider"));
                    
                //    if(f.PreValueSource.Type != null)
                //        f.PreValueSource.Type.LoadSettings(f.PreValueSource);
                //}

                f.Settings = settings.GetSettingsAsList(f.Id);

               
                //if (f.FieldType.HasSettings())
                //    f.FieldType.LoadSettings(f.Settings);

                f.Condition = conditionStorage.GetFieldCondition(f);
                if (f.Condition == null)
                {
                    f.Condition = new FieldCondition();
                    f.Condition.Enabled = false;
                    f.Condition.ActionType = FieldConditionActionType.Show;
                    f.Condition.LogicType = FieldConditionLogicType.All;
                }
                l.Add(f);
            }

            rr.Dispose();

            return l;
        }

        public List<Field> GetAllFields()
        {
            List<Field> l = new List<Field>();

            string sql = @"SELECT UFfields.*, UFpages.form AS form, UFfieldsets.sortorder as FieldsetIndex, UFpages.sortorder as PageIndex 
                            From UFfields 
                            INNER JOIN UFfieldsets ON UFfieldsets.id = fieldset
                            INNER JOIN UFpages ON UFpages.id = UFfieldsets.page
                            ORDER by UFFields.Caption ASC";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);
            
            while (rr.Read())
            {
                Field f = Field.CreateFromDataReader(rr);

                if (!rr.IsNull("prevalueProvider"))
                    f.PreValueSource = prevalueSourceStorage.GetPrevalueSource(rr.GetGuid("prevalueProvider"));

                f.Settings = settings.GetSettingsAsList(f.Id);

                //if (f.FieldType.HasSettings())
                //    f.FieldType.LoadSettings(f.Settings);

                f.Condition = conditionStorage.GetFieldCondition(f);
                if (f.Condition == null)
                {
                    f.Condition = new FieldCondition();
                    f.Condition.Enabled = false;
                    f.Condition.ActionType = FieldConditionActionType.Show;
                    f.Condition.LogicType = FieldConditionLogicType.All;
                }
                l.Add(f);
            }

            rr.Dispose();

            return l;
        }

        //The fastest way to test if a field is actually there
        public bool FieldExists(Guid id) {

            string sql = @"SELECT UFfields.Caption FROM UFfields where UFfields.id = @id";
            string caption = sqlHelper.ExecuteScalar<string>(sql, sqlHelper.CreateParameter("@id", id));

            return !string.IsNullOrEmpty(caption);
        }


        public Field GetField(Guid id)
        {
            string sql = @"SELECT UFfields.*, UFpages.form AS form, UFfieldsets.sortorder as FieldsetIndex, UFpages.sortorder as PageIndex  
                            From UFfields 
                            INNER JOIN UFfieldsets ON UFfieldsets.id = fieldset
                            INNER JOIN UFpages ON UFpages.id = UFfieldsets.page
                            where UFfields.id = @id";
            
            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

            Field f = new Field();
            if(rr.Read()){
                f = Field.CreateFromDataReader(rr);

                if (!rr.IsNull("prevalueProvider") && rr.GetGuid("prevalueProvider") != Guid.Empty)
                    f.PreValueSource = prevalueSourceStorage.GetPrevalueSource(rr.GetGuid("prevalueProvider"));

                f.Settings = settings.GetSettingsAsList(f.Id);
                
                //if (f.FieldType.HasSettings())
                //    f.FieldType.LoadSettings(f.Settings);

                f.Condition = conditionStorage.GetFieldCondition(f);
                if (f.Condition == null)
                {
                    f.Condition = new FieldCondition();
                    f.Condition.Enabled = false;
                    f.Condition.ActionType = FieldConditionActionType.Show;
                    f.Condition.LogicType = FieldConditionLogicType.All;
                }
            }

            rr.Dispose();
            return f;
        }     

      

        #region IDisposable Members

        public void Dispose()
        {
            prevalueSourceStorage.Dispose();
            prevalueSourceStorage = null;

            conditionStorage.Dispose();
            conditionStorage = null;

            sqlHelper.Dispose();
            sqlHelper = null;
        }

        #endregion
    }
}
