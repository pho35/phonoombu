using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstPostModel
    {
        public List<Like> Likes { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
    }
}