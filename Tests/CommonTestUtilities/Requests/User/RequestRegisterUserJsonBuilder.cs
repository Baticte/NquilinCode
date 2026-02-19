using Bogus;
using NquilinCode.Communication.Requests;

namespace CommonTestUtilities.Requests.User;

public static class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) =>
            {
                var normalizedName = u.Name
                    .ToLowerInvariant()
                    .Replace(" ", ".");

                return $"{normalizedName}@email.com";
            })
            .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength));
    }

}