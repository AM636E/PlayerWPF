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

        private List<int> _shuffledIndeces = new List<int>();
        private bool _shoofled = false;

        public Song CurrentSong
        {
            get { return this[_currentSongIndex]; }
        }

        public Playlist()
            : base()
        { }

        public Playlist(PlayerHandler player)
            : this()
        {

            _player = player;
        }

        /*
         * void Shuffle()
         * 
         * rearanges indeces of song in random order
         * uses Fisher-Yates shuffle algotithm
         */
        public void Shuffle()
        {
            Random rnd = new Random();
            int j = 0;
            for (var i = this.Count - 1; i > 1; i++)
            {
                //generate random number j ( 0 =< j < items left );
                j = rnd.Next(0, i);

                this[i].NormalIndex = i;

                _shuffledIndeces.Add(j);
            }
            
            _shoofled = true;
        }

        public void UnShuffle()
        {
            _shoofled = false;
        }

        /*
         * void SearchForSongs(dirs[])
         * 
         * Search for supported song formats in dirs
         * 
         * @dirs Array of DirectoryInfo
         */
        List<string> _searchedFiles = new List<string>();
        public void SearchForSongs(DirectoryInfo[] dirs)
        {
            foreach (DirectoryInfo directory in dirs)
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
                catch (Exception e)
                {
                    console.log(this, " ", e.Message);
                }

                SearchForSongs(directory.GetDirectories());
            }
        }

        /*
         * public void Add(dif)
         * Searched in directory for supported files add adds them to playlist
         * 
         * @param dir - System.IO.DirectoryInfo object folder to search in ;
         */
        public void Add(DirectoryInfo dir)
        {
            _searchedFiles.Clear();
            try
            {
                SearchForSongs(dir.GetDirectories());
            }
            catch (Exception e)
            {
                console.log(e);
            }

            for (var i = 0; i < _searchedFiles.Count; i++)
            {
                this.Add(new Song(_searchedFiles[i]));
            }
        }

        /*
         * IEnumerable<int> Search(pattern)
         * 
         * returns all song indeces that contains pattern
         * 
         * if pattern is empty string thows EmptyStringException
         * 
         * @param pattern - string to check
         * @return matched song indeces
         */
        public IEnumerable<int> SearchInPlaylist(string pattern)
        {
          if(pattern == "")
          {
              throw new EmptyStringException();
          }

         /* return (from s in this
                 where reg.IsMatch(s.ToString())
                 select this.IndexOf(s)).Distinct();  */
          List<int> a = new List<int>();

          for (var i = 0; i < this.Count; i ++ )
          {
              if(this[i].ToString().ToLower().IndexOf(pattern.ToLower()) != -1)
              {
                  a.Add(i);
              }
          }

              return a;
        }

        /*
         * void Play(Player)
         * Plays a playlist in given player
         * after song ended index of song is changed
         * 
         * @param player - PlayerHandler to play playlist int
         */
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

       /*
        * On player song ended changes index of song to the next
        */
        void player_SongEnded(object sender, EventArgs e)
        {
            _currentSongIndex = (++_currentSongIndex >= this.Count) ? 0 : _currentSongIndex;

            if(_shoofled == true)
            {
                _currentSongIndex = _shuffledIndeces[_currentSongIndex];
            }
        }
    }
}