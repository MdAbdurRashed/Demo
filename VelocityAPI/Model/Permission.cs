namespace VelocityAPI.Model
{
    public class Permission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PageId { get; set; }

        public User? User { get; set; }
        public Page? Page { get; set; }
    }
}
