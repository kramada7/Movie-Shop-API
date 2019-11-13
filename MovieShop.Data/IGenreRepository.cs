using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MovieShop.Entities;

namespace MovieShop.Data
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<IEnumerable<Movie>> GetMoviesByGenre(int genreId);
    }
}
