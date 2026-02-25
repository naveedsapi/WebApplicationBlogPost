using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplicationBlogPost.Models.ViewModels
{
    public class EditPostViewModel
    {
        /// <summary>
        /// this property is used to display the data in the edit form and also to update the data in the database
        /// </summary>
        public Post Post { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IFormFile FeatureImage { get; set; }
    }
}
