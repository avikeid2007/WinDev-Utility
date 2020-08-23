namespace WinDevUtility
{
    public class ListofListTable
    {
        private System.Guid _id;
        private string _text;
        private int? _rowCount;
        private string _remarks;
        private byte[] _rowVersion;
        private string _spare1;
        private string _spare2;
        private string _spare3;
        private string _spare4;
        private string _spare5;
        private System.DateTimeOffset? _syncDate;
        private bool _isActive;
        private bool _canDownload;

        public System.Guid Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;

                }
            }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;

                }
            }
        }
        public int? RowCount
        {
            get { return _rowCount; }
            set
            {
                if (_rowCount != value)
                {
                    _rowCount = value;

                }
            }
        }
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;

                }
            }
        }
        public byte[] RowVersion
        {
            get { return _rowVersion; }
            set
            {
                if (_rowVersion != value)
                {
                    _rowVersion = value;

                }
            }
        }
        public string Spare1
        {
            get { return _spare1; }
            set
            {
                if (_spare1 != value)
                {
                    _spare1 = value;

                }
            }
        }
        public string Spare2
        {
            get { return _spare2; }
            set
            {
                if (_spare2 != value)
                {
                    _spare2 = value;

                }
            }
        }
        public string Spare3
        {
            get { return _spare3; }
            set
            {
                if (_spare3 != value)
                {
                    _spare3 = value;

                }
            }
        }
        public string Spare4
        {
            get { return _spare4; }
            set
            {
                if (_spare4 != value)
                {
                    _spare4 = value;

                }
            }
        }
        public string Spare5
        {
            get { return _spare5; }
            set
            {
                if (_spare5 != value)
                {
                    _spare5 = value;

                }
            }
        }
        public System.DateTimeOffset? SyncDate
        {
            get { return _syncDate; }
            set
            {
                if (_syncDate != value)
                {
                    _syncDate = value;
                }
            }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                }
            }
        }
        public bool CanDownload
        {
            get { return _canDownload; }
            set
            {
                if (_canDownload != value)
                {
                    _canDownload = value;

                }
            }
        }

    }

}
