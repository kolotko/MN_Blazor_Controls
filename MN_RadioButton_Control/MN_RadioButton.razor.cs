using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN_RadioButton_Control
{
    public partial class MN_RadioButton<TItem>
    {
        /// <summary>
        /// Main parameter responsible for set and get current radio button value
        /// </summary>
        [Parameter]
        public TItem RadioButtonValue
        {
            get
            {
                return _radioButtonValue;
            }
            set
            {
                if (_radioButtonValue is not null && value is not null && _radioButtonValue.Equals(value))
                    return;

                _radioButtonValue = value;
                RadioButtonValueChanged.InvokeAsync(_radioButtonValue);
            }
        }
        /// <summary>
        /// Event used to binding RadioButtonValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<TItem> RadioButtonValueChanged { get; set; }
        /// <summary>
        /// Auxiliary private input value
        /// </summary>
        private TItem _radioButtonValue { get; set; }
        /// <summary>
        /// Responsible for set radio button options
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
            }
        }
        // <summary>
        /// Auxiliary private radio button options
        /// </summary>
        private List<TItem> _itemsList { get; set; }
        /// <summary>
        /// Template to display radio button options
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> ContentList { get; set; }

        /// <summary>
        /// Set new value
        /// </summary>
        private void setNewValue(TItem item)
        {
            RadioButtonValue = item;
        }

        /// <summary>
        /// Set style depend on value for control. 
        /// </summary>
        private string setSelectedStyle(TItem item)
        {
            if (RadioButtonValue.Equals(item))
                return "mn_radioButton_selectedItem";

            return "";
        }
    }
}
