using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Rentals;
using CleanArchitecture.Domain.Reviews.Events;

namespace CleanArchitecture.Domain.Reviews;

public sealed class Review : Entity
{
  private Review(
    Guid id,
    Guid vehicleId,
    Guid rentalId,
    Guid userId,
    Rating rating,
    Comment comment,
    DateTime? creationDate
  ) : base(id)
  {
    VehicleId = vehicleId;
    RentalId = rentalId;
    UserId = userId;
    Rating = rating;
    Comment = comment;
    CreationDate = creationDate;
  }

  public Guid VehicleId { get; private set; }
  public Guid RentalId { get; private set; }
  public Guid UserId { get; private set; }
  public Rating Rating { get; private set; }
  public Comment Comment { get; private set; }
  public DateTime? CreationDate { get; private set; }

  public static Result<Review> Create(
    Rental rental,
    Rating rating,
    Comment comment,
    DateTime creationDate
  )
  {
    if (rental.Status != RentalStatus.Completed)
    {
      return Result.Failure<Review>(ReviewErrors.NotEligible);
    }

    var review = new Review(
      Guid.NewGuid(),
      rental.VehicleId,
      rental.Id,
      rental.UserId,
      rating,
      comment,
      creationDate
    );

    review.RaiseDomainEvent(new ReviewCreateDomainEvent(review.Id));

    return review;
  }
}