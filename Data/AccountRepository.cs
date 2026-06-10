using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BankApp.Models;
using Microsoft.Extensions.Configuration;

namespace BankApp.Data
{
    public class AccountRepository
    {
        private readonly string _connectionString;

        // Inject IConfiguration to read the connection string from appsettings.json
        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public List<Account> GetAllAccounts()
        {
            var accounts = new List<Account>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID, AccountHolder, Balance FROM Accounts";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var account = new Account()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                AccountHolder = reader["AccountHolder"]?.ToString() ?? string.Empty,
                                Balance = Convert.ToDecimal(reader["Balance"])
                            };
                            accounts.Add(account);
                        }
                    }
                }
            }
            return accounts;
        }
    }
}