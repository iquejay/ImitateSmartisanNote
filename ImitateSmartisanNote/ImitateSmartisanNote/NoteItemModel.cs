using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImitateSmartisanNote
{
    public class NoteItemModel : INotifyPropertyChanged
    {
        public NoteItemModel(DateTime dt, string note)
        {
            this.Note = note;
            this.DateAndTime = dt;
        }
        private string _note;
        private string _dateTip;
        private string _noteTitle;
        private bool _isStared;

        public string Identity;
        public string DateTip
        {
            get
            {
                return _dateTip;
            }

            set
            {
                if (_dateTip != value)
                {
                    _dateTip = value;
                    this.NotifyPropertyChanged(DateTip);
                }
            }
        }

        public bool IsStared
        {
            get {
                return _isStared;
            }
           set
            {
                if (_isStared != value)
                {
                    _isStared = value;
                    this.NotifyPropertyChanged("IsStared");
       
                }
            }
        }
        public string NoteTitle
        {
            get
            {
                return _noteTitle;
            }

            set
            {
                if (_noteTitle != value)
                {
                    _noteTitle = value;
                    this.NotifyPropertyChanged(NoteTitle);
                }
            }
        }
        private DateTime _dateTime;
        public DateTime DateAndTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                    UpdateDateTip();
                    NotifyPropertyChanged("DateTip");
                }
            }
        }
        private string Date
        {
            get
            {
                string extra ="";

                DateTime dt = DateAndTime;//DateTime.Parse(_date);
                
                DateTime now = DateTime.Now;

                if (now.Year == dt.Year)
                {
                    if (now.DayOfYear - dt.DayOfYear == 1)
                        extra = "昨天";
                    else if (now.DayOfYear - dt.DayOfYear == 0)
                        extra = "今天";
                    else if (now.DayOfYear - dt.DayOfYear < 5)
                    {
                        extra = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dt.DayOfWeek);
                    }
                    else
                        extra = dt.Month + "月" + dt.Day + "日";
                }
                else
                {
                    extra = dt.Year + "年"+dt.Month + "月" + dt.Day + "日";
                }
                return extra;
            }
        }

        private string Time
        {
            get
            {
                string extra="";
                //TimeSpan span = ;// TimeSpan.Parse(_time);
                if (DateAndTime.Hour > 18)
                    extra = "晚上";
                else if (DateAndTime.Hour > 12)
                    extra = "下午";
                else if (DateAndTime.Hour > 6)
                    extra = "上午";
                else
                    extra = "凌晨";
                return extra+DateAndTime.ToShortTimeString();
            }
        }

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                if (_note != value)
                {
                    _note = value;
                    NotifyPropertyChanged("Note");
                    UpdateNoteTitle();
                    NotifyPropertyChanged("NoteTitle");
                }
            }
        }

        private void UpdateDateTip()
        {
            this.DateTip = this.MakeDateTip();
        }

        private string MakeDateTip()
        {
            return string.Format("{0} {1}", this.Date , this.Time);
        }

        private void UpdateNoteTitle()
        {
            this.NoteTitle = MakeNoteTitle();
        }

        private string MakeNoteTitle()
        {
            string note = this.Note.Trim();
            int index = note.IndexOf('\n');
            return note.Substring(0, index == -1? note.Length:index);
        }

        private void NotifyPropertyChanged(string strPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
