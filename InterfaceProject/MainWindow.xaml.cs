using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.IO;
using System.Security;
using Prism.Services.Dialogs;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace InterfaceProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DeleteImage.IsEnabled = false;
            ClearImages.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                int count_images = 0;
                // Read the files
                foreach (String file in openFileDialog.FileNames)
                {
                    // Create
                    try
                    {
                        ImageListBox.Items.Add(file);
                        count_images++;
                    }
                    catch (SecurityException ex)
                    {
                        ProgressConsole.Text += ("\n> Security error. Please contact your administrator for details.\n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details (send to Support):\n\n" + ex.StackTrace + "\n"
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image - probably related to Windows file system permissions.
                        ProgressConsole.Text += ("\n> Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message + "\n");
                    }
                }
                ProgressConsole.Text += "\n> Загруженно " + count_images + " изображение(й)\n";
                DeleteImage.IsEnabled = true;
                ClearImages.IsEnabled = true;
            }
        }

        private void ImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageListBox.SelectedIndex >= 0 && ImageListBox.SelectedIndex < ImageListBox.Items.Count)
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri((string)ImageListBox.SelectedItem);
                bi3.EndInit();
                CurrentImage.Source = bi3;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.Items.Count > 0)
            {
                ImageListBox.Items.RemoveAt(ImageListBox.SelectedIndex);
                if (ImageListBox.Items.Count == 0)
                {
                    DeleteImage.IsEnabled = false;
                    ClearImages.IsEnabled = false;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ImageListBox.Items.Clear();
            DeleteImage.IsEnabled = false;
            ClearImages.IsEnabled = false;
        }

        // _____________________________________________________________________________________________________________________
        // _____________________________________________________________________________________________________________________

        Grid MainGrid = new Grid(@"C:\Users\egshh\source\repos\Read_mesh\Read_mesh\meshh200.msh");

        public float Length(Vector3 A, Vector3 B)
        {
            return (float)Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y) + (A.Z - B.Z) * (A.Z - B.Z));
        }
        public static double Length(Point A, Point B)
        {
            return Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y) + (A.z - B.z) * (A.z - B.z));
        }
        public static float ScalarMult(Vector3 A, Vector3 B)
        {
            return A.X * B.X + A.Y * B.Y + A.Z * B.Z;
        }
        public static float Length(Vector3 A)
        {
            return (float)Math.Sqrt((A.X) * (A.X) + (A.Y) * (A.Y) + (A.Z) * (A.Z));
        }
        public static Vector3 Rotation_X(Vector3 A, float angle_rad)
        {
            Vector3 new_A = new Vector3();

            float cos = (float)Math.Cos(angle_rad);
            float sin = (float)Math.Sin(angle_rad);

            new_A.X = A.X;
            new_A.Y = cos * A.Y - sin * A.Z;
            new_A.Z = sin * A.Y + cos * A.Z;

            return new_A;
        }
        public static Vector3 Rotation_Y(Vector3 A, float angle_rad)
        {
            Vector3 new_A = new Vector3();

            float cos = (float)Math.Cos(angle_rad);
            float sin = (float)Math.Sin(angle_rad);

            new_A.X = cos * A.X + sin * A.Z;
            new_A.Y = A.Y;
            new_A.Z = -sin * A.X + cos * A.Z;

            return new_A;
        }
        public static Vector3 Rotation_Z(Vector3 A, float angle_rad)
        {
            Vector3 new_A = new Vector3();

            float cos = (float)Math.Cos(angle_rad);
            float sin = (float)Math.Sin(angle_rad);

            new_A.X = cos * A.X - sin * A.Y;
            new_A.Y = sin * A.X + cos * A.Y;
            new_A.Z = A.Z;

            return new_A;
        }
        public static Vector3 Rotation(Vector3 A, Vector3 angle_rad, bool is_rotation_X, bool is_rotation_Y, bool is_rotation_Z)
        {
            Vector3 new_A = new Vector3(A);

            if (is_rotation_X) new_A = Rotation_X(new_A, angle_rad.X);
            if (is_rotation_Y) new_A = Rotation_Y(new_A, angle_rad.Y);
            if (is_rotation_Z) new_A = Rotation_Z(new_A, angle_rad.Z);

            return new_A;
        }
        public static Vector3 Normal(List<Vector3> A)
        {
            Vector3 n = new Vector3();
            n.X = (A[1].Y - A[0].Y) * (A[2].Z - A[0].Z) - (A[1].Z - A[0].Z) * (A[2].Y - A[0].Y);
            n.Y = (A[1].Z - A[0].Z) * (A[2].X - A[0].X) - (A[1].X - A[0].X) * (A[1].Z - A[0].Z);
            n.Z = (A[1].X - A[0].X) * (A[2].Y - A[0].Y) - (A[1].Y - A[0].Y) * (A[2].X - A[0].X);
            n = n / Length(n);

            return n;
        }
        public static Vector3 Normal(Vector3 A0, Vector3 A1, Vector3 A2)
        {
            Vector3 n = new Vector3();
            n.X = (A1.Y - A0.Y) * (A2.Z - A0.Z) - (A1.Z - A0.Z) * (A2.Y - A0.Y);
            n.Y = (A1.Z - A0.Z) * (A2.X - A0.X) - (A1.X - A0.X) * (A1.Z - A0.Z);
            n.Z = (A1.X - A0.X) * (A2.Y - A0.Y) - (A1.Y - A0.Y) * (A2.X - A0.X);
            n = n / Length(n);

            return n;
        }
        public static float det3(float[,] A)
        {
            return A[0, 0] * A[1, 1] * A[2, 2] + A[0, 1] * A[1, 2] * A[2, 0] + A[1, 0] * A[0, 2] * A[2, 1]
                  - A[0, 2] * A[1, 1] * A[2, 0] - A[0, 0] * A[2, 1] * A[1, 2] - A[0, 1] * A[1, 0] * A[2, 2];
        }
        public static double det3(double[,] A)
        {
            return A[0, 0] * A[1, 1] * A[2, 2] + A[0, 1] * A[1, 2] * A[2, 0] + A[1, 0] * A[0, 2] * A[2, 1]
                  - A[0, 2] * A[1, 1] * A[2, 0] - A[0, 0] * A[2, 1] * A[1, 2] - A[0, 1] * A[1, 0] * A[2, 2];
        }
        public static float[] Plane_eq(Vector3 P0, Vector3 P1, Vector3 P2)
        {
            float[] A = new float[4];

            float[,] Matr = new float[3, 3];
            Matr[0, 0] = 1; Matr[0, 1] = P0.Y; Matr[0, 2] = P0.Z;
            Matr[1, 0] = 1; Matr[1, 1] = P1.Y; Matr[1, 2] = P1.Z;
            Matr[2, 0] = 1; Matr[2, 1] = P2.Y; Matr[2, 2] = P2.Z;
            A[0] = det3(Matr);
            Matr[0, 0] = P0.X; Matr[0, 1] = 1; Matr[0, 2] = P0.Z;
            Matr[1, 0] = P1.X; Matr[1, 1] = 1; Matr[1, 2] = P1.Z;
            Matr[2, 0] = P2.X; Matr[2, 1] = 1; Matr[2, 2] = P2.Z;
            A[1] = det3(Matr);
            Matr[0, 0] = P0.X; Matr[0, 1] = P0.Y; Matr[0, 2] = 1;
            Matr[1, 0] = P1.X; Matr[1, 1] = P1.Y; Matr[1, 2] = 1;
            Matr[2, 0] = P2.X; Matr[2, 1] = P2.Y; Matr[2, 2] = 1;
            A[2] = det3(Matr);
            Matr[0, 0] = P0.X; Matr[0, 1] = P0.Y; Matr[0, 2] = P0.Z;
            Matr[1, 0] = P1.X; Matr[1, 1] = P1.Y; Matr[1, 2] = P1.Z;
            Matr[2, 0] = P2.X; Matr[2, 1] = P2.Y; Matr[2, 2] = P2.Z;
            A[3] = -det3(Matr);
            return A;
        }
        public static float[] Kramer(float[,] A, float[] F) //only for A[3,3]
        {
            float[] X = new float[3];

            for (int j = 0; j < X.Length; j++)
            {
                X[j] = 0;
            }

            float D = det3(A);
            float[,] A_new = new float[3, 3];
            for (int i = 0; i < F.Length; i++)
            {
                A_new[i, 0] = F[i];
                A_new[i, 1] = A[i, 1];
                A_new[i, 2] = A[i, 2];
            }
            X[0] = det3(A_new) / D;

            for (int i = 0; i < F.Length; i++)
            {
                A_new[i, 0] = A[i, 0];
                A_new[i, 1] = F[i];
                A_new[i, 2] = A[i, 2];
            }
            X[1] = det3(A_new) / D;

            for (int i = 0; i < F.Length; i++)
            {
                A_new[i, 0] = A[i, 0];
                A_new[i, 1] = A[i, 1];
                A_new[i, 2] = F[i];
            }
            X[2] = det3(A_new) / D;

            float[] Y = new float[F.Length];
            //float[] X_res = new float[3];
            for (int i = 0; i < F.Length; i++)
            {
                //X_res[i] = (float)X[i];
                Y[i] = 0;
                for (int j = 0; j < F.Length; j++)
                {
                    Y[i] += A[i, j] * X[j];
                }
                if (Math.Abs((Y[i] - F[i]) / F[i]) > 1E-5)
                {
                    MessageBox.Show("error in SLAE (Kramer)!!!(" + Y[i] + ", " + F[i] + ") => " + Math.Abs((Y[i] - F[i]) / F[i]) + "\n");
                }
            }

            return X;
        }
        public static Vector3 Progection_of_point_into_plane(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 X)
        {
            float[,] Matrix = new float[3, 3];
            float[] right = new float[3];

            Matrix[0, 0] = (P1.Y - P0.Y) * (P2.Z - P0.Z) - (P1.Z - P0.Z) * (P2.Y - P0.Y);
            Matrix[0, 1] = (P1.Z - P0.Z) * (P2.X - P0.X) - (P1.X - P0.X) * (P2.Z - P0.Z);
            Matrix[0, 2] = (P1.X - P0.X) * (P2.Y - P0.Y) - (P1.Y - P0.Y) * (P2.X - P0.X);

            Matrix[1, 0] = P1.X - P0.X;
            Matrix[1, 1] = P1.Y - P0.Y;
            Matrix[1, 2] = P1.Z - P0.Z;

            Matrix[2, 0] = P2.X - P0.X;
            Matrix[2, 1] = P2.Y - P0.Y;
            Matrix[2, 2] = P2.Z - P0.Z;

            right[0] = P0.X * Matrix[0, 0] + P0.Y * Matrix[0, 1] + P0.Z * P0.X * Matrix[0, 2];
            right[1] = X.X * Matrix[1, 0] + X.Y * Matrix[1, 1] + X.Z * P0.X * Matrix[1, 2];
            right[2] = X.X * Matrix[2, 0] + X.Y * Matrix[2, 1] + X.Z * P0.X * Matrix[2, 2];

            var _X = Kramer(Matrix, right);

            return new Vector3(_X[0], _X[1], _X[2]);
        }
        public static Vector3 ConvertPointIntoVector3(Point3D A)
        {
            return new Vector3((float)A.X, (float)A.Y, (float)A.Z);
            //return new Vector3((float)A.x, (float)A.y, (float)A.z);
        }
        public static float VectorsAngle(Vector3 A, Vector3 B)
        {
            return (float)Math.Acos(ScalarMult(A, B) / (Length(A) * Length(B)));
        }
        public static void ReadStringIntoInt(string text, out List<int> result)
        {
            result = new List<int>();

            String s_num = "";
            List<int> nums = new List<int>();
            List<int> div = new List<int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != ' ' && text[i] != '\n' && text[i] != ',' && text[i] != '-')
                {
                    s_num += text[i]; // делаешь с символами то, что тебе нужно
                }
                else
                {
                    if (s_num != "")
                    {
                        nums.Add(Convert.ToInt32(s_num));
                        div.Add(0);
                        s_num = "";
                    }
                }

                if (text[i] == '-')
                {
                    div[nums.Count - 1] = 1;
                }

                if (i == (text.Length - 1) && text[i] != ' ' && text[i] != '\n' && text[i] != ',')
                {
                    nums.Add(Convert.ToInt32(s_num));
                    div.Add(0);
                }
            }

            for (int i = 0; i < nums.Count; i++)
            {
                if (div[i] == 1 && i != (nums.Count - 1))
                {

                    for (int j = nums[i]; j < nums[i + 1]; j++)
                    {
                        result.Add(j);
                    }
                }
                else
                {
                    result.Add(nums[i]);
                }
            }
            return;
        }

        public class Camera
        {
            public Vector3 position;
            public Vector3 view;
            public Vector3 up;
            public float offset_step;
            public float zoom_step;
            public float zNear;
            public float zFar;

            public Camera()
            {
                zNear = 0.1f;
                zFar = 1000.0f;

                position = new Vector3();
                position.X = (float)(400);
                position.Y = (float)(400);
                position.Z = (float)(400);

                view = new Vector3();
                view = new Vector3(0, 0, 0) - position;
                up = new Vector3(-1f, 1f, -1.0f);

                offset_step = 1f;
                zoom_step = 1f;
            }
            public Camera(Grid base_grid)
            {
                zNear = 0.1f;
                zFar = 3.0f * (float)Length(base_grid.top_point, base_grid.bottom_point);

                position = new Vector3();
                position.X = (float)(base_grid.top_point.x * 2);
                position.Y = (float)(base_grid.top_point.y * 2);
                position.Z = (float)(base_grid.top_point.z * 2);

                view = new Vector3();
                Vector3 grid_centr = new Vector3();
                grid_centr.X = (float)((base_grid.top_point.x + base_grid.bottom_point.x) / 2.0);
                grid_centr.Y = (float)((base_grid.top_point.y + base_grid.bottom_point.y) / 2.0);
                grid_centr.Z = (float)((base_grid.top_point.z + base_grid.bottom_point.z) / 2.0);
                view = grid_centr - position;
                view = view / Length(view);

                Vector3 OZ = new Vector3(0, 0, 1);
                float angle_OZ_view = (float)Math.Acos(ScalarMult(OZ, view) / (Length(OZ) * Length(view)));
                //scaling
                OZ = (float)(Length(view) / Math.Sin(angle_OZ_view - Math.PI / 2.0)) * OZ;
                up = new Vector3(view + OZ);
                up = up / Length(up);

                offset_step = (float)(Length(base_grid.top_point, base_grid.bottom_point) * 0.01); //1%
                zoom_step = (float)(Length(base_grid.top_point, base_grid.bottom_point) * 0.005);
            }
        }
        Camera camera = new Camera();

        //public class PointLight
        //{
        //    public Vector3 position;
        //    public Vector4 ambient; //Мощность фонового освещения
        //    public Vector4 diffuse; //Мощность рассеянного освещения
        //    public Vector4 specular; //Мощность отраженного освещения
        //    public Vector3 attenuation; //Коэффициенты затухания

        //    public float constant;
        //    public float linear;
        //    public float quadratic;
        //};
        //public PointLight point_light;
        //public class Material
        //{
        //    public Vector3 ambient; //цвет под фоновым освещением
        //    public Vector3 diffuse; //цвет под рассеяннным освещением
        //    public Vector3 specular; //цвет под отраженным освещением

        //    public float shininess; //радиус рассеяния
        //};

        //public class ViewGrid
        //{
        //    public List<List<int>> volumes_by_faces; //id+1 фейса, если id>0, значит положительная нормаль фейса считается внешней иначе надо нормали менять местами
        //    public List<Vector4> volumes_color;

        //    public List<Vector3> vertexes;
        //    public List<List<Vector3>> edges;
        //    public List<List<Vector3>> faces;
        //    public List<List<Vector3>> normal_for_face;

        //    public ViewGrid()
        //    {
        //    }
        //    public ViewGrid(Grid base_grid)
        //    {
        //        if (base_grid != null)
        //        {
        //            vertexes = new List<Vector3>();
        //            for (int i = 0; i < base_grid.nodes.Count(); i++)
        //            {
        //                vertexes.Add(new Vector3((float)base_grid.xyz[i].x, (float)base_grid.xyz[i].y, (float)base_grid.xyz[i].z));
        //            }

        //            edges = new List<List<Vector3>>();
        //            for (int i = 0; i < base_grid.lines.Count(); i++)
        //            {
        //                edges.Add(new List<Vector3>());
        //                int id_vertex = base_grid.lines[i].vertex_left;
        //                edges[i].Add(new Vector3((float)base_grid.xyz[id_vertex].x, (float)base_grid.xyz[id_vertex].y, (float)base_grid.xyz[id_vertex].z));
        //                id_vertex = base_grid.edges[i].vertex_right;
        //                edges[i].Add(new Vector3((float)base_grid.xyz[id_vertex].x, (float)base_grid.xyz[id_vertex].y, (float)base_grid.xyz[id_vertex].z));
        //            }

        //            faces = new List<List<Vector3>>();
        //            normal_for_face = new List<List<Vector3>>();
        //            for (int i = 0; i < base_grid.faces.Count(); i++)
        //            {
        //                faces.Add(new List<Vector3>());
        //                normal_for_face.Add(new List<Vector3>());

        //                faces[i].Add(new Vector3()); //id 0
        //                for (int j = 0; j < base_grid.faces[i].vertex.Count(); j++)
        //                {
        //                    int id_vertex = base_grid.faces[i].vertex[j];
        //                    //id j+1
        //                    faces[i].Add(new Vector3((float)base_grid.xyz[id_vertex].x, (float)base_grid.xyz[id_vertex].y, (float)base_grid.xyz[id_vertex].z));
        //                    faces[i][0] += faces[i][j + 1] / base_grid.faces[i].vertex.Count(); //central point
        //                }
        //                faces[i].Add(faces[i][1]); //final point

        //                for (int j = 0; j < faces[i].Count() - 2; j++)
        //                {
        //                    normal_for_face[i].Add(new Vector3(Normal(faces[i][0], faces[i][j + 1], faces[i][j + 2])));
        //                }
        //            }

        //            volumes_by_faces = new List<List<int>>();
        //            volumes_color = new List<Vector4>();
        //            for (int i = 0; i < base_grid.volumes.Count(); i++)
        //            {
        //                Vector3 volume_centr = new Vector3(ConvertPointIntoVector3(base_grid.GetCentrWeight(i)));

        //                volumes_by_faces.Add(new List<int>());
        //                for (int j = 0; j < base_grid.volumes[i].child_face.Count(); j++)
        //                {
        //                    volumes_by_faces[i].Add(base_grid.volumes[i].child_face[j] + 1);

        //                    float[] plane = Plane_eq(this.faces[volumes_by_faces[i][j] - 1][0],
        //                                             this.faces[volumes_by_faces[i][j] - 1][1],
        //                                             this.faces[volumes_by_faces[i][j] - 1][2]);
        //                    int signum_centr = Math.Sign(plane[0] * volume_centr.X + plane[1] * volume_centr.Y + plane[2] * volume_centr.Z + plane[3]);
        //                    if (signum_centr != 0)
        //                        volumes_by_faces[i][j] *= signum_centr;
        //                }

        //                Random rand_uniform = new Random(i);
        //                int r = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //                int g = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //                int b = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //                volumes_color.Add(new Vector4(r / 255.0f, g / 255.0f, b / 255.0f, 1.0f));
        //            }
        //        }
        //    }
        //}
        //public ViewGrid viewGrid;

        //public Vector3 CalcPointLight(PointLight point_light_curr, Material material, Vector3 normal, Vector3 fragPos, Vector3 viewDir)
        //{
        //    Vector3 lightDir = Vector3.Normalize(point_light_curr.position - fragPos);
        //    // diffuse shading
        //    float diff = Math.Max(Vector3.Dot(normal, lightDir), 0.0f);
        //    // specular shading
        //    System.Numerics.Vector3 lightDir_sn = new System.Numerics.Vector3(lightDir.X, lightDir.Y, lightDir.Z);
        //    System.Numerics.Vector3 normal_sn = new System.Numerics.Vector3(normal.X, normal.Y, normal.Z);
        //    System.Numerics.Vector3 reflectDir_sn = System.Numerics.Vector3.Reflect(-lightDir_sn, normal_sn);
        //    Vector3 reflectDir = new Vector3(reflectDir_sn.X, reflectDir_sn.Y, reflectDir_sn.Z);
        //    float spec = (float)Math.Pow(Math.Max(Vector3.Dot(viewDir, reflectDir), 0.0f), material.shininess);
        //    // attenuation затухание
        //    float distance = Length(point_light_curr.position - fragPos);
        //    float attenuation = 1.0f / (point_light_curr.constant + point_light_curr.linear * distance +
        //                   point_light_curr.quadratic * (distance * distance));
        //    // combine results
        //    //Vector3 ambient = point_light_curr.ambient * Vector3(GL.tex(material.diffuse, GL.TEX TexCoords));
        //    //Vector3 diffuse = point_light_curr.diffuse * diff * Vector3(texture(material.diffuse, TexCoords));
        //    //Vector3 specular = point_light_curr.specular * spec * Vector3(texture(material.specular, TexCoords));
        //    material.ambient *= attenuation;
        //    material.diffuse *= attenuation;
        //    material.specular *= attenuation;
        //    return (material.ambient + material.diffuse + material.specular);
        //}

        private void ShowVertexes()
        {
            foreach (var vertex in MainGrid.nodes)
            {
                GL.Begin(PrimitiveType.Points);
                GL.Color3((byte)120, (byte)37, (byte)37);
                GL.PointSize(1000);
                GL.Vertex3((float)vertex.get_x(), (float)vertex.get_y(), (float)vertex.get_z());
                GL.End();
            }
        }
        //private void ShowEdges()
        //{
        //    foreach (var edge in MainGrid.lines)
        //    {

        //        Random rand_uniform = new Random(0);
        //        int r = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //        int g = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //        int b = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));

        //        GL.Begin(PrimitiveType.Lines);
        //        GL.LineWidth(2);
        //        GL.Color3((byte)200, (byte)200, (byte)200);
        //        GL.Vertex3(edge[0]);
        //        GL.Vertex3(edge[1]);
        //        GL.End();
        //    }
        //}
        //private void ShowVolumes_withLight()
        //{
        //    int id_el = 0;
        //    foreach (var elem in MainGrid.Elems)
        //    {
        //        foreach (var face in elem.local_faces)
        //        {
        //            GL.Begin(PrimitiveType.Polygon);

        //            Random rand_uniform = new Random(id_el);
        //            int r = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //            int g = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //            int b = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));

        //            GL.Color3((byte)r, (byte)g, (byte)b);
        //            //GL.Color3((byte)255, (byte)0, (byte)0);
        //            foreach (var local_id_vertex in face)
        //            {
        //                int global_id_vertex = elem.nodes[local_id_vertex];
        //                Point vertex = MainGrid.xyz[global_id_vertex];
        //                GL.Vertex3((float)vertex.x, (float)vertex.y, (float)vertex.z);
        //            }
        //            GL.End();
        //        }
        //        id_el++;
        //    }
        //}
        //private void ShowVolumes_withoutLight()
        //{
        //    int id_el = 0;
        //    //var elem = MainGrid.Elems[0];
        //    foreach (var elem in MainGrid.Elems)
        //    {
        //        Random rand_uniform = new Random(id_el);
        //        int r = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //        int g = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));
        //        int b = (int)(rand_uniform.NextDouble() * Math.Abs(255 - 0) + Math.Min(0, 255));

        //        foreach (var face in elem.local_faces)
        //        {
        //            GL.Begin(PrimitiveType.Polygon);
        //            GL.Color3((byte)r, (byte)g, (byte)b);
        //            //GL.Color3((byte)255, (byte)0, (byte)0);
        //            foreach (var local_id_vertex in face)
        //            {
        //                int global_id_vertex = elem.nodes[local_id_vertex];
        //                Point vertex = MainGrid.xyz[global_id_vertex];
        //                GL.Vertex3((float)vertex.x, (float)vertex.y, (float)vertex.z);
        //            }
        //            GL.End();
        //        }
        //        id_el++;
        //    }
        //}
        //private void ShowVolumes_withoutLight_byTriangles()
        //{
        //    List<int> id_elements;
        //    ReadStringIntoInt(List_volume_number.Text, out id_elements);
        //    id_elements.Sort();

        //    int k = 0;
        //    if (VolumeInTransparent.IsChecked == true)
        //    {
        //        ShowFaces_withoutLight_byTriangles(0.2f, id_elements);
        //    }
        //    foreach (var id_elem in id_elements)
        //    {
        //        var elem = viewGrid.volumes_by_faces[id_elem];

        //        foreach (var id_face in elem)
        //        {
        //            GL.Begin(PrimitiveType.TriangleFan);
        //            GL.Color4(viewGrid.volumes_color[id_elem]);
        //            foreach (var vertex in viewGrid.faces[Math.Abs(id_face) - 1])
        //            {
        //                GL.Vertex3(vertex);
        //            }
        //            GL.End();
        //        }
        //    }
        //}
        //private void ShowVolumes_withoutLight_byTriangles_all()
        //{
        //    for (int id_elem = 0; id_elem < viewGrid.volumes_by_faces.Count(); id_elem++)
        //    {
        //        var elem = viewGrid.volumes_by_faces[id_elem];

        //        foreach (var id_face in elem)
        //        {
        //            GL.Begin(PrimitiveType.TriangleFan);
        //            GL.Color4(viewGrid.volumes_color[id_elem]);
        //            foreach (var vertex in viewGrid.faces[Math.Abs(id_face) - 1])
        //            {
        //                GL.Vertex3(vertex);
        //            }
        //            GL.End();
        //        }
        //    }
        //}
        //private void ShowFaces_withoutLight_byTriangles(float alpha, List<int> non_print_elements)
        //{
        //    //var elem = viewGrid.volumes_by_faces[0];
        //    for (int id_faces = 0; id_faces < viewGrid.faces.Count(); id_faces++)
        //    {
        //        bool find_in_del = false;
        //        foreach (var id_elem in non_print_elements)
        //        {
        //            foreach (var id_faces_in_elem in this.viewGrid.volumes_by_faces[id_elem])
        //            {
        //                if (Math.Abs(id_faces_in_elem) - 1 == id_faces)
        //                {
        //                    find_in_del = true;
        //                    break;
        //                }
        //            }
        //            if (find_in_del == true)
        //                break;
        //        }
        //        if (find_in_del == false)
        //        {
        //            var face = viewGrid.faces[id_faces];
        //            GL.Begin(PrimitiveType.TriangleFan);
        //            GL.Color4(106f / 255f, 106f / 255f, 90f / 205f, alpha);
        //            foreach (var vertex in face)
        //            {
        //                GL.Vertex3(vertex);
        //            }
        //            GL.End();
        //        }
        //    }
        //}

        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            // GL.ClearColor(new Color4((byte)164, (byte)197, (byte)245, (byte)100));
            GL.ClearColor(new Color4((byte)0, (byte)0, (byte)0, (byte)100));

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.ClearDepth(1000.0F);
        }

        private void glControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.PointSmooth);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.ClearDepth(1000.0F);

            GL.ClearColor(new Color4((byte)0, (byte)0, (byte)0, (byte)100));


            //camera
            Matrix4 view = Matrix4.LookAt(camera.position, camera.position + camera.view, camera.up);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)glControl.Width / (float)glControl.Height,
                camera.zNear,
                camera.zFar);
            view = view * perspective;
            //Matrix4 orthographic = Matrix4.CreateOrthographicOffCenter(-600, 600, -400, 400, -10, 1000);
            //view = view * orthographic;
            GL.LoadMatrix(ref view);

            // Включаем блендинг для отрисовки прозрачности
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); //(исходный, целевой)
            GL.Disable(EnableCap.CullFace); //выключить отсечение обратных поверхностей

            //GL.Enable(EnableCap.Lighting);
            //GL.Enable(EnableCap.Light0);
            //GL.PushMatrix();
            //GL.LoadIdentity();
            //lighting
            //float[] l_diffuse = { 1f, 1f, 1f };
            //float[] l_pos = { 250, 250, 50, 1f };
            //float[] s_dir = { 0, 0, -1, 1 };
            //GL.Light(LightName.Light0, LightParameter.Diffuse, l_diffuse);
            //GL.Light(LightName.Light0, LightParameter.Position, l_pos);
            //GL.Light(LightName.Light0, LightParameter.SpotDirection, s_dir);
            //GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 0.5f);
            // GL.Light(LightName.Light0, LightParameter.LinearAttenuation, 0.2f);
            //GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, 0.4f);
            //GL.Light(LightName.Light0, LightParameter.ConstantAttenuation, 0.1f);
            //GL.Light(LightName.Light0, LightParameter.LinearAttenuation, 0.6f);
            //GL.Light(LightName.Light0, LightParameter.QuadraticAttenuation, 10f);

            //GL.Enable(EnableCap.Light1);
            //GL.PushMatrix();
            //GL.LoadIdentity();
            //lighting
            //float[] l_diffuse_1 = { 10f, 10f, 10f };
            //float[] l_pos_1 = { 50, 300, 300, 1f };
            //float[] s_dir_1 = { 0, 0, -1, 1 };
            //GL.Light(LightName.Light1, LightParameter.Diffuse, l_diffuse_1);
            //GL.Light(LightName.Light1, LightParameter.Position, l_pos_1);
            //GL.Light(LightName.Light1, LightParameter.ConstantAttenuation, 0.001f);
            //GL.Light(LightName.Light1, LightParameter.LinearAttenuation, 0.06f);
            //GL.Light(LightName.Light1, LightParameter.QuadraticAttenuation, 10f);
            // GL.PopMatrix();

            // Draw objects here
            GL.Begin(PrimitiveType.Points);
            GL.PointSize(2);
            GL.Color3((byte)255, (byte)0, (byte)0);
            GL.Vertex3((float)0, (float)0, (float)0);
            GL.End();

            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3((byte)255, (byte)0, (byte)0);
            GL.Vertex3((float)MainGrid.bottom_point.x * 2, (float)0, (float)0);
            GL.Vertex3((float)MainGrid.top_point.x * 2, (float)0, (float)0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3((byte)0, (byte)255, (byte)0);
            GL.Vertex3((float)0, (float)MainGrid.bottom_point.y * 2, (float)0);
            GL.Vertex3((float)0, (float)MainGrid.top_point.y * 2, (float)0);
            GL.End();
            GL.Begin(PrimitiveType.Lines);
            GL.Color3((byte)0, (byte)0, (byte)255);
            GL.Vertex3((float)0, (float)0, (float)MainGrid.bottom_point.z * 2);
            GL.Vertex3((float)0, (float)0, (float)MainGrid.top_point.z * 2);
            GL.End();

            ShowVertexes();

            //ShowVolumes_withoutLight_byTriangles();
            //ShowVolumes_withoutLight_byTriangles();
            //ShowFaces_withoutLight_byTriangles();
            //ShowEdges();
            //ShowFaces_withoutLight_byTriangles(0.3f, new List<int>());
            //ShowVertexes();

            //ShowVolumes_withLight();



            glControl.SwapBuffers();
        }

        private void glControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    //Forward 
            //    case System.Windows.Forms.Keys.Z: camera.position += camera.view * camera.zoom_step; break;
            //    //Backwards
            //    case System.Windows.Forms.Keys.X: camera.position -= camera.view * camera.zoom_step; break;
            //    //Left
            //    case System.Windows.Forms.Keys.A: camera.position -= Vector3.Normalize(Vector3.Cross(camera.view, camera.up)) * camera.offset_step; break;
            //    //Right
            //    case System.Windows.Forms.Keys.D: camera.position += Vector3.Normalize(Vector3.Cross(camera.view, camera.up)) * camera.offset_step; break;
            //    //Up
            //    case System.Windows.Forms.Keys.W: camera.position += camera.up * camera.offset_step; break;
            //    //Down
            //    case System.Windows.Forms.Keys.S: camera.position -= camera.up * camera.offset_step; break;
            //}
            //cam_position.Text = "Camera position is (" + camera.position.X + "; " + camera.position.Y + "; " + camera.position.Z + ")";
            //glControl.Invalidate();
        }
        private void glControl_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        //public MeshView(Grid MainGrid)
        //{
        //    this.MainGrid = MainGrid;

        //    //Point start = new Point(MainGrid.xyz_sort[MainGrid.xyz_sort.Count() - 1] + 0.1* MainGrid.xyz_sort[MainGrid.xyz_sort.Count() - 1]);
        //    if (MainGrid != null)
        //    {
        //        this.MainGrid.CreateTopology();
        //        this.viewGrid = new ViewGrid(this.MainGrid);
        //        camera = new Camera(this.MainGrid);

        //    }

        //    InitializeComponent();

        //    if (MainGrid != null)
        //    {
        //        Volume_number.Text = "Всего объемов " + this.viewGrid.volumes_by_faces.Count();
        //        //List_volume_number.Text = "0-" + (this.viewGrid.volumes_by_faces.Count()-1);
        //        List_volume_number.Text = "0-" + (this.viewGrid.volumes_by_faces.Count() - 1);
        //    }
        //}
        private void RotationCamera_Rotation_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Vector3 angle = new Vector3();
            //    bool is_rotation_X = false;
            //    bool is_rotation_Y = false;
            //    bool is_rotation_Z = false;

            //    Vector3 rotation_axis_0 = new Vector3();
            //    rotation_axis_0.X = (float)((MainGrid.top_point.x + MainGrid.bottom_point.x) / 2.0);
            //    rotation_axis_0.Y = (float)((MainGrid.top_point.y + MainGrid.bottom_point.y) / 2.0);
            //    rotation_axis_0.Z = (float)((MainGrid.top_point.z + MainGrid.bottom_point.z) / 2.0);
            //    Vector3 move_to_Axis = new Vector3(rotation_axis_0);

            //    switch (Convert.ToString(((Button)e.OriginalSource).Name))
            //    {
            //        case "RotationCamera_X_minus":
            //            angle.X = (float)(-1.0 * Convert.ToDouble(AngleRotation_X.Text) * Math.PI / 180.0);
            //            is_rotation_X = true;
            //            rotation_axis_0.X = (float)MainGrid.bottom_point.x;
            //            move_to_Axis.X = 0;
            //            break;
            //        case "RotationCamera_X_plus":
            //            angle.X = (float)(1.0 * Convert.ToDouble(AngleRotation_X.Text) * Math.PI / 180.0);
            //            is_rotation_X = true;
            //            rotation_axis_0.X = (float)MainGrid.bottom_point.x;
            //            move_to_Axis.X = 0;
            //            break;
            //        case "RotationCamera_Y_minus":
            //            angle.Y = (float)(-1.0 * Convert.ToDouble(AngleRotation_Y.Text) * Math.PI / 180.0);
            //            is_rotation_Y = true;
            //            rotation_axis_0.Y = (float)MainGrid.bottom_point.y;
            //            move_to_Axis.Y = 0;
            //            break;
            //        case "RotationCamera_Y_plus":
            //            angle.Y = (float)(1.0 * Convert.ToDouble(AngleRotation_Y.Text) * Math.PI / 180.0);
            //            is_rotation_Y = true;
            //            move_to_Axis.Y = 0;
            //            rotation_axis_0.Y = (float)MainGrid.bottom_point.y;
            //            break;
            //        case "RotationCamera_Z_minus":
            //            angle.Z = (float)(-1.0 * Convert.ToDouble(AngleRotation_Z.Text) * Math.PI / 180.0);
            //            is_rotation_Z = true;
            //            move_to_Axis.Z = 0;
            //            rotation_axis_0.Z = (float)MainGrid.bottom_point.z;
            //            break;
            //        case "RotationCamera_Z_plus":
            //            angle.Z = (float)(1.0 * Convert.ToDouble(AngleRotation_Z.Text) * Math.PI / 180.0);
            //            is_rotation_Z = true;
            //            rotation_axis_0.Z = (float)MainGrid.bottom_point.z;
            //            move_to_Axis.Z = 0;
            //            break;
            //    }

            //    Vector3 new_cam_position = new Vector3();
            //    new_cam_position = Rotation(camera.position - move_to_Axis, angle, is_rotation_X, is_rotation_Y, is_rotation_Z) + move_to_Axis;

            //    Vector3 new_cam_view = new Vector3();
            //    new_cam_view = Rotation(camera.view, angle, is_rotation_X, is_rotation_Y, is_rotation_Z);
            //    new_cam_view /= Length(new_cam_view);

            //    Vector3 new_cam_up = new Vector3();
            //    new_cam_up = Rotation(camera.up, angle, is_rotation_X, is_rotation_Y, is_rotation_Z);
            //    new_cam_up /= Length(new_cam_up);

            //    camera.position = new_cam_position;
            //    camera.view = new_cam_view;
            //    camera.up = new_cam_up;

            //    System.Windows.Forms.PaintEventArgs a = null;

            //    glControl.Invalidate();
            //}
            //catch (Exception e1)
            //{
            //    MessageBox.Show("Error: " + e1.Message);
            //}
        }

        private void Crossection_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    float step = (float)Convert.ToDouble(Step_crossections.Text);
            //    float new_zNear = camera.zNear;
            //    switch (Convert.ToString(((Button)e.OriginalSource).Name))
            //    {
            //        case "Crossection_minus":
            //            new_zNear -= step;
            //            break;
            //        case "Crossection_plus":
            //            new_zNear += step;
            //            break;
            //    }

            //    if (new_zNear > 0 && new_zNear < camera.zFar)
            //    {
            //        camera.zNear = new_zNear;
            //        glControl.Invalidate();
            //    }
            //}
            //catch (Exception e1)
            //{
            //    MessageBox.Show("Error: " + e1.Message);
            //}
        }

        private void ViewVolumes_Click(object sender, RoutedEventArgs e)
        {
            glControl.Invalidate();
        }
        
        // _____________________________________________________________________________________________________________________
        // _____________________________________________________________________________________________________________________


    }
}
