using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehicles;

public sealed class Vehicle : Entity
{
  public Vehicle(
    Guid id,
    Model model,
    Vin vin,
    Money price,
    Money maintenance,
    DateTime lastRentalDate,
    List<Accessory> accessories,
    Address? address
  ) : base(id)
  {
    Model = model;
    Vin = vin;
    Price = price;
    Maintenance = maintenance;
    LastRentalDate = lastRentalDate;
    Accessories = accessories;
    Address = address;
  }

  public Model? Model { get; private set; }
  public Vin? Vin { get; private set; }
  public Address? Address { get; private set; }
  public Money? Price { get; private set; }
  public Money? Maintenance { get; private set; }
  public DateTime? LastRentalDate { get; internal set; }
  public List<Accessory> Accessories { get; private set; } = new();
}