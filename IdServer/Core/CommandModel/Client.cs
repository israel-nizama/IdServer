namespace IdServer.Core.CommandModel
{
    public class Client : BaseEntity
    {
        public string ClientId { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
    }
}
