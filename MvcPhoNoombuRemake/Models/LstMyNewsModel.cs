using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;
using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.Models
{
    public class LstMyNewsModel
    {
        [Required]
        [Display(Name = "Contenu du post")]
        public string ContentPost { get; set; }

        [Display(Name = "Date de publication")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DatePublication { get; set; }

        [Display(Name = "Auteur du post")]
        public virtual User Author { get; set; }

        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Post> MyNews { get; set; }
    }
}