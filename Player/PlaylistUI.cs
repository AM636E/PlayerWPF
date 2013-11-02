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
            ListViewItem[] items = new ListViewItem[p.Count];

            //for()

            return items;
        }

        public void ShowInListView(ListView lv)
        {
            foreach(Object o in this)
            {
                lv.Items.Add(o);
            }
        }
    }
}