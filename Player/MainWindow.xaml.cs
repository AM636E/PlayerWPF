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
using QuartzTypeLib;
using NAudio;
using NAudio.Wave;

namespace Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayerHandler _player = new PlayerHandler();

        public MainWindow()
        {            
            InitializeComponent();

            _player.Play(@"D:\just music\Воздух\04-Ты распят был.mp3");
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
    }
}
