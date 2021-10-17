using _036_MoviesMvcBilgeAdam.Contexts;
using _036_MoviesMvcBilgeAdam.Entities;
using _036_MoviesMvcBilgeAdam.Models;
using _036_MoviesMvcBilgeAdam.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace _036_MoviesMvcBilgeAdam.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _db;
        private readonly MovieService _movieService;
        private readonly DirectorService _directorService;

        public MoviesController()
        {
            _db = new MoviesContext();
            _movieService = new MovieService(_db);
            _directorService = new DirectorService(_db);
        }

        // GET: Movies
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            List<Movie> movies = _db.Movies.ToList();
            return View(movies);
        }

        // GET: Movies/List
        public ActionResult List()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login", "Account");
                List<MovieModel> model = _movieService.GetQuery().ToList();
                return View("MovieList", model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/ListAfterDelete?result=1
        public ActionResult ListAfterDelete(int? result = null)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (result.HasValue)
                {
                    if (result.Value == 1)
                        TempData["Message"] = "Movie deleted successfully.";
                    else if (result.Value == 0)
                        TempData["Message"] = "Movie could not be deleted because there are relational reviews.";
                    else // -1
                        TempData["Message"] = "An error occured while deleting the movie!";
                }
                List<MovieModel> model = _movieService.GetQuery().ToList();
                return View("MovieList", model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

/*
                                                                                ActionResult
ViewResult (View())  ContentResult (Content()) EmptyResult   FileContentResult (File()) HttpResults JavaScriptResult (JavaScript())  JsonResult (Json())   RedirectResults
*/
        public ContentResult GetHtmlContent()
        {
            //return new ContentResult();
            return Content("<b><i>Content result.</i></b>", "text/html");
        }
        public ActionResult GetMoviesXmlContent() // XML döndürme işlemleri genelde bu şekilde yapılmaz, web servisler üzerinden döndürülür!
        {
            List<MovieModel> movies = _movieService.GetQuery().ToList();
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            xml += "<MovieModels>";
            foreach (MovieModel movie in movies)
            {
                xml += "<MovieModel>";
                xml += "<Id>" + movie.Id + "</Id>";
                xml += "<Name>" + movie.Name + "</Name>";
                xml += "<ProductionYear>" + movie.ProductionYear + "</ProductionYear>";
                xml += "<BoxOfficeReturn>" + movie.BoxOfficeReturn + "</BoxOfficeReturn>";
                xml += "</MovieModel>";
            }
            xml += "</MovieModels>";
            return Content(xml, "application/xml");
        }
        public string GetString()
        {
            return "String.";
        }
        public EmptyResult GetEmpty()
        {
            return new EmptyResult();
        }

        // GET: Movies/Create
        //public ActionResult Create()
        [HttpGet] // bu action method selector yazılmadığında default'u HttpGet'tir
        //public ViewResult Create()
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Login", "Account");
            try
            {
                List<int> years = new List<int>();
                for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
                {
                    years.Add(year);
                }
                ViewBag.Years = years;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                ViewBag.Directors = directors;

                //return new ViewResult();
                return View();
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/CreateSubmit
        //public ActionResult CreateSubmit(string Name, double? BoxOfficeReturn, string ProductionYear)
        //{
        //    return Content("Movie submitted: Name = " + Name + ", BoxOfficeReturn = " + BoxOfficeReturn + ", ProductionYear = " + ProductionYear);
        //}
        // POST: Movies/Create
        [HttpPost]
        public ActionResult Create(string Name, double? BoxOfficeReturn, string ProductionYear, List<int> DirectorIds)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole("Admin"))
                return RedirectToAction("Login", "Account");
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return Content("Name must not be empty.");
                if (Name.Length > 250)
                    return Content("Name must have maximum 250 characters.");
                if (ProductionYear == "")
                    return Content("Production Year must be selected.");
                MovieModel model = new MovieModel()
                {
                    Name = Name,
                    BoxOfficeReturn = BoxOfficeReturn,
                    ProductionYear = ProductionYear,
                    DirectorIds = DirectorIds
                };
                _movieService.Add(model);

                //return RedirectToAction("List", "Movies");
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/Details/7
        public ActionResult Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                //if (id == null)
                if (!id.HasValue)
                {
                    //return View("Exception");
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required!");
                }
                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id.Value);
                if (model == null)
                {
                    //return View("Exception");
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound); // 404
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Movie not found!");
                    return HttpNotFound();
                }
                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/Edit/7
        public ActionResult Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
                if (model == null)
                    return HttpNotFound();
                
                List<int> years = new List<int>();
                for (int year = DateTime.Today.Year + 1; year >= 1930; year--)
                {
                    years.Add(year);
                }
                List<SelectListItem> yearSelectListItems = years.Select(y => new SelectListItem()
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();
                SelectList yearSelectList = new SelectList(yearSelectListItems, "Value", "Text", model.ProductionYear); // DropDownList

                //ViewBag.Years = yearSelectList;
                ViewData["Years"] = yearSelectList;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                MultiSelectList directorMultiSelectList = new MultiSelectList(directors, "Id", "FullName", model.DirectorIds); // ListBox

                ViewData["Directors"] = directorMultiSelectList;

                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // POST: Movies/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(string Name, double? BoxOfficeReturn, string ProductionYear)
        public ActionResult Edit(MovieModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (ModelState.IsValid)
                {
                    _movieService.Update(model);
                    return RedirectToAction("List");
                }

                List<int> years = new List<int>();
                for (int year = DateTime.Now.Date.Year + 1; year >= 1930; year--)
                {
                    years.Add(year);
                }
                List<SelectListItem> yearSelectListItems = years.Select(y => new SelectListItem()
                {
                    Value = y.ToString(),
                    Text = y.ToString()
                }).ToList();
                SelectList yearSelectList = new SelectList(yearSelectListItems, "Value", "Text", model.ProductionYear);
                ViewBag.Years = yearSelectList;

                List<DirectorModel> directors = _directorService.GetQuery().ToList();
                MultiSelectList directorMultiSelectList = new MultiSelectList(directors, "Id", "FullName", model.DirectorIds); // ListBox

                ViewData["Directors"] = directorMultiSelectList;

                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // GET: Movies/DeleteMovie/7
        //public ActionResult DeleteMovie(int? id)
        //{
        //    try
        //    {
        //        if (id == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        bool result = _movieService.Delete(id.Value);
        //        if (result)
        //            TempData["Message"] = "Movie deleted successfully.";
        //        else
        //            TempData["Message"] = "Movie could not be deleted because there are relational reviews.";
        //        return RedirectToAction("List");
        //    }
        //    catch (Exception exc)
        //    {
        //        TempData["Message"] = "An error occured while deleting the movie!";
        //        return RedirectToAction("List");
        //    }
        //}

        // GET: Movies/Delete/7
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                MovieModel model = _movieService.GetQuery().SingleOrDefault(m => m.Id == id);
                if (model == null)
                    return HttpNotFound();

                // Bu tip model özelleştirme işlemleri ya servislerde ya da modellerde yapılmalıdır!
                //if (model.Directors != null && model.Directors.Count > 0)
                //{
                //    model.DirectorNamesHtml = "";
                //    foreach (DirectorModel directorModel in model.Directors)
                //    {
                //        model.DirectorNamesHtml += directorModel.Name + " " + directorModel.Surname + "<br />";
                //    }
                //}

                return View(model);
            }
            catch (Exception exc)
            {
                return View("Exception");
            }
        }

        // POST: Movies/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                bool result = _movieService.Delete(id.Value);
                if (result)
                    return RedirectToAction("ListAfterDelete", new { result = 1 }); // ~/Movies/ListAfterDelete?result=1 : query string
                return RedirectToAction("ListAfterDelete", new { result = 0 }); // ~/Movies/ListAfterDelete?result=0
            }
            catch (Exception exc)
            {
                return RedirectToAction("ListAfterDelete", new { result = -1 }); // ~/Movies/ListAfterDelete?result=-1
            }
        }
    }
}