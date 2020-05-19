using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XFTest.ViewModels;
using Prism.Services.Dialogs;
using XFTest.Models;
using Xamarin.Forms.Internals;

namespace XFTest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CleaningList : ContentPage
    {
        public CleaningList()
        {
            InitializeComponent();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bidingContext = this.BindingContext as CleaningListViewModel;
            if (bidingContext != null)
            {
                bidingContext.CalanderData.ForEach(CD=>CD.IsToHighliteDate=false);
            }
            var currentItem = (e.CurrentSelection.FirstOrDefault() as SubCalanderContract);
            if(currentItem!=null)
            {
                currentItem.IsToHighliteDate = true;
                bidingContext.SelectedDateLabel = $"{currentItem.Date} {currentItem.Day}";
                System.Threading.Tasks.Task.Run(async() => 
                { 
                  await bidingContext.GetCarWashList();
                });
            }
        }

        private void CollectionView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var bidingContext = this.BindingContext as CleaningListViewModel;
            if (bidingContext != null)
            {
                bidingContext.IsToShowCalander = false;
            }
        }
    }
}