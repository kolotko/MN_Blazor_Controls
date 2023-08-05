using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN_Search_Control
{
    public partial class MN_Search : IAsyncDisposable
    {
        /// <summary>
        /// Main parameter responsible for set and get current search value
        /// </summary>
        [Parameter]
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                if (_searchValue == value)
                    return;

                _searchValue = value;
                contentAvaliable();
                SearchValueChanged.InvokeAsync(_searchValue);
                triggerGetHint();
            }
        }

        /// <summary>
        /// Event used to binding SearchValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<string> SearchValueChanged { get; set; }

        /// <summary>
        /// Event to generate hint
        /// </summary>
        [Parameter]
        public EventCallback<string> GenerateHint { get; set; }

        /// <summary>
        /// Auxiliary private search value
        /// </summary>
        private string _searchValue { get; set; }

        /// <summary>
        /// Responsible for set items list
        /// </summary>
        [Parameter]
        public List<string> ItemsList { get; set; }

        /// <summary>
        /// Label for control
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Responsible for control additional styles in main container
        /// </summary>
        private bool _searchContainerDataAvaliable = false;

        /// <summary>
        /// Reference for input. Main intence is set focus when user click on control
        /// </summary>
        private ElementReference _refSearch;

        /// <summary>
        /// Template to display list
        /// </summary>
        [Parameter]
        public RenderFragment<string> ContentList { get; set; }

        /// <summary>
        /// JsFileReference
        /// </summary>
        private Lazy<Task<IJSObjectReference>> jsModuleTask;

        /// <summary>
        /// Initialize Component
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            jsModuleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/MN_Search_Control/MN_Search.razor.js").AsTask());

            var module = await jsModuleTask.Value;
            var elementsId = new[] {
                $"mn_searchContainer-{this.GetHashCode()}",
                $"mn_searchWrapper-{this.GetHashCode()}",
                $"mn_searchInput-{this.GetHashCode()}",
                $"mn_searchList-{this.GetHashCode()}",
            };
            await module.InvokeAsync<string>("MN_Search_CheckThatElementIsInListAndToogle", new object[] { elementsId }, $"mn_searchContainer-{this.GetHashCode()}", "mn_showList");
            await module.InvokeAsync<string>("MN_Search_InputEnterEvent", $"mn_searchInput-{this.GetHashCode()}", $"mn_searchContainer-{this.GetHashCode()}", "mn_showList");
        }

        /// <summary>
        /// Set focus on input
        /// </summary>
        private void setSearchFocus()
        {
            _refSearch.FocusAsync();
            setContentAvaliable();
        }

        /// <summary>
        /// Unset input focus
        /// </summary>
        private void unsetSearchFocus()
        {
            contentAvaliable();
        }

        /// <summary>
        /// Set class depends on content available
        /// </summary>
        private void setContentAvaliable()
        {
            if (!_searchContainerDataAvaliable)
                _searchContainerDataAvaliable = true;
        }

        /// <summary>
        /// Unset class depends on content available
        /// </summary>
        private void unsetContentAvaliable()
        {
            if (_searchContainerDataAvaliable)
                _searchContainerDataAvaliable = false;
        }

        /// <summary>
        /// Set or remove style depends on move back label in control. 
        /// If input is empty but user keep focus, style keep label back.
        /// </summary>
        private void contentAvaliable()
        {

            if (!string.IsNullOrEmpty(_searchValue))
            {
                setContentAvaliable();
                return;
            }

            unsetContentAvaliable();
        }

        /// <summary>
        /// Set selected value
        /// </summary>
        /// <param name="item"></param>
        private void setNewValue(string item)
        {
            SearchValue = item;
        }

        /// <summary>
        /// Trigger event depends on get hint
        /// </summary>
        private void triggerGetHint()
        {
            GenerateHint.InvokeAsync(_searchValue);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (jsModuleTask.IsValueCreated)
            {
                var module = await jsModuleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
