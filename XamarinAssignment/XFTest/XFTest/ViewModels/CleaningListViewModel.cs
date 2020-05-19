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
        public ICommand RefreshCommand { get { return new Command(async() => {await RefreshData(); }); } }
        public ICommand CloseCalander { get { return new Command(() => { IsToShowCalander = false; }); } }
        #endregion

        #region Constructore
        public CleaningListViewModel( IDialogService dialogService, INavigationService navigationService
            , ICarFitServices carFitServices)
        {
            _carFitServices = carFitServices;
            CarWashList = new ObservableCollection<CarWashDataContract>();
            PopulateCalanderData();
            System.Threading.Tasks.Task.Run(async () => 
            { 
               await GetCarWashList();
            });
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
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var feb = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),
            };
            var mar = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var april = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
            };
            var may = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var june = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
            };
            var July = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var August = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
            };
            var Sep = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var oct = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var nov = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };
            var dec = new List<SubCalanderContract>
            {
                new SubCalanderContract(1,"Mon"),new SubCalanderContract(2,"Tue"),new SubCalanderContract(3,"Wed"),
                new SubCalanderContract(4,"Thu"),new SubCalanderContract(5,"Fri"),new SubCalanderContract(6,"Sat"),
                new SubCalanderContract(7,"sun"),new SubCalanderContract(8,"Mon"),new SubCalanderContract(9,"Tue"),
                new SubCalanderContract(10,"Wed"),new SubCalanderContract(11,"Thu"),new SubCalanderContract(12,"Fri"),
                new SubCalanderContract(13,"Sat"),new SubCalanderContract(14,"Sun"),new SubCalanderContract(15,"Mon"),
                new SubCalanderContract(16,"Tue"),new SubCalanderContract(17,"Wed"),new SubCalanderContract(18,"Thu"),
                new SubCalanderContract(19,"Fri"),new SubCalanderContract(20,"Sat"),new SubCalanderContract(21,"Sun"),
                new SubCalanderContract(22,"Mon"),new SubCalanderContract(23,"Tue"),new SubCalanderContract(24,"Wed"),
                new SubCalanderContract(25,"Thu"),new SubCalanderContract(26,"Fri"),new SubCalanderContract(27,"Sat"),
                new SubCalanderContract(28,"Sun"),new SubCalanderContract(29,"Mon"),new SubCalanderContract(30,"Tue"),
                new SubCalanderContract(31,"Wed"),
            };

            calanderBackUp = new ObservableCollection<SubCalanderContract>
            {
                new SubCalanderContract(1,jan,"January"),
                new SubCalanderContract(2,feb,"Febrauary"),
                new SubCalanderContract(3,mar,"March"),
                new SubCalanderContract(4,april,"April"),
                new SubCalanderContract(5,may,"May"),
                new SubCalanderContract(6,june,"June"),
                new SubCalanderContract(7,July,"July"),
                new SubCalanderContract(8,August,"August"),
                new SubCalanderContract(9,Sep,"September"),
                new SubCalanderContract(10,oct,"October"),
                new SubCalanderContract(11,nov,"November"),
                new SubCalanderContract(12,dec,"December"),
            };
            var month = System.DateTime.Now.Month;
            DateLabel = System.DateTime.Now.ToString("yyyy MMMM");
            selectedMonthData = calanderBackUp[month - 1];
            SelectedDateLabel = "Today";
            CalanderData =new ObservableCollection<SubCalanderContract>(calanderBackUp[month - 1].Month);
        }

        private void NextMonthClicked()
        {
            if (selectedMonthData?.MonthId == 12)
            {
                selectedMonthData = calanderBackUp[0];
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} 2020";
            }
            else
            {
                var monthId = selectedMonthData.MonthId + 1;
                selectedMonthData = calanderBackUp.FirstOrDefault(CB => CB.MonthId == monthId);
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} 2020";
            }
        }

        private void PreviousMonthClicked()
        {
            if (selectedMonthData?.MonthId == 1)
            {
                selectedMonthData = calanderBackUp[11];
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} 2020";
            }
            else
            {
                var monthId = selectedMonthData.MonthId - 1;
                selectedMonthData = calanderBackUp.FirstOrDefault(CB => CB.MonthId == monthId);
                CalanderData = new ObservableCollection<SubCalanderContract>(selectedMonthData?.Month);
                DateLabel = $"{selectedMonthData?.MonthName} 2020";
            }
        }

        async System.Threading.Tasks.Task RefreshData()
        {
            IsRefreshing = true;
           await  GetCarWashList();
        }

        public async System.Threading.Tasks. Task GetCarWashList()
        {
            CarWashList.Clear();
            IsBusy = true;
            IsRefreshing = false;
            var result= await _carFitServices.GetAllCarServices();
            if(result.Success)
            {
                result.Data.ForEach(D =>
                {
                    var data = D;
                    D.Tasks.ForEach(T =>
                    {
                        D.DisplayTask += T.Title;
                        D.ToatalTimeForTasks+= T.TimesInMinutes;
                    });
                    D.Location = (D.HouseOwnerLatitude + D.HouseOwnerLongitude).ToString();
                    CarWashList.Add(D);
                });
            }
            IsBusy = false;
        }
        #endregion
    }
}
