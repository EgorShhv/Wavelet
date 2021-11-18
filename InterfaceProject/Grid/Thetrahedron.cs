using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForGraphicsMethods
{
    public class Thetrahedron:Figure
    {
        private int id_node_1;
        private int id_node_2;
        private int id_node_3;
        private int id_node_4;

        public Thetrahedron()
        {
            this.material = 0;
            id_node_1 = 0;
            id_node_2 = 0;
            id_node_3 = 0;
            id_node_4 = 0;
        }

        public Thetrahedron(int id_node_1, int id_node_2, int id_node_3, int id_node_4, int material)
        {
            this.material = 0;
            this.id_node_1 = id_node_1;
            this.id_node_2 = id_node_2;
            this.id_node_3 = id_node_3;
            this.id_node_4 = id_node_4;
        }

        public int get_node_1() { return id_node_1; }
        public int get_node_2() { return id_node_2; }
        public int get_node_3() { return id_node_3; }
        public int get_node_4() { return id_node_4; }
        public void set_node_1(int node_1) { this.id_node_1 = node_1; }
        public void set_node_2(int node_2) { this.id_node_2 = node_2; }
        public void set_node_3(int node_3) { this.id_node_3 = node_3; }
        public void set_node_4(int node_4) { this.id_node_4 = node_4; }

        //перевод линии в строку
        public override string toString()
        {
            string tehtrahedronStr = $"{material} {id_node_1} {id_node_2} {id_node_3} {id_node_4}";
            return tehtrahedronStr;
        }
    }
}
