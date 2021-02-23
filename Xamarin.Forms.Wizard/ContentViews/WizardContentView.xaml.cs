using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Wizard.Abstractions;
using Xamarin.Forms.Wizard.EventHandlers;
using Xamarin.Forms.Wizard.ViewModels;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Wizard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardContentView : ContentView
    {
        private readonly WizardViewModel _viewModel;
        public event EventHandler<WizardFinishedEventArgs> OnFinished
        {
            add
            {
                _viewModel.OnFinished += value;
            }

            remove
            {
                _viewModel.OnFinished -= value;
            }
        }

        private WizardContentView()
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

        public WizardContentView(IEnumerable<WizardItemViewModel> items, bool isAnimationEnabled = true, string nextLabelText = null,
                          string backLabelText = null, string finishLabelText = null, string skipLabelText = null,
                          Color? progressBarColor = null) : this()
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

            if (!String.IsNullOrEmpty(skipLabelText))
                _viewModel.SkipButtonLabelText = skipLabelText;

            //default color - green
            _viewModel.ProgressBarColor = progressBarColor ?? Color.Green;

            if (!items.Any())
                throw new ArgumentException("Provide items.", nameof(items));

            _viewModel.Items = items.ToList();

            var item = _viewModel.Items[0];
            var args = new List<object>(1 + item.AdditionalParameters.Count());
            args.Add(item.ViewModel);
            args.AddRange(item.AdditionalParameters);

            if (!item.Type.IsSubclassOf(typeof(View)) && item.Type != typeof(View))
                throw new ArgumentException(item.Type + " has to be derived from View", nameof(items));

            var view = Activator.CreateInstance(item.Type, args.ToArray()) as View;

            if (!(view is IWizardView))
                throw new ArgumentException(item.Type + " must implement IWizardView interface", nameof(items));

            item.View = view;

            _viewModel.CurrentItem = item;
            _viewModel.Title = _viewModel.Items[0].Title;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            var currentItem = _viewModel.Items[_viewModel.GetCurrentItemIndex()].ViewModel;

            var result = await _viewModel.DecreaseCurrentItemIndex();
            if (result)
                UpdateCurrentItem(false, currentItem);
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            var currentItem = _viewModel.Items[_viewModel.GetCurrentItemIndex()].ViewModel;

            var result = await _viewModel.IncreaseCurrentItemIndex();
            if (result)
                await UpdateCurrentItem(true, currentItem);
        }

        private async void SkipButton_Clicked(object sender, EventArgs e)
        {
            var currentItem = _viewModel.Items[_viewModel.GetCurrentItemIndex()].ViewModel;

            var result = await _viewModel.IncreaseCurrentItemIndex(true);
            if (result)
                await UpdateCurrentItem(true, currentItem);
        }

        private async Task UpdateCurrentItem(bool isNext, BaseViewModel previousViewModel = null)
        {
            var item = _viewModel.Items[_viewModel.GetCurrentItemIndex()];
            var args = new List<object>(1 + (previousViewModel != null ? 1 : 0) + item.AdditionalParameters.Count());
            args.Add(item.ViewModel);
            if (previousViewModel != null)
                args.Add(previousViewModel);

            args.AddRange(item.AdditionalParameters);

            if (!item.Type.IsSubclassOf(typeof(View)) && item.Type != typeof(View))
                throw new ArgumentException(item.Type + " has to be derived from View", nameof(item));

            var view = Activator.CreateInstance(item.Type, args.ToArray()) as View;

            if (!(view is IWizardView))
                throw new ArgumentException(item.Type + " must implement IWizardView interface", nameof(item));

            item.View = view;

            if (_viewModel.IsAnimationEnabled)
                await StepContent.TranslateTo(isNext ? -1000 : 1000, StepContent.Y);

            _viewModel.CurrentItem = item;
            _viewModel.Title = item.Title;
            _viewModel.IsSkippable = item.IsSkippable;

            if (_viewModel.IsAnimationEnabled)
                await StepContent.TranslateTo(0, 0);

            await (_viewModel.CurrentItem.View as IWizardView)?.OnAppearing();
        }
    }
}