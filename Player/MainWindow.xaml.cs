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

            IWavePlayer waveOutDevice = new WaveOut();
            WaveStream mainOutputStream = CreateInputStream(@"D:\music\Song2\03. Музыка ночи (менуэт) (В. А. Моцарт).mp3");

            waveOutDevice.Init(mainOutputStream);

            waveOutDevice.Play();
        }

        WaveStream CreateInputStream(string filename)
        {
            WaveChannel32 inputStream;
            if(filename.EndsWith(".mp3"))
            {
                WaveStream mp3Reader = new Mp3FileReader(filename);
                inputStream = new WaveChannel32(mp3Reader);
            }
            else
            {
                throw new InvalidOperationException("unsupported extension");
            }

            return inputStream;
        }
    }
}
