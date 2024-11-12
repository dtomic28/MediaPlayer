using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MediaPlayer.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> playlist = new List<string> { "Song 1", "Song 2", "Song 3" };
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            CurrentTimeLabel.Content = "--:--";
            PlaylistView.ItemsSource = playlist;
            MediaTitle.Text = "Kiša Metaka - Drai Millionen";
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PlaylistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PlaylistView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlaylistView.SelectedItem is ListViewItem selectedItem)
            {
                string? itemName = selectedItem.Content.ToString();
                Debug.WriteLine(itemName);
            }
            else if(PlaylistView.SelectedItem is string itemString)
            {
                Debug.WriteLine(itemString);
            }
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }



    }
}