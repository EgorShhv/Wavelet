using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForGraphicsMethods
{
    public class Line : Figure
    {
        private int id_node_1;
        private int id_node_2;

        public Line()
        {
            id_node_1 = 0;
            id_node_2 = 0;
            this.material = 0;
        }

        public Line(int id_node_1, int id_node_2, int material)
        {
            this.material = material;
            this.id_node_1 = id_node_1;
            this.id_node_2 = id_node_2;
        }

        public int get_node_1() { return id_node_1; }
        public int get_node_2() { return id_node_2; }
        public void set_node_1(int node_1) { this.id_node_1 = node_1; }
        public void set_node_2(int node_2) { this.id_node_2 = node_2; }

        public override string toString()
        {
            string lineStr = $"{material} {id_node_1} {id_node_2}";
            return lineStr;
        }
    }
}
