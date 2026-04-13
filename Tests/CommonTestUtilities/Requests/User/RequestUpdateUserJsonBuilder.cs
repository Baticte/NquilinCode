using Bogus;
using NquilinCode.Communication.Requests;

namespace CommonTestUtilities.Requests.User;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) =>
            {
                var normalizedName = u.Name
                    .ToLowerInvariant()
                    .Replace(" ", ".");

                var domain = f.Internet.DomainName();

                return $"{normalizedName}@{domain}";
            });
    }
}