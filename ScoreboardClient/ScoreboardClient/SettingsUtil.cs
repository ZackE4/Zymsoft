using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient
{
    public class SettingsUtil
    {
        private const string SettingsFileName = "SavedSettings.data";
        public static string SettingsFolderPath { get; set; }

        public async static Task<string> GetSetting(string key)
        {
            string value = "";
            try
            {
                await CreateSetingsFileIfNeccesary();
                string[] settings = await System.IO.File.ReadAllLinesAsync(Path.Combine(SettingsFolderPath, SettingsFileName));
                foreach(var setting in settings)
                {
                    if (setting.Trim().StartsWith(key))
                    {
                        value = setting.Split("::: ")[1];
                        break;
                    }
                }
            }
            catch(Exception e)
            {

            }
            return value;
        }

        public async static Task SetSetting(string key, string value)
        {
            try
            {
                await CreateSetingsFileIfNeccesary();
                string[] settings = await System.IO.File.ReadAllLinesAsync(Path.Combine(SettingsFolderPath, SettingsFileName));
                var newSettings = new List<string>();
                bool settingSet = false;
                foreach (var setting in settings)
                {
                    if (setting.Trim().StartsWith(key))
                    {
                        newSettings.Add(key + "::: " + value);
                        settingSet = true;
                    }
                    else
                    {
                        newSettings.Add(setting);
                    }
                }
                if (!settingSet)
                {
                    newSettings.Add(key + "::: " + value);
                }

                await System.IO.File.WriteAllLinesAsync(Path.Combine(SettingsFolderPath, SettingsFileName), newSettings);
            }
            catch (Exception e)
            {

            }
        }

        private async static Task CreateSetingsFileIfNeccesary()
        {
            Directory.CreateDirectory(SettingsFolderPath);
            if(!File.Exists(Path.Combine(SettingsFolderPath, SettingsFileName)))
            {
                await File.WriteAllTextAsync(Path.Combine(SettingsFolderPath, SettingsFileName), "");
            }
        }
    }
}
