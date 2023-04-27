using Microsoft.Extensions.Options;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChatAppAPI.Helpers
{
    public class DataContext
    {
        private DbSettings _dbSettings;

        public DataContext(IOptions<DbSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database}; TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true";
            return new SqlConnection(connectionString);
        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            // create database if it doesn't exist
            var connectionString = $"Server={_dbSettings.Server}; Database=master; TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true";
            using var connection = new SqlConnection(connectionString);
            var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{_dbSettings.Database}') CREATE DATABASE [{_dbSettings.Database}];";
            await connection.ExecuteAsync(sql);
        }

        private async Task _initTables()
        {
            // create tables if they don't exist
            using var connection = CreateConnection();
            await _initRoles();
            await _initUsers();
            await _initUserContacts();
            await _initConversations();
            await _initParticipants();

            async Task _initUsers()
            {
                var sql = """
                IF OBJECT_ID('Users', 'U') IS NULL
                CREATE TABLE Users (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    FirstName NVARCHAR(MAX),
                    LastName NVARCHAR(MAX),
                    Email NVARCHAR(MAX),
                    PasswordHash NVARCHAR(MAX),
                    Avatar NVARCHAR(MAX),
                    Bio NVARCHAR(MAX),
                    RoleId INT NOT NULL foreign key REFERENCES  Roles(Id), 
                );
            """;
                await connection.ExecuteAsync(sql);
            }
            async Task _initRoles()
            {
                var sql = """
                IF OBJECT_ID('Roles', 'U') IS NULL
                CREATE TABLE Roles (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
                    Name NVARCHAR(MAX)
                );
            """;
                await connection.ExecuteAsync(sql);
            }
            async Task _initUserContacts()
            {
                var sql = """
                IF OBJECT_ID('UserContacts', 'U') IS NULL
                CREATE TABLE UserContacts (
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                UserId INT NOT NULL foreign key REFERENCES  Users(Id),
                ContactId INT NOT NULL foreign key REFERENCES  Users(Id),       
                );
            """;
                await connection.ExecuteAsync(sql);
            }
            async Task _initConversations()
            {
                var sql = """
                IF OBJECT_ID('Conversations', 'U') IS NULL
                CREATE TABLE Conversations (
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                
                Name NVARCHAR(MAX),
                Avatar NVARCHAR(MAX)
                );
            """;
                await connection.ExecuteAsync(sql);
            }
            async Task _initParticipants()
            {
                var sql = """
                IF OBJECT_ID('Participants', 'U') IS NULL
                CREATE TABLE Participants (
                Id INT NOT NULL PRIMARY KEY IDENTITY,
                ConversationId INT NOT NULL foreign key REFERENCES  Conversations(Id),
                UserId INT NOT NULL foreign key REFERENCES  Users(Id),
                Nickname NVARCHAR(MAX)
                );
            """;
                await connection.ExecuteAsync(sql);
            }
        }
    }
}
