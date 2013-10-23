using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace Player
{
    class PlayerHandler
    {
        static IWavePlayer waveOutDevice;// = new WaveOut();
        static WaveStream mainOutputStream;// = CreateInputStream(@"D:\music\Song2\03. Музыка ночи (менуэт) (В. А. Моцарт).mp3");
        static string _currFile;

        static PlayerHandler()
        {
            waveOutDevice = new WaveOut();
        }

        public static void LoadFile(string filename)
        {
            _currFile = filename;
            mainOutputStream = CreateInputStream(filename);
            waveOutDevice.Init(mainOutputStream);
        }

        public static void Play()
        {
            waveOutDevice.Play();
        }

        public static void Play(string filename)
        {
            LoadFile(filename);
            waveOutDevice.Play();
        }

        public static void Pause()
        {
            waveOutDevice.Pause();
        }

        public static void Seek(long offset)
        {
            mainOutputStream = CreateInputStream(_currFile);
            mainOutputStream.Seek(offset, System.IO.SeekOrigin.Begin);
            waveOutDevice.Init(mainOutputStream);
        }

        public static void StartFromTime()
        {

        }

        static WaveStream CreateInputStream(string filename)
        {
            WaveChannel32 inputStream;
            if (filename.EndsWith(".mp3"))
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
