using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using System.Xml;

namespace Umbraco.Forms.Migration
{
    public class Page : IDependencySupport, IFormObject
    {
        
        public static Page CreatePage()
        {
            Page page = new Page();
            page.FieldSets = new List<FieldSet>();

            return page;
        }

        public static Page CreateFromDataReader(IRecordsReader reader)
        {
            Page page = new Page();
            page.Id = reader.GetGuid("id");
            page.Caption = reader.GetString("caption");
            page.SortOrder = reader.GetInt("sortorder");
            page.FieldSets = new List<FieldSet>();
            page.Form = reader.GetGuid("form");
            return page;
        }

        public XmlNode ToXml(XmlDocument xd) {
            XmlNode p = umbraco.xmlHelper.addTextNode(xd, "page", string.Empty);
            
            p.AppendChild(umbraco.xmlHelper.addTextNode(xd, "id", Id.ToString()));
            p.AppendChild(umbraco.xmlHelper.addTextNode(xd, "caption", Caption));
            p.AppendChild(umbraco.xmlHelper.addTextNode(xd, "sortorder", SortOrder.ToString())); 
            
            foreach (FieldSet fs in FieldSets) {
                p.AppendChild(fs.ToXml(xd));
            }

            return p;
        }

        public List<FieldSet> FieldSets{ get; set;}

        public string Caption { get; set;}

        public int SortOrder { get; set; }

        public Guid Id { get; set; }

        public Guid Form { get; set; }
  
    }
}
