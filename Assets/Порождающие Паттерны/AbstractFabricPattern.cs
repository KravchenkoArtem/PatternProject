using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Паттерн "Абстрактная фабрика" (Abstract Factory) предоставляет интерфейс для 
//создания семейств взаимосвязанных объектов с определенными интерфейсами без 
//указания конкретных типов данных объектов.

//Когда использовать абстрактную фабрику
//1) Когда система не должна зависеть от способа создания и компоновки 
//новых объектов
//2) Когда создаваемые объекты должны использоваться вместе и являются взаимосвязанными

public class AbstractFabricPattern : MonoBehaviour {

    //Абстрактный класс фабрики AbstractFactory определяет методы для создания 
    //объектов.Причем методы возвращают абстрактные продукты, а не их конкретные 
    //реализации.
    abstract class AbstractFactory
    {
        public abstract AbstractProductA CreateProductA();
        public abstract AbstractProductB CreateProductB();
    }
    //Конкретные классы фабрик ConcreteFactory1 и ConcreteFactory2 реализуют 
    //абстрактные методы базового класса и непосредственно определяют какие 
    //конкретные продукты использовать
    class ConcreteFactory1 : AbstractFactory
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA1();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB1();
        }
    }

    class ConcreteFactory2 : AbstractFactory
    {
        public override AbstractProductA CreateProductA()
        {
            return new ProductA2();
        }

        public override AbstractProductB CreateProductB()
        {
            return new ProductB2();
        }
    }
    //Абстрактные классы AbstractProductA и AbstractProductB определяют 
    //интерфейс для классов, объекты которых будут создаваться в программе.
    abstract class AbstractProductA { }
    abstract class AbstractProductB { }
    //Конкретные классы ProductA1 / ProductA2 и ProductB1 / ProductB2 представляют 
    //конкретную реализацию абстрактных классов
    class ProductA1 : AbstractProductA { }
    class ProductB1 : AbstractProductB { }
    class ProductA2 : AbstractProductA { }
    class ProductB2 : AbstractProductB { }

    //Класс клиента Client использует класс фабрики для создания объектов.
    //При этом он использует исключительно абстрактный класс фабрики AbstractFactory 
    //и абстрактные классы продуктов AbstractProductA и AbstractProductB и никак не 
    //зависит от их конкретных реализаций
    class Client
    {
        private AbstractProductA abstractProductA;
        private AbstractProductB abstractProductB;

        public Client (AbstractFactory factory)
        {
            abstractProductB = factory.CreateProductB();
            abstractProductA = factory.CreateProductA();
        }

        public void Run() { }
    }

    //Реальный пример
    private void Start()
    {
        Hero elf = new Hero(new ElfFactory());
        elf.Hit();
        elf.Run();

        Hero voin = new Hero(new VoinFactory());
        voin.Hit();
        voin.Run();
    }

    // абстрактный класс - оружие
    abstract class Weapon
    {
        public abstract void Hit();
    }

    // абстрактный класс движение
    abstract class Movement
    {
        public abstract void Move();
    }

    // класс арбалет
    class Arbalet : Weapon
    {
        public override void Hit()
        {
            Debug.Log("Стреляем из арбалета");
        }
    }
    // класс меч
    class Sword : Weapon
    {
        public override void Hit()
        {
            Debug.Log("Бьем мечом");
        }
    }

    // движение - бег
    class RunMovement : Movement
    {
        public override void Move()
        {
            Debug.Log("Бежим");
        }
    }
    // движение полета
    class FlyMovement : Movement
    {
        public override void Move()
        {
            Debug.Log("Летим");
        }
    }
    // класс абстрактной фабрики 
    abstract class HeroFactory
    {
        public abstract Movement CreateMovement();
        public abstract Weapon CreateWeapon();
    }
    // Фабрика создания летящего героя с арбалетом
    class ElfFactory : HeroFactory
    {
        public override Movement CreateMovement()
        {
            return new FlyMovement();
        }

        public override Weapon CreateWeapon()
        {
            return new Arbalet();
        }
    }

    // Фабрика создания бегущего героя с мечом
    class VoinFactory : HeroFactory
    {
        public override Movement CreateMovement()
        {
            return new RunMovement();
        }

        public override Weapon CreateWeapon()
        {
            return new Sword();
        }
    }
    // клиент - герой
    class Hero
    {
        private Weapon weapon;
        private Movement movement;
        public Hero (HeroFactory factory)
        {
            weapon = factory.CreateWeapon();
            movement = factory.CreateMovement();
        }

        public void Run()
        {
            movement.Move();
        }
        public void Hit()
        {
            weapon.Hit();
        }
    }

}
