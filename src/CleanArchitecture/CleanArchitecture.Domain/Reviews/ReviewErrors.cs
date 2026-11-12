using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public static class ReviewErrors
{
  public static readonly Error NotEligible = new(
    "Review.NotEligible",
    "This review and/or rating isn't eligible because the rental isn't completed yet"
  );
}