using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class FieldConditionStorage : IDisposable
    {
        private ISqlHelper sqlHelper;
        private FieldConditionRuleStorage storage;

        //public FieldConditionStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public FieldConditionStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connect){
            sqlHelper = connect;
            storage = new FieldConditionRuleStorage(connect);
        }

        public FieldCondition GetAllFieldConditions()
        {
            string sql = "SELECT * from UFfieldconditions";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql);

            FieldCondition fc = null;
            if (rr.Read())
            {
                fc = new FieldCondition();
                fc = FieldCondition.CreateFromDataReader(rr);
                fc.Rules = storage.GetAllFieldConditionRules(fc);
            }


            rr.Dispose();

            return fc;
        }

        public FieldCondition GetFieldCondition(Guid id)
        {
            string sql = "SELECT * from UFfieldconditions where id = @id";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

            FieldCondition fc = new FieldCondition();
            if (rr.Read())
            {
                fc = FieldCondition.CreateFromDataReader(rr);
                fc.Rules.AddRange(storage.GetAllFieldConditionRules(fc));
            }

            rr.Dispose();

            return fc;
        }

        public FieldCondition GetFieldCondition(Field field)
        {
            string sql = "SELECT * from UFfieldconditions where field = @field";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@field", field.Id));

            FieldCondition fc = null;
            if (rr.Read())
            {
                fc = new FieldCondition();
                fc = FieldCondition.CreateFromDataReader(rr);
                fc.Rules = storage.GetAllFieldConditionRules(fc);
            }

           

            rr.Dispose();

            return fc;
        }

        public FieldCondition InsertFieldCondition(Field field, bool enabled, FieldConditionActionType actionType, FieldConditionLogicType logicType)
        {
            FieldCondition fc = new FieldCondition();
            fc.Field = field.Id;
            fc.Enabled = enabled;
            fc.ActionType= actionType;
            fc.LogicType = logicType;
            return InsertFieldCondition(fc);    
        }

        public FieldCondition InsertFieldCondition(FieldCondition fieldCondition, bool ignoreFieldCondtionRules = false)
        {
            fieldCondition.Id = Guid.NewGuid();

            string sql = "INSERT INTO UFfieldconditions(id, field, enabled, actiontype, logictype) VALUES(@id, @field, @enabled, @actiontype, @logictype);";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", fieldCondition.Id),
                sqlHelper.CreateParameter("@field", fieldCondition.Field),
                sqlHelper.CreateParameter("@enabled", fieldCondition.Enabled),
                sqlHelper.CreateParameter("@actiontype", fieldCondition.ActionType),
                sqlHelper.CreateParameter("@logictype", fieldCondition.LogicType));

            if (fieldCondition.Rules != null)
            {
                List<FieldConditionRule> _rules = new List<FieldConditionRule>();
                foreach (FieldConditionRule fcr in fieldCondition.Rules)
                {
                    fcr.FieldCondition = fieldCondition.Id;

                    if (!ignoreFieldCondtionRules)
                    {
                        _rules.Add(storage.InsertFieldConditionRule(fcr));
                    }
                    else
                    {
                        _rules.Add(fcr);
                    }

                }

                fieldCondition.Rules = _rules;
            }
            return fieldCondition;
        }

        public bool DeleteFieldCondition(Field field)
        {
            
            DeleteFieldCondition(field.Condition);

            return true;
        }

        public bool DeleteFieldCondition(FieldCondition fieldCondition)
        {

            storage.DeleteFieldConditionRules(fieldCondition);
           
            string sql = "DELETE FROM UFfieldconditions where id = @id";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", fieldCondition.Id));

            return true;
        }

        public FieldCondition UpdateFieldCondition(FieldCondition fieldCondition)
        {
            string sql = "UPDATE UFfieldconditions set  enabled = @enabled, actiontype = @actiontype, logictype = @logictype WHERE field = @field";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@field", fieldCondition.Field),
                sqlHelper.CreateParameter("@enabled", fieldCondition.Enabled),
                sqlHelper.CreateParameter("@actiontype", fieldCondition.ActionType),
                sqlHelper.CreateParameter("@logictype", fieldCondition.LogicType));

            fieldCondition.Id = sqlHelper.ExecuteScalar<Guid>("Select Id from UFfieldconditions where field = @field",
                                                              sqlHelper.CreateParameter("@field", fieldCondition.Field));
            
            storage.DeleteFieldConditionRules(fieldCondition);
            if (fieldCondition.Rules != null)
            {
                List<FieldConditionRule> _rules = new List<FieldConditionRule>();
                foreach (FieldConditionRule fcr in fieldCondition.Rules)
                {
                    fcr.FieldCondition = fieldCondition.Id;
                   
                    _rules.Add(storage.InsertFieldConditionRule(fcr));

                }

                fieldCondition.Rules = _rules;
            }
            return fieldCondition;
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
