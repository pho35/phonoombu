using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.DAL
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }
        public virtual User userfollower { get; set; }
        public virtual User userfollowed { get; set; }
    }
}
