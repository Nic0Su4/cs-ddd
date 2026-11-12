using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehicles;

public static class VehicleErrors
{
    public static Error NotFound = new(
        "Vehicle.Found",
        "Vehicle Not Found with this Id");
}