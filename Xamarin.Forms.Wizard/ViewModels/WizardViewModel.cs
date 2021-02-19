using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Wizard.Abstractions;

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
        }

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
        public bool IsAnimationEnabled { get; set; }

        public async Task IncreateCurrentItemIndex()
        {
            var newIndex = _currentItemIndex + 1;

            if (newIndex > 0)
                IsNotFirstItem = true;

            if (newIndex == Items.Count() - 1)
            {
                IsLastItem = true;
                NextButtonLabel = FinishButtonLabelText;
            }

            var item = Items[_currentItemIndex].View as IWizardView;
            var result = await item.OnNext();
            if (!result)
                return;

            if (newIndex > Items.Count() - 1)
                return;

            _currentItemIndex = newIndex;
            ProgressBarProgress = Math.Truncate(10 * (double)(_currentItemIndex + 1) / (Items.Count == 0 ? 1 : Items.Count)) / 10;
        }

        public async Task DecreaseCurrentItemIndex()
        {
            var newIndex = _currentItemIndex - 1;
            if (newIndex < 0)
                return;

            if (newIndex == 0)
                IsNotFirstItem = false;

            var item = Items[_currentItemIndex].View as IWizardView;
            var result = await item.OnPrevious();
            if (!result)
                return;

            if (newIndex != Items.Count() - 1)
            {
                IsLastItem = false;
                NextButtonLabel = NextButtonLabelText;
            }

            _currentItemIndex = newIndex;
            ProgressBarProgress = Math.Truncate(10 * (double)(_currentItemIndex + 1) / (Items.Count == 0 ? 1 : Items.Count)) / 10;
        }

        public int GetCurrentItemIndex()
        {
            return _currentItemIndex;
        }

    }
}
