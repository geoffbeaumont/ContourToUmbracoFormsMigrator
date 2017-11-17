using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration
{
    public class Record
    {
        public static Record Create()
        {
            Record record = new Record();

            return record;
        }

        public Record()
        {
        }

        public static Record CreateFromDataReader(IRecordsReader reader)
        {
            Record record = Create();
            record.Id            = reader.GetGuid("Id");
            record.Form          = reader.GetGuid("Form");
            record.Created       = reader.GetDateTime("Created");
            record.Updated       = reader.GetDateTime("Updated");
            record.State         = reader.GetInt("State");
            record.currentPage   = reader.GetGuid("currentPage");
            record.umbracoPageId = reader.GetInt("umbracoPageId");
            record.IP            = reader.GetString("IP");
            record.MemberKey     = reader.GetString("MemberKey");
            return record;
        }

        public Guid Id { get; set; }
        public Guid Form { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int State { get; set; }
        public Guid currentPage { get; set; }
        public int umbracoPageId { get; set; }
        public string IP { get; set; }
        public string MemberKey { get; set; }
        public Dictionary<Guid, RecordField> RecordFields { get; set; }
    }
}