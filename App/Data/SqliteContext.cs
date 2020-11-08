using App.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace App.Data
{
    public static class SqliteContext
    {
        #region Configuration and Properties
        private static string _dbPath { get; set; }
        public static async Task UseSQLiteAsync(string dbName = "sqlite.db")
        {
            //await ApplicationData.Current.LocalFolder.CreateFileAsync(dbName, CreationCollisionOption.ReplaceExisting);
            await ApplicationData.Current.LocalFolder.CreateFileAsync(dbName, CreationCollisionOption.OpenIfExists);
            _dbPath = $"Filename={Path.Combine(ApplicationData.Current.LocalFolder.Path, dbName)}";

            SqliteConnection db = new SqliteConnection(_dbPath);
           
            db.Open();

            string query = "CREATE TABLE IF NOT EXISTS Customers(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, Created DATETIME NOT NULL); CREATE TABLE IF NOT EXISTS Issues(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, CustomerId INTEGER NOT NULL, Title TEXT NOT NULL, Description TEXT NOT NULL, Status TEXT NOT NULL, Created DATETIME NOT NULL, FOREIGN KEY(CustomerId) REFERENCES Customers(Id)); CREATE TABLE IF NOT EXISTS Comments (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IssueId INTEGER NOT NULL, Description TEXT NOT NULL, Created DATETIME NOT NULL, FOREIGN KEY (IssueId) REFERENCES Issues(Id));";
      
            SqliteCommand cmd = new SqliteCommand(query, db);
            await cmd.ExecuteNonQueryAsync();

            db.Close();
        }

        #endregion

        #region Create Methods
        public static async Task<long> CreateCustomerAsync(Customer customer)
        {
            long id = 0;
            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = @"INSERT INTO Customers VALUES(null,@FirstName,@LastName,@Created);";
            SqliteCommand cmd = new SqliteCommand(query, db);

            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();
            
            cmd.CommandText = "SELECT last_insert_rowid()";
            id = (long)await cmd.ExecuteScalarAsync();

            db.Close();

            return id;
        }

        public static async Task<long> CreateIssueAsync(Issue issue)
        {
            long id = 0;
            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "INSERT INTO Issues VALUES(null,@CustomerId,@Title,@Description,@Status,@Created);";
            SqliteCommand cmd = new SqliteCommand(query, db);

            cmd.Parameters.AddWithValue("@CustomerId", issue.CustomerId);
            cmd.Parameters.AddWithValue("@Title", issue.Title);
            cmd.Parameters.AddWithValue("@Description", issue.Description);
            cmd.Parameters.AddWithValue("@Status", issue.Status);
            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "SELECT last_insert_rowid()";
            id = (long)await cmd.ExecuteScalarAsync();

            db.Close();

            return id;
        }

        public static async Task<long> CreateCommentAsync(Comment comment)
        {
            long id = 0;
            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "INSERT INTO Comments VALUES(null,@IssueId,@Description,@Created);";
            SqliteCommand cmd = new SqliteCommand(query, db);

            cmd.Parameters.AddWithValue("@Id", comment.Id);
            cmd.Parameters.AddWithValue("@IssueId", comment.IssueId);
            cmd.Parameters.AddWithValue("@Description", comment.Description);
            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "SELECT last_insert_rowid()";
            id = (long)await cmd.ExecuteScalarAsync();

            db.Close();

            return id;
        }

        #endregion

        #region Get Methods
        public static async Task<List<Customer>> GetCustomersAsync()
        {
            List<Customer> customers = new List<Customer>();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Customers";
            SqliteCommand cmd = new SqliteCommand(query, db);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    customers.Add(
                        new Customer(
                            result.GetInt64(0),     // Id
                            result.GetString(1),    // FirstName
                            result.GetString(2),    // LastName
                            result.GetDateTime(3)   // Created
                            )
                        );
                }
            }

            db.Close();

            return customers;
        }

        public static async Task<Customer> GetCustomerByIdAsync(long id)
        {
            Customer customer = new Customer();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Customers WHERE Id = @Id";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@Id", id);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    customer = new Customer(
                        result.GetInt64(0),     // Id
                        result.GetString(1),    // FirstName
                        result.GetString(2),    // LastName
                        result.GetDateTime(3)   // Created
                    );
                }
            }

            db.Close();

            return customer;
        }

        public static async Task<List<Issue>> GetIssuesAsync(int rowCount)
        {
            List<Issue> issues = new List<Issue>();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Issues ORDER BY Created DESC LIMIT @RowCount";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@RowCount", rowCount);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    issues.Add(
                        new Issue(
                            result.GetInt64(0),     // Id
                            result.GetInt64(1),     // CustomerId
                            result.GetString(2),    // Title
                            result.GetString(3),    // Description
                            result.GetString(4),    // Status
                            result.GetDateTime(5)   // Created
                            )
                        );
                }
            }

            db.Close();

            return issues;
        }

        public static async Task<List<Issue>> GetIssuesByStatusAsync(string status, int rowCount)
        {
            List<Issue> issues = new List<Issue>();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Issues WHERE Status = @Status ORDER BY Created DESC LIMIT @RowCount";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@RowCount", rowCount);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    issues.Add(
                        new Issue(
                            result.GetInt64(0),     // Id
                            result.GetInt64(1),     // CustomerId
                            result.GetString(2),    // Title
                            result.GetString(3),    // Description
                            result.GetString(4),    // Status
                            result.GetDateTime(5)   // Created
                            )
                        );
                }
            }

            db.Close();

            return issues;
        }

        public static async Task<Issue> GetIssueByIdAsync(long id)
        {
            Issue issue = new Issue();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Issues WHERE Id = @Id";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@Id", id);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    new Issue(
                        result.GetInt64(0),     // Id
                        result.GetInt64(1),     // CustomerId
                        result.GetString(2),    // Title
                        result.GetString(3),    // Description
                        result.GetString(4),    // Status
                        result.GetDateTime(5)   // Created
                        );
                }
            }

            db.Close();

            return issue;
        }

        public static async Task<List<Comment>> GetCommentsByIssueIdAsync(long issueId, int rowCount)
        {
            List<Comment> comments = new List<Comment>();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Comments WHERE IssueId = @IssueId ORDER BY Created DESC LIMIT @RowCount";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@IssueId", issueId);
            cmd.Parameters.AddWithValue("@RowCount", rowCount);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    comments.Add(
                        new Comment(
                        result.GetInt64(0),     // Id
                        result.GetInt64(1),     // IssueId
                        result.GetString(2),    // Description
                        result.GetDateTime(3)   // Created
                        )
                    );
                }
            }

            db.Close();

            return comments;
        }

        public static async Task<Comment> GetCommentByIdAsync(long id)
        {
            Comment comment = new Comment();

            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "SELECT * FROM Comments WHERE Id = @Id";
            SqliteCommand cmd = new SqliteCommand(query, db);
            cmd.Parameters.AddWithValue("@Id", id);

            var result = await cmd.ExecuteReaderAsync();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    comment = new Comment(
                        result.GetInt64(0),     // Id
                        result.GetInt64(1),     // IssueId
                        result.GetString(2),    // Description
                        result.GetDateTime(3)   // Created
                    );
                }
            }

            db.Close();

            return comment;
        }

        #endregion

        #region Update Methods
        public static async Task<long> UpdateIssueAsync(Issue issue)
        {
            long id = 0;
            SqliteConnection db = new SqliteConnection(_dbPath);

            db.Open();

            string query = "UPDATE Issues SET Status = @Status WHERE Id = @Id";
            SqliteCommand cmd = new SqliteCommand(query, db);

            cmd.Parameters.AddWithValue("@Status", issue.Status);
            cmd.Parameters.AddWithValue("@Id", issue.Id);
            id = (long)await cmd.ExecuteNonQueryAsync();

            db.Close();

            return id;
        }

        #endregion
    }
}
