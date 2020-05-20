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
using System.Collections.ObjectModel;
using System.Collections;

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
                bidingContext.SelectedDateLabel = string.Empty;
                var currentSelectedItems = ((IEnumerable)e.CurrentSelection).Cast<SubCalanderContract>().ToList();
                if (currentSelectedItems != null&& currentSelectedItems.Any())
                {
                    currentSelectedItems.ForEach(D => bidingContext.SelectedDateLabel = $"{bidingContext.SelectedDateLabel} {D.Date} {D.Day},");
                    bidingContext.GetCarWashList(new List<SubCalanderContract>(currentSelectedItems));
                }
                else
                {
                    bidingContext.GetCarWashList();
                }
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