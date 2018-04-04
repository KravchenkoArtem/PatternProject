using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Фабричный метод(Factory Method) - это паттерн, который определяет интерфейс 
//для создания объектов некоторого класса, но непосредственное решение о том, 
//объект какого класса создавать происходит в подклассах.То есть паттерн 
//предполагает, что базовый класс делегирует создание объектов классам-наследникам.

//Когда надо применять паттерн
//1) Когда заранее неизвестно, объекты каких типов необходимо создавать
//2) Когда система должна быть независимой от процесса создания новых 
//объектов и расширяемой: в нее можно легко вводить новые классы, объекты 
//которых система должна создавать.
//3) Когда создание новых объектов необходимо делегировать из базового 
//класса классам наследникам

public class FabricPattern : MonoBehaviour
{
    //Абстрактный класс Product определяет интерфейс класса, 
    //объекты которого надо создавать.
    abstract class Product { }
    //Конкретные классы ConcreteProductA и ConcreteProductB представляют реализацию 
    //класса Product.Таких классов может быть множество
    class ConcreteProductA : Product { }
    class ConcreteProductB : Product { }

    //Абстрактный класс Creator определяет абстрактный фабричный метод 
    //FactoryMethod(), который возвращает объект Product.
    abstract class Creator
    {
        public abstract Product FactoryMethod();
    }
    //Конкретные классы ConcreteCreatorA и ConcreteCreatorB - наследники класса 
    //Creator, определяющие свою реализацию метода FactoryMethod(). 
    //Причем метод FactoryMethod() каждого отдельного класса-создателя возвращает 
    //определенный конкретный тип продукта.Для каждого конкретного класса 
    //продукта определяется свой конкретный класс создателя.
    class ConcreteCreatorA : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }
    class ConcreteCreatorB : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }
    //Таким образом, класс Creator делегирует создание объекта Product своим 
    //наследникам.А классы ConcreteCreatorA и ConcreteCreatorB могут 
    //самостоятельно выбирать какой конкретный тип продукта им создавать.

    // Реальный пример
    private void Start()
    {
        Developer dev = new PanelDeveloper("ОО ШлакоблокСтрой");
        House house = dev.Create();

        dev = new WoodenDeveloper("Гавнострой");
        House house2 = dev.Create();
    }
    // абстрактный класс строительной компании
    abstract class Developer
    {
        public string Name { get; set; } // обязывает реализовать имя компании. 

        public Developer (string n)
        {
            Name = n;
        }
        // Фабричный метод
        abstract public House Create(); // и создать какой либо из домов.
    }
    // строит панельные дома
    class PanelDeveloper : Developer
    {
        public PanelDeveloper(string n) : base(n) { Debug.Log(n.ToString()); }

        public override House Create()
        {
            return new PanelHouse();
        }
    }
    // строит деревянные дома
    class WoodenDeveloper : Developer
    {
        public WoodenDeveloper(string n) : base(n) { Debug.Log(n.ToString()); }

        public override House Create()
        {
            return new WoodenHouse();
        }
    }

    class PanelHouse : House
    {
        public PanelHouse()
        {
            Debug.Log("Панельный дом построен");
        }
    }

    class WoodenHouse : House
    {
        public WoodenHouse()
        {
            Debug.Log("Деревянный дом построен");
        }
    }

    abstract class House { }

    //Таким образом, система получится легко расширяемой.Правда, 
    //недостатки паттерна тоже очевидны - для каждого нового продукта 
    //необходимо создавать свой класс создателя.
}
