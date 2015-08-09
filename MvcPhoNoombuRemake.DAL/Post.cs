using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.DAL
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Contenu du post")]
        public string ContentPost { get; set; }

        [Display(Name = "Date de publication")]
        public DateTime DatePublication { get; set; }

        [Display(Name = "Auteur du post")]
        public virtual User Author { get; set; }

        public virtual List<Like> Likes { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
