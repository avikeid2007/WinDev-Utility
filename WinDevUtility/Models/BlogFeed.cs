
using Microsoft.Toolkit.Parsers.Rss;
using Prism.Commands;
using Prism.Mvvm;
using Windows.UI.Xaml;

namespace WinDevUtility.Models
{
    public class BlogFeed : BindableBase
    {
        private RssSchema _blog;
        private Visibility _isSummaryVisible;
        private Visibility _isContantVisible;
        public RssSchema Blog
        {
            get { return _blog; }
            set
            {
                if (_blog != value)
                {
                    _blog = value;
                    RaisePropertyChanged();
                }
            }
        }
        public Visibility IsSummaryVisible
        {
            get { return _isSummaryVisible; }
            set
            {
                if (_isSummaryVisible != value)
                {
                    _isSummaryVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public Visibility IsContantVisible
        {
            get { return _isContantVisible; }
            set
            {
                if (_isContantVisible != value)
                {
                    _isContantVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public DelegateCommand ShowMoreCommand => new DelegateCommand(OnShowMoreCommandExecuted);
        private void OnShowMoreCommandExecuted()
        {
            if (IsSummaryVisible == Visibility.Visible)
            {
                IsSummaryVisible = Visibility.Collapsed;
                IsContantVisible = Visibility.Visible;
            }
            else
            {
                IsSummaryVisible = Visibility.Visible;
                IsContantVisible = Visibility.Collapsed;
            }
        }



    }
}
