using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceProject
{
    public class Point
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
