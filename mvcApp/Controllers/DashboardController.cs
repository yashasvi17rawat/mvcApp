using Microsoft.AspNetCore.Mvc;
using mvcApp.Data;
using mvcApp.Dto;
using mvcApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace mvcApp.Controllers
{
    [Authorize]
    public class DashboardController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SuccessMessages=TempData["SuccessMessages"];
            ViewBag.ErrorMessage=TempData["ErrorMessage"];
            var list = _context.Products.Select(x => new ProductDto {
                ID = x.ID,
                Color = x.Color,
                Description = x.Description,
                Price = x.Price,
                ProductName = x.ProductName
            }).ToList();
            return View(list);
        }
        public IActionResult ProductForm()
        {
            return View();
        }

        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            Console.WriteLine("in create product controller action");
            if(dto == null)
            {
                ViewBag.ErrorMessage = "Please Fill in the details.";
                return View("ProductForm");
            }
            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Color = dto.Color
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessages"]="Product added to the queue.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == productId);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["ErrorMessage"]="Product deleted Successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateProductForm (int productId)
        {
            var product = await _context.Products.Select(
                x => new ProductDto{
                    ID = x.ID,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    Color = x.Color,
                    Price = x.Price
                    }
            ).FirstOrDefaultAsync(p => p.ID == productId);
            
            return View(product);
        }
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            var product=await _context.Products.FirstOrDefaultAsync(p => p.ID == dto.ID);
            
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.Color = dto.Color;
            product.ProductName = dto.ProductName;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessages"] = "Product updated successfully.";
            return RedirectToAction("Index");
        }
    
    }
}