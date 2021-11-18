using ForGraphicsMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceProject
{
    public class Grid
    {
        // --------------------------------------------------------------
        // string path = @"C:\Users\egshh\source\repos\Read_mesh\Read_mesh\meshh200.msh";
        public Point top_point { get; set; }
        public Point bottom_point { get; set; }

        public List<Node> nodes;
        public List<Line> lines;
        public List<Surface> surfaces;
        public List<Thetrahedron> thetrahedrons;

        // --------------------------------------------------------------
        
        public Grid()
        {
            nodes = new List<Node>();
            lines = new List<Line>();
            surfaces = new List<Surface>();
            thetrahedrons = new List<Thetrahedron>();
        }

        public Grid(string path_gmsh_file)
        {
            top_point = new Point(100, 100, 100);
            bottom_point = new Point(-40, -40, -40);
            nodes = new List<Node>();
            lines = new List<Line>();
            surfaces = new List<Surface>();
            thetrahedrons = new List<Thetrahedron>();

            nodes = read_from_file(path_gmsh_file, ref lines, ref surfaces, ref thetrahedrons);
        }


        public List<Node> read_from_file(
            string path_file,
            ref List<Line> line_list,
            ref List<Surface> surface_list,
            ref List<Thetrahedron> tetrahedron_list)
        {
            List<Node> nodes = new List<Node>();
            try
            {
                using (StreamReader sr = new StreamReader(path_file, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == "$MeshFormat")
                        {
                            string version_msh;
                            if ((version_msh = sr.ReadLine()) != null)
                            {
                                double main_version = Convert.ToDouble(version_msh.Split(' ')[0], CultureInfo.InvariantCulture);
                                if (main_version != 2.2)
                                {
                                    Console.WriteLine("Предупреждение: Версия программы, создавшей msh файл отличается от версии 2.2\n" +
                                        "Результаты могут быть некорректными");
                                    continue;
                                }
                            }
                        }

                        // Заполнения массива точек.
                        if (line == "$Nodes")
                        {
                            int count_Nodes = Convert.ToInt32(sr.ReadLine());

                            string tmp_line = sr.ReadLine();

                            while ((tmp_line != "$EndNodes") && (tmp_line != null))
                            {
                                nodes.Add(new Node(
                                        Convert.ToInt32(tmp_line.Split(' ')[0], CultureInfo.InvariantCulture),
                                        Convert.ToDouble(tmp_line.Split(' ')[1], CultureInfo.InvariantCulture),
                                        Convert.ToDouble(tmp_line.Split(' ')[2], CultureInfo.InvariantCulture),
                                        Convert.ToDouble(tmp_line.Split(' ')[3], CultureInfo.InvariantCulture)
                                        )
                                    );

                                tmp_line = sr.ReadLine();
                            }
                        }

                        if (line == "$Elements")
                        {
                            int count_elements = Convert.ToInt32(sr.ReadLine(), CultureInfo.InvariantCulture);

                            string tmp_string = sr.ReadLine();

                            while ((tmp_string != "$EndElements") && (tmp_string != null))
                            {
                                switch (Convert.ToInt32(tmp_string.Split(' ')[1], CultureInfo.InvariantCulture))
                                {
                                    // Если элемент является Линией.
                                    case 1:
                                        {
                                            string[] line_string = tmp_string.Split(' ');
                                            line_list.Add(new Line(
                                                Convert.ToInt32(line_string[5], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(line_string[6], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(line_string[3], CultureInfo.InvariantCulture)
                                                ));
                                            break;
                                        }
                                    // Если элемент является Поверхностью.
                                    case 2:
                                        {
                                            string[] surface_string = tmp_string.Split(' ');
                                            surface_list.Add(new Surface(
                                                Convert.ToInt32(surface_string[5], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(surface_string[6], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(surface_string[7], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(surface_string[3], CultureInfo.InvariantCulture)
                                                ));
                                            break;
                                        }
                                    // Это тоже какая-то фигура, но пока на будушее.
                                    //case 3:
                                    //    {

                                    //        break;
                                    //    }
                                    // Если элемент является Тетраэдром.
                                    case 4:
                                        {
                                            string[] tetrahedron_string = tmp_string.Split(' ');
                                            tetrahedron_list.Add(new Thetrahedron(
                                                Convert.ToInt32(tetrahedron_string[5], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(tetrahedron_string[6], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(tetrahedron_string[7], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(tetrahedron_string[8], CultureInfo.InvariantCulture),
                                                Convert.ToInt32(tetrahedron_string[3], CultureInfo.InvariantCulture)
                                                ));
                                            break;
                                        }
                                    default:
                                        break;
                                }

                                tmp_string = sr.ReadLine();
                            }
                        }
                    }
                }
            }
            catch (Exception exep)
            {
                Console.WriteLine(exep.Message);
            }

            return nodes;
        }
    }
}
