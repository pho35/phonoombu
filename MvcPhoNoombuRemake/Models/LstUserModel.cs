using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstUserModel
    {
        public List<User> Users { get; set; }
        public List<Follow> Follows { get; set; }
    }
}