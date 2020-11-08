using App.Data;
using System.Collections.Generic;

namespace App.ViewModels
{
    public static class SettingsViewModel
    {
        public static List<string> Statuses { get; set; }
        public static int MaxItemsCount { get; set; }

        public static void Populate()
        {
            Statuses = SettingsContext.GetStatuses();

            MaxItemsCount = SettingsContext.GetMaxItemsCount();
        }
    }
}
