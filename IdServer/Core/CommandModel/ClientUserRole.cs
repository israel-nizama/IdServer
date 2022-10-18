namespace IdServer.Core.CommandModel
{
    public class ClientUserRole : BaseEntity
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public int UserId { get; set; }
        public string RoleCode { get; set; }
    }
}
