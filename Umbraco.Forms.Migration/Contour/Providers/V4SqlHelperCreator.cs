using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;
using Umbraco.Forms.Migration.Interfaces;

namespace Umbraco.Forms.Migration.Providers
{
    public class V4SqlHelperCreator: ISqlHelperCreator
    {
        public ISqlHelper Create(string connString)
        {
            return DataLayerHelper.CreateSqlHelper(connString);
        }
    }
}
