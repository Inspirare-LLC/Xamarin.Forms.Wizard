using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Wizard.ViewModels
{
    /// <summary>
    /// Base class for wizard item view model
    /// </summary>
    public class WizardItemViewModel : BaseViewModel
    {
        private WizardItemViewModel()
        {
            AdditionalParameters = new List<object>();
            ViewModel = new BaseViewModel();
        }

        public WizardItemViewModel(string title, Type type, BaseViewModel viewModel, params object[] additionalParameters) : this()
        {
            Title = title;
            Type = type;
            ViewModel = viewModel ?? new BaseViewModel();
            AdditionalParameters = additionalParameters ?? new object[0];
        }

        private Type _type;
        public Type Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private BaseViewModel _viewModel;
        public BaseViewModel ViewModel
        {
            get { return _viewModel; }
            set { SetProperty(ref _viewModel, value); }
        }

        private IEnumerable<object> _additionalParameters;
        public IEnumerable<object> AdditionalParameters
        {
            get { return _additionalParameters; }
            set { SetProperty(ref _additionalParameters, value); }
        }

        private View _view;
        public View View
        {
            get { return _view; }
            set { SetProperty(ref _view, value); }
        }
    }
}
