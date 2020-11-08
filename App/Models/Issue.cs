using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace App.Models
{
    public class Issue : INotifyPropertyChanged
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }

        public List<Comment> Comments { get;set;}
        public Customer Customer { get; set; }

        public Issue()
        {
        }

        public Issue(long customerId, string title, string description, string status)
        {
            CustomerId = customerId;
            Title = title;
            Description = description;
            Status = status;
        }

        public Issue(long id, long customerId, string title, string description, string status, DateTime created)
        {
            Id = id;
            CustomerId = customerId;
            Title = title;
            Description = description;
            Status = status;
            Created = created;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
