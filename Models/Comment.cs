using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationBlogPost.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The UserName is Requird!")]
        [MaxLength(100, ErrorMessage = "The Username Cannot Exceed 100 Characters")]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentDate { get; set; }

        [Required]
        public string Content { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; }

    }
}
