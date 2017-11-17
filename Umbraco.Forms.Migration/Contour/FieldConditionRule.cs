using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration
{
    public class FieldConditionRule : IFormObject
    {
        public static FieldConditionRule Create()
        {
            FieldConditionRule fcr = new FieldConditionRule();
            return fcr;
        }

        public static FieldConditionRule CreateFromDataReader(IRecordsReader reader)
        {
            FieldConditionRule fcr = Create();
            fcr.Id = reader.GetGuid("id");
            fcr.FieldCondition = reader.GetGuid("FieldCondition");
            fcr.Field = reader.GetGuid("Field");
            fcr.Operator = (FieldConditionRuleOperator)System.Enum.Parse(typeof(FieldConditionRuleOperator), reader.GetInt("Operator").ToString());
            fcr.Value = reader.GetString("Value");

            return fcr;
        }

        public Guid Id { get; set; }
        public Guid FieldCondition { get; set; }
        public Guid Field { get; set; }
        public FieldConditionRuleOperator Operator { get; set; }
        public string Value { get; set; }
    }
}
