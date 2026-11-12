using CleanArchitecture.Domain.Rentals;
using CleanArchitecture.Domain.Rentals.Events;
using CleanArchitecture.Domain.Users;
using MediatR;

namespace CleanArchitecture.Application.Rentals.ReserveRental;

internal sealed class ReserveRentalDomainEventHandler(
    IRentalRepository rentalRepository,
    IUserRepository userRepository,
    IEmailService emailService) : INotificationHandler<RentalReservedDomainEvent>
{
    
    
    public Task Handle(RentalReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}