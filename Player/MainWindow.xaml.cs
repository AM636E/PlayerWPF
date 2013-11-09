using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using QuartzTypeLib;
using NAudio;
using NAudio.Wave;
using System.IO;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayerHandler _player = new PlayerHandler();
        private Playlist _pl;
        public MainWindow()
        {            
            InitializeComponent();
            _player.AddTimerHandler(SongHandler);
            _player.NewSongStarted += _player_NewSongStarted;
            _pl = new Playlist();

            _pl.Add(new DirectoryInfo(@"D:\just music"));

            _pl.Shuffle();

            _pl.Play(_player);

            _pl.ShowInListView(_playlist);

            _pl.ClickedOnSong += _pl_ClickedOnSong;

            _player.SongEnded += (o, e) =>
                {
                    _player.Play(_pl.CurrentSong);
                };
        }

        void _pl_ClickedOnSong(object sender, EventArgs e)
        {
            _player.Play(_pl.CurrentSong);
        }

        void _player_NewSongStarted(object sender, EventArgs e)
        {
            _playStatus.Value = 0;
            _playStatus.Maximum = _player.SongLengthSeconds;
            _totalTime.Content = _player.TotalTimeLabel;
        }

        private void SongHandler(object sender, EventArgs e)
        {
            if(_player.PlaybackState != PlaybackState.Paused)
            {
                _currentTime.Content = _player.CurrentTimeLable;
                _playStatus.Value++;
            }
        }

        private void _play_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _player.Play();
            _pause.Visibility = System.Windows.Visibility.Visible;
            _play.Visibility = System.Windows.Visibility.Hidden;
        }

        private void _pause_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _player.Pause();
            _pause.Visibility = System.Windows.Visibility.Hidden;
            _play.Visibility = System.Windows.Visibility.Visible;
        }

        private void _playStatus_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _player.Scroll(_playStatus.Value);
        }

        private void _playStatus_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _player.Scroll(_playStatus.Value);
        }

        private void _searchBar_KeyUp(object sender, KeyEventArgs e)
        {
            IEnumerable<int> toShow;
            try
            {
                toShow = _pl.SearchInPlaylist(_searchBar.Text);//indeces to hide
            }
            catch(EmptyStringException ex)
            {
                _pl.SetVisibillity(Enumerable.Range(0, _pl.Count), _playlist, System.Windows.Visibility.Visible);
                console.log(ex);
                return;
            }
            List<int> toHide = Enumerable.Range(0, _pl.Count).ToList<int>();//indeces to show. now all playlist

            //remove indeces to show from indeces to hide
            foreach (var i in toShow)
            {
                try
                {
                    toHide.RemoveAt(i);
                }
                catch(Exception x)
                {
                    console.log(x.Message);
                    console.log(x.StackTrace);
                }
            }
            
            _pl.SetVisibillity((IEnumerable<int>)toHide, _playlist, System.Windows.Visibility.Collapsed);
            _pl.SetVisibillity(toShow, _playlist, System.Windows.Visibility.Visible);
        }
    }
}
