using CleanArchitecture.Application.Abstractions.Clock;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Rentals;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehicles;

namespace CleanArchitecture.Application.Rentals.ReserveRental;

internal sealed class ReserveRentalCommandHandler(
    IUserRepository userRepository,
    IVehicleRepository vehicleRepository,
    IRentalRepository rentalRepository,
    PriceService priceService,
    IUnitOfWork uow,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<ReserveRentalCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReserveRentalCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
        if (vehicle is null)
        {
            return Result.Failure<Guid>(VehicleErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await rentalRepository.IsOverlapingAsync(vehicle, duration, cancellationToken))
        {
            return Result.Failure<Guid>(RentalErrors.Overlap);
        }

        var rental = Rental.Reserve(
            vehicle,
            user.Id,
            duration,
            dateTimeProvider.CurrentTime,
            priceService);

        rentalRepository.Add(rental);

        await uow.SaveChangesAsync(cancellationToken);
        return rental.Id;
    }
}