using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationBlogPost.Models
{
    public class Post
    {
        [Key]
        public   int   Id { get; set; }

        [Required(ErrorMessage ="The Titel is Requird!")]
        [MaxLength(200, ErrorMessage ="The Title Cannot Exceed 200 Characters")]
        public   string Title { get; set; }

        [Required(ErrorMessage = "The Content is Requird!")]
        public   string Content { get; set; }


        [Required(ErrorMessage = "The Author is Requird!")]
        [MaxLength(100, ErrorMessage = "The Author Cannot Exceed 100 Characters")]
        public  string Author { get; set; }

        [ValidateNever]
        public  string FeathureImagePath { get; set; }


        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        [ForeignKey("Category")]

        //this code only for display of category name in the post index page and create page
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; }
    }
}
