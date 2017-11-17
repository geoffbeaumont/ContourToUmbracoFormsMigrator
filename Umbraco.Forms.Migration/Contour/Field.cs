using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using System.Xml;
using System.Xml.Serialization;

namespace Umbraco.Forms.Migration
{
    public class Field : IDependencySupport, IFormObject
    {
        public static Field Create() {
            Field f = new Field();
            f.Values = new List<object>();
            f.PreValues = new List<PreValue>();
            f.Settings = new List<Umbraco.Forms.Migration.Data.Storage.Setting<string, string>>();
            return f;
        }

        public static Field CreateFromDataReader(IRecordsReader reader)
        {
            Field f = Create();
            f.Id = reader.GetGuid("id");
            f.Values = new List<object>();
            f.PreValues = new List<PreValue>();
            
            f.Caption = reader.GetString("caption");
            f.ToolTip = reader.GetString("tooltip");
            
            f.RequiredErrorMessage = reader.GetString("RequiredErrorMessage");
            f.InvalidErrorMessage = reader.GetString("InvalidErrorMessage"); 

            f.DataSourceFieldKey = reader.GetObject("DataSourceField");

            f.FieldSet = reader.GetGuid("fieldset");
            f.Form = reader.GetGuid("form");
            f.Mandatory = reader.GetBoolean("mandatory");

            f.RegEx = reader.GetString("regex");

            f.SortOrder = reader.GetInt("sortOrder");
            
            f.FieldsetIndex = reader.GetInt("FieldsetIndex");
            f.PageIndex = reader.GetInt("PageIndex");

            f.FieldTypeId = reader.GetGuid("fieldType");
            Type _t = Umbraco.Forms.Core.Providers.FieldTypeProviderCollection.Instance.GetProvider(f.FieldTypeId).GetType();
            f.FieldType = (Umbraco.Forms.Core.FieldType)Activator.CreateInstance(_t);
            
            f.Settings = new List<Umbraco.Forms.Migration.Data.Storage.Setting<string, string>>();

            return f;
        }

        public XmlNode ToXml(XmlDocument xd) {
            XmlNode f = umbraco.xmlHelper.addTextNode(xd, "field", string.Empty);

            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "id", Id.ToString()));
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "caption", Caption));
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "sortorder", SortOrder.ToString()));
            
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "mandatory", Mandatory.ToString()));
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "form", Form.ToString()));
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "fieldset", FieldSet.ToString()));
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "regex", RegEx));
            
            f.AppendChild(umbraco.xmlHelper.addTextNode(xd, "fieldtype", FieldType.Id.ToString()));

            return f;
        }

        [XmlIgnore]
        public List<Object> Values { get; set; }
        public List<PreValue> PreValues {get; set;}

        public string Caption { get; set; }

        public string ToolTip { get; set; }

        [XmlIgnore]
        public Umbraco.Forms.Core.FieldType FieldType { get; set; }

        [XmlIgnore]
        public Object DataSourceFieldKey { get; set; }
            
        public int SortOrder { get; set; }

        public int PageIndex { get; set; }

        public int FieldsetIndex { get; set; }
        
        public Guid Id { get; set; }

        public Guid FieldSet { get; set; }

        public Guid Form { get; set; }

        public Guid FieldTypeId { get; set; }

        public bool Mandatory { get; set; }

        public string RegEx { get; set; }

        public string RequiredErrorMessage { get; set; }
        public string InvalidErrorMessage { get; set; }

        [XmlIgnore]
        public FieldPreValueSource PreValueSource { get; set; }

        public Guid PreValueSourceId { get; set; }
                
        public List<Umbraco.Forms.Migration.Data.Storage.Setting<string, string>> Settings { get; set; }

        public FieldCondition Condition { get; set; }
    }
}
