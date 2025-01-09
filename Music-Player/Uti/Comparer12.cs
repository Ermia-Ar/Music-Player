using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.Uti
{
    class Comparer12<T> : IComparer<MUSIC>
    {
        public int Compare(MUSIC? x, MUSIC? y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
