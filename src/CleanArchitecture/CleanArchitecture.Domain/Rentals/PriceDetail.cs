using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Rentals;

public record PriceDetail(
  Money PeriodPrice,
  Money Maintenance,
  Money Accessories,
  Money TotalPrice
);