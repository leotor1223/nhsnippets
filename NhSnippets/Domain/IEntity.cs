using System;

namespace NhSnippets.Domain
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}