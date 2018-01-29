using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;

namespace LFVGame
{
    public static class Util
    {
        public static void FinalizeListItems(System.Collections.IList list)
        {
            for (int i = list.Count-1; i > -1; --i)
            {
                IDisposable item = list[i] as IDisposable;
                list.RemoveAt(i);
                item.Dispose();
            }
            GC.SuppressFinalize(list);
        }
    }
}
