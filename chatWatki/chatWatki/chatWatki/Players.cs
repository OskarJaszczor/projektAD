using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatWatki
{
    class Players
    {
        public string ip { set; get; }
        public string nickname { set; get; }

        public Players(string ipe, string n)
        {
            ip = ipe;
            nickname = n;
        }
    }
}
