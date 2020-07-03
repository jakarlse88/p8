using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace CalHealth.Blazor.Client.Shared
{
    public partial class Banner : IDisposable
    {
        [Inject] private IJSRuntime JsRuntime { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private bool IsHomePage { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += HandleUriChanged;
        }

        private void HandleUriChanged(object sender, LocationChangedEventArgs ea)
        {
            if (ea.Location.Contains("booking", StringComparison.OrdinalIgnoreCase))
            {
                IsHomePage = false;
                StateHasChanged();
            }
            else
            {
                IsHomePage = true;
                StateHasChanged();
            }
        }

        private async Task NavigateToAnchor(string anchorName)
        {
            if (string.IsNullOrWhiteSpace(anchorName))
            {
                throw new ArgumentNullException(nameof(anchorName));
            }
        
            await JsRuntime.InvokeVoidAsync("scrollToAnchor", anchorName);
        }
        
        public void Dispose()
        {
            NavigationManager.LocationChanged += HandleUriChanged;
        }
    }
}