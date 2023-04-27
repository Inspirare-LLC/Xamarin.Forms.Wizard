using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Wizard.Abstractions;
using Xamarin.Forms.Wizard.ViewModels;

namespace Demo.Core.Views
{
    public partial class Step1Page : ContentView, IWizardView
    {
        public Step1Page(BaseViewModel currentViewModel, BaseViewModel PreviousViewModel)
        {
            InitializeComponent();
        }

        public Step1Page(BaseViewModel currentViewModel)
        {
            InitializeComponent();
        }

        public async Task OnDissapearing()
        {
        }

        public async Task<bool> OnNext(BaseViewModel viewModel)
        {
            //perform validation here
            return true;
        }

        public async Task<bool> OnPrevious(BaseViewModel viewModel)
        {
            //perform validation here
            return true;
        }

        async Task IWizardView.OnAppearing()
        {
        }
    }
}