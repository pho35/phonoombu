using System.ComponentModel.DataAnnotations;

namespace MvcPhoNoombuRemake.DAL
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public virtual User LikeFromUser { get; set; }
        public virtual Post LikeForPost { get; set; }
    }
}
