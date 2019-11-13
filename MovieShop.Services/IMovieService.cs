using MovieShop.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetHighestGrossingMovies();
    }
}
