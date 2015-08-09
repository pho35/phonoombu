using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPhoNoombuRemake.DAL;

namespace MvcPhoNoombuRemake.Models
{
    public class LstTypeGroup
    {
        public List<Groups> groupsPerso { get; set; }
        public List<Groups> groupsJoin { get; set; }
        public List<Groups> groupsAll { get; set; }
    }
}