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
        public MainWindow()
        {            
            InitializeComponent();

            PlayerHandler.Play(@"D:\music\Song2\03. Музыка ночи (менуэт) (В. А. Моцарт).mp3");

            //PlayerHandler.Seek(8);
        }

       
    }
}
