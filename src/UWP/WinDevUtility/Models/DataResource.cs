using Prism.Mvvm;
using System.Collections.Generic;

namespace WinDevUtility.Models
{
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
                    RaisePropertyChanged();
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
