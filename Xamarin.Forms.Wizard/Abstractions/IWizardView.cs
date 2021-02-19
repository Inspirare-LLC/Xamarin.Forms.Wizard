using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Wizard.Abstractions
{
    public interface IWizardView
    {
        Task<bool> OnNext();
        Task<bool> OnPrevious();
    }
}
