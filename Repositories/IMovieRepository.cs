using System;
using System.Collections.Generic;
using SerwisFilmowy.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerwisFilmowy.Repositories
{
    interface IMovieRepository
    {
        bool Create(Movies movie);
        Movies Read(string Title);
        List<Movies> ReadAll();
        bool Update(Movies movie, string selectedTitle);
        bool Delete(Movies movie);
    }
}
