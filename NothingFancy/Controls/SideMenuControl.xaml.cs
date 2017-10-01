using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NothingFancy.Data;
using NothingFancy.Tracks.ViewModel;

namespace NothingFancy.Controls
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        private readonly SideMenuViewModel _vm;
        
        public SideMenuControl()
        {
            InitializeComponent();
            this.DataContext = _vm = new SideMenuViewModel();
            _vm.TrackListInstance = new TrackListViewModel(new TrackRepository());
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var numberOfInstances = _vm.TrackListInstance.Refresh(_vm.SearchTerm);

            if (numberOfInstances == 0)
                MessageBox.Show(
                    $"There is no tracks in our database with term: {_vm.SearchTerm}.\n Enter something like: Happy");
        }

        public TrackViewModel GetSelectedAudio()
        {
            return _vm.TrackListInstance.SelectedAudio;
        }
    }
}
