using System.ComponentModel.DataAnnotations;

namespace WebApplicationBlogPost.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Category Name is Requrid!")]
        [MaxLength(100, ErrorMessage = "The Category Name Cannot Exceed 100 Charactres!")]
        public string Name { get; set; }
    
        public string? Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
