using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Player
{
    partial class Playlist : List<Song>
    {
        private PlayerHandler _player;
        private int _currentSongIndex = 0;

        public Song CurrentSong
        {
            get { return this[_currentSongIndex]; }
        }

        public Playlist()
            :base()
        { }

        public Playlist(PlayerHandler player)
            :this()
        {
            _player = player;
        }

        //Uses Fisher-Yates Shuffle algorithm
        public void Shuffle()
        {
            Random rnd = new Random();
            int j = 0;
            Song tmp = null;
            for(var i = this.Count - 1; i > 1; i ++ )
            {
                //generate random number j ( 0 =< j < items countdown );
                j = rnd.Next(0, i);

                //swap items
                tmp = this[j];
                this[j] = this[i];
                this[i] = tmp;
            }
        }

        List<string> _searchedFiles = new List<string>();
        public void SearchForSongs(DirectoryInfo[] dirs)
        {
            foreach(DirectoryInfo directory in dirs)
            {
                try
                {
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        if (file.Name.EndsWith(".mp3"))
                        {
                            _searchedFiles.Add(file.FullName);
                        }
                    }
                }
                catch(Exception e)
                {
                    console.log(this, " ", e.Message);
                }

                SearchForSongs(directory.GetDirectories());
            }
        }

        public void Add(DirectoryInfo dir)
        {
            _searchedFiles.Clear();
            try
            {
                SearchForSongs(dir.GetDirectories());
            }
            catch(Exception e)
            {
                console.log(e);
            }

            for(var i = 0 ; i < _searchedFiles.Count; i ++ )
            {
                this.Add(new Song(_searchedFiles[i]));
            }
        }

        public Playlist Search(string pattern)
        {
            Playlist results = new Playlist();

            return results;
        }

        public void Play(Player.PlayerHandler player)
        {
            _player = player;
            player.SongEnded += player_SongEnded;
            player.Play(this[0]);
        }

        public void Play()
        {
            Play(_player);
        }

        void player_SongEnded(object sender, EventArgs e)       
        {
           _currentSongIndex = (++_currentSongIndex > this.Count) ? 0 : _currentSongIndex;
        }
    }
}
