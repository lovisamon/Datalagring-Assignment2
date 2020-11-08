using App.Data;
using App.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public static class CustomerViewModel
    {
        private static ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();
        public static ObservableCollection<Customer> Customers => _customers;

        public static async Task PopulateAsync()
        {
            _customers.Clear();
            List<Customer> list = await SqliteContext.GetCustomersAsync();
            foreach (Customer customer in list)
            {
                _customers.Add(customer);
            }
        }
    }
}
