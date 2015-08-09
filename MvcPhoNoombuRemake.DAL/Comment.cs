using System;
using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.DAL
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string CommentContent { get; set; }

        public DateTime DatePublication { get; set; }
        public virtual User CommentAuthor { get; set; }
        public virtual Post PostConcerned { get; set; }
    }
}
