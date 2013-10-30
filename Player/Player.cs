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
        public event EventHandler SongEnded;
        public event EventHandler SongStarted;


        private double _secondsPlayed = 0;
        public double SecondsPlayed { get { return _secondsPlayed; } }
        private IWavePlayer waveOut;
        private WaveStream fileWaveStream;
        private Action<float> setVolumeDelegate;
        private string _currentSong;
        private DispatcherTimer _songPlayTimer;

        public PlayerHandler()
        {
            _songPlayTimer = new DispatcherTimer();
            _songPlayTimer.Interval = new TimeSpan(0, 0, 1);
            AddTimerHandler(
            (o, e) =>
            {
                _secondsPlayed = SongTotalTime.Seconds;
                if(_secondsPlayed >= SongLengthSeconds && SongEnded != null )
                {
                    SongEnded(this, EventArgs.Empty);
                }
            });
        }

        public void AddTimerHandler(EventHandler handler)
        {
            _songPlayTimer.Tick += handler;
        }

        public string CurrentSong { get { return _currentSong; } }
        public TimeSpan CurrentTime { get { return fileWaveStream.CurrentTime; } }
        public TimeSpan SongTotalTime { get { return fileWaveStream.TotalTime; } }
        public double SongLengthSeconds { get { return fileWaveStream.TotalTime.TotalSeconds; } }
        public double SongCurrentSeconds { get { return CurrentTime.TotalSeconds; } }
        
        public PlaybackState PlaybackState { get { return waveOut.PlaybackState; } }

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
                    if (SongStarted != null)
                    {
                        SongStarted(this, EventArgs.Empty);
                    }

                    return;
                }
            }
        }

        private void Play(WaveStream stream)
        {
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
                sampleProvider = CreateInputStream(stream);
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

            _songPlayTimer.Start();

            if(NewSongStarted != null)
            {
                NewSongStarted(this, EventArgs.Empty);
            }
            
            waveOut.Play();
        }

        public void Play(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            _currentSong = filename;
            Play(GetStreamForFile(filename));
        }

        public void Play(Song song)
        {
            _currentSong = song.Path;
            Play(song.Stream);
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

        private ISampleProvider CreateInputStream(WaveStream plugin)
        {
            this.fileWaveStream = plugin;
            var waveChannel = new SampleChannel(this.fileWaveStream, true);
            var postVolumeMeter = new MeteringSampleProvider(waveChannel);

            return postVolumeMeter;
        }

        private ISampleProvider CreateInputStream(Song song)
        {
            return CreateInputStream(song.Stream);
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            var plugin = GetPluginForFile(fileName);
            if (plugin == null)
            {
                throw new InvalidOperationException("Unsupported file extension");
            }

            return CreateInputStream(plugin);
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
