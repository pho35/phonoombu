using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstMemberGroup
    {
        public Groups groupsConcerned { get; set; }
        public List<User> Members { get; set; }
        public List<Follow> UserSuggest { get; set; }
    }
}