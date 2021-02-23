using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Wizard.Abstractions;
using Xamarin.Forms.Wizard.EventHandlers;

namespace Xamarin.Forms.Wizard.ViewModels
{
    /// <summary>
    /// Main implementation of wizard view model
    /// </summary>
    public class WizardViewModel : BaseViewModel
    {
        public WizardViewModel()
        {
            NextButtonLabelText = NextButtonLabel = "Next";
            BackButtonLabelText = BackButtonLabel = "Back";
            FinishButtonLabelText = "Finish";
            SkipButtonLabelText = SkipButtonLabel = "Skip";
        }

        public event EventHandler<WizardFinishedEventArgs> OnFinished;

        private List<WizardItemViewModel> _items;
        public List<WizardItemViewModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private WizardItemViewModel _currentItem;
        public WizardItemViewModel CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }

        private bool _isLastItem;
        public bool IsLastItem
        {
            get { return _isLastItem; }
            private set { SetProperty(ref _isLastItem, value); }
        }

        private bool _isNotFirstItem;
        public bool IsNotFirstItem
        {
            get { return _isNotFirstItem; }
            private set { SetProperty(ref _isNotFirstItem, value); }
        }

        private bool _isSkippable;
        public bool IsSkippable
        {
            get { return _isSkippable; }
            set { SetProperty(ref _isSkippable, value); }
        }

        private string _backButtonLabel;
        public string BackButtonLabel
        {
            get { return _backButtonLabel; }
            set { SetProperty(ref _backButtonLabel, value); }
        }

        private string _nextButtonLabel;
        public string NextButtonLabel
        {
            get { return _nextButtonLabel; }
            set { SetProperty(ref _nextButtonLabel, value); }
        }

        private string _skipButtonLabel;
        public string SkipButtonLabel
        {
            get { return _skipButtonLabel; }
            set { SetProperty(ref _skipButtonLabel, value); }
        }

        private double _progressBarProgress;
        public double ProgressBarProgress
        {
            get { return _progressBarProgress; }
            set { SetProperty(ref _progressBarProgress, value); }
        }

        private Color _progressBarColor;
        public Color ProgressBarColor
        {
            get { return _progressBarColor; }
            set { SetProperty(ref _progressBarColor, value); }
        }

        private int _currentItemIndex;

        public string NextButtonLabelText { get; set; }
        public string BackButtonLabelText { get; set; }
        public string FinishButtonLabelText { get; set; }
        public string SkipButtonLabelText { get; set; }
        public bool IsAnimationEnabled { get; set; }

        public async Task<bool> IncreaseCurrentItemIndex(bool skip = false)
        {
            var newIndex = _currentItemIndex + 1;

            var item = Items[_currentItemIndex].View as IWizardView;
            var itemViewModel = Items[_currentItemIndex].ViewModel;

            //if skip, don't call on next
            if (!skip)
            {
                var result = await item.OnNext(itemViewModel);
                if (!result)
                    return false;
            }

            //if finished, short circuit and exit
            if (newIndex == Items.Count)
            {
                OnFinished?.Invoke(null, new WizardFinishedEventArgs(Items));
                return true;
            }

            if (newIndex > Items.Count() - 1)
                return false;

            if (newIndex > 0)
                IsNotFirstItem = true;

            if (newIndex == Items.Count() - 1)
            {
                IsLastItem = true;
                NextButtonLabel = FinishButtonLabelText;
            }

            await item.OnDissapearing();
            _currentItemIndex = newIndex;
            ProgressBarProgress = Math.Truncate(10 * (double)(_currentItemIndex + 1) / (Items.Count == 0 ? 1 : Items.Count)) / 10;
            return true;
        }

        public async Task<bool> DecreaseCurrentItemIndex()
        {
            var newIndex = _currentItemIndex - 1;
            if (newIndex < 0)
                return false;

            var item = Items[_currentItemIndex].View as IWizardView;
            var itemViewModel = Items[_currentItemIndex].ViewModel;

            var result = await item.OnPrevious(itemViewModel);
            if (!result)
                return false;

            if (newIndex == 0)
                IsNotFirstItem = false;

            if (newIndex != Items.Count() - 1)
            {
                IsLastItem = false;
                NextButtonLabel = NextButtonLabelText;
            }

            await item.OnDissapearing();
            _currentItemIndex = newIndex;
            ProgressBarProgress = Math.Truncate(10 * (double)(_currentItemIndex + 1) / (Items.Count == 0 ? 1 : Items.Count)) / 10;
            return true;
        }

        public int GetCurrentItemIndex()
        {
            return _currentItemIndex;
        }

    }
}
