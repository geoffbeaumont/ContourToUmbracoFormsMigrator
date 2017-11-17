using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using System.Xml;

namespace Umbraco.Forms.Migration
{
    public class FieldSet : IDependencySupport, IFormObject
    {

        public static FieldSet Create() {
            FieldSet fs = new FieldSet();
            fs.Fields = new List<Field>();
            return fs;
        }

        public static FieldSet CreateFromDataReader(IRecordsReader reader)
        {
            FieldSet fs = new FieldSet();
            fs.Id = reader.GetGuid("id");
            fs.Caption = reader.GetString("caption");
            fs.SortOrder = reader.GetInt("sortorder");
            fs.Fields = new List<Field>();
            fs.Page = reader.GetGuid("page");

            return fs;
        }

        public XmlNode ToXml(XmlDocument xd) {
            XmlNode fs = umbraco.xmlHelper.addTextNode(xd, "fieldset", string.Empty);

            fs.AppendChild(umbraco.xmlHelper.addTextNode(xd, "id", Id.ToString()));
            fs.AppendChild(umbraco.xmlHelper.addTextNode(xd, "caption", Caption));
            fs.AppendChild(umbraco.xmlHelper.addTextNode(xd, "sortorder", SortOrder.ToString()));

            foreach (Field f in Fields) {
                fs.AppendChild(f.ToXml(xd));
            }

            return fs;
        }

        public List<Field> Fields { get; set; }     

        public string Caption { get; set; }

        public int SortOrder { get; set; }

        public Guid Id { get; set; }

        public Guid Page { get; set; }

      
    }
}
