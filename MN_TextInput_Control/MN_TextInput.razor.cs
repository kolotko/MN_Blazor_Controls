using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MN_TextInput_Control
{
    public partial class MN_TextInput
    {
        /// <summary>
        /// Main parameter responsible for set and get current input value
        /// </summary>
        [Parameter]
        public string InputValue
        {
            get => _inputValue;
            set
            {
                if (_inputValue == value)
                    return;

                _inputValue = value;
                contentAvaliable();
                InputValueChanged.InvokeAsync(_inputValue);
            }
        }

        /// <summary>
        /// Event used to binding InputValue. It's possible to set and get value from control using 1 field in control.
        /// </summary>
        [Parameter]
        public EventCallback<string> InputValueChanged { get; set; }

        /// <summary>
        /// Auxiliary private input value
        /// </summary>
        private string _inputValue { get; set; }

        /// <summary>
        /// Label for control
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Reference for input. Main intence is set focus when user click on control
        /// </summary>
        private ElementReference _refTextInput;

        /// <summary>
        /// Responsible for control additional styles in main container
        /// </summary>
        private string _textInputContainerClass = "";

        /// <summary>
        /// Set focus on input
        /// </summary>
        private void setInputFocus()
        {
            _refTextInput.FocusAsync();
            _textInputContainerClass = "mn_contentAvaliable";
        }

        /// <summary>
        /// Unset input focus
        /// </summary>
        private void unsetInputFocus()
        {
            contentAvaliable();
        }

        /// <summary>
        /// Set or remove style depend for move back label in control. 
        /// If input is empty but user keep focus, style keep label back.
        /// </summary>
        private void contentAvaliable()
        {
            if (!string.IsNullOrEmpty(_inputValue))
            {
                _textInputContainerClass = "mn_contentAvaliable";
                return;
            }

            _textInputContainerClass = "";
        }
    }
}
