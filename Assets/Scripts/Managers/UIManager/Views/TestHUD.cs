using TMPro;
using Entities;
using UnityEngine;

namespace UI
{
    public class TestHUD : Window
    {
        public override void Show(UIViewArguments arguments)
        {
            base.Show(arguments);
            
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
        }
        
        public override void Hide()
        {
            base.Hide();
        }
    }
}