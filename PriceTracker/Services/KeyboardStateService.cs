using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceTracker.Services
{
    public class KeyboardStateService
    {
        public event Action? OnKeyboardChanged;

        private bool _isKeyboardVisible;
        public bool IsKeyboardVisible
        {
            get => _isKeyboardVisible;
            private set
            {
                if (_isKeyboardVisible != value)
                {
                    _isKeyboardVisible = value;
                    OnKeyboardChanged?.Invoke();
                }
            }
        }

        public void SetKeyboardVisible(bool visible)
        {
            IsKeyboardVisible = visible;
        }
    }


}