using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Services;
using MovieShop.Entities;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //controllerbase because we dont need view. If you need view then use Controller
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenres();
            if (genres.Any())
            {
                return Ok(genres);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{id}/movies")]
        public async Task<IActionResult> GetMoviesByGenre(int id)
        {
            var movies = await _genreService.GetMoviesByGenre(id);
            return Ok(movies);
        }
    }
}