using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace App.Data
{
    public static class SettingsContext
    {
        public static Settings Settings;

        public static List<string> GetStatuses()
        {
            return Settings.Statuses;
        }

        public static int GetMaxItemsCount()
        {
            return Settings.MaxItemsCount;
        }

        public static async Task UseSettingsAsync()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = null;

            try
            {
                storageFile = await storageFolder.GetFileAsync(@"Files\settings.json");
            }
            catch
            {
                storageFile = await storageFolder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
                StreamWriter writer = new StreamWriter(await storageFile.OpenStreamForWriteAsync());

                string json = "{\"statuses\": [ \"pending\", \"active\", \"closed\" ], \"maxitemscount\": 10}";
                await writer.WriteLineAsync(json);

                await writer.FlushAsync();
                writer.Close();
                writer.Dispose();
            }

            Settings = JsonConvert.DeserializeObject<Settings>(await FileIO.ReadTextAsync(storageFile));
        }
    }
}
