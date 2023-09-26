using TaHooK.Api.DAL.Common.Entities.Interfaces;

namespace TaHooK.Api.DAL.Common.Entities
{
    public abstract record EntityBase : IEntity
    {
        public required Guid Id { get; init; }
    }
}
