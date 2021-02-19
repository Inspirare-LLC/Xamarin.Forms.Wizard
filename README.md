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
- Create `WizardPage` with parameters and use it as any other content page. Example: `App.Current.MainPage = new WizardPage(new List<WizardItemViewModel>() { wizardPage1Item, wizardPage2Item, wizardPage3Item }, false)`. In the parameters of this page you can provide more parameters to customize animations, progress bar color, button labels. 

# Contributions

Any contributions are welcome in the form of pull requests.

# Issues

Issues can be raised in the `Issue` section where I'll try to address all of them.
