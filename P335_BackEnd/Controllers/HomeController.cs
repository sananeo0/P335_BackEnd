using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P335_BackEnd.Data;
using P335_BackEnd.Entities;
using P335_BackEnd.Models;

namespace P335_BackEnd.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index(int? categoryId)
    {
        var categories = _dbContext.Categories
            .Include(x=>x.CategoryImage)
            .ToList();

        var featured = _dbContext.ProductTypeProducts
                   .Include(x => x.ProductType)
                   .Include(x => x.Product)
                   .Where(x => x.ProductType.Name == "Featured" && (categoryId == null ? true : x.Product.CategoryId == categoryId))
                   .Select(x => x.Product)
                   .ToList();

        var latest = _dbContext.ProductTypeProducts
            .Include(x => x.ProductType)
            .Include(x => x.Product)
            .Where(x => x.ProductType.Name == "Latest")
            .Select(x => x.Product)
            .ToList();

        var topRated = _dbContext.ProductTypeProducts
            .Include(x => x.ProductType)
            .Include(x => x.Product)
            .Where(x => x.ProductType.Name == "Top Rated")
            .Select(x => x.Product)
            .ToList();

        var review = _dbContext.ProductTypeProducts
            .Include(x => x.ProductType)
            .Include(x => x.Product)
            .Where(x => x.ProductType.Name == "Review")
            .Select(x => x.Product)
            .ToList();


        var model = new HomeIndexVM
        {
            Categories = categories,
            FeaturedProducts = featured,
            LatestProducts = latest,
            ReviewProducts = review,
            TopRatedProducts = topRated
        };
        
        return View(model);
    }
}