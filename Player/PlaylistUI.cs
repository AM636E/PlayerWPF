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
        public static explicit operator List<ListViewItem>(Playlist p)
        {
            return p.ToListViewItems().ToList<ListViewItem>();
        }

        public IEnumerable<ListViewItem> ToListViewItems()
        {
            return ToListViewItems(Enumerable.Range(0, this.Count));            
        }

        public IEnumerable<ListViewItem> ToListViewItems(IEnumerable<int> _indeces)
        {
            return from item in this
                   where _indeces.Contains(this.IndexOf(item))
                   select (ListViewItem)item;
        }

        public void ShowInListView(ListView lv)
        {
            ShowInListView(Enumerable.Range(0, this.Count), lv);
        }

        public static void ShowInListView(IEnumerable<ListViewItem> items, ListView lv)
        {
            foreach(var i in items)
            {
                lv.Items.Add(i);
            }
        }

        public void ShowInListView(IEnumerable<int> indeces, ListView lv)
        {
            Playlist.ShowInListView(this.ToListViewItems(indeces), lv);
        }
    }
}