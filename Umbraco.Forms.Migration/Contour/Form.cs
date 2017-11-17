using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using System.Xml;
using Umbraco.Forms.Migration.Enums;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Umbraco.Forms.Migration
{
    public class Form : IDependencySupport, IFormObject, ICloneable
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public FormFieldIndication FieldIndicationType { get; set; }
        public string Indicator { get; set; }

        public bool ShowValidationSummary { get; set; }
        public bool HideFieldValidation { get; set; }

        public string RequiredErrorMessage { get; set; }
        public string InvalidErrorMessage { get; set; }

        public string MessageOnSubmit { get; set; }
        public int GoToPageOnSubmit { get; set; }
        public string XPathOnSubmit { get; set; }

        public Boolean ManualApproval { get; set; }
        public Boolean Archived { get; set; }
        public Boolean StoreRecordsLocally { get; set; }
        public Boolean DisableDefaultStylesheet { get; set; }

        public static Form Create()
        {
            Form form = new Form();
            form.Pages = new List<Page>();
            //FormSettings defaults = new FormSettings();
            //form = defaults.Set(form);
            form.MessageOnSubmit = "";
            form.Indicator = "*";
            form.InvalidErrorMessage = "{0} is not valid";
            form.ManualApproval = false;
            form.ShowValidationSummary = false;
            form.StoreRecordsLocally = true;
            form.RequiredErrorMessage = "{0} is mandatory";
            form.HideFieldValidation = false;
            form.Archived = false;
            form.FieldIndicationType = FormFieldIndication.MarkMandatoryFields;

            return form;
        }

        public Form()
        {
        }
               

        public static Form CreateFromDataReader(IRecordsReader reader)
        {
            Form form = Create();
            form.Id = reader.GetGuid("id");
            form.Pages = new List<Page>();
            form.Name = reader.GetString("name");
            form.Created = reader.GetDateTime("created");
            form.MessageOnSubmit = reader.GetString("MessageOnSubmit");
            form.GoToPageOnSubmit = reader.GetInt("GotoPageOnSubmit");

            form.ShowValidationSummary = reader.GetBoolean("ShowValidationSummary");
            
            form.HideFieldValidation = reader.GetBoolean("HideFieldValidation");
            form.RequiredErrorMessage = reader.GetString("RequiredErrorMessage");
            form.InvalidErrorMessage = reader.GetString("InvalidErrorMessage");
            form.ManualApproval = reader.GetBoolean("ManualApproval");
            form.Archived = reader.GetBoolean("Archived");
            form.StoreRecordsLocally = reader.GetBoolean("StoreRecordsLocally");

            if(!reader.IsNull("DataSource"))
                form.DataSource = reader.GetGuid("DataSource");

            if (!reader.IsNull("FieldIndicationType"))
            {
                form.FieldIndicationType = (FormFieldIndication)System.Enum.Parse(typeof(FormFieldIndication), reader.GetInt("FieldIndicationType").ToString());
                form.Indicator = reader.GetString("Indicator");
            }

            if (!reader.IsNull("DisableDefaultStylesheet"))
            {
                form.DisableDefaultStylesheet = reader.GetBoolean("DisableDefaultStylesheet");
            }

            //check for xpath setting
            //Umbraco.Forms.Migration.Data.Storage.SettingsStorage ss = new Umbraco.Forms.Migration.Data.Storage.SettingsStorage();

            //string set = ss.GetSetting(form.Id, "XPathOnSubmit");
            //if (!string.IsNullOrEmpty(set))
            //    form.XPathOnSubmit = set;

            //ss.Dispose();

            return form;
        }

        public List<Page> Pages { get; set; }

        //public bool RequiresScriptManager() {
        //    return  (AllFields.Where(field => (field.FieldType.RequiresScriptManager == true)).Count() > 0);
        //}

        public XmlNode ToXml(XmlDocument xd) {
            XmlNode n = umbraco.xmlHelper.addTextNode(xd, "form", string.Empty);

            foreach (Page p in Pages) {
                n.AppendChild(p.ToXml(xd));   
            }

            return n;
        }


        [XmlIgnore]
        public List<Field> AllFields {
            get {
                List<Field> _all = new List<Field>();
                
                foreach (Page p in Pages) {
                    foreach (FieldSet fs in p.FieldSets) {
                        foreach (Field f in fs.Fields)
                            _all.Add(f);
                    }
                }
                

                return _all;
            }
        }

        public Guid DataSource {get; set;}
        public Guid Id { get; set; }
        
        #region IDependencySupport Members

       
        //public List<IDependencyType> SupportedDependencies { get; set; }

        #endregion

        #region ICloneable Members

        public object Clone() {
            object clone;
            using (MemoryStream stream = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                // Serialize this object
                formatter.Serialize(stream, this);
                stream.Position = 0;
                // Deserialize to another object
                clone = formatter.Deserialize(stream);
            }

            return clone;
        }

        #endregion
    }
}
