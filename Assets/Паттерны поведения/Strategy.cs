using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Паттерн Стратегия (Strategy) представляет шаблон проектирования, 
/// который определяет набор алгоритмов, инкапсулирует каждый из них и 
/// обеспечивает их взаимозаменяемость. В зависимости от ситуации мы можем 
/// легко заменить один используемый алгоритм другим. При этом замена алгоритма 
/// происходит независимо от объекта, который использует данный алгоритм.
/// </summary>

/// Когда использовать стратегию?

//-Когда есть несколько родственных классов, которые отличаются поведением.Можно задать один основной класс, а разные варианты поведения вынести в отдельные классы и при необходимости их применять

//-Когда необходимо обеспечить выбор из нескольких вариантов алгоритмов, которые можно легко менять в зависимости от условий

//-Когда необходимо менять поведение объектов на стадии выполнения программы

//-Когда класс, применяющий определенную функциональность, ничего не должен знать о ее реализации

/// Формальное определение паттерна на языке C# может выглядеть следующим образом:
public class FormalStrategy
{
    /// <summary>
    /// Интерфейс IStrategy, который определяет метод Algorithm(). 
    /// Это общий интерфейс для всех реализующих его алгоритмов. 
    /// Вместо интерфейса здесь также можно было бы использовать абстрактный класс.
    /// </summary>
    public interface IStrategy
    {
        void Algorithm();
    }

    /// <summary>
    /// Классы ConcreteStrategy1 и ConcreteStrategy, 
    /// которые реализуют интерфейс IStrategy, предоставляя свою версию метода 
    /// Algorithm(). Подобных классов-реализаций может быть множество.
    /// </summary>
    public class ConcreteStrategy1 : IStrategy
    {
        public void Algorithm()
        { }
    }

    public class ConcreteStrategy2 : IStrategy
    {
        public void Algorithm()
        { }
    }

    /// <summary>
    /// Класс Context хранит ссылку на объект IStrategy и связан с интерфейсом 
    /// IStrategy отношением агрегации.
    /// </summary>
    public class Context
    {
        ///В данном случае объект IStrategy заключена в свойстве 
        ///ContextStrategy, хотя также для нее можно было бы определить 
        ///приватную переменную, а для динамической установки использовать 
        ///специальный метод.
        public IStrategy ContextStrategy { get; set; }

        public Context(IStrategy _strategy)
        {
            ContextStrategy = _strategy;
        }

        public void ExecuteAlgorithm()
        {
            ContextStrategy.Algorithm();
        }
    }
}
/// <summary>
/// Теперь рассмотрим конкретный пример. Существуют различные легковые машины, 
/// которые используют разные источники энергии: электричество, бензин, газ и 
/// так далее. Есть гибридные автомобили. В целом они похожи и отличаются 
/// преимущественно видом источника энергии. Не говоря уже о том, что мы можем 
/// изменить применяемый источник энергии, модифицировав автомобиль. И в данном 
/// случае вполне можно применить паттерн стратегию:
/// </summary>
public class Strategy : MonoBehaviour
{
    private void Start()
    {
        Cars auto = new Cars(4, "Mercedes", new PetrolMove());
        auto.Moves();
        auto.Movable = new ElectricMove();
        auto.Moves();
        
    }
}

public interface IMove
{
    void Move();
}

class PetrolMove : IMove
{
    public void Move()
    {
        Debug.Log("Перемещение на бензине");
    }
}

class ElectricMove : IMove
{
    public void Move()
    {
        Debug.Log("Перемещение на электричестве");
    }
}

public class Cars
{
    protected int passengers; // кол-во пассажиров
    protected string model; // модель автомобиля

    public Cars(int num, string model, IMove mov)
    {
        this.passengers = num;
        this.model = model;
        Movable = mov;
    }
    public IMove Movable { private get; set; }
    public void Moves()
    {
        Movable.Move();
    }
}
///В данном случае в качестве IStrategy выступает интерфейс IMovable, 
///определяющий метод Move(). А реализующий этот интерфейс семейство алгоритмов 
///представлено классами ElectricMove и PetroleMove. И данные алгоритмы использует 
///класс Car.

