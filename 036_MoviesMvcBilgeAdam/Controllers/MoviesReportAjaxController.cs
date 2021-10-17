using _036_MoviesMvcBilgeAdam.Contexts;
using _036_MoviesMvcBilgeAdam.Models;
using _036_MoviesMvcBilgeAdam.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace _036_MoviesMvcBilgeAdam.Controllers
{
    // AJAX: Asynchronous Javascript and XML

    public class MoviesReportAjaxController : Controller
    {
        private MoviesContext db;
        private MovieReportService movieReportService;

        public MoviesReportAjaxController()
        {
            db = new MoviesContext();
            movieReportService = new MovieReportService(db);
        }

        // GET: MoviesReportAjax
        public ActionResult Index()
        {
            IQueryable<MovieReportLeftOuterJoinModel> leftOuterJoinQuery;
            List<MovieReportLeftOuterJoinModel> leftOuterJoinList = null;

            leftOuterJoinQuery = movieReportService.GetLeftOuterJoinQuery();

            leftOuterJoinList = leftOuterJoinQuery.ToList();

            List<SelectListItem> productionYearSelectListItems = new List<SelectListItem>();
            for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
            {
                productionYearSelectListItems.Add(new SelectListItem()
                {
                    Value = year.ToString(),
                    Text = year.ToString()
                });
            }

            MoviesReportAjaxIndexViewModel viewModel = new MoviesReportAjaxIndexViewModel()
            {
                LeftOuterJoinList = leftOuterJoinList,
                ProductionYearMultiSelectList = new MultiSelectList(productionYearSelectListItems, "Value", "Text")
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(MoviesReportAjaxIndexViewModel moviesReport)
        {
            IQueryable<MovieReportLeftOuterJoinModel> leftOuterJoinQuery;
            List<MovieReportLeftOuterJoinModel> leftOuterJoinList = null;

            leftOuterJoinQuery = movieReportService.GetLeftOuterJoinQuery();

            if (!string.IsNullOrWhiteSpace(moviesReport.MovieName))
                leftOuterJoinQuery = leftOuterJoinQuery.Where(model => model.MovieName.ToUpper().Contains(moviesReport.MovieName.ToUpper().Trim()));
            if (moviesReport.ProductionYears != null && moviesReport.ProductionYears.Count > 0)
                leftOuterJoinQuery = leftOuterJoinQuery.Where(model => moviesReport.ProductionYears.Contains(model.MovieProductionYear));
            double boxOfficeReturn1;
            double boxOfficeReturn2;
            if (!string.IsNullOrWhiteSpace(moviesReport.BoxOfficeReturn1))
            {
                if (double.TryParse(moviesReport.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                    leftOuterJoinQuery = leftOuterJoinQuery.Where(model => model.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
            }
            if (!string.IsNullOrWhiteSpace(moviesReport.BoxOfficeReturn2))
            {
                if (double.TryParse(moviesReport.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                    leftOuterJoinQuery = leftOuterJoinQuery.Where(model => model.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
            }
            DateTime reviewDate1;
            DateTime reviewDate2;
            if (!string.IsNullOrWhiteSpace(moviesReport.ReviewDate1))
            {
                reviewDate1 = DateTime.Parse(moviesReport.ReviewDate1, new CultureInfo("en"));
                leftOuterJoinQuery = leftOuterJoinQuery.Where(model => model.ReviewDateValue >= reviewDate1);
            }
            if (!string.IsNullOrWhiteSpace(moviesReport.ReviewDate2))
            {
                reviewDate2 = DateTime.Parse(moviesReport.ReviewDate2, new CultureInfo("en"));
                leftOuterJoinQuery = leftOuterJoinQuery.Where(model => model.ReviewDateValue <= reviewDate2);
            }

            leftOuterJoinList = leftOuterJoinQuery.ToList();

            moviesReport.LeftOuterJoinList = leftOuterJoinList;

            return PartialView("_MoviesReportAjax", moviesReport);
        }

        public ActionResult Json()
        {
            var model = movieReportService.GetLeftOuterJoinQuery().ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}