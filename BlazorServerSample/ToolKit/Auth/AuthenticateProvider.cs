using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace BlazorServerSample.ToolKit.Auth
{
    public class AuthenticateProvider : AuthenticationStateProvider
    {
        private static readonly AuthenticationState UnauthorizedAuthenticationState = new AuthenticationState(new ClaimsPrincipal());
        private ClaimsPrincipal? _principal;
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public AuthenticateProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_principal is null)
            {
                return await Task.FromResult(UnauthorizedAuthenticationState);
            }
            return await Task.FromResult(new AuthenticationState(_principal));
        }

        public async Task LoadAuthenticationStateAsync()
        {
            var result = await _protectedLocalStorage.GetAsync<string>("authkey");
            if (result.Success)
            {
                var name = result.Value!;
                await UpdateSignInStatusAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                        new(ClaimTypes.Name, name)
                        },
                        "Blazor"
                    )
                ));
            }
        }

        public async Task UpdateSignInStatusAsync(ClaimsPrincipal? principal)
        {
            _principal = principal;
            if (_principal?.Identity?.IsAuthenticated ?? false)
            {
                await _protectedLocalStorage.SetAsync("authkey", _principal.Identity.Name!);
            }
            else
            {
                await _protectedLocalStorage.DeleteAsync("authkey");
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
