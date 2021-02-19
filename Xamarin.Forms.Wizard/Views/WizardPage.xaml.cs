using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Wizard.Abstractions;
using Xamarin.Forms.Wizard.ViewModels;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Wizard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardPage : ContentPage
    {
        private readonly WizardViewModel _viewModel;

        private WizardPage()
        {
            InitializeComponent();
            var swipeLeftGestureRecognizer = new SwipeGestureRecognizer();
            swipeLeftGestureRecognizer.Direction = SwipeDirection.Left;
            swipeLeftGestureRecognizer.Threshold = 40;
            swipeLeftGestureRecognizer.Swiped += (args, obj) => NextButton_Clicked(null, null);

            var swipeRightGestureRecognizer = new SwipeGestureRecognizer();
            swipeRightGestureRecognizer.Direction = SwipeDirection.Right;
            swipeRightGestureRecognizer.Threshold = 40;
            swipeRightGestureRecognizer.Swiped += (args, obj) => BackButton_Clicked(null, null);

            Content.GestureRecognizers.Add(swipeLeftGestureRecognizer);
            Content.GestureRecognizers.Add(swipeRightGestureRecognizer);
        }

        public WizardPage(IEnumerable<WizardItemViewModel> items, bool isAnimationEnabled = true, string nextLabelText = null,
                          string backLabelText = null, string finishLabelText = null, Color? progressBarColor = null) : this()
        {
            _viewModel = new WizardViewModel();
            BindingContext = _viewModel;
            _viewModel.IsAnimationEnabled = isAnimationEnabled;

            if (!String.IsNullOrEmpty(nextLabelText))
                _viewModel.NextButtonLabelText = nextLabelText;

            if (!String.IsNullOrEmpty(backLabelText))
                _viewModel.BackButtonLabelText = backLabelText;

            if (!String.IsNullOrEmpty(finishLabelText))
                _viewModel.FinishButtonLabelText = finishLabelText;

            //default color - green
            _viewModel.ProgressBarColor = progressBarColor ?? Color.Green;

            if (!items.Any())
                throw new ArgumentException("Provide items.", nameof(items));

            foreach (var item in items)
            {
                var args = new List<object>(1 + item.AdditionalParameters.Count());
                args.Add(item.ViewModel);
                args.AddRange(item.AdditionalParameters);

                if (!item.Type.IsSubclassOf(typeof(View)) && item.Type != typeof(View))
                    throw new ArgumentException(item.Type + " has to be derived from View", nameof(items));

                var view = Activator.CreateInstance(item.Type, args.ToArray()) as View;

                if (!(view is IWizardView))
                    throw new ArgumentException(item.Type + " must implement IWizardInterface", nameof(items));

                item.View = view;
            }

            _viewModel.Items = items.ToList();
            _viewModel.CurrentItem = _viewModel.Items[0];
            _viewModel.Title = _viewModel.Items[0].Title;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.DecreaseCurrentItemIndex();
            UpdateCurrentItem(false);
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.IncreateCurrentItemIndex();
            UpdateCurrentItem(true);
        }

        private async void UpdateCurrentItem(bool isNext)
        {
            if (_viewModel.IsAnimationEnabled)
                await StepContent.TranslateTo(isNext ? -1000 : 1000, StepContent.Y);

            _viewModel.CurrentItem = _viewModel.Items[_viewModel.GetCurrentItemIndex()];
            _viewModel.Title = _viewModel.Items[_viewModel.GetCurrentItemIndex()].Title;

            if (_viewModel.IsAnimationEnabled)
                await StepContent.TranslateTo(0, 0);
        }
    }
}