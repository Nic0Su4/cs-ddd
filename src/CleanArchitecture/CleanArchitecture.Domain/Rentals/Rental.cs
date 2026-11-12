using System.IO.Pipelines;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Rentals.Events;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehicles;

namespace CleanArchitecture.Domain.Rentals;

public sealed class Rental : Entity
{
  private Rental(
    Guid id,
    Guid vehicleId,
    Guid userId,
    DateRange duration,
    Money periodPrice,
    Money maintenance,
    Money accessories,
    Money totalPrice,
    RentalStatus status,
    DateTime creationDate
  ) : base(id)
  {
    VehicleId = vehicleId;
    UserId = userId;
    Duration = duration;
    PeriodPrice = periodPrice;
    Maintenance = maintenance;
    Accessories = accessories;
    TotalPrice = totalPrice;
    Status = status;
    CreationDate = creationDate;
  }

  public Guid VehicleId { get; private set; }
  public Guid UserId { get; private set; }
  public Money? PeriodPrice { get; private set; }
  public Money? Maintenance { get; private set; }
  public Money? Accessories { get; private set; }
  public Money TotalPrice { get; private set; }
  public RentalStatus Status { get; private set; }
  public DateRange Duration { get; private set; }
  public DateTime? CreationDate { get; private set; }
  public DateTime? ConfirmationDate { get; private set; }
  public DateTime? DenialDate { get; private set; }
  public DateTime? CompletedDate { get; private set; }
  public DateTime? CancellationDate { get; private set; }

  public static Rental Reserve(
    Vehicle vehicle,
    Guid userId,
    DateRange duration,
    DateTime creationDate,
    PriceService priceService
  )
  {
    var priceDetail = priceService.CalculatePrice(
      vehicle,
      duration
    );

    var rental = new Rental(
      Guid.NewGuid(),
      vehicle.Id,
      userId,
      duration,
      priceDetail.PeriodPrice,
      priceDetail.Maintenance,
      priceDetail.Accessories,
      priceDetail.TotalPrice,
      RentalStatus.Reserved,
      creationDate
    );

    rental.RaiseDomainEvent(new RentalReservedDomainEvent(rental.Id));

    vehicle.LastRentalDate = creationDate;

    return rental;
  }

  public Result Confirm(DateTime utcNow)
  {
    if (Status != RentalStatus.Reserved)
    {
      return Result.Failure(RentalErrors.NotReserved);
    }

    Status = RentalStatus.Confirmed;
    ConfirmationDate = utcNow;

    RaiseDomainEvent(new RentalConfirmedDomainEvent(Id));

    return Result.Success();
  }

  public Result Deny(DateTime utcNow)
  {
    if (Status != RentalStatus.Reserved)
    {
      return Result.Failure(RentalErrors.NotReserved);
    }

    Status = RentalStatus.Denied;
    DenialDate = utcNow;
    RaiseDomainEvent(new RentalDeniedDomainEvent(Id));

    return Result.Success();
  }

  public Result Cancel(DateTime utcNow)
  {
    if (Status != RentalStatus.Confirmed)
    {
      return Result.Failure(RentalErrors.NotConfirmed);
    }

    var currDate = DateOnly.FromDateTime(utcNow);

    if (currDate > Duration.Start)
    {
      return Result.Failure(RentalErrors.AlreadyStarted);
    }

    Status = RentalStatus.Cancelled;
    CancellationDate = utcNow;
    RaiseDomainEvent(new RentalCancelledDomainEvent(Id));

    return Result.Success();
  }

  public Result Complete(DateTime utcNow)
  {
    if (Status != RentalStatus.Confirmed)
    {
      return Result.Failure(RentalErrors.NotConfirmed);
    }

    Status = RentalStatus.Confirmed;
    CompletedDate = utcNow;
    RaiseDomainEvent(new RentalCompletedDomainEvent(Id));

    return Result.Success();
  }
}