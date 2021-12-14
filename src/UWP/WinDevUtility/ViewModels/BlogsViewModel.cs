using AsyncCommands;
using Microsoft.Toolkit.Parsers.Rss;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDevUtility.Constants;
using WinDevUtility.Core.Services;
using WinDevUtility.Helpers;
using WinDevUtility.Models;
using Windows.Storage;
using Windows.UI.Xaml;

namespace WinDevUtility.ViewModels
{
    public class BlogsViewModel : ViewModelBase
    {
        private IIdentityService _identityService;
        private List<RssSchema> _blogFeedList;
        private RssSchema _selectedBlog;
        private ObservableCollection<BlogFeed> _blogList;
        private BlogFeed _selectOption;
        private Visibility _isRelatedContentVisible;
        public ICommand CheckedCommand => new AsyncCommand<RssSchema>(OnCheckedCommandExecutedAsync);
        public ICommand UncheckedCommand => new AsyncCommand<RssSchema>(OnUncheckedCommandExecutedAsync);
        bool isRefreshed = false;
        bool isUpdated = false;


        public BlogsViewModel(IIdentityService identityService)
        {
            _identityService = identityService;
            IsRelatedContentVisible = Visibility.Collapsed;
            _ = GetFeedsAsync();
        }

        public Visibility IsRelatedContentVisible
        {
            get { return _isRelatedContentVisible; }
            set
            {
                if (_isRelatedContentVisible != value)
                {
                    _isRelatedContentVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        public BlogFeed SelectOption
        {
            get { return _selectOption; }
            set
            {
                if (_selectOption != value)
                {
                    _selectOption = value;
                    if (value != null)
                    {

                        UpdateBlogLists();
                        _ = SetBlogAsync(value);
                    }
                    RaisePropertyChanged();
                }
            }
        }

        private async Task SetBlogAsync(BlogFeed value)
        {
            if (nameof(BlogType.Favourite).Equals(value.Type))
            {
                if (!_identityService.IsLoggedIn())
                {
                    await _identityService.LoginAsync();
                }
                SelectedBlog = Globals.FavList.FirstOrDefault();
            }
            else
            {
                SelectedBlog = BlogFeedList.FirstOrDefault();
            }
        }

        public ObservableCollection<BlogFeed> BlogList
        {
            get { return _blogList; }
            set
            {
                _blogList = value;
                RaisePropertyChanged();
            }
        }
        public RssSchema SelectedBlog
        {
            get { return _selectedBlog; }
            set
            {
                _selectedBlog = value;
                if (value != null)
                {
                    IsRelatedContentVisible = Visibility.Visible;
                }
                else
                {
                    IsRelatedContentVisible = Visibility.Collapsed;
                }
                RaisePropertyChanged();
            }
        }
        public List<RssSchema> BlogFeedList
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
        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            _ = SettingsStorageExtensions.SaveSettingAsync(PageTokens.BlogsPage, nameof(PageTokens));
        }
        private async Task GetFavListAsync()
        {
            Globals.FavList = await ApplicationData.Current.LocalFolder.ReadAsync<List<RssSchema>>("fav.json");

        }
        private async Task GetFeedsAsync()
        {
            await Task.WhenAll(GetLatestBlogsAsync(), GetFavListAsync());
            BlogList = new ObservableCollection<BlogFeed>();
            BlogList.Add(new BlogFeed
            {
                Blog = new ObservableCollection<RssSchema>(BlogFeedList),
                Type = nameof(BlogType.Latest),
            });
            BlogList.Add(new BlogFeed
            {
                Blog = Globals.FavList != null ? new ObservableCollection<RssSchema>(Globals.FavList) : null,
                Type = nameof(BlogType.Favourite),
            });
            SelectedBlog = BlogFeedList.FirstOrDefault();
        }

        private async Task GetLatestBlogsAsync()
        {
            BlogFeedList = await ParseRSSAsync();
        }

        public async Task<List<RssSchema>> ParseRSSAsync()
        {
            string feed = null;
            string feed2 = null;
            var feedlist = new List<RssSchema>();
            using (var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync("https://devblogs.microsoft.com/dotnet/feed/");
                    //feed = await client.GetStringAsync("https://visualstudiomagazine.com/rss-feeds/blogs.aspx");
                    feed2 = await client.GetStringAsync("https://feed.infoq.com/");
                    //feed2 = await client.GetStringAsync("https://techcommunity.microsoft.com/plugins/custom/microsoft/o365/custom-blog-rss?tid=6653983339843425263&category=gxcuf89792&label=&blogArticles=&size=20&title=true");
                }
                catch { }
            }
            var parser = new RssParser();
            if (feed != null)
            {
                feedlist.AddRange(parser.Parse(feed));
            }
            if (feed2 != null)
            {
                feedlist.AddRange(parser.Parse(feed2));
            }
            return feedlist.OrderByDescending(x => x.PublishDate).ToList();
        }
        private async Task OnCheckedCommandExecutedAsync(RssSchema obj)
        {
            if (obj == null)
            {
                return;
            }
            if (!_identityService.IsLoggedIn())
            {
                var result = await _identityService.LoginAsync();
                if (result == Core.Helpers.LoginResultType.Success)
                {
                    UpdateFavlist(obj);
                }
            }
            else
            {
                UpdateFavlist(obj);
            }
        }

        private void UpdateFavlist(RssSchema obj)
        {
            if (Globals.FavList == null)
            {
                Globals.FavList = new List<RssSchema>();
                Globals.FavList.Add(obj);
                isRefreshed = true;
                isUpdated = true;
            }
            else
            {
                if (!Globals.FavList.Any(x => x.InternalID == obj.InternalID))
                {
                    Globals.FavList.Add(obj);
                    isRefreshed = true;
                    isUpdated = true;
                }
            }
        }

        private void UpdateBlogLists()
        {
            if (isRefreshed && BlogList?.Count > 0)
            {
                isRefreshed = false;
                BlogList.FirstOrDefault(x => x.Type == nameof(BlogType.Latest)).Blog = new ObservableCollection<RssSchema>(BlogFeedList);
                BlogList.FirstOrDefault(x => x.Type == nameof(BlogType.Favourite)).Blog = new ObservableCollection<RssSchema>(Globals.FavList);
            }
        }

        private async Task OnUncheckedCommandExecutedAsync(RssSchema obj)
        {
            if (obj == null && Globals.FavList == null)
            {
                return;
            }
            else
            {
                if (Globals.FavList.Any(x => x.InternalID == obj.InternalID))
                {
                    Globals.FavList.Remove(obj);
                    isRefreshed = true;
                    isUpdated = true;
                }
            }
            await SaveFavListAsync();
        }

        private async Task SaveFavListAsync()
        {
            if (isUpdated && _identityService.IsLoggedIn())
            {
                await ApplicationData.Current.LocalFolder.SaveAsync("fav.json", Globals.FavList);
                isUpdated = false;
            }
        }
    }
}
