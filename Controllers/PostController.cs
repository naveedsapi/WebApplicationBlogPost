using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationBlogPost.Data;
using WebApplicationBlogPost.Models.ViewModels;

namespace WebApplicationBlogPost.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtension = { ".png", "jpeg", "jpg" };

        public PostController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(int? categoryId)
        {
            //Despling BlogPost
            var postQuery = _context.Posts.Include(p => p.Category).AsQueryable();
            if (categoryId.HasValue)
            {
                var postsQuery = postQuery.Where(p => p.CategoryId == categoryId);
            }
            var posts = postQuery.ToList();
            ViewBag.Categories=_context.Categories.ToList();
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Detial(int id) 
        {
            // This code for To Display Comments Category with BlogPost
            if (id == null)
            {
                return NotFound();
            }
            var post=_context.Posts.Include(p=>p.Category).Include(p=>p.Comments).FirstOrDefault(p=>p.Id==id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //Despling Category Dropdown
            var postViewModel = new PostViewModel();
            postViewModel.Categories = _context.Categories.Select(c =>
            new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(postViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            // Insertion Blog Post 
            if (ModelState.IsValid)
            {

                var inputfileExtension = Path.GetExtension(postViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inputfileExtension);

                if (isAllowed)
                {
                    ModelState.AddModelError("", "Invalid Image format. Allowed format are .jpeg, .png, .jpg");
                    return View(postViewModel);
                }
               postViewModel.Post.FeathureImagePath = await UploadFileToFolder(postViewModel.FeatureImage);
               await _context.Posts.AddAsync(postViewModel.Post);
               await _context.SaveChangesAsync();
             return  RedirectToAction("Index");

            }
            // this Code for when Dropdown Refresh or if there is error in the form but dropdown not refreshing
            postViewModel.Categories = _context.Categories.Select(c =>
            new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }
            ).ToList();

            return View(postViewModel);


           
        }

        private async Task<String> UploadFileToFolder(IFormFile file)
        {
            var inputfileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + inputfileExtension;
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var imagesFolderPath = Path.Combine(wwwRootPath, "images");
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            var filePaht=Path.Combine(imagesFolderPath, fileName);
            try
            {
                await using (var fileStream = new FileStream(filePaht, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {

                return "Error Uploead Images:"+ ex.Message;
            }

            return "/images/"+fileName;
        }


    }
    }
