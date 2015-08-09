using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.DAL
{
    public class Groups
    {
        [Key]
        public int GroupsId { get; set; }

        [Display(Name = "Nom du groupe")]
        public string GroupsName { get; set; }

        public virtual User Owner { get; set; }

        public virtual List<User> Members { get; set; }

    }
}
