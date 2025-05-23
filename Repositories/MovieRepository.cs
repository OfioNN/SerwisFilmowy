﻿using SerwisFilmowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using System.IO;
using System.Data;
using FirebirdSql.Data.FirebirdClient;


namespace SerwisFilmowy.Repositories {

    class MovieRepository : IMovieRepository {
        
        private static string folderPath = Path.Combine(Environment.CurrentDirectory, @"Databases");
        private static string dbPath = Path.Combine(folderPath, "MoviesDB.fdb");
        private static string connectionString = $@"Database={dbPath}; User=SYSDBA; Password=masterkey; ServerType=1;";

        public MovieRepository() {

            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            if (!File.Exists(dbPath)) {
                try {

                    FbConnection.CreateDatabase(connectionString);

                    using (FbConnection connection = new FbConnection(connectionString)) {
                        try {
                            connection.Open();

                            if (connection.State == ConnectionState.Open) {

                                string dbQuery = @"CREATE TABLE Movies (
                                                ID INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY, 
                                                TITLE CHARACTER VARYING(100),
                                                GENRE CHARACTER VARYING(50), 
                                                RELASE INTEGER, 
                                                DIRECTOR CHARACTER VARYING(50),   
                                                STAFF CHARACTER VARYING(200),   
                                                DESCRIPTION CHARACTER VARYING(2000), 
                                                IMAGE BLOB
                                                );";

                                using (FbCommand command = new FbCommand(dbQuery, connection))
                                    command.ExecuteNonQuery();
                            }

                            connection.Close();

                        }
                        catch (Exception ex) {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public bool Create(Movies movie) {
            bool isCreated = false;

            using (FbConnection connection = new FbConnection(connectionString)) {
                try {
                    connection.Open();

                    if (connection.State == ConnectionState.Open) {

                        string dbQuery = @$"INSERT INTO Movies 
                                        (TITLE, GENRE, RELASE, DIRECTOR, STAFF, DESCRIPTION, IMAGE) 
                                        VALUES 
                                        ('{movie.Title}', '{movie.Genre}', '{movie.Year}', '{movie.Director}', '{movie.Staff}', '{movie.Description}', @Image);";

                        using (FbCommand command = new FbCommand(dbQuery, connection)) {

                            command.Parameters.Add("@Image", FbDbType.Binary).Value = movie.Image;

                            if (command.ExecuteNonQuery() == 1)
                                isCreated = true;
                        }


                    }

                    connection.Close();

                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return isCreated;
        }

        public Movies Read(string Title) {
            Movies movie = new Movies();

            using (FbConnection connection = new FbConnection(connectionString)) {
                try {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {

                        string dbQuery = @"SELECT 
                                        ID, TITLE, GENRE, RELASE, DIRECTOR, STAFF, DESCRIPTION, IMAGE
                                        FROM Movies WHERE 
                                        TITLE = @Title";

                        using (FbCommand command = new FbCommand(dbQuery, connection)) {
                            command.Parameters.Add("@Title", FbDbType.VarChar).Value = Title;

                            using (FbDataReader reader = command.ExecuteReader()) {
                                if (reader.Read()) {

                                    movie.Id = reader.GetInt32(0);
                                    movie.Title = reader.GetString(1);
                                    movie.Genre = reader.GetString(2);
                                    movie.Year = reader.GetInt32(3);
                                    movie.Director = reader.GetString(4);
                                    movie.Staff = reader.GetString(5);
                                    movie.Description = reader.GetString(6);
                                    movie.Image = (byte[])reader.GetValue(7);

                                }
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return movie;
        }

        public List<Movies> ReadAll() {
            List<Movies> movies = new List<Movies>();

            using (FbConnection connection = new FbConnection(connectionString)) {
                try {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {

                        string dbQuery = @"SELECT 
                                        ID, TITLE, GENRE, RELASE, DIRECTOR, STAFF, DESCRIPTION, IMAGE
                                        FROM Movies";

                        using (FbCommand command = new FbCommand(dbQuery, connection)) {
                            using (FbDataReader reader = command.ExecuteReader()) {
                                while (reader.Read()) {
                                    Movies movie = new Movies {
                                        Id = reader.GetInt32(0),
                                        Title = reader.GetString(1),
                                        Genre = reader.GetString(2),
                                        Year = reader.GetInt32(3),
                                        Director = reader.GetString(4),
                                        Staff = reader.GetString(5),
                                        Description = reader.GetString(6),
                                        Image = (byte[])reader.GetValue(7)
                                    };
                                    movies.Add(movie);
                                }
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return movies;
        }

        public bool Update(Movies movie, string selectedTitle) {
            bool isUpdated = false;

            using (FbConnection connection = new FbConnection(connectionString)) {
                try {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {
                        
                        string dbQuery = @"UPDATE Movies SET 
                                        TITLE = @Title, GENRE = @Genre, RELASE = @Year, DIRECTOR = @Director, STAFF = @Staff, DESCRIPTION = @Description, IMAGE = @Image 
                                        WHERE 
                                        TITLE = @SelectedTitle";
                        
                        using (FbCommand command = new FbCommand(dbQuery, connection)) {
                            command.Parameters.AddWithValue("@Title", movie.Title);
                            command.Parameters.AddWithValue("@Genre", movie.Genre);
                            command.Parameters.AddWithValue("@Year", movie.Year);
                            command.Parameters.AddWithValue("@Director", movie.Director);
                            command.Parameters.AddWithValue("@Staff", movie.Staff);
                            command.Parameters.AddWithValue("@Description", movie.Description);
                            command.Parameters.AddWithValue("@Image", movie.Image);
                            command.Parameters.AddWithValue("@SelectedTitle", selectedTitle);

                            if (command.ExecuteNonQuery() == 1)
                                isUpdated = true;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return isUpdated;
        }

        public bool Delete(Movies movie) {
            bool isDeleted = false;

            using (FbConnection connection = new FbConnection(connectionString)) {
                try {
                    connection.Open();
                    if (connection.State == ConnectionState.Open) {

                        string dbQuery = @"DELETE FROM Movies 
                                        WHERE 
                                        TITLE = @Title";

                        using (FbCommand command = new FbCommand(dbQuery, connection)) {
                            command.Parameters.AddWithValue("@Title", movie.Title);

                            if (command.ExecuteNonQuery() == 1)
                                isDeleted = true;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return isDeleted;
        }
    }
}
