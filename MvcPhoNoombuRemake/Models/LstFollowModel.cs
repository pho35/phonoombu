using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstFollowModel
    {
        public List<Follow> LstUserFollowed { get; set; }
        public List<Follow> LstUserFollower { get; set; }
    }
}