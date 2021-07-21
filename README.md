# Xamarin.Forms.Wizard
Simple Xamarin Forms wizard control

# Nuget package
Available as nuget package at https://www.nuget.org/packages/Xamarin.Forms.Wizard/
Install it in shared code project.

# Usage

- Create wizard item view models for each step in the wizard
  - Create content view which inherits from `IWizardView` interface. Define the content for that step in the content view and implement `OnNext` and `OnPrevious` steps where you can performs validations, stop the wizard from moving onto the next step by returning `false` and etc.
  - Create view model which inherits from `BaseViewModel` view model and is used in the earlier created content view
- Produce a list of `WizarditemViewModel` items containing earlier created (content view, view model) pair. Example: `var wizardPageItem1 = new WizardItemViewModel("Header for the section", typeof(WizardStep1), new WizardStep1ViewModel())` where `WizardStep1` is the content view and `WizardStep1ViewModel` is the view model for that content view
- Create `WizardContentView` with parameters and use it as any other content view. Example: `Content = new WizardContentView(new List<WizardItemViewModel>() { wizardPage1Item, wizardPage2Item, wizardPage3Item }, false)`. In the parameters of this page you can provide more parameters to customize animations, progress bar color, button labels. 

## Usage Example
```csharp
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
```

```csharp
public class WizardStep1ViewModel : Xamarin.Forms.Wizard.ViewModels.BaseViewModel
{
    public WizardStep1ViewModel()
    {
    }
}
```

```csharp
public partial class WizardHost : ContentPage
{
    public WizardHost()
    {

        var wizardPageItem1 = new WizardItemViewModel("Step 1 Title", typeof(Step1Page), new WizardStep1ViewModel());
        var wizardPageItem2 = new WizardItemViewModel("Step 2 Title", typeof(Step2Page), new WizardStep2ViewModel());
        var wizardPageItem3 = new WizardItemViewModel("Step 3 Title", typeof(Step3Page), new WizardStep3ViewModel());

        Content = new WizardContentView(new List<WizardItemViewModel>() { wizardPageItem1, wizardPageItem2, wizardPageItem3 });
        InitializeComponent();
    }
}
```

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="wiztest.Views.Wizard.WizardHost">
</ContentPage>
```

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="wiztest.Views.Wizard.Step1Page">
    <ContentView.Content>
        <StackLayout>
            <Label Text="This steps content goes here" />
        </StackLayout>
    </ContentView.Content>
</ContentView>
```


# Contributions

Any contributions are welcome in the form of pull requests.

# Issues

Issues can be raised in the `Issue` section where I'll try to address all of them.
