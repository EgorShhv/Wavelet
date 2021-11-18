using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ForGraphicsMethods
{
    public class Node : Figure
    {
        // ID точки.
        private int id;
        // Координаты точки в пространстве.
        private double x;
        private double y;
        private double z;


        // -------------------------------------

        public Node()
        {
            this.id = 0;
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Node(int id, double x, double y, double z)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double get_x() { return this.x; }
        public double get_y() { return this.y; }
        public double get_z() { return this.z; }
        public int get_id() { return this.id; }
        public void set_x(double new_x) { this.x = new_x; }
        public void set_y(double new_y) { this.y = new_y; }
        public void set_z(double new_z) { this.z = new_z; }

        public override string toString()
        {
            string pointStr = $"{id} {x} {y} {z}";
            return pointStr;
        }
    }
}

