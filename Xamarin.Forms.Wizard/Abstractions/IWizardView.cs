using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Wizard.ViewModels;

namespace Xamarin.Forms.Wizard.Abstractions
{
    public interface IWizardView
    {
        Task<bool> OnNext(BaseViewModel viewModel);
        Task<bool> OnPrevious(BaseViewModel viewModel);
        Task OnAppearing();
        Task OnDissapearing();
    }
}
