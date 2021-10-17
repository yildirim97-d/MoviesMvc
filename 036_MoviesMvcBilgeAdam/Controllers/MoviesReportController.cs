using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using _036_MoviesMvcBilgeAdam.Contexts;
using _036_MoviesMvcBilgeAdam.Models;
using _036_MoviesMvcBilgeAdam.Services;
using OfficeOpenXml;

namespace _036_MoviesMvcBilgeAdam.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesReportController : Controller
    {
        private MoviesContext db;
        private MovieReportService movieReportService;

        public MoviesReportController()
        {
            db = new MoviesContext();
            movieReportService = new MovieReportService(db);
        }

        // GET: MoviesReport
        //public ActionResult Index(int? OnlyMatchedValue = null)
        public ActionResult Index(MoviesReportIndexViewModel moviesReport)
        {
            IQueryable<MovieReportInnerJoinModel> innerJoinQuery;
            IQueryable<MovieReportLeftOuterJoinModel> leftOuterJoinQuery;

            List<MovieReportInnerJoinModel> innerJoinList = null;
            List<MovieReportLeftOuterJoinModel> leftOuterJoinList = null;

            //if ((OnlyMatchedValue ?? 0) == 1) // only matched, inner join
            if (moviesReport.OnlyMatchedValue == 1)
            {
                innerJoinQuery = movieReportService.GetInnerJoinQuery();
                if (!string.IsNullOrWhiteSpace(moviesReport.MovieName))
                    innerJoinQuery = innerJoinQuery.Where(model => model.MovieName.ToUpper().Contains(moviesReport.MovieName.ToUpper().Trim()));
                if (moviesReport.ProductionYears != null && moviesReport.ProductionYears.Count > 0)
                    innerJoinQuery = innerJoinQuery.Where(model => moviesReport.ProductionYears.Contains(model.MovieProductionYear));
                double boxOfficeReturn1;
                double boxOfficeReturn2;
                if (!string.IsNullOrWhiteSpace(moviesReport.BoxOfficeReturn1))
                {
                    if (double.TryParse(moviesReport.BoxOfficeReturn1.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn1))
                        innerJoinQuery = innerJoinQuery.Where(model => model.MovieBoxOfficeReturnValue >= boxOfficeReturn1);
                }
                if (!string.IsNullOrWhiteSpace(moviesReport.BoxOfficeReturn2))
                {
                    if (double.TryParse(moviesReport.BoxOfficeReturn2.Trim().Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out boxOfficeReturn2))
                        innerJoinQuery = innerJoinQuery.Where(model => model.MovieBoxOfficeReturnValue <= boxOfficeReturn2);
                }
                DateTime reviewDate1;
                DateTime reviewDate2;
                if (!string.IsNullOrWhiteSpace(moviesReport.ReviewDate1))
                {
                    reviewDate1 = DateTime.Parse(moviesReport.ReviewDate1, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(model => model.ReviewDateValue >= reviewDate1);
                }
                if (!string.IsNullOrWhiteSpace(moviesReport.ReviewDate2))
                {
                    reviewDate2 = DateTime.Parse(moviesReport.ReviewDate2, new CultureInfo("en"));
                    innerJoinQuery = innerJoinQuery.Where(model => model.ReviewDateValue <= reviewDate2);
                }

                innerJoinList = innerJoinQuery.ToList();
            }
            else // not only matched, left outer join
            {
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
            }

            List<SelectListItem> onlyMatchedSelectListItems = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "1",
                    Text = "Yes"

                    //, Selected = true
                },
                new SelectListItem()
                {
                    Value = "0",
                    Text = "No"
                }
            };

            List<SelectListItem> productionYearSelectListItems = new List<SelectListItem>();
            for (int year = DateTime.Now.Year + 1; year >= 1930; year--)
            {
                productionYearSelectListItems.Add(new SelectListItem()
                {
                    Value = year.ToString(),
                    Text = year.ToString()
                });
            }

            moviesReport.InnerJoinList = innerJoinList;
            moviesReport.LeftOuterJoinList = leftOuterJoinList;
            moviesReport.OnlyMatchedSelectList = new SelectList(onlyMatchedSelectListItems, "Value", "Text");
            moviesReport.ProductionYearMultiSelectList = new MultiSelectList(productionYearSelectListItems, "Value", "Text");

            if (Session["MoviesReport"] != null)
                Session.Remove("MoviesReport");
            Session["MoviesReport"] = moviesReport;

            //return View(innerJoinList);
            return View(moviesReport);
        }

        public ActionResult Export()
        {
            MoviesReportIndexViewModel viewModel = new MoviesReportIndexViewModel();
            MoviesReportIndexViewModel sessionModel;
            if (Session["MoviesReport"] != null)
            {
                sessionModel = Session["MoviesReport"] as MoviesReportIndexViewModel;
                viewModel.InnerJoinList = sessionModel.InnerJoinList;
                viewModel.LeftOuterJoinList = sessionModel.LeftOuterJoinList;
            }
            return View(viewModel);
        }

        // Excel işlemleri için:
        // https://spreadsheetlight.com/
        // https://www.epplussoftware.com/
        // https://www.c-sharpcorner.com/article/export-data-excel-in-asp-net-mvc/
        public void MoviesReportExcel()
        {
            MoviesReportIndexViewModel viewModel = new MoviesReportIndexViewModel();
            MoviesReportIndexViewModel sessionModel;
            if (Session["MoviesReport"] != null)
            {
                sessionModel = Session["MoviesReport"] as MoviesReportIndexViewModel;
                viewModel.InnerJoinList = sessionModel.InnerJoinList;
                viewModel.LeftOuterJoinList = sessionModel.LeftOuterJoinList;
                if (viewModel.InnerJoinList != null && viewModel.InnerJoinList.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Movies Report");
                    excelWorksheet.Cells["A1"].Value = "Movie Name";
                    excelWorksheet.Cells["B1"].Value = "Movie Production Year";
                    excelWorksheet.Cells["C1"].Value = "Movie Box Office Return";
                    excelWorksheet.Cells["D1"].Value = "Director Name";
                    excelWorksheet.Cells["E1"].Value = "Is Director Retired?";
                    excelWorksheet.Cells["F1"].Value = "Review Content";
                    excelWorksheet.Cells["G1"].Value = "Review Rating";
                    excelWorksheet.Cells["H1"].Value = "Reviewer";
                    excelWorksheet.Cells["I1"].Value = "Review Date";
                    int row = 2;
                    foreach (var item in viewModel.InnerJoinList)
                    {
                        excelWorksheet.Cells[string.Format("A{0}", row)].Value = item.MovieName;
                        excelWorksheet.Cells[string.Format("B{0}", row)].Value = item.MovieProductionYear;
                        excelWorksheet.Cells[string.Format("C{0}", row)].Value = item.MovieBoxOfficeReturn;
                        excelWorksheet.Cells[string.Format("D{0}", row)].Value = item.DirectorFullName;
                        excelWorksheet.Cells[string.Format("E{0}", row)].Value = item.DirectorRetired;
                        excelWorksheet.Cells[string.Format("F{0}", row)].Value = item.ReviewContent;
                        excelWorksheet.Cells[string.Format("G{0}", row)].Value = item.ReviewRating;
                        excelWorksheet.Cells[string.Format("H{0}", row)].Value = item.ReviewReviewer;
                        excelWorksheet.Cells[string.Format("I{0}", row)].Value = item.ReviewDate;
                        row++;
                    }
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=MoviesReport.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
                else if (viewModel.LeftOuterJoinList != null && viewModel.LeftOuterJoinList.Count > 0)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Movies Report");
                    excelWorksheet.Cells["A1"].Value = "Movie Name";
                    excelWorksheet.Cells["B1"].Value = "Movie Production Year";
                    excelWorksheet.Cells["C1"].Value = "Movie Box Office Return";
                    excelWorksheet.Cells["D1"].Value = "Director Name";
                    excelWorksheet.Cells["E1"].Value = "Is Director Retired?";
                    excelWorksheet.Cells["F1"].Value = "Review Content";
                    excelWorksheet.Cells["G1"].Value = "Review Rating";
                    excelWorksheet.Cells["H1"].Value = "Reviewer";
                    excelWorksheet.Cells["I1"].Value = "Review Date";
                    int row = 2;
                    foreach (var item in viewModel.LeftOuterJoinList)
                    {
                        excelWorksheet.Cells[string.Format("A{0}", row)].Value = item.MovieName;
                        excelWorksheet.Cells[string.Format("B{0}", row)].Value = item.MovieProductionYear;
                        excelWorksheet.Cells[string.Format("C{0}", row)].Value = item.MovieBoxOfficeReturn;
                        excelWorksheet.Cells[string.Format("D{0}", row)].Value = item.DirectorFullName;
                        excelWorksheet.Cells[string.Format("E{0}", row)].Value = item.DirectorRetired;
                        excelWorksheet.Cells[string.Format("F{0}", row)].Value = item.ReviewContent;
                        excelWorksheet.Cells[string.Format("G{0}", row)].Value = item.ReviewRating;
                        excelWorksheet.Cells[string.Format("H{0}", row)].Value = item.ReviewReviewer;
                        excelWorksheet.Cells[string.Format("I{0}", row)].Value = item.ReviewDate;
                        row++;
                    }
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=MoviesReport.xlsx");
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.End();
                }
            }
        }
    }
}