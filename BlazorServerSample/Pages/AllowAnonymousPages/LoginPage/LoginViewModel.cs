using BlazorServerSample.ToolKit.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorServerSample.Pages.AllowAnonymousPages.LoginPage
{
    public class LoginViewModel : ComponentBase
    {
        [Inject]
        public required NavigationManager NavigationManager { get; set; }
        [Inject]
        public required AuthenticateProvider AuthenticationStateProvider { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        protected string UserId { get; set; } = string.Empty;
        protected string Password { get; set; } = string.Empty;

        protected async Task Login()
        {
            bool result = false;

            if(UserId == "test" && Password == "test")
            {
                result = true;
            }

            if (result)
            {
                await AuthenticationStateProvider.UpdateSignInStatusAsync(new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new (ClaimTypes.Name, "ユーザ名"),
                            },
                            "Custom"
                        )
                    ));
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "ログインに失敗しました。";
            }
        }
    }
}
