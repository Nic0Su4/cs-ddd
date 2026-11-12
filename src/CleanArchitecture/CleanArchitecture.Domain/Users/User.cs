using System.Security.Cryptography;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;

namespace CleanArchitecture.Domain.Users;

public sealed class User : Entity
{
  private User(
    Guid id,
    Name name,
    Lastname lastname,
    Email email
  ) : base(id)
  {
    Name = name;
    Lastname = lastname;
    Email = email;
  }

  public Name? Name { get; private set; }
  public Lastname? Lastname { get; private set; }
  public Email? Email { get; private set; }

  public static User Create(
    Name name,
    Lastname lastname,
    Email email
  )
  {
    var user = new User(
      Guid.NewGuid(),
      name,
      lastname,
      email
    );

    user.RaiseDomainEvent(new UserCreateDomainEvent(user.Id));

    return user;
  }
}