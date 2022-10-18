namespace IdServer.Core.CommandModel
{
    public class User : BaseEntity
    {
        public User(string userName, string firstName, string lastName, string email, string clubCode, string createdBy)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ClubCode = clubCode;
            EmailConfirmed = false;
            SetCreateAudit(createdBy);
        }

        public User() {}

        public void ConfirmEmail(string passwordHash)
        {
            PasswordHash = passwordHash;
            EmailConfirmed = true;
        }

        public int Id { get; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ClubCode { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
    }
}
