using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationBlogPost.Data;
using WebApplicationBlogPost.Models;
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            //This code for To Display Data in Edit Form and Display Category Dropdown
            if (id == null)
            {
                return NotFound();
            }

            var PostFromDb = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if(PostFromDb == null)
            {
                return NotFound();
            }

            EditPostViewModel editPostViewModel = new EditPostViewModel
            {
                Post = PostFromDb,
                Categories = _context.Categories.Select(c =>
                new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }
                   ).ToList()
            };
            
            return View(editPostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostViewModel editPostViewModel)
        {
            //this code for Update Blog Post and Update Image in Edit Form
            if (!ModelState.IsValid)
            {
                return View(editPostViewModel);
            }
            var postFromDb = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == editPostViewModel.Post.Id);
            if (postFromDb == null)
            {
                return NotFound();
            }
            if( editPostViewModel.FeatureImage != null)
            {
                var inputfileExtension = Path.GetExtension(editPostViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = _allowedExtension.Contains(inputfileExtension);
                if (isAllowed)
                {
                    ModelState.AddModelError("", "Invalid Image format. Allowed format are .jpeg, .png, .jpg");
                    return View(editPostViewModel);
                }
                var existingFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", Path.GetFileName(postFromDb.FeathureImagePath));
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
                editPostViewModel.Post.FeathureImagePath = await UploadFileToFolder(editPostViewModel.FeatureImage);
            }
            else
            {
                editPostViewModel.Post.FeathureImagePath = postFromDb.FeathureImagePath;
            }
             _context.Posts.Update(editPostViewModel.Post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult AddComment([FromBody]Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Json(new {
                username = comment.UserName,
                commentDate = comment.CommentDate.ToString("MMMM dd, yyyy"),
                content = comment.Content
            });

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
