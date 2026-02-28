using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationBlogPost.Data;
using WebApplicationBlogPost.Models;
using WebApplicationBlogPost.Models.ViewModels;

namespace WebApplicationBlogPost.Controllers
{

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
             ViewBag.CategoryList = _context.Categories.OrderBy(c => c.Name).ToList();
            if(ViewBag.CategoryList == null)
            {
                return NotFound();
            }
            return View(ViewBag.CategoryList);
        }



        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        { // This Code for Adding New Category To Db
            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(categoryViewModel.Category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int CategoryId)
        {
            //this Code for to Ready form for Updating Category
            if (CategoryId == null)
            {
                return NotFound();
            }
            var editCategoryViewModel = new EditCategoryViewModel();
            editCategoryViewModel.Category =await _context.Categories.FirstOrDefaultAsync(c => c.Id == CategoryId);
            if (editCategoryViewModel ==null)
            {
                return NotFound();
            }
            
            return View(editCategoryViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryViewModel  editCategoryViewModel)
        {
            //this Code for Updating Category
            if(!ModelState.IsValid)
            {
                return View(editCategoryViewModel);
            }
             
            await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id ==editCategoryViewModel.Category.Id);
            if (editCategoryViewModel != null)
            {
                _context.Categories.Update(editCategoryViewModel.Category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");  
            }
            return View(editCategoryViewModel);
           
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // This Code to Ready Category For Deletion
            if (id==null)
            {
                return NotFound();   
            }
            var categoryFD =  await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (categoryFD == null)
            {
                return NotFound();
            }
            return View(categoryFD);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            //This Code To Delet Category Completly
            if (id == null)
            {
                return NotFound();
            }
            var categoryFD = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (categoryFD == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(categoryFD);
              await _context.SaveChangesAsync();
             return   RedirectToAction("Index");
        }
    }
}
