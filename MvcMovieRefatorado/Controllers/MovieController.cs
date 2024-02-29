using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MovieController : Controller
    {
        private readonly MvcMovieContext _context;

        public MovieController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return _context.Movie != null ?
                        View(await _context.Movie.ToListAsync()) :
                        Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.Studios = new SelectList(_context.Studio, "Id", "Name");
            ViewBag.Artists = new MultiSelectList(_context.Artist, "Id", "Name");
            
            return View();
        }
        
        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,StudioId,Studio")] Movie movie, string[] Artists)
        {
            Console.WriteLine($"Create method called. Movie: {movie.Title}, StudioId: {movie.StudioId}");

            // foreach (var modelState in ModelState.Values)
            // {
            //     foreach (var error in modelState.Errors)
            //     {
            //         Console.WriteLine($"Error: {error.ErrorMessage}");
            //     }
            // }
            List<Artist> listArtist = new();
            foreach (var id in Artists)
            {
                var artist = _context.Artist.FirstOrDefault(m => m.Id == Int32.Parse(id));
                listArtist.Add(artist);
            }

            movie.Artists = listArtist;

            if (ModelState.IsValid)
            {
                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();
                Console.WriteLine("Movie saved to the database.");
                return RedirectToAction(nameof(Index));
            }
            
            // Se ocorrer um erro de validação, recarregue a lista de estúdios e artistas no ViewBag
            ViewBag.Studios = new SelectList(_context.Studio, "Id", "Name");
            ViewBag.Artists = new SelectList(_context.Artist, "Id", "Name", movie.Artists?.Select(a => a.Id).ToArray());
            Console.WriteLine("Validation error. Movie not saved.");
            return View(movie);
        }
    /*
        //adiciona um Movie
        public IActionResult AddArtist(int? id,Artist artist)
        {
            //seleciona o movie
            var movie = _context.Movie.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            // Adicionar o novo item à lista
            movie.Artists.Add(artist);
            // Redirecionar de volta à página
            return View(movie);
        }
        */
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
