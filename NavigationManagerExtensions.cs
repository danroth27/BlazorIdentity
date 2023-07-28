using Microsoft.AspNetCore.Components;

namespace BlazorIdentity;

public static class NavigationManagerExtensions
{
    public static string GetUriWithQuery(this NavigationManager navigationManager, string uri, object query)
    {
        var parametersDict = query.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(query));
        return navigationManager.GetUriWithQueryParameters(uri, parametersDict);
    }

    public static string GetUriWithQueryParameter(this NavigationManager navigationManager, string uri, string name, object? value)
        => navigationManager.GetUriWithQueryParameters(uri, new Dictionary<string, object?> { { name, value } });

    public static void NavigateToLocal(this NavigationManager navigationManager, string uri)
    {
        try
        {
            var relative = navigationManager.ToBaseRelativePath(navigationManager.ToAbsoluteUri(uri).AbsoluteUri);
            //navigationManager.NavigateTo(relative);
            // Workaround for https://github.com/dotnet/aspnetcore/issues/49670
            navigationManager.NavigateTo(navigationManager.ToAbsoluteUri(relative).AbsoluteUri);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("The supplied URL is not local. A URL with an absolute path is considered local if it does not have a host/authority part.");
        }
    }
}
