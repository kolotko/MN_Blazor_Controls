using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MN_Select_Control
{
    public partial class MN_Select<TItem> : IAsyncDisposable
    {
        /// <summary>
        /// Label for control
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Responsible for set items list
        /// </summary>
        [Parameter]
        public List<TItem> ItemsList
        {
            get
            {
                return _itemsList;
            }
            set
            {
                _itemsList = value;
                isSelectedValueOnList();
            }
        }

        // <summary>
        /// Auxiliary private items list
        /// </summary>
        private List<TItem> _itemsList { get; set; }

        /// <summary>
        /// Template to display list
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ContentList { get; set; }

        /// <summary>
        /// Template to selected value
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ContentSelect { get; set; }

        /// <summary>
        /// Main parameter responsible for set and get current select value
        /// </summary>
        [Parameter]
        public TItem SelectValue
        {
            get
            {
                return _selectValue;
            }
            set
            {
                if (_selectValue is not null && value is not null && _selectValue.Equals(value))
                    return;

                _selectValue = value;
                isSelectedValueOnList();
                if (_selectValue is not null && value is not null && _selectValue.Equals(value))
                    SelectValueChanged.InvokeAsync(_selectValue);
            }
        }

        /// <summary>
        /// Auxiliary private select value
        /// </summary>
        private TItem _selectValue { get; set; }

        /// <summary>
        /// Event used to binding InputValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<TItem> SelectValueChanged { get; set; }

        /// <summary>
        /// Responsible for control additional styles in main container
        /// </summary>
        private bool _selectContainerDataAvaliable { get; set; } = false;

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
               "import", "./_content/MN_Select_Control/MN_Select.razor.js").AsTask());

            var module = await jsModuleTask.Value;
            var elementsId = new[] {
                $"mn_selectContainer-{this.GetHashCode()}",
                $"mn_selectWrapper-{this.GetHashCode()}",
                $"mn_freeSpaceFiller-{this.GetHashCode()}",
                $"mn_selectSelectedValueWrapper-{this.GetHashCode()}",
                $"mn_select-image-container-{this.GetHashCode()}",
                $"mn_selectSvgArrow-{this.GetHashCode()}",
                $"mn_selectSvgArrowPath-{this.GetHashCode()}"
            };
            await module.InvokeAsync<string>("MN_Select_CheckThatElementIsInListAndToogle", new object[] { elementsId }, $"mn_selectContainer-{this.GetHashCode()}", "mn_showList");
        }

        /// <summary>
        /// Check that selected item is avaliable on list (if avaliable then display selected item)
        /// </summary>
        private void isSelectedValueOnList()
        {
            if (_selectValue is not null && _itemsList is not null && _itemsList.Contains(_selectValue))
            {
                addSelectedItemStyle();
                return;
            }

            removeSelectedItemStyle();
        }

        /// <summary>
        /// Set selected value
        /// </summary>
        /// <param name="item"></param>
        private void setNewValue(TItem item)
        {
            SelectValue = item;
        }

        /// <summary>
        /// Set selected item style
        /// </summary>
        private void addSelectedItemStyle()
        {
            if (!_selectContainerDataAvaliable)
                _selectContainerDataAvaliable = true;
        }

        /// <summary>
        /// Remove selected item style
        /// </summary>
        private void removeSelectedItemStyle()
        {
            if (_selectContainerDataAvaliable)
                _selectContainerDataAvaliable = false;
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
