using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Forms.Migration.DataLayer;

namespace Umbraco.Forms.Migration.Data.Storage {
    
        [Serializable]
        public struct Setting<K,V>
        {
          public K Key {get;set;}
          public V Value {get;set;}
        }


        public class SettingsStorage : IDisposable {

            private ISqlHelper sqlHelper;

            //public SettingsStorage() {
            //    init(Helper.SqlHelper);
            //}

            public SettingsStorage(ISqlHelper connection) {
                init(connection);
            }

            private void init(ISqlHelper connection) {
                sqlHelper = connection;
            }

           

            public Dictionary<string, string> GetSettings(Guid id) {

                string sql = "SELECT * from UFSettings where id = @id";
                IRecordsReader settingsReader = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

                Dictionary<string, string> settings = new Dictionary<string, string>();

                while (settingsReader.Read()) {
                    settings.Add(settingsReader.GetString("key"), settingsReader.GetString("value"));
                }

                settingsReader.Dispose();

                return settings;
            }

            public string GetSetting(Guid id, string key)
            {
                string set = string.Empty;

                string sql = "SELECT * from UFSettings where id = @id and [key] = @key";
                IRecordsReader settingsReader = sqlHelper.ExecuteReader(sql, 
                    sqlHelper.CreateParameter("@id", id),
                    sqlHelper.CreateParameter("@key", key));

                if (settingsReader.Read())
                    set = settingsReader.GetString("value");

                settingsReader.Dispose();

                return set;
            }

            //added list variant since dictionary doesn't support serialization (form export/import)
            public List<Setting<string, string>> GetSettingsAsList(Guid id)
            {

                string sql = "SELECT * from UFSettings where id = @id";
                IRecordsReader settingsReader = sqlHelper.ExecuteReader(sql, sqlHelper.CreateParameter("@id", id));

                List<Setting<string, string>> settings = new List<Setting<string, string>>();

                while (settingsReader.Read())
                {
                    Setting<string, string> setting = new Setting<string, string>();
                    setting.Key = settingsReader.GetString("key");
                    setting.Value = settingsReader.GetString("value");
                    settings.Add(setting);
                }

                settingsReader.Dispose();

                return settings;
            }

            public void ClearSetting(Guid id, string key)
            {
                string sql = "DELETE from UFSettings where id = @id and [key] = @key";
                sqlHelper.ExecuteNonQuery(sql, 
                    sqlHelper.CreateParameter("@id", id),
                    sqlHelper.CreateParameter("@key", key));
            }


            public void ClearSettings(Guid id) {
                string sql = "DELETE from UFSettings where id = @id";
                sqlHelper.ExecuteNonQuery(sql, sqlHelper.CreateParameter("@id", id));
            }


            public void InsertSetting(Guid id, string key, string value)
            {

               
                string sql = "INSERT INTO UFSettings(id, [key], value) VALUES(@id, @key, @value)";
                sqlHelper.ExecuteNonQuery(sql,
                    sqlHelper.CreateParameter("@id", id),
                    sqlHelper.CreateParameter("@key", key),
                    sqlHelper.CreateParameter("@value", value));
               

            }

            public void InsertSettings(Guid id, List<Setting<string, string>> settings)
            {

                foreach (Setting<string, string> s in settings)
                {
                    string sql = "INSERT INTO UFSettings(id, [key], value) VALUES(@id, @key, @value)";
                    sqlHelper.ExecuteNonQuery(sql,
                        sqlHelper.CreateParameter("@id", id),
                        sqlHelper.CreateParameter("@key", s.Key),
                        sqlHelper.CreateParameter("@value", s.Value));
                }

            }

            public void InsertSettings(Guid id, Dictionary<string, string> settings)
            {

                foreach (KeyValuePair<string, string> s in settings)
                {
                    string sql = "INSERT INTO UFSettings(id, [key], value) VALUES(@id, @key, @value)";
                    sqlHelper.ExecuteNonQuery(sql,
                        sqlHelper.CreateParameter("@id", id),
                        sqlHelper.CreateParameter("@key", s.Key),
                        sqlHelper.CreateParameter("@value", s.Value));
                }

            }

            public void UpdateSetting(Guid id, string key, string value)
            {
                ClearSetting(id,key);
                InsertSetting(id, key,value);
            }

            public void UpdateSettings(Guid id, Dictionary<string, string> settings) {
                ClearSettings(id);
                InsertSettings(id, settings);
            }

            public void UpdateSettings(Guid id, List<Setting<string, string>> settings)
            {
                ClearSettings(id);
                InsertSettings(id, settings);
            }


            #region IDisposable Members

            public void Dispose() {
                sqlHelper.Dispose();
                sqlHelper = null;
            }

            #endregion
        }

    
}
