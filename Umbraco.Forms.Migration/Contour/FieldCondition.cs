using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration
{
    public class FieldCondition : IFormObject
    {
        public static FieldCondition Create()
        {
            FieldCondition fc = new FieldCondition();
            fc.Rules = new List<FieldConditionRule>();
            return fc;
        }

        public static FieldCondition CreateFromDataReader(IRecordsReader reader)
        {
            FieldCondition fc = Create();
            fc.Id = reader.GetGuid("id");
            fc.Field = reader.GetGuid("Field");
            fc.Enabled = reader.GetBoolean("Enabled");
            fc.ActionType = (FieldConditionActionType)System.Enum.Parse(typeof(FieldConditionActionType), reader.GetInt("ActionType").ToString());
            fc.LogicType = (FieldConditionLogicType)System.Enum.Parse(typeof(FieldConditionLogicType), reader.GetInt("LogicType").ToString());

            fc.Rules = new List<FieldConditionRule>();

            return fc;
        }

        public Guid Id { get; set; }

        public Guid Field { get; set; }

        public bool Enabled { get; set; }

        public FieldConditionActionType ActionType { get; set; }

        public FieldConditionLogicType  LogicType { get; set; }

        public List<FieldConditionRule> Rules { get; set; }
    }
}
