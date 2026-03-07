using Microsoft.AspNetCore.Components;

namespace Burrow.Auth.Components.Account.Shared;

public partial class ShowRecoveryCodes
{
    [Parameter]
    public string[] RecoveryCodes { get; set; } = [];

    [Parameter]
    public string? StatusMessage { get; set; }
}
