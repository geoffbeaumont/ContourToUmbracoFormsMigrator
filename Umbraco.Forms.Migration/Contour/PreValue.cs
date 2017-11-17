using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration
{
    public class PreValue
    {
        public static PreValue Create()
        {
            PreValue pv= new PreValue();
            return pv;
        }

        public static PreValue CreateFromDataReader(IRecordsReader reader)
        {
            PreValue pv = new PreValue();
            pv.Id = reader.GetGuid("id");
            pv.Value = reader.GetString("value");
            pv.Field = reader.GetGuid("field");
            pv.SortOrder = reader.GetInt("sortorder");
            return pv;
        }

        public Object Id { get; set; }
        public Guid Field { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
    }
}
