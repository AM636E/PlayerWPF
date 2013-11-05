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
            return (List<ListViewItem>)p.ToListViewItems();
        }

        public IEnumerable<ListViewItem> ToListViewItems()
        {
            return ToListViewItems((IEnumerable<int>)from s in this select this.IndexOf(s));            
        }

        public IEnumerable<ListViewItem> ToListViewItems(IEnumerable<int> _indeces)
        {
            return from item in this
                   where _indeces.Contains(this.IndexOf(item))
                   select (ListViewItem)item;
        }

        public void ShowInListView(ListView lv)
        {
            List<ListViewItem> items =  this.ToListViewItems().ToList<ListViewItem>();

            for(var i = 0; i < (items.Count<ListViewItem>()); i ++ )
            {
                lv.Items.Add(items[i]);
            }
        }
    }
}