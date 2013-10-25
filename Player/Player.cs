using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows;

namespace Player
{
    class PlayerHandler
    {
        private IWavePlayer waveOut;
        private string fileName = null;
        private WaveStream fileWaveStream;
        private Action<float> setVolumeDelegate;
        private string _currentSong;

        public void Play(string filename)
        {
            if (waveOut != null)
            {
                if (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    return;
                }
                else if (waveOut.PlaybackState == PlaybackState.Paused)
                {
                    waveOut.Play();
                    return;
                }
            }

           // MessageBox.Show(string.IsNullOrEmpty(filename).ToString());

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            try
            {
                CreateWaveOut();
            }
            catch (Exception driverCreateException)
            {
                MessageBox.Show(String.Format("{0}", driverCreateException.Message));
                return;
            }

            ISampleProvider sampleProvider = null;
            try
            {
                sampleProvider = CreateInputStream(filename);
            }
            catch (Exception createException)
            {
                MessageBox.Show(String.Format("{0}", createException.Message), "Error Loading File");
                return;
            }

            try
            {
                waveOut.Init(new SampleToWaveProvider(sampleProvider));
            }
            catch (Exception initException)
            {
                MessageBox.Show(String.Format("{0}", initException.Message), "Error Initializing Output");
                return;
            }
         //   fileWaveStream.CurrentTime = TimeSpan.FromSeconds(200);
            _currentSong = filename;
           // setVolumeDelegate(); 
            waveOut.Play();
        }

        public void Scroll(double seconds)
        {
          //  fileWaveStream.Seek((long)seconds, System.IO.SeekOrigin.Current);
            
            fileWaveStream.CurrentTime = TimeSpan.FromSeconds(seconds);
        }

        private void CreateWaveOut()
        {
            CloseWaveOut();
        //    int latency = 300;
            this.waveOut = new WaveOut(); //new Mp3FileReader();//SelectedOutputDevicePlugin.CreateDevice(latency);
           // this.waveOut.PlaybackStopped += OnPlaybackStopped;
        }

        private WaveStream GetPluginForFile(String filename)
        {
            //switch(filename.Substring()
            return new Mp3FileReader(filename);
        }

        private WaveStream GetStreamForFile(String filename)
        {
            if(filename.EndsWith(".mp3"))
            {
                return new Mp3FileReader(filename);
            }
            if(filename.EndsWith(".wav"))
            {
                return new WaveFileReader(filename);
            }

            throw new InvalidOperationException("Unsuported extension");
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            var plugin = GetPluginForFile(fileName);
            if (plugin == null)
            {
                throw new InvalidOperationException("Unsupported file extension");
            }
            this.fileWaveStream = plugin;// plugin.CreateWaveStream(fileName);
            var waveChannel = new SampleChannel(this.fileWaveStream, true);
            this.setVolumeDelegate = (vol) => waveChannel.Volume = vol;
      //      waveChannel.PreVolumeMeter += OnPreVolumeMeter;

            var postVolumeMeter = new MeteringSampleProvider(waveChannel);
      //      postVolumeMeter.StreamVolume += OnPostVolumeMeter;

            return postVolumeMeter;
        }

        private void CloseWaveOut()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
            }
            if (fileWaveStream != null)
            {
                // this one really closes the file and ACM conversion
                fileWaveStream.Dispose();
                this.setVolumeDelegate = null;
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }
    }
}
