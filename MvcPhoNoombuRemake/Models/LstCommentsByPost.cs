using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstCommentsByPost
    {
        public Post postConcerned { get; set; }
        public Comment unComment { get; set; }
        //public List<Comment> lstCommentsByPost { get; set; }
    }
}