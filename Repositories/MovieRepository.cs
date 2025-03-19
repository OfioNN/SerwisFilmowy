using SerwisFilmowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.UI.Xaml;
using System.IO;

namespace SerwisFilmowy.Repositories
{
    class MovieRepository : IMovieRepository
        {

        private static string _dbFilePath = Path.Combine(Environment.CurrentDirectory, "dbMovies.db");
        private static string _connectionString = $"Data Source={_dbFilePath}";

        public MovieRepository() {
            if (IsDatabaseExist() == false) {
                SqliteConnection dbConnection = new SqliteConnection(_connectionString);

                dbConnection.Open();

                if (dbConnection.State == System.Data.ConnectionState.Open) { 
                    string dbQuery = "CREATE TABLE Movies (Id INTEGER PRIMARY KEY AUTOINCREMENT, Title TEXT, Genre TEXT, Year INTEGER, Director TEXT, Description TEXT, Image BLOB)";
                    SqliteCommand dbCommand = new SqliteCommand(dbQuery, dbConnection);
                }
            }
        }

        bool IsDatabaseExist() {
            bool isExist = false;
            if (File.Exists(_dbFilePath)) {
                isExist = true;
            }
            return isExist;
        }

        public bool Create(Movies movie) {
            bool isCreated = false;

            SqliteConnection dbConnection = new SqliteConnection(_connectionString);

            dbConnection.Open();

            if(dbConnection.State == System.Data.ConnectionState.Open) {
                SqliteCommand dbCommand = new SqliteCommand();
                dbCommand.Connection = dbConnection;
                string dbQuery = $"INSERT INTO Movies (Title, Genre, Year, Director, Description, Image) VALUES ('{movie.Title}', '{movie.Genre}', @Year, @Director, @Description, @Image)";
                dbCommand.CommandText = dbQuery;
                dbCommand.Parameters.AddWithValue("@Title", movie.Title);
                dbCommand.Parameters.AddWithValue("@Genre", movie.Genre);
                dbCommand.Parameters.AddWithValue("@Year", movie.Year);
                dbCommand.Parameters.AddWithValue("@Director", movie.Director);
                dbCommand.Parameters.AddWithValue("@Description", movie.Description);
                dbCommand.Parameters.Add("@Image", SqliteType.Blob, movie.Image.Length).Value = movie.Image;
                if (dbCommand.ExecuteNonQuery() == 1) {
                    isCreated = true;
                }
            }

            dbConnection.Close();

            return isCreated;
        }
        public Movies Read(int Id) {
            Movies movie = new Movies();

            return movie;
        }
        public List<Movies> ReadAll() {
            List<Movies> movies = new List<Movies>();

            return movies;
        }
        public bool Update(Movies movie) {
            bool isUpdated = false;

            return isUpdated;
        }
        public bool Delete(Movies movie) {
            bool isDeleted = false;

            return isDeleted;
        }
    }
}
