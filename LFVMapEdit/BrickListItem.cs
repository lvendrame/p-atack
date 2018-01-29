using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LFVMapControler;

namespace LFVMapEdit
{
    public class BrickListItem: ListViewItem
    {
        private Brick fbrk_Brick;
        public Brick Brick
        {
            get { return fbrk_Brick; }
            set { fbrk_Brick = value; }
        }

    }
}
