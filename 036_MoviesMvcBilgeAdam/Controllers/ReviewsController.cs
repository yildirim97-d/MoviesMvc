using System;
using _036_MoviesMvcBilgeAdam.Contexts;
using _036_MoviesMvcBilgeAdam.Entities;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using _036_MoviesMvcBilgeAdam.Models;
using _036_MoviesMvcBilgeAdam.Services;

namespace _036_MoviesMvcBilgeAdam.Controllers
{
    [HandleError]
    public class ReviewsController : Controller
    {
        private MoviesContext db = new MoviesContext();

        private ReviewService reviewService;
        private MovieService movieService;

        public ReviewsController()
        {
            reviewService = new ReviewService(db);
            movieService = new MovieService(db);
        }

        // GET: Reviews
        public ActionResult Index()
        {
            //var reviews = db.Reviews.Include(r => r.Movie);
            //return View(reviews.ToList());
            return View(reviewService.GetQuery().ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }

            //Review review = db.Reviews.Find(id);
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);

            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public ActionResult Create()
        {
            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name");
            //return View();

            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name");

            ReviewModel model = new ReviewModel();
            reviewService.FillAllRatings(model);

            return View(model);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        //public ActionResult Create([Bind(Include = "Id,Content,Rating,Reviewer,Date,MovieId")] Review review)
        public ActionResult Create(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                //db.Reviews.Add(review);
                //db.SaveChanges();
                reviewService.Add(review);

                return RedirectToAction("Index");
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // GET: Reviews/Edit/5
        //[Authorize(Users = "leo@alsac.com")]
        [Authorize(Users = "leo@alsac.com,angel@alsac.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Review review = db.Reviews.Find(id);
            ReviewModel review = reviewService.GetQuery().SingleOrDefault(r => r.Id == id);

            if (review == null)
            {
                return HttpNotFound();
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "leo@alsac.com,angel@alsac.com")]
        //public ActionResult Edit([Bind(Include = "Id,Content,Rating,Reviewer,Date,MovieId")] Review review)
        public ActionResult Edit(ReviewModel review)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(review).State = EntityState.Modified;
                //db.SaveChanges();
                reviewService.Update(review);

                return RedirectToAction("Index");
            }

            //ViewBag.MovieId = new SelectList(db.Movies, "Id", "Name", review.MovieId);
            ViewBag.Movies = new SelectList(movieService.GetQuery().ToList(), "Id", "Name", review.MovieId);
            reviewService.FillAllRatings(review);

            return View(review);
        }

        // AlertifyJS kullanıldığı için delete view'ı dönülmemektedir.
        // GET: Reviews/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Review review = db.Reviews.Find(id);
        //    if (review == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(review);
        //}

        // POST: Reviews/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Review review = db.Reviews.Find(id);
        //    db.Reviews.Remove(review);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin,User")]
        //[Authorize]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            reviewService.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult TestException()
        {
            throw new Exception("Reviews Test Exception!");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
