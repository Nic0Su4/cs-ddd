using System.Security.AccessControl;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehicles;

namespace CleanArchitecture.Domain.Rentals;

public class PriceService
{
  public PriceDetail CalculatePrice(Vehicle vehicle, DateRange period)
  {
    var currency = vehicle.Price!.Currency;

    var periodPrice = new Money(
      period.NumberOfDays * vehicle.Price.Amount,
      currency
    );

    decimal percentageCharge = 0;

    foreach (var accessory in vehicle.Accessories)
    {
      percentageCharge += accessory switch
      {
        Accessory.AppleCar or Accessory.AndroidCar => 0.05m,
        Accessory.AirConditioning => 0.01m,
        Accessory.Maps => 0.01m,
        Accessory.Wifi => 0.02m,
        _ => 0
      };
    }

    var accesoryCharges = Money.Zero(currency);

    if (percentageCharge > 0)
    {
      accesoryCharges = new Money(
        periodPrice.Amount * percentageCharge,
        currency
      );
    }

    var totalPrice = Money.Zero();
    totalPrice += periodPrice;

    if (!vehicle.Maintenance!.IsZero())
    {
      totalPrice += vehicle.Maintenance;
    }

    totalPrice += accesoryCharges;

    return new PriceDetail(
      periodPrice,
      vehicle.Maintenance,
      accesoryCharges,
      totalPrice
    );
  }
}