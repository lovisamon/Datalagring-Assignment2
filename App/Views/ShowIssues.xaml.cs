using App.Data;
using App.Models;
using App.ViewModels;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowIssues : Page
    {
        ObservableCollection<string> _filters = new ObservableCollection<string>() { "show all" };
        public ObservableCollection<string> Filters => _filters;

        public ShowIssues()
        {
            this.InitializeComponent();

            IssueViewModel.PopulateAsync(SettingsViewModel.MaxItemsCount).GetAwaiter();
            lvIssues.ItemsSource = IssueViewModel.Issues;


            cbxFilters.ItemsSource = Filters;
            foreach (string status in SettingsViewModel.Statuses)
            {
                Filters.Add(status);
            }
            cbxFilters.SelectedIndex = 0;

            cbxStatuses.ItemsSource = SettingsViewModel.Statuses;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filter = cbxFilters.SelectedValue.ToString();

            if (filter == "show all")
            {
                IssueViewModel.PopulateAsync(SettingsViewModel.MaxItemsCount).GetAwaiter();
            }
            else
            {
                IssueViewModel.PopulateByStatusAsync(filter, SettingsViewModel.MaxItemsCount).GetAwaiter();
            }
        }

        private async void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (lvIssues.SelectedItem == null) return;

            Issue issue = (Issue)lvIssues.SelectedItem;

            string newStatus = cbxStatuses.SelectedValue.ToString();
            issue.Status = newStatus;

            await SqliteContext.UpdateIssueAsync(issue);

            // Update list 
            // IssueViewModel.ReplaceIssue(lvIssues.SelectedIndex, issue);
        }

        private async void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (lvIssues.SelectedItem == null) return;

            Issue issue = (Issue)lvIssues.SelectedItem;
            
            Comment comment = new Comment(
                issue.Id,
                tbxCommentDescription.Text
                );
            long id = await SqliteContext.CreateCommentAsync(comment);

            tbxCommentDescription.Text = "";

            // Update list
            // issue.Comments = await SqliteContext.GetCommentsByIssueIdAsync(issue.Id, SettingsViewModel.MaxItemsCount);
            // IssueViewModel.ReplaceIssue(lvIssues.SelectedIndex, issue);
        }
    }
}
