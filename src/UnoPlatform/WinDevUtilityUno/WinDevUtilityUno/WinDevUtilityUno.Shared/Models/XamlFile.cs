using BasicMvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WinDevUtilityUno.Models
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
                    OnPropertyChanged();
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
    public class DataResource : BindableBase
    {
        private string key;

        public string Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    OnPropertyChanged();
                }
            }
        }

        public XamlFile DefinedInXamlFile { get; set; }
        public List<XamlFile> UsedInXamlFiles { get; set; }

        public string Occurrences
        {
            get
            {
                var o = string.Empty;
                foreach (var f in UsedInXamlFiles)
                {
                    o += f.FileName + " ";
                }
                return o;
            }
        }
    }
}
