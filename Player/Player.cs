using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Threading;
using System.Linq;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows;

namespace Player
{
    class PlayerHandler
    {
        public event EventHandler NewSongStarted;
        public event EventHandler SongPaused;
        public event EventHandler SongStarted;

        private IWavePlayer waveOut;
        private WaveStream fileWaveStream;
        private Action<float> setVolumeDelegate;
        private string _currentSong;
        private DispatcherTimer _songPlayTimer;

        public PlayerHandler()
        {
            _songPlayTimer = new DispatcherTimer();
            _songPlayTimer.Interval = new TimeSpan(0, 0, 1);
        }

        public void AddTimerHandler(EventHandler handler)
        {
            _songPlayTimer.Tick += handler;
        }

        public string CurrentSong { get { return _currentSong; } }
        public TimeSpan SongTotalTime { get { return fileWaveStream.TotalTime; } }
        public double SongLength { get { return fileWaveStream.TotalTime.TotalSeconds; } }
        
        public PlaybackState PlaybackState { get { return waveOut.PlaybackState; } }

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
            
            _currentSong = filename;
            _songPlayTimer.Start();

            if(NewSongStarted != null)
            {
                NewSongStarted(this, EventArgs.Empty);
            }
            
            waveOut.Play();
        }

        public void Scroll(double seconds)
        {
            fileWaveStream.CurrentTime = TimeSpan.FromSeconds(seconds);
        }

        private void CreateWaveOut()
        {
            CloseWaveOut();
            this.waveOut = new WaveOut();
        }

        public void Play()
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
                    _songPlayTimer.Start();
                    if(SongStarted != null)
                    {
                        SongStarted(this, EventArgs.Empty);
                    }

                    return;
                }
            }
            
        }

        public void Pause()
        {
            _songPlayTimer.Stop();
            waveOut.Pause();
        }

        private WaveStream GetPluginForFile(String filename)
        {
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
            this.fileWaveStream = plugin;
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
