using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Auth;
using MvcMovie.Data;
using MvcMovie.Data.Security;
using MvcMovie.Models;

namespace MvcMovieRefatorado.Controllers
{
    public class LoginController : Controller
    {
        private readonly MvcMovieContext _context;
        private readonly IAuthService _authService;

        public LoginController(MvcMovieContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: Login
        // public async Task<IActionResult> Index()
        // {
        //       return _context.Login != null ? 
        //                   View(await _context.Login.ToListAsync()) :
        //                   Problem("Entity set 'MvcMovieContext.Login'  is null.");
        // }


        // GET: Login/Create
        public IActionResult Index()
   {
      return View();
   }

   // POST: Login/Authenticate
   // To protect from overposting attacks, enable the specific properties you want to bind to.
   // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
   [HttpPost]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Login([Bind("Email,Password")] Login user)
   {
    Console.WriteLine($"entrei aqui 1");
    
      if (ModelState.IsValid)
      {
        Console.WriteLine($"entrei aqui 2");
         user.Password = Utils.HashPassword(user.Password ?? "");
         var userInDb = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
         //if (userInDb.Email == user.Email)

         var token = _authService.GenerateJwtToken(userInDb.Name, "admin");
         
         // Adicione o token ao contexto da solicitação para que o middleware possa acessá-lo
         HttpContext.Items["PersonalToken"] = token;
         Console.WriteLine($"{token}");

         return RedirectToAction("Index", "Home");
      }

      return View(user);
   }

        private bool LoginExists(int id)
        {
          return (_context.Login?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
