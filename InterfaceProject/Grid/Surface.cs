using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForGraphicsMethods
{
    //поверхность: состоит из координат 1(x,y,z),2(x,y,z),3(x,y,z) 
    public class Surface:Figure
    {
        private int id_node_1;
        private int id_node_2;
        private int id_node_3;

        public Surface()
        {
            this.material = 0;
            id_node_1 = 0;
            id_node_2 = 0;
            id_node_3 = 0;
        }

        public Surface(int id_node_1, int id_node_2, int id_node_3, int material)
        {
            this.material = material;
            this.id_node_1 = id_node_1;
            this.id_node_2 = id_node_2;
            this.id_node_3 = id_node_3;
        }

        public int get_node_1() { return id_node_1; }
        public int get_node_2() { return id_node_2; }
        public int get_node_3() { return id_node_3; }
        public void set_node_1(int node_1) { this.id_node_1 = node_1; }
        public void set_node_2(int node_2) { this.id_node_2 = node_2; }
        public void set_node_3(int node_3) { this.id_node_3 = node_3; }

        //перевод линии в строку
        public override string toString()
        {
            string surfaceStr = $"{material} {id_node_1} {id_node_2} {id_node_3}";
            return surfaceStr;
        }
    }
}
