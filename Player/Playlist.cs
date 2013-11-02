using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    class Playlist : List<Song>
    {
        private int _currentSongIndex = 0;

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

        public void Play(Player.PlayerHandler player)
        {
            player.SongEnded += player_SongEnded;
            player.Play(this[0]);
        }

        void player_SongEnded(object sender, EventArgs e)       
        {
            PlayerHandler p = sender as PlayerHandler;
            p.Stop();
            if(_currentSongIndex >= this.Count)
            {
                _currentSongIndex = 0;
            }

            p.Play(this[_currentSongIndex]);
        }
    }
}
