using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieList.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieList.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext context { get; set; }
        public MovieController(MovieContext ctx)
        {
            context = ctx;
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            ViewBag.Genres = context.Genres.OrderBy(g => g.Name).ToList();              //get genre data
            return View("Edit", new Movie());   //send empty object
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Genres = context.Genres.OrderBy(g => g.Name).ToList();              //get genre data
            var movie = context.Movies.Find(id);  //read data for the id
            return View("Edit", movie);           //send movie object to view
        }
        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.MovieId == 0)         // add the movie
                {
                    context.Movies.Add(movie);
                }
                else                           // update the movie
                {
                    context.Movies.Update(movie);
                }
                context.SaveChanges();         // saves to db
                return RedirectToAction("Index", "Home");   // reloads data
            }
            else
            {
                ViewBag.Action = (movie.MovieId == 0) ? "Add" : "Edit";
                ViewBag.Genres = context.Genres.OrderBy(g => g.Name).ToList();              //get genre data
                return View(movie);                                                         // returns movie object to view
            }

            //Need delete methods
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = context.Movies.Find(id);
            return View(movie);
        }
        [HttpPost]
        public IActionResult Delete(Movie movie)
        {
            context.Movies.Remove(movie);
            context.SaveChanges();         // saves to db
            return RedirectToAction("Index", "Home");   // reloads data
        }
    }
}
