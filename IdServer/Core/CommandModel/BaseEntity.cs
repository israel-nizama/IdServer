namespace IdServer.Core.CommandModel
{
    public abstract class BaseEntity
    {
        protected void SetCreateAudit(string createBy)
        {
            CreatedBy = createBy;
            CreatedOn = DateTime.UtcNow;
        }

        public byte[] Timestamp { get; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
