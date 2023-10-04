using TaHooK.Api.DAL.Entities.Interfaces;

namespace TaHooK.Api.DAL.Entities
{
    public abstract record EntityBase : IEntity
    {
        public required Guid Id { get; set; }
    }
}
