using Microsoft.Toolkit.Parsers.Rss;
using Prism.Windows.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace WinDevUtility.ViewModels
{
    public class BlogsViewModel : ViewModelBase
    {
        private ObservableCollection<RssSchema> _blogFeedList;
        public ObservableCollection<RssSchema> BlogFeedList
        {
            get { return _blogFeedList; }
            set
            {
                if (_blogFeedList != value)
                {
                    _blogFeedList = value;
                    RaisePropertyChanged();
                }
            }
        }
        private RssSchema _selectedBlog;

        public RssSchema SelectedBlog
        {
            get { return _selectedBlog; }
            set
            {
                _selectedBlog = value;
                RaisePropertyChanged();
            }
        }

        public BlogsViewModel()
        {
            _ = GetFeedsAsync();
        }

        private async Task GetFeedsAsync()
        {
            BlogFeedList = new ObservableCollection<RssSchema>(await ParseRSSAsync());
        }
        public async Task<IEnumerable<RssSchema>> ParseRSSAsync()
        {
            string feed = null;
            using (var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync("https://devblogs.microsoft.com/dotnet/feed/");
                    //feed = await client.GetStringAsync("https://visualstudiomagazine.com/rss-feeds/blogs.aspx");
                    //feed = await client.GetStringAsync("https://feed.infoq.com/");
                    //feed = await client.GetStringAsync("https://techcommunity.microsoft.com/plugins/custom/microsoft/o365/custom-blog-rss?tid=6653983339843425263&category=gxcuf89792&label=&blogArticles=&size=20&title=true");
                }
                catch { }
            }
            if (feed != null)
            {
                var parser = new RssParser();
                return parser.Parse(feed);
            }
            return null;
        }
    }
}
