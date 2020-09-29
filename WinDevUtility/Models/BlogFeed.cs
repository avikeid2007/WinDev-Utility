
using Microsoft.Toolkit.Parsers.Rss;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace WinDevUtility.Models
{
    public class BlogFeed : BindableBase
    {
        private ObservableCollection<RssSchema> _blog;
        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<RssSchema> Blog
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
    }
}
