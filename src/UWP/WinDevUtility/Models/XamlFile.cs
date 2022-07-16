using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WinDevUtility.Models
{
    public class XamlFile : BindableBase
    {
        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string FileName
        {
            get
            {
                var parts = path.Split('\\');
                return parts.LastOrDefault();
            }
        }

        public string XAML { get; set; }
        public int Count { get; set; }
    }
    public class GroupInfoCollection<T> : ObservableCollection<T>
    {
        public object Key { get; set; }

        public new IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)base.GetEnumerator();
        }
    }
}
