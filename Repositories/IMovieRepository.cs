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
        Movies Read(int Id);
        List<Movies> ReadAll();
        bool Update(Movies movie);
        bool Delete(Movies movie);
    }
}
