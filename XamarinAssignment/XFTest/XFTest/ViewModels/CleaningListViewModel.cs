using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using XFTest.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Xamarin.Forms;
using XFTest.Services;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Linq;
using XFTest.AppStrings;
using Xamarin.Forms.Internals;

namespace XFTest.ViewModels
{
	public class CleaningListViewModel : BindableBase, INotifyPropertyChanged
    {
        #region Properties and commands
        ICarFitServices _carFitServices;
        SubCalanderContract selectedMonthData;
        public SubCalanderContract choosenDateDate;

        public event PropertyChangedEventHandler PropertyChanged;

        bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; NotifyPropertyChanged("IsBusy"); } }

        bool _isrefreshing;
        public bool IsRefreshing { get => _isrefreshing; set { _isrefreshing = value; NotifyPropertyChanged("IsRefreshing");  } }

        bool _isToShowCalander;
        public bool IsToShowCalander { get { return _isToShowCalander; } set { _isToShowCalander = value;NotifyPropertyChanged("IsToShowCalander"); } }

        string _dateLabel;
        public string DateLabel { get { return _dateLabel; } set { _dateLabel = value;NotifyPropertyChanged(nameof(DateLabel)); } }

        string _selectedDateLabel;
        public string SelectedDateLabel { get => _selectedDateLabel; set { _selectedDateLabel = value;NotifyPropertyChanged("SelectedDateLabel"); } }

        ObservableCollection<CarWashDataContract> _carWashList;
        public ObservableCollection<CarWashDataContract> CarWashList 
        { 
            get { return _carWashList; }
            set { _carWashList = value;NotifyPropertyChanged("CarWashList"); }
        }

        ObservableCollection<SubCalanderContract> calanderBackUp;

        ObservableCollection<SubCalanderContract> _calanderData;
        public ObservableCollection<SubCalanderContract> CalanderData 
        {
            get { return _calanderData; }
            set { _calanderData = value;NotifyPropertyChanged("CalanderData"); }
        }

        public ICommand ToogleCalanderCommand { get { return new Command(() => { IsToShowCalander = !IsToShowCalander; }); } }
        public ICommand PreviousMonthCommand { get { return new Command(() => { PreviousMonthClicked(); }); } }
        public ICommand NextMonthCommand { get { return new Command(() => { NextMonthClicked(); }); } }
        public ICommand RefreshCommand { get { return new Command(() => { RefreshData(); }); } }
        public ICommand CloseCalander { get { return new Command(() => { IsToShowCalander = false; }); } }
        #endregion

        #region Constructore
        public CleaningListViewModel( IDialogService dialogService, INavigationService navigationService
            , ICarFitServices carFitServices)
        {
            _carFitServices = carFitServices;
            CarWashList = new ObservableCollection<CarWashDataContract>();
            PopulateCalanderData();
            GetCarWashList();
        }
        #endregion

        #region Tasks and methods
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
        void PopulateCalanderData()
        {
            var jan = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };
            var feb = new List<SubCalanderContract>
            {
               new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday)
            };
            var mar = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };
            var april = new List<SubCalanderContract>
            {
                 new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
            };
            var may = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };
            var june = new List<SubCalanderContract>
            {
               new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
            };
            var July = new List<SubCalanderContract>
            {
              new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };
            var August = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Tuesday),
            };
            var Sep = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
            };
            var oct = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };
            var nov = new List<SubCalanderContract>
            {
                 new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
            };
            var dec = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday),new SubCalanderContract(5,Appstrings.Label_Short_Friday),new SubCalanderContract(6,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday),new SubCalanderContract(8,Appstrings.Label_Short_Monday),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday),new SubCalanderContract(11,Appstrings.Label_Short_Thursday),new SubCalanderContract(12,Appstrings.Label_Short_Friday),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday),new SubCalanderContract(14,Appstrings.Label_Short_Sunday),new SubCalanderContract(15,Appstrings.Label_Short_Monday),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday),new SubCalanderContract(18,Appstrings.Label_Short_Thursday),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday),new SubCalanderContract(20,Appstrings.Label_Short_Saturday),new SubCalanderContract(21,Appstrings.Label_Short_Sunday),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday),new SubCalanderContract(26,Appstrings.Label_Short_Friday),new SubCalanderContract(27,Appstrings.Label_Short_Saturday),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday),new SubCalanderContract(29,Appstrings.Label_Short_Monday),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday),
            };

            calanderBackUp = new ObservableCollection<SubCalanderContract>
            {
                new SubCalanderContract(1,jan,Appstrings.Label_January),
                new SubCalanderContract(2,feb,Appstrings.Label_Febuary),
                new SubCalanderContract(3,mar,Appstrings.Label_March),
                new SubCalanderContract(4,april,Appstrings.Label_April),
                new SubCalanderContract(5,may,Appstrings.Label_May),
                new SubCalanderContract(6,june,Appstrings.Label_June),
                new SubCalanderContract(7,July,Appstrings.Label_July),
                new SubCalanderContract(8,August,Appstrings.Label_August),
                new SubCalanderContract(9,Sep,Appstrings.Label_September),
                new SubCalanderContract(10,oct,Appstrings.Label_October),
                new SubCalanderContract(11,nov,Appstrings.Label_November),
                new SubCalanderContract(12,dec,Appstrings.Label_December),
            };
            var month = System.DateTime.Now.Month;
            DateLabel = System.DateTime.Now.ToString(GlobalHelpers.GlobalConstants.DateFormat);
            selectedMonthData = calanderBackUp[month - 1];
            SelectedDateLabel = Appstrings.Label_Today;
            CalanderData =new ObservableCollection<SubCalanderContract>(calanderBackUp[month - 1].Month);
        }

        private void NextMonthClicked()
        {
            if (selectedMonthData?.MonthId == 12)
            {
                selectedMonthData = calanderBackUp[0];
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} {Appstrings.Label_Year2020}";
            }
            else
            {
                var monthId = selectedMonthData.MonthId + 1;
                selectedMonthData = calanderBackUp.FirstOrDefault(CB => CB.MonthId == monthId);
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} {Appstrings.Label_Year2020}";
            }
        }

        private void PreviousMonthClicked()
        {
            if (selectedMonthData?.MonthId == 1)
            {
                selectedMonthData = calanderBackUp[11];
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} {Appstrings.Label_Year2020}";
            }
            else
            {
                var monthId = selectedMonthData.MonthId - 1;
                selectedMonthData = calanderBackUp.FirstOrDefault(CB => CB.MonthId == monthId);
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} {Appstrings.Label_Year2020}";
            }
        }

        void RefreshData()
        {
            IsRefreshing = true;
            GetCarWashList();
        }

        public async void GetCarWashList()
        {
            CarWashList.Clear();
            IsBusy = true;
            IsRefreshing = false;
            try
            {
                var result = await _carFitServices.GetAllCarServices();
                if (result.Success)
                {

                    result.Data.ForEach(D =>
                    {
                        D.Tasks.ForEach(T =>
                        {
                            D.DisplayTask += T.Title;
                            D.ToatalTimeForTasks += T.TimesInMinutes;
                        });
                        D.Location = (D.HouseOwnerLatitude + D.HouseOwnerLongitude);
                        CarWashList.Add(D);
                    });

                    //For finding distance from next service 
                    var carWashIndex = 1;
                    CarWashList.ForEach(D =>
                    {
                        if (CarWashList.Count > 1 && carWashIndex < CarWashList.Count)
                        {
                            D.DistanceFromNextService = CarWashList[carWashIndex + 1].Location - D.Location;
                        }
                        else
                        {
                            D.DistanceFromNextService = 0;
                        }
                    });
                }
            }
            catch(Exception ex)
            {

            }
            IsBusy = false;
        }
        #endregion
    }
}
