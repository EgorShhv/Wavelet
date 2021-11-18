using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForGraphicsMethods
{
    //класс фигура, общий класс с методами для фигур
    public abstract class Figure
    {
        public int material; //материал
        

        //перевод фигуры в строку
        public abstract string toString();
    }
}
