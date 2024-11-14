using Microsoft.AspNetCore.Identity;

namespace Application.Common.Extensions;

public static class IdentityResultExtensions
{
    public static string ToErrorMessage(this IdentityResult result)
    {
        var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
        return errors;
    }
}
