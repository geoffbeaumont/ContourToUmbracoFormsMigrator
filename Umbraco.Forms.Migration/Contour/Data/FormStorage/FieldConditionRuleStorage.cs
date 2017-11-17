using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration;

namespace Umbraco.Forms.Migration.Data.Storage
{
    public class FieldConditionRuleStorage : IDisposable
    {
        private ISqlHelper sqlHelper;

        //public FieldConditionRuleStorage()
        //{
        //    init(Helper.SqlHelper);
        //}

        public FieldConditionRuleStorage(ISqlHelper connection)
        {
            init(connection);
        }

        private void init(ISqlHelper connect)
        {
            sqlHelper = connect;
        }

        public FieldConditionRule GetFieldConditionRule(Guid id)
        {
            string sql = "SELECT * from UFfieldconditionrules where id = @id";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

            FieldConditionRule fcr = new FieldConditionRule();
            if (rr.Read())
            {
                fcr = FieldConditionRule.CreateFromDataReader(rr);
            }

            rr.Dispose();

            return fcr;
        }

        public List<FieldConditionRule> GetAllFieldConditionRules(FieldCondition fieldCondition)
        {
            List<FieldConditionRule> l = new List<FieldConditionRule>();

            string sql = "SELECT * from UFfieldconditionrules where fieldcondition = @fieldcondition";

            IRecordsReader rr = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@fieldcondition", fieldCondition.Id));

            FieldConditionRule fcr = new FieldConditionRule();
            while (rr.Read())
            {
                fcr = FieldConditionRule.CreateFromDataReader(rr);
                l.Add(fcr);
            }

            rr.Dispose();

            return l;
        }

        public FieldConditionRule InsertFieldConditionRule(FieldConditionRule fieldConditionRule)
        {
            fieldConditionRule.Id = Guid.NewGuid();

            string sql = "INSERT INTO UFfieldconditionrules(id, fieldcondition, field, operator, value) VALUES(@id, @fieldcondition, @field, @operator, @value);";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", fieldConditionRule.Id),
                sqlHelper.CreateParameter("@fieldcondition", fieldConditionRule.FieldCondition),
                sqlHelper.CreateParameter("@field", fieldConditionRule.Field),
                sqlHelper.CreateParameter("@operator", fieldConditionRule.Operator),
                sqlHelper.CreateParameter("@value", fieldConditionRule.Value));

            return fieldConditionRule;
        }

        public bool DeleteFieldConditionRules(FieldCondition fieldCondition)
        {
            string sql = "DELETE FROM UFfieldconditionrules where fieldcondition = @fieldcondition";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@fieldcondition", fieldCondition.Id));

            return true;
        }

        public bool DeleteFieldConditionRule(FieldConditionRule fieldConditionRule)
        {
            string sql = "DELETE FROM UFfieldconditionrules where id = @id";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", fieldConditionRule.Id));

            return true;
        }

        public FieldConditionRule UpdateFieldConditionRule(FieldConditionRule fieldConditionRule)
        {
            string sql = "UPDATE UFfieldconditionrules set fieldcondition = @fieldcondition, field = @field, operator = @operator, value = @value WHERE id = @id";

            sqlHelper.ExecuteNonQuery(sql,
                sqlHelper.CreateParameter("@id", fieldConditionRule.Id),
                sqlHelper.CreateParameter("@fieldcondition", fieldConditionRule.FieldCondition),
                sqlHelper.CreateParameter("@field", fieldConditionRule.Field),
                sqlHelper.CreateParameter("@operator", fieldConditionRule.Operator),
                sqlHelper.CreateParameter("@value", fieldConditionRule.Value));

            return fieldConditionRule;
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
