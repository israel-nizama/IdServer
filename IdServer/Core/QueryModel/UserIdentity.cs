namespace IdServer.Core.QueryModel
{
    public class UserIdentity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleCode { get; set; }
        public string ClubCode { get; set; }
        public string Scope { get; set; }
    }
}
