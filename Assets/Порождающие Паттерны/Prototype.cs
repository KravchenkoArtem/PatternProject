using System;
using UnityEngine;
using System.IO; // запись считывание примитивных типов и т.д
using System.Runtime.Serialization; // используем для полного копирования
using System.Runtime.Serialization.Formatters.Binary; // используем для полного копирования
//Чтобы вручную не создавать у клонированного объекта вложенный объект Point, здесь используются механизмы бинарной сериализации.

//Паттерн Прототип(Prototype) позволяет создавать объекты на основе уже ранее 
//созданных объектов-прототипов.То есть по сути данный паттерн предлагает 
//технику клонирования объектов.

//Когда использовать Прототип?
//  1)Когда конкретный тип создаваемого объекта должен определяться 
//  динамически во время выполнения
//  2)Когда нежелательно создание отдельной иерархии классов фабрик 
//  для создания объектов-продуктов из параллельной иерархии классов
//  (как это делается, например, при использовании паттерна Абстрактная фабрика)
//  3)Когда клонирование объекта является более предпочтительным вариантом нежели 
//  его создание и инициализация с помощью конструктора.Особенно когда известно, 
//  что объект может принимать небольшое ограниченное число возможных состояний.

public class Prototype : MonoBehaviour
{

	void Start ()
    {
        IFigure figure = new Rectangle(30, 40);
        IFigure clonedFigure = figure.Clone();
        figure.GetInfo();
        clonedFigure.GetInfo();

        Circle figureCircle = new Circle(30,50,60);
        // применяем глубокое копирование
        Circle clonedCircleFigure = figureCircle.DeepCopy() as Circle;
        //Circle clonedCircleFigure = figureCircle.Clone() as Circle;
        figureCircle.Point.X = 100; // изменяем координаты начальной фигуры
        figureCircle.GetInfo(); // figureCircle.Point.x = 100
        clonedCircleFigure.GetInfo(); // clonedCircleFigure.Point.x = 100 // тоже является 100 т.к ссылочная переменная меняет значение и клона
    }

    interface IFigure
    {
        IFigure Clone();
        void GetInfo();
    }

    class Rectangle : IFigure
    {
        int width;
        int height;

        public Rectangle(int w, int h)
        {
            this.width = w;
            this.height = h;
        }

        public IFigure Clone()
        {
            return new Rectangle(this.width, this.height);
            //return this.MemberwiseClone() as IFigure;
            //В то же время надо учитывать, что метод MemberwiseClone() 
            //осуществляет неполное копирование - то есть копирование 
            //значимых типов. Если же класс фигуры содержал бы объекты 
            //ссылочных типов, то оба объекта после клонирования содержали 
            //бы ссылку на один и тот же ссылочный объект.Например, пусть 
            //фигура круг имеет свойство ссылочного типа
        }

        public void GetInfo()
        {
            Debug.Log(string.Format("Прямоугольник длиной {0} и шириной {1}", height, width));
        }
    }

    // Circle это тестирование MemberwiseClone() со ссылочными значением (class).
    [Serializable] //все классы, объекты которых подлежат копированию, должны быть помечены атрибутом Serializable.
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        //public Point(int X, int Y)
        //{
        //    this.X = X;
        //    this.Y = Y;
        //}
    }

    [Serializable]
    class Circle : IFigure
    {
        int radius;
        public Point Point { get; set; }

        public Circle(int r, int x, int y)
        {
            this.radius = r;
            this.Point = new Point {X = x, Y = y }; // LOL
        }

        public IFigure Clone()
        {
            //return new Circle(this.radius);
            return this.MemberwiseClone() as IFigure;
            //В то же время надо учитывать, что метод MemberwiseClone() 
            //осуществляет неполное копирование - то есть копирование 
            //значимых типов. Если же класс фигуры содержал бы объекты 
            //ссылочных типов, то оба объекта после клонирования содержали 
            //бы ссылку на один и тот же ссылочный объект.Например, пусть 
            //фигура круг имеет свойство ссылочного типа.
        }
        //чтобы избежать этого надо применить полное копирование:

        public object DeepCopy()
        {
            object figure = null;
            using (MemoryStream tempStream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter(null,
                    new StreamingContext(StreamingContextStates.Clone));
                binFormatter.Serialize(tempStream, this);
                tempStream.Seek(0, SeekOrigin.Begin);

                figure = binFormatter.Deserialize(tempStream);
            }
            return figure;
        }

        public void GetInfo()
        {
            Debug.Log(string.Format("Круг радиусом {0} и центром в точке({1}, {2})", radius, Point.X, Point.Y));
        }
    }
}
//Client: создает объекты прототипов с помощью метода Clone()
class Client
{
    void Operation()
    {
        Prototyp prototype = new ConcretePrototyp1(1);
        Prototyp clone = prototype.Clone();
        prototype = new ConcretePrototyp2(2);
        clone = prototype.Clone();
    }
}

//Prototype: определяет интерфейс для клонирования самого себя, который, 
//как правило, представляет метод Clone()
abstract class Prototyp
{
    public int ID { get; private set; }
    public Prototyp(int id)
    {
        this.ID = id;
    }
    public abstract Prototyp Clone();
}

//ConcretePrototype1 и ConcretePrototype2: конкретные реализации прототипа.
//Реализуют метод Clone()
class ConcretePrototyp1 : Prototyp
{
    public ConcretePrototyp1(int id) : base(id) { }
    public override Prototyp Clone()
    {
        return new ConcretePrototyp1(ID);
    }
}

class ConcretePrototyp2 : Prototyp
{
    public ConcretePrototyp2(int id) : base(id) { }
    public override Prototyp Clone()
    {
        return new ConcretePrototyp2(ID);
    }
}



