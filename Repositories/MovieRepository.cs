using SerwisFilmowy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerwisFilmowy.Repositories
{
    class MovieRepository : IMovieRepository
        {
        public bool Create(Movies movie) {
            bool isCreated = false;

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
