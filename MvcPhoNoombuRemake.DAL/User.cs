using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcPhoNoombuRemake.DAL
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Display(Name = "Nom")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateBirth { get; set; }

        public string Gender { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }

        public virtual List<Follow> MyFollow { get; set; }
        public virtual List<Post> LstPost { get; set; }
        public virtual List<Groups> MyGroups { get; set; }
        public virtual List<Like> MyLstLike { get; set; }
        public virtual List<Comment> MyLstComment { get; set; }
    }
}
