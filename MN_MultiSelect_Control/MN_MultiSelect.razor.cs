using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN_MultiSelect_Control
{
    public partial class MN_MultiSelect<TItem> : IAsyncDisposable
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
                initializeMultiSelectCheckboxContainerClassDictionary(value);
                _itemsList = value;
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
        public RenderFragment<TItem> ContentMultiSelect { get; set; }

        /// <summary>
        /// Main parameter responsible for set and get current select value
        /// </summary>
        [Parameter]
        public List<TItem> MultiSelectValue
        {
            get
            {
                return _multiSelectValue;
            }
            set
            {
                if (_multiSelectValue is not null && value is not null && _multiSelectValue.Equals(value))
                    return;

                _multiSelectValue = value;
                isMultiSelectedValue();
                if (_multiSelectValue is not null && value is not null && _multiSelectValue.Equals(value))
                    MultiSelectValueChanged.InvokeAsync(_multiSelectValue);
            }
        }

        /// <summary>
        /// Auxiliary private select value
        /// </summary>
        private List<TItem> _multiSelectValue { get; set; }

        /// <summary>
        /// Event used to binding InputValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<List<TItem>> MultiSelectValueChanged { get; set; }

        private Dictionary<string, string> _multiSelectCheckboxContainerClassDictionary { get; set; }

        /// <summary>
        /// Responsible for control additional styles in main container
        /// </summary>
        private bool _multiSelectContainerDataAvaliable { get; set; } = false;

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
               "import", "./_content/MN_MultiSelect_Control/MN_MultiSelect.razor.js").AsTask());

            var module = await jsModuleTask.Value;
            List<string> elementsId = new List<string>() {
                $"mn_multiSelectContainer-{this.GetHashCode()}",
                $"mn_multiSelectWrapper-{this.GetHashCode()}",
                $"mn_freeSpaceFiller-{this.GetHashCode()}",
                $"mn_multiSelectSelectedValueContainer-{this.GetHashCode()}",
                $"mn_multiSelectSelectedValueWrapper-{this.GetHashCode()}",
                $"mn_multiSelect-image-container-{this.GetHashCode()}",
                $"mn_multiSelectSvgArrow-{this.GetHashCode()}",
                $"mn_multiSelectSvgArrowPath-{this.GetHashCode()}",
                $"mn_multiSelectSelectedValueContainer-{this.GetHashCode()}",
                $"mn_multiSelectSelectedValueContainer-{this.GetHashCode()}-label",
                $"mn_listSelect-{this.GetHashCode()}",
                $"mn_multiSelect-ul-{this.GetHashCode()}",
            };

            List<string> ignoredElementsId = new List<string>();
            foreach (var item in _itemsList)
            {
                ignoredElementsId.Add($"mn_multiSelect-li-{this.GetHashCode()}-{item.GetHashCode()}");
                ignoredElementsId.Add($"mn_multiSelectCheckboxContainer-{this.GetHashCode()}-{item.GetHashCode()}");
                ignoredElementsId.Add($"mn_multiSelectCheckboxInput-{this.GetHashCode()}-{item.GetHashCode()}");
                ignoredElementsId.Add($"mn_multiSelect-checkbox-svg-{this.GetHashCode()}-{item.GetHashCode()}");
                ignoredElementsId.Add($"mn_multiSelect-checkbox-path-{this.GetHashCode()}-{item.GetHashCode()}");
                ignoredElementsId.Add($"mn_multiSelectCheckboxContent-{this.GetHashCode()}-{item.GetHashCode()}");
            }
            await module.InvokeAsync<string>("MN_MultiSelectCheckThatElementIsInListAndToogle", new object[] { elementsId }, $"mn_multiSelectContainer-{this.GetHashCode()}", "mn_showList", new object[] { ignoredElementsId });
        }

        /// <summary>
        /// Initialize _multiSelectCheckboxContainerClassDictionary when user set new ItemsList
        /// </summary>
        /// <param name="itemsList"></param>
        private void initializeMultiSelectCheckboxContainerClassDictionary(List<TItem> itemsList)
        {
            if (!detectChangesBetweenItemsListMultiSelectCheckboxContainerClassDictionary(itemsList))
                return;

            _multiSelectCheckboxContainerClassDictionary = new Dictionary<string, string>();

            foreach (var item in itemsList)
            {
                _multiSelectCheckboxContainerClassDictionary.Add(item.GetHashCode().ToString(), "");
            }
        }

        /// <summary>
        /// Detect new value on ItemsList. If user click on element on list we should not detect changes
        /// </summary>
        /// <param name="itemsList"></param>
        /// <returns></returns>
        private bool detectChangesBetweenItemsListMultiSelectCheckboxContainerClassDictionary(List<TItem> itemsList)
        {
            if (_multiSelectCheckboxContainerClassDictionary is not null)
            {
                var currentObjectHashCode = _multiSelectCheckboxContainerClassDictionary.Keys.ToArray();
                var newObjectHashCode = itemsList.Select(x => x.GetHashCode().ToString()).ToArray();
                var r = currentObjectHashCode.Except(newObjectHashCode).ToList();
                if (r.Count() == 0)
                    return false;
            }

            return true;

        }

        /// <summary>
        /// Check that selected item is avaliable on list (if avaliable then display selected item)
        /// </summary>
        private void isMultiSelectedValue()
        {
            if (_multiSelectValue is not null && _multiSelectValue.Count > 0)
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
            changeSelectedValue(item);
            setCheckboxStyle(item);
            isMultiSelectedValue();
            MultiSelectValueChanged.InvokeAsync(_multiSelectValue);
        }

        /// <summary>
        /// Set or remove item on selected list
        /// </summary>
        /// <param name="item"></param>
        private void changeSelectedValue(TItem item)
        {
            if (MultiSelectValue.Contains(item))
            {
                MultiSelectValue.Remove(item);
                return;
            }

            MultiSelectValue.Add(item);
        }

        /// <summary>
        /// Set or remove style depend on value for control. 
        /// </summary>
        private void setCheckboxStyle(TItem item)
        {
            if (MultiSelectValue.Contains(item))
            {
                _multiSelectCheckboxContainerClassDictionary[item.GetHashCode().ToString()] = "mn_multiSelectCheckboxSet";
                return;
            }

            _multiSelectCheckboxContainerClassDictionary[item.GetHashCode().ToString()] = "";
        }

        /// <summary>
        /// Set selected item style
        /// </summary>
        private void addSelectedItemStyle()
        {
            if (!_multiSelectContainerDataAvaliable)
            {
                _multiSelectContainerDataAvaliable = true;
                return;
            }
        }

        /// <summary>
        /// Remove selected item style
        /// </summary>
        private void removeSelectedItemStyle()
        {
            if (_multiSelectContainerDataAvaliable)
            {
                _multiSelectContainerDataAvaliable = false;
                return;
            }
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
