using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//Строитель(Builder) - шаблон проектирования, который инкапсулирует создание 
//объекта и позволяет разделить его на различные этапы.

//Когда использовать паттерн Строитель?
//  1) Когда процесс создания нового объекта не должен зависеть от 
//  того, из каких частей этот объект состоит и как эти части связаны между собой
//  2) Когда необходимо обеспечить получение различных вариаций объекта в процессе 
//  его создания

// Пример применения 
public class Builder : MonoBehaviour
{
    void Start()
    {
        // содаем объект пекаря
        Baker baker = new Baker();
        // создаем билдер для ржаного хлеба
        BreadBuilder builder = new RyeBreadBuilder();
        // выпекаем
        Bread ryeBread = baker.Bake(builder);
        Debug.Log(ryeBread.ToString());
        // оздаем билдер для пшеничного хлеба
        builder = new WheatBreadBuilder();
        Bread wheatBread = baker.Bake(builder);
        Debug.Log(wheatBread.ToString());
    }
}

    class Bread
    {
        // мука
        public Flour flour { get; set; }
        // соль
        public Salt salt { get; set; }
        // пищевые добавки
        public Additives Additives { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (flour != null)
                sb.Append(flour.Sort + "\n");
            if (salt != null)
                sb.Append("Соль \n");
            if (Additives != null)
                sb.Append("Добавки: " + Additives.Name + " \n");
            return sb.ToString();
        }
    }
    // мука
    class Flour
    {
        // какого сорта мука
        public string Sort { get; set; }
    }
    class Salt { }
    // пищевые добавки
    class Additives { public string Name { get; set; } }

    // абстрактный класс строителя
    abstract class BreadBuilder
    {
        public Bread Bread { get; private set; }
        public void CreateBread()
        {
            Bread = new Bread();
        }

        public abstract void SetFlour();
        public abstract void SetSalt();
        public abstract void SetAdditives();
    }

    class Baker
    {
        public Bread Bake(BreadBuilder breadBuilder)
        {
            breadBuilder.CreateBread();
            breadBuilder.SetFlour();
            breadBuilder.SetSalt();
            breadBuilder.SetAdditives();
            return breadBuilder.Bread;
        }
    }

    // строитель для ржаного хлеба
    class RyeBreadBuilder : BreadBuilder
    {
        public override void SetFlour()
        {
            this.Bread.flour = new Flour { Sort = "Ржаная мука 1 сорт" };
        }

        public override void SetSalt()
        {
            this.Bread.salt = new Salt();
        }

        public override void SetAdditives()
        {
            // не используется
        }
    }
    // строитель для пшеничного хлеба
    class WheatBreadBuilder : BreadBuilder
    {
        public override void SetFlour()
        {
            this.Bread.flour = new Flour { Sort = "Пшеничная мука высший сорт" };
        }

        public override void SetSalt()
        {
            this.Bread.salt = new Salt();
        }

        public override void SetAdditives()
        {
            this.Bread.Additives = new Additives { Name = "улучшитель хлебопекарный" };
        }
    }

// Формальное Определение
namespace Formal
{
    class Client
    {
        void Start()
        {
            Builders builder = new ConcreteBuilder();
            Director director = new Director(builder);
            director.Construct();
            Product product = builder.GetResult();
        }
    }
    //Director: распорядитель - создает объект, используя объекты Builder
    class Director
    {
        Builders buidler;
        public Director (Builders builder)
        {
            this.buidler = builder;
        }
        public void Construct()
        {
            buidler.BuildPartA();
            buidler.BuildPartB();
            buidler.BuildPartC();
        }
    }
    //Builder: определяет интерфейс для создания различных частей объекта Product
    abstract class Builders
    {
        public abstract void BuildPartA();
        public abstract void BuildPartB();
        public abstract void BuildPartC();
        public abstract Product GetResult();
    }
    //Product: представляет объект, который должен быть создан.
    //В данном случае все части объекта заключены в списке parts.
    class Product
    {
        List<object> parts = new List<object>();
        public void Add (string part)
        {
            parts.Add(part);
        }
    }
    //ConcreteBuilder: конкретная реализация Buildera.
    //Создает объект Product и определяет интерфейс для доступа к нему
    class ConcreteBuilder : Builders
    {
        Product product = new Product();
        public override void BuildPartA()
        {
            product.Add("Part A");
        }
        public override void BuildPartB()
        {
            product.Add("Part B");
        }
        public override void BuildPartC()
        {
            product.Add("Part C");
        }
        public override Product GetResult()
        {
            return product;
        }
    }
}


