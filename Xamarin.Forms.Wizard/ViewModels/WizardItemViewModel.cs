using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public WizardItemViewModel(string title, Type type, BaseViewModel viewModel, bool isSkippable = false, Func<Task> customButtonAction = null, string customButtonLabel = null, params object[] additionalParameters) : this()
        {
            Title = title;
            Type = type;
            ViewModel = viewModel ?? new BaseViewModel();
            AdditionalParameters = additionalParameters ?? new object[0];
            IsSkippable = isSkippable;
            CustomButtonAction = customButtonAction;
            CustomButtonLabel = customButtonLabel;
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

        private bool _isSkippable;
        public bool IsSkippable
        {
            get { return _isSkippable; }
            set { SetProperty(ref _isSkippable, value); }
        }

        private Func<Task> _customButtonAction;
        public Func<Task> CustomButtonAction
        {
            get { return _customButtonAction; }
            set { SetProperty(ref _customButtonAction, value); }
        }

        private string _customButtonLabel;
        public string CustomButtonLabel
        {
            get { return _customButtonLabel; }
            set { SetProperty(ref _customButtonLabel, value); }
        }
    }
}
