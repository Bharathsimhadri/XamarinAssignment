using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace XFTest.Models
{
    public class SubCalanderContract: BindableBase
    { 
        public int Date { get;set;}
        public string Day { get; set; }
        public int MonthId { get; set; }
        public string MonthName { get; set; }

        bool _isToHighlightDate;
        public bool IsToHighliteDate { get=>_isToHighlightDate; set { _isToHighlightDate = value; SetProperty(ref _isToHighlightDate, value); } }

        public List<SubCalanderContract> Month { get; set; }
        public SubCalanderContract(int date,string day)
        {
            Date = date;
            Day = day;
        }
        public SubCalanderContract(int monthId, List<SubCalanderContract> _month,string _monthName)
        {
            MonthId = monthId;
            Month = _month;
            MonthName = _monthName;
        }
    }

    public class CalanderContract
    {

    }
}
