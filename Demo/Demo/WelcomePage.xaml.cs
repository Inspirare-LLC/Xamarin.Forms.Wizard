using Demo.Core.ViewModels;
using Demo.Core.Views;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Wizard.ViewModels;
using Xamarin.Forms.Wizard.Views;

namespace Demo.Core
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            var wizardPageItem1 = new WizardItemViewModel("Step 1 Title", typeof(Step1Page), new WizardStep1ViewModel());
            var wizardPageItem2 = new WizardItemViewModel("Step 2 Title", typeof(Step2Page), new WizardStep2ViewModel());
            var wizardPageItem3 = new WizardItemViewModel("Step 3 Title", typeof(Step3Page), new WizardStep3ViewModel());

            Content = new WizardContentView(new List<WizardItemViewModel>() { wizardPageItem1, wizardPageItem2, wizardPageItem3 });

            InitializeComponent();
        }
    }
}
