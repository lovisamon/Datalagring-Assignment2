using App.Data;
using App.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App.ViewModels
{
    public static class IssueViewModel
    {
        private static ObservableCollection<Issue> _issues = new ObservableCollection<Issue>();
        public static ObservableCollection<Issue> Issues => _issues;

        public static void ReplaceIssue(int index, Issue issue)
        {
            _issues[index] = issue;
        }

        public static async Task PopulateAsync(int maxItemsCount)
        {
            _issues.Clear();
            List<Issue> list = await SqliteContext.GetIssuesAsync(maxItemsCount);
            foreach (Issue issue in list)
            {
                issue.Comments = await SqliteContext.GetCommentsByIssueIdAsync(issue.Id, maxItemsCount);
                issue.Customer = await SqliteContext.GetCustomerByIdAsync(issue.CustomerId);
                _issues.Add(issue);
            }
        }
        
        public static async Task PopulateByStatusAsync(string status, int maxItemsCount)
        {
            _issues.Clear();
            List<Issue> list = await SqliteContext.GetIssuesByStatusAsync(status, maxItemsCount);
            foreach (Issue issue in list)
            {
                issue.Comments = await SqliteContext.GetCommentsByIssueIdAsync(issue.Id, maxItemsCount);
                issue.Customer = await SqliteContext.GetCustomerByIdAsync(issue.CustomerId);
                _issues.Add(issue);
            }
        }
    }
}
