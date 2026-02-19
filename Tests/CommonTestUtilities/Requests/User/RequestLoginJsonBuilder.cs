using Bogus;
using NquilinCode.Communication.Requests;

namespace CommonTestUtilities.Requests.User;

public static class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build(int passwordLenght = 10)
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password(passwordLenght));
    }
}