using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Шаблонный метод (Template Method) определяет общий алгоритм поведения подклассов, 
/// позволяя им переопределить отдельные шаги этого алгоритма без изменения его 
/// структуры.
/// </summary>

///Когда использовать шаблонный метод?

///-Когда планируется, что в будущем подклассы должны будут переопределять 
///различные этапы алгоритма без изменения его структуры

///-Когда в классах, реализующим схожий алгоритм, происходит дублирование кода.
///Вынесение общего кода в шаблонный метод уменьшит его дублирование в подклассах.

// Формальная реализация
/// AbstractClass: определяет шаблонный метод TemplateMethod(), который реализует 
/// алгоритм. 
/// Алгоритм может состоять из последовательности вызовов других методов, 
/// часть из которых может быть абстрактными и должны быть переопределены в 
/// классах-наследниках. При этом сам метод TemplateMethod(), представляющий 
/// структуру алгоритма, переопределяться не должен.
namespace Template
{
    abstract class AbstractClass
    {
        public void TemplateMethod()
        {
            Operation1();
            Operation2();
        }
        public abstract void Operation1();
        public abstract void Operation2();
    }
    /// ConcreteClass: подкласс, который может переопределять различные методы 
    /// родительского класса.
    class ConcreteClass : AbstractClass
    {
        public override void Operation1() { }
        public override void Operation2() { }
    }


    public class Template : MonoBehaviour
    {
        void Start()
        {
            School school = new School();
            University university = new University();

            school.Learn();
            university.Learn();
        }
    }

    class School : Education
    {
        public override void Enter()
        {
            Debug.Log("Идем в первый класс");
        }

        public override void Study()
        {
            Debug.Log("Посещаем уроки, делаем домашние задания");
        }

        public override void GetDocument()
        {
            Debug.Log("Получаем аттестат о среднем образовании");
        }
    }

    class University : Education
    {
        public override void Enter()
        {
            Debug.Log("Сдаем вступительные экзамены и поступаем в ВУЗ");
        }

        public override void Study()
        {
            Debug.Log("Посещаем лекции");
            Debug.Log("Проходим практику");
        }

        public override void PassExams()
        {
            Debug.Log("Сдаем экзамен по специальности");
        }

        public override void GetDocument()
        {
            Debug.Log("Получаем диплом о высшем образовании");
        }
    }

    abstract class Learning
    {
        public abstract void Learn();
    }

    /// <summary>
    /// Также надо отметить ситуацию с наследованием базового класса. 
    /// Например, у нас может быть ситуация, когда базовый класс Education сам 
    /// наследует метод Learn от другого класса:
    /// </summary>
    abstract class Education : Learning
    {
        public sealed override void Learn()
        {
            Enter();
            Study();
            PassExams();
            GetDocument();
        }

        /// <summary>
        /// При этом в базовом классе необязательно определять все методы алгоритма 
        /// как абстрактные. Можно определить реализацию методов по умолчанию, как в 
        /// случае с методом PassExams().
        /// </summary>
        public abstract void Enter();
        public abstract void Study();
        public virtual void PassExams()
        {
            Debug.Log("Сдаем выпускные экзамены");
        }
        public abstract void GetDocument();
    }


}



