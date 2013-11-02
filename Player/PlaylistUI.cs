﻿using System;
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
            return p.ToListItems();
        }

        public ListViewItem[] ToListItems()
        {
            ListViewItem[] items = new ListViewItem[p.Count];

            for (var i = 0; i < p.Count; i++)
            {
                items[i] = new ListViewItem();
                items[i].Content = p[i];
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