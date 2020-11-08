using System;

namespace App.Models
{
    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
        public DateTime Created { get; set; }

        public Customer()
        {
        }

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Customer(long id, string firstName, string lastName, DateTime created)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Created = created;
        }
    }

}
