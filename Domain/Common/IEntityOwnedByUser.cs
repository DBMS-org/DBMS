namespace Domain.Common
{
    public interface IEntityOwnedByUser
    {
        public int OwningUserId { get; }
    }
} 