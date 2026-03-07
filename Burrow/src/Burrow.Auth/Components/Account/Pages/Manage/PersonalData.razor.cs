using Microsoft.AspNetCore.Components;
using Burrow.Auth.Components.Account;

namespace Burrow.Auth.Components.Account.Pages.Manage;

public partial class PersonalData
{
    [Inject] private IdentityUserAccessor UserAccessor { get; set; } = default!;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _ = await UserAccessor.GetRequiredUserAsync(HttpContext);
    }
}
