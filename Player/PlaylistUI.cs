using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Player
{
    partial class Playlist
    {
        public static implicit operator ListViewItem[] (Playlist p)
        {
            return p.ToListViewItems();
        }

        public ListViewItem[] ToListViewItems()
        {
            ListViewItem[] items = new ListViewItem[this.Count];

            for (var i = 0; i < this.Count; i++)
            {
                items[i] = new ListViewItem();
                items[i].Content = this[i];
                items[i].MouseDoubleClick += (o, e) =>
                {
                    Song song = ((o as ListViewItem).Content as Song);
                    _currentSongIndex = this.IndexOf(song);

                    _player.Play(song);
                };
            }

            return items;
        }

        public void ShowInListView(ListView lv)
        {
            foreach (var i in (ListViewItem[])this)
            {
                lv.Items.Add(i);
            }
        }
    }
}