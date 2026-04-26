using System.Net;
using SmartFix.Domain.Exceptions;
using SmartFix.Domain.ValueObjects;

namespace SmartFix.Domain.Aggregates;

public class Client : User
{
    public string? PhoneNumber { get; private set; }
    public int PersonalDiscount { get; private set; } = 0;
    public ClientStatus Status { get; private set; }

    private Client()
    {
    }

    public static Client Create(string email, string passwordHash, string? name, string? phoneNumber)
    {
        return new Client
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Name = name,
            PhoneNumber = phoneNumber,
            PersonalDiscount = 0,
            Status = ClientStatus.New
        };
    }

    public void SetPersonalDiscount(int discountPercent)
    {
        if (discountPercent < 0 || discountPercent > 100)
            throw new HttpException(HttpStatusCode.BadRequest, "Скидка должна быть от 0 до 100%");
        PersonalDiscount = discountPercent;
    }

    public void UpdateClient(string email, string name, string phone)
    {
        Email = email;
        Name = name;
        PhoneNumber = phone;
    }
    
    public void SetStatus(ClientStatus newStatus)
    {
        Status = newStatus;
    }
}