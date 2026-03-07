using Burrow.Auth.Components.Account;
using Microsoft.AspNetCore.Components;

namespace Burrow.Auth.Tests.Components.Account;

public class IdentityRedirectManagerTests
{
    private static TestNavigationManager CreateNavigationManager(string baseUri = "https://localhost/")
    {
        var nav = new TestNavigationManager();
        nav.Init(baseUri, baseUri);
        return nav;
    }

    [Fact]
    public void StatusCookieName_IsExpectedValue()
    {
        Assert.Equal("Identity.StatusMessage", IdentityRedirectManager.StatusCookieName);
    }

    [Fact]
    public void RedirectTo_WithRelativeUri_CallsNavigateTo()
    {
        var navigationManager = CreateNavigationManager();
        var manager = new IdentityRedirectManager(navigationManager);

        Assert.Throws<InvalidOperationException>(() => manager.RedirectTo("account/login"));
        Assert.Equal("account/login", navigationManager.LastNavigatedUri);
    }

    [Fact]
    public void RedirectTo_WithAbsoluteUriMatchingBase_ConvertsToRelative()
    {
        var navigationManager = CreateNavigationManager("https://localhost/");
        var manager = new IdentityRedirectManager(navigationManager);

        Assert.Throws<InvalidOperationException>(() => manager.RedirectTo("https://localhost/account/login"));
        Assert.Equal("account/login", navigationManager.LastNavigatedUri);
    }

    [Fact]
    public void RedirectTo_ThrowsInvalidOperationException()
    {
        var navigationManager = CreateNavigationManager();
        var manager = new IdentityRedirectManager(navigationManager);

        var exception = Assert.Throws<InvalidOperationException>(() => manager.RedirectTo("account/login"));
        Assert.Contains(nameof(IdentityRedirectManager), exception.Message);
    }

    [Fact]
    public void RedirectTo_WithNullUri_RedirectsToEmptyString()
    {
        var navigationManager = CreateNavigationManager();
        var manager = new IdentityRedirectManager(navigationManager);

        Assert.Throws<InvalidOperationException>(() => manager.RedirectTo(null));
        Assert.Equal("", navigationManager.LastNavigatedUri);
    }

    /// <summary>
    /// A concrete <see cref="NavigationManager"/> implementation for use in unit tests.
    /// Exposes an <c>Initialize</c> method and records the last URI passed to <c>NavigateTo</c>.
    /// </summary>
    private sealed class TestNavigationManager : NavigationManager
    {
        public string? LastNavigatedUri { get; private set; }

        public void Init(string baseUri, string uri) => base.Initialize(baseUri, uri);

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            LastNavigatedUri = uri;
        }
    }
}
