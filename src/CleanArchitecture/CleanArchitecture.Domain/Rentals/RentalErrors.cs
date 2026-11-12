using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Rentals;

public static class RentalErrors
{
  public static Error NotFound = new Error(
    "Rental.Found",
    "The Rental with the specified Id wasn't found"
  );

  public static Error Overlap = new Error(
    "Rental.Overlap",
    "The Rental is being taken by two or more clients at the same time on the same date"
  );

  public static Error NotReserved = new Error(
    "Rental.NotReserved",
    "The Rental isn't reserved"
  );

  public static Error NotConfirmed = new Error(
    "Rental.NotConfirmed",
    "The Rental isn't confirmed"
  );

  public static Error AlreadyStarted = new Error(
    "Rental.AlreadyStarted",
    "The Rental has already started"
  );
}