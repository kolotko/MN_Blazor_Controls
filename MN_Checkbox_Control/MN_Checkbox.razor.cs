using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN_Checkbox_Control
{
    public partial class MN_Checkbox
    {
        /// <summary>
        /// Main parameter responsible for set and get current checkbox value
        /// </summary>
        [Parameter]
        public bool CheckboxValue
        {
            get => _checkboxValue;
            set
            {
                if (_checkboxValue == value)
                    return;

                _checkboxValue = value;
                setStyle();
                CheckboxValueChanged.InvokeAsync(_checkboxValue);
            }
        }
        /// <summary>
        /// Event used to binding CheckboxValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<bool> CheckboxValueChanged { get; set; }
        /// <summary>
        /// Auxiliary private input value
        /// </summary>
        private bool _checkboxValue { get; set; }

        /// <summary>
        /// It is used to pass content from outside the control. User using this field can render custom view of text.
        /// </summary>
        [Parameter]
        public RenderFragment Content { get; set; }

        /// <summary>
        /// Responsible for control additional styles in main container
        /// </summary>
        private string _checkboxContainerClass { get; set; }

        /// <summary>
        /// Toggle value
        /// </summary>
        private void toggleValue()
        {
            CheckboxValue = !CheckboxValue;
        }

        /// <summary>
        /// Set or remove style depend on value for control. 
        /// </summary>
        private void setStyle()
        {
            if (CheckboxValue)
            {
                _checkboxContainerClass = "mn_checkboxSet";
                return;
            }

            _checkboxContainerClass = "";
        }
    }
}
