using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals.Events;

public sealed record RentalReservedDomainEvent(Guid RentalId) : IDomainEvent;