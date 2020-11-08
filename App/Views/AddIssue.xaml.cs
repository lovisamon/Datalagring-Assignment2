using App.Data;
using App.Models;
using App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddIssue : Page
    {
        private StorageFile _file = null;
        public AddIssue()
        {
            this.InitializeComponent();
            CustomerViewModel.PopulateAsync().GetAwaiter();
            cbxCustomers.ItemsSource = CustomerViewModel.Customers;
        }

        private async void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            long id = await SqliteContext.CreateCustomerAsync(new Customer(tbxFirstName.Text, tbxLastName.Text));
            // Update list by fetching from database for good measures
            CustomerViewModel.Customers.Add(SqliteContext.GetCustomerByIdAsync(id).GetAwaiter().GetResult());
        }

        private async void btnAddIssue_Click(object sender, RoutedEventArgs e)
        {
            if (cbxCustomers.SelectedItem == null) return;

            Issue issue = new Issue(
                ((Customer)cbxCustomers.SelectedItem).Id,
                tbxTitle.Text,
                tbxDescription.Text,
                "pending"
                );

            await SqliteContext.CreateIssueAsync(issue);
        }
    }
}
