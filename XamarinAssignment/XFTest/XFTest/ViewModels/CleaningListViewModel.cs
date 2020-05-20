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

        bool _isWashListEmpty;
        public bool IsWashListEmpty { get { return _isWashListEmpty; } set { _isWashListEmpty = value;NotifyPropertyChanged("IsWashListEmpty"); } }

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
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,1),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,1),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,1),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,1),new SubCalanderContract(5,Appstrings.Label_Short_Friday,1),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,1),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,1),new SubCalanderContract(8,Appstrings.Label_Short_Monday,1),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,1),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,1),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,1),new SubCalanderContract(12,Appstrings.Label_Short_Friday,1),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,1),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,1),new SubCalanderContract(15,Appstrings.Label_Short_Monday,1),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,1),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,1),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,1),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,1),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,1),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,1),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,1),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,1),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,1),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,1),new SubCalanderContract(26,Appstrings.Label_Short_Friday,1),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,1),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,1),new SubCalanderContract(29,Appstrings.Label_Short_Monday,1),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,1),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,1),
            };
            var feb = new List<SubCalanderContract>
            {
               new SubCalanderContract(1,Appstrings.Label_Short_Monday,2),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,2),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,2),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,2),new SubCalanderContract(5,Appstrings.Label_Short_Friday,2),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,2),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,2),new SubCalanderContract(8,Appstrings.Label_Short_Monday,2),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,2),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,2),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,2),new SubCalanderContract(12,Appstrings.Label_Short_Friday,2),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,2),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,2),new SubCalanderContract(15,Appstrings.Label_Short_Monday,2),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,2),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,2),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,2),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,2),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,2),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,2),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,2),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,2),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,2),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,2),new SubCalanderContract(26,Appstrings.Label_Short_Friday,2),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,2),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,2),new SubCalanderContract(29,Appstrings.Label_Short_Monday,2)
            };
            var mar = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,3),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,3),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,3),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,3),new SubCalanderContract(5,Appstrings.Label_Short_Friday,3),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,3),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,3),new SubCalanderContract(8,Appstrings.Label_Short_Monday,3),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,3),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,3),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,3),new SubCalanderContract(12,Appstrings.Label_Short_Friday,3),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,3),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,3),new SubCalanderContract(15,Appstrings.Label_Short_Monday,3),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,3),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,3),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,3),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,3),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,3),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,3),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,3),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,3),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,3),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,3),new SubCalanderContract(26,Appstrings.Label_Short_Friday,3),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,3),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,3),new SubCalanderContract(29,Appstrings.Label_Short_Monday,3),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,3),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,3),
            };
            var april = new List<SubCalanderContract>
            {
                 new SubCalanderContract(1,Appstrings.Label_Short_Monday,4),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,4),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,4),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,4),new SubCalanderContract(5,Appstrings.Label_Short_Friday,4),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,4),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,4),new SubCalanderContract(8,Appstrings.Label_Short_Monday,4),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,4),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,4),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,4),new SubCalanderContract(12,Appstrings.Label_Short_Friday,4),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,4),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,4),new SubCalanderContract(15,Appstrings.Label_Short_Monday,4),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,4),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,4),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,4),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,4),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,4),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,4),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,4),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,4),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,4),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,4),new SubCalanderContract(26,Appstrings.Label_Short_Friday,4),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,4),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,4),new SubCalanderContract(29,Appstrings.Label_Short_Monday,4),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,4),
            };
            var may = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,5),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,5),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,5),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,5),new SubCalanderContract(5,Appstrings.Label_Short_Friday,5),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,5),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,5),new SubCalanderContract(8,Appstrings.Label_Short_Monday,5),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,5),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,5),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,5),new SubCalanderContract(12,Appstrings.Label_Short_Friday,5),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,5),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,5),new SubCalanderContract(15,Appstrings.Label_Short_Monday,5),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,5),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,5),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,5),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,5),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,5),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,5),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,5),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,5),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,5),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,5),new SubCalanderContract(26,Appstrings.Label_Short_Friday,5),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,5),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,5),new SubCalanderContract(29,Appstrings.Label_Short_Monday,5),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,5),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,5),
            };
            var june = new List<SubCalanderContract>
            {
               new SubCalanderContract(1,Appstrings.Label_Short_Monday,6),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,6),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,6),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,6),new SubCalanderContract(5,Appstrings.Label_Short_Friday,6),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,6),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,6),new SubCalanderContract(8,Appstrings.Label_Short_Monday,6),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,6),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,6),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,6),new SubCalanderContract(12,Appstrings.Label_Short_Friday,6),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,6),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,6),new SubCalanderContract(15,Appstrings.Label_Short_Monday,6),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,6),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,6),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,6),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,6),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,6),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,6),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,6),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,6),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,6),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,6),new SubCalanderContract(26,Appstrings.Label_Short_Friday,6),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,6),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,6),new SubCalanderContract(29,Appstrings.Label_Short_Monday,6),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,6),
            };
            var July = new List<SubCalanderContract>
            {
              new SubCalanderContract(1,Appstrings.Label_Short_Monday,7),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,7),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,7),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,7),new SubCalanderContract(5,Appstrings.Label_Short_Friday,7),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,7),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,7),new SubCalanderContract(8,Appstrings.Label_Short_Monday,7),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,7),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,7),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,7),new SubCalanderContract(12,Appstrings.Label_Short_Friday,7),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,7),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,7),new SubCalanderContract(15,Appstrings.Label_Short_Monday,7),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,7),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,7),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,7),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,7),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,7),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,7),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,7),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,7),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,7),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,7),new SubCalanderContract(26,Appstrings.Label_Short_Friday,7),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,7),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,7),new SubCalanderContract(29,Appstrings.Label_Short_Monday,7),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,7),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,7),
            };
            var August = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,8),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,8),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,8),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,8),new SubCalanderContract(5,Appstrings.Label_Short_Friday,8),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,8),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,8),new SubCalanderContract(8,Appstrings.Label_Short_Monday,8),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,8),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,8),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,8),new SubCalanderContract(12,Appstrings.Label_Short_Friday,8),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,8),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,8),new SubCalanderContract(15,Appstrings.Label_Short_Monday,8),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,8),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,8),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,8),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,8),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,8),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,8),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,8),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,8),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,8),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,8),new SubCalanderContract(26,Appstrings.Label_Short_Friday,8),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,8),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,8),new SubCalanderContract(29,Appstrings.Label_Short_Monday,8),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,8),
                new SubCalanderContract(31,Appstrings.Label_Short_Tuesday,8),
            };
            var Sep = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,9),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,9),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,9),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,9),new SubCalanderContract(5,Appstrings.Label_Short_Friday,9),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,9),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,9),new SubCalanderContract(8,Appstrings.Label_Short_Monday,9),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,9),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,9),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,9),new SubCalanderContract(12,Appstrings.Label_Short_Friday,9),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,9),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,9),new SubCalanderContract(15,Appstrings.Label_Short_Monday,9),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,9),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,9),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,9),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,9),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,9),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,9),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,9),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,9),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,9),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,9),new SubCalanderContract(26,Appstrings.Label_Short_Friday,9),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,9),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,9),new SubCalanderContract(29,Appstrings.Label_Short_Monday,9),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,9),
            };
            var oct = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,10),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,10),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,10),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,10),new SubCalanderContract(5,Appstrings.Label_Short_Friday,10),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,10),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,10),new SubCalanderContract(8,Appstrings.Label_Short_Monday,10),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,10),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,10),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,10),new SubCalanderContract(12,Appstrings.Label_Short_Friday,10),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,10),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,10),new SubCalanderContract(15,Appstrings.Label_Short_Monday,10),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,10),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,10),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,10),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,10),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,10),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,10),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,10),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,10),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,10),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,10),new SubCalanderContract(26,Appstrings.Label_Short_Friday,10),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,10),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,10),new SubCalanderContract(29,Appstrings.Label_Short_Monday,10),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,10),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,10),
            };
            var nov = new List<SubCalanderContract>
            {
                 new SubCalanderContract(1,Appstrings.Label_Short_Monday,11),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,11),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,1),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,11),new SubCalanderContract(5,Appstrings.Label_Short_Friday,11),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,11),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,11),new SubCalanderContract(8,Appstrings.Label_Short_Monday,11),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,11),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,11),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,11),new SubCalanderContract(12,Appstrings.Label_Short_Friday,11),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,11),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,11),new SubCalanderContract(15,Appstrings.Label_Short_Monday,11),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,11),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,11),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,11),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,11),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,11),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,11),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,11),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,11),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,11),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,11),new SubCalanderContract(26,Appstrings.Label_Short_Friday,11),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,11),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,11),new SubCalanderContract(29,Appstrings.Label_Short_Monday,11),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,11),
            };
            var dec = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,Appstrings.Label_Short_Monday,12),new SubCalanderContract(2,Appstrings.Label_Short_Tuesday,12),new SubCalanderContract(3,Appstrings.Label_Short_Wednesday,12),
                new SubCalanderContract(4,Appstrings.Label_Short_Thursday,12),new SubCalanderContract(5,Appstrings.Label_Short_Friday,12),new SubCalanderContract(6,Appstrings.Label_Short_Saturday,12),
                new SubCalanderContract(7,Appstrings.Label_Short_Sunday,12),new SubCalanderContract(8,Appstrings.Label_Short_Monday,12),new SubCalanderContract(9,Appstrings.Label_Short_Tuesday,12),
                new SubCalanderContract(10,Appstrings.Label_Short_Wednesday,12),new SubCalanderContract(11,Appstrings.Label_Short_Thursday,12),new SubCalanderContract(12,Appstrings.Label_Short_Friday,12),
                new SubCalanderContract(13,Appstrings.Label_Short_Saturday,12),new SubCalanderContract(14,Appstrings.Label_Short_Sunday,12),new SubCalanderContract(15,Appstrings.Label_Short_Monday,12),
                new SubCalanderContract(16,Appstrings.Label_Short_Tuesday,12),new SubCalanderContract(17,Appstrings.Label_Short_Wednesday,12),new SubCalanderContract(18,Appstrings.Label_Short_Thursday,12),
                new SubCalanderContract(19,Appstrings.Label_Short_Friday,12),new SubCalanderContract(20,Appstrings.Label_Short_Saturday,12),new SubCalanderContract(21,Appstrings.Label_Short_Sunday,12),
                new SubCalanderContract(22,Appstrings.Label_Short_Monday,12),new SubCalanderContract(23,Appstrings.Label_Short_Tuesday,12),new SubCalanderContract(24,Appstrings.Label_Short_Wednesday,12),
                new SubCalanderContract(25,Appstrings.Label_Short_Thursday,12),new SubCalanderContract(26,Appstrings.Label_Short_Friday,12),new SubCalanderContract(27,Appstrings.Label_Short_Saturday,12),
                new SubCalanderContract(28,Appstrings.Label_Short_Sunday,12),new SubCalanderContract(29,Appstrings.Label_Short_Monday,12),new SubCalanderContract(30,Appstrings.Label_Short_Tuesday,12),
                new SubCalanderContract(31,Appstrings.Label_Short_Wednesday,12),
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
            IsToShowCalander = false;
            IsRefreshing = true;
            GetCarWashList();
        }

        public async void GetCarWashList(List<SubCalanderContract> selectedDates=null)
        {
            CarWashList.Clear();
            IsRefreshing= IsWashListEmpty = false;
            IsBusy = true;
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
                    if(selectedDates!=null&&selectedDates.Any())
                    {
                        var sortedCarWashList = new List<CarWashDataContract>();
                        foreach (var date in selectedDates)
                        {
                           var data= CarWashList.Where(C => C.ServiceDate.Date.Day ==date.Date&&C.ServiceDate.Month==date.MonthId).ToList();
                            sortedCarWashList.AddRange(data);
                        }
                        CarWashList = new ObservableCollection<CarWashDataContract>(sortedCarWashList);
                    }
                    else
                    {
                        IsToShowCalander = false;
                        SelectedDateLabel = Appstrings.Label_Today;
                    }
                    IsWashListEmpty = CarWashList != null && !CarWashList.Any();
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
