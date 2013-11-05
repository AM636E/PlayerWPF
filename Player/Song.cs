using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.Windows.Controls;

namespace Player
{
    class Song
    {
        private string _path;
        private int _normalIndex;//index in playlist before shuffle

        public int NormalIndex { get { return _normalIndex; } set { _normalIndex = value; } }

        public string Path { get { return _path; } }
        public string Extension { get { return _path.Substring(_path.LastIndexOf('.')); } }

        public Song()
        { }

        public Song(string path)
        {
            _path = path;
  
        }

        public WaveStream GetStream()
        {
            switch(Extension)
            {
                case ".mp3":
                    {
                        return new Mp3FileReader(_path);
                    }
                case ".wav":
                    {
                        return new WaveFileReader(_path);
                    }
                default:
                    {
                        throw new OperationCanceledException("Not Supported Extension");
                    }
            }
        }

        public  static explicit operator ListViewItem(Song s)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Content = s;

            return lvi;
        }

        public override string ToString()
        {            
            return this.Path.Substring(this.Path.LastIndexOfAny(new char[]{'\\', '/'}) + 1);
        }
    }
}