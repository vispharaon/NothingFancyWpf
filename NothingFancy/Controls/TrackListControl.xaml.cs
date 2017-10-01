using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using NothingFancy.Tracks.ViewModel;

namespace NothingFancy.Controls
{
    /// <summary>
    /// Interaction logic for TrackListControl.xaml
    /// </summary>
    public partial class TrackListControl : UserControl
    {
        public TrackListControl()
        {
            InitializeComponent();
        }
        
        public TrackViewModel GetSelectedAudio()
        {
            var selected = Items.ItemsSource.OfType<TrackViewModel>().FirstOrDefault(x => x.IsSelected);
            return selected;
        }
        

        private void TrackItem_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var selectedTrackItem = sender as TrackControl;
            if (selectedTrackItem != null)
            {
                var vm = (TrackViewModel)selectedTrackItem.DataContext;
                
                foreach (var item in CurrentTrackList().Items)
                {   
                    item.IsSelected = item.Equals(vm);
                }

                CurrentTrackList().RefreshCurrent();
            }
        }

        private TrackListViewModel CurrentTrackList()
        {
            return (TrackListViewModel)this.DataContext;
        }
    }
}
