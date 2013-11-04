using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;


namespace Player
{
    class Song
    {
        private string _path;
        private WaveStream _stream;
        private int _normalIndex;//index in playlist before shuffle

        public int NormalIndex { get { return _normalIndex; } set { _normalIndex = value; } }

        public string Path { get { return _path; } }
        public string Extension { get { return _path.Substring(_path.LastIndexOf('.')); } }
        public WaveStream Stream { get { return _stream; } }

        public Song()
        { }

        public Song(string path)
        {
            _path = path;
            _stream = GetStream();
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

        public override string ToString()
        {            
            return this.Path.Substring(this.Path.LastIndexOfAny(new char[]{'\\', '/'}) + 1);
        }
    }
}
