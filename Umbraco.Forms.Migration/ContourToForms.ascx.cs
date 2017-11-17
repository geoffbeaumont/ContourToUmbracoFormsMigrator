using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Umbraco.Forms.Migration
{
    public partial class ContourToForms : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var success = true;
            try
            {
                var s = new MigrationService
                {
                    IgnoreRecords = this.ignoreRecords.Checked
                };
                s.Migrate(TextBox1.Text);
            }
            catch (Exception ex)
            {
                success = false;
                Literal1.Text = "<h4>Oops something whent wrong</h4><h5>" + ex.Message + "</h5><p>" + ex.StackTrace +"</p>";
            }
            if(success)
                Literal1.Text = "<h4>Migration completed<h4><p>Your forms have been migrated from the Contour instance.</p>";
            
        }
    }
}