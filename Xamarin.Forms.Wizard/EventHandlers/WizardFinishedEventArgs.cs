using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Wizard.ViewModels;

namespace Xamarin.Forms.Wizard.EventHandlers
{
    public class WizardFinishedEventArgs : EventArgs
    {
        public WizardFinishedEventArgs(IEnumerable<WizardItemViewModel> wizardItemViewModels)
        {
            WizardItemViewModels = wizardItemViewModels;
        }


        public IEnumerable<WizardItemViewModel> WizardItemViewModels { get; private set; }
    }
}
