using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Object = System.Object;

// Одиночка (Singleton, Синглтон) - порождающий паттерн, который гарантирует, 
//что для определенного класса будет создан только один объект, а также предоставит 
//к этому объекту точку доступа.

//Когда надо использовать Синглтон? Когда необходимо, чтобы для класса существовал 
//только один экземпляр

public class Singleton : MonoBehaviour
{
    private void Start()
    {
        // у нас не получится изменить ОС, так как объект уже создан
        Computer comp = new Computer();
        comp.Launch("Windows 9.1");
        //comp.OS = OS.getInstance("Windows 11");
        Debug.Log(comp.OS.Name);

        (new Thread(() =>  // создаем новый поток который обращается к синглтону
        {
            Computer comp2 = new Computer();
            comp2.Launch("Windows 11.1");
            Debug.Log(comp2.OS.Name);
        })).Start();
    }
}

class Computer
{
    public OS OS { get; set; }
    public void Launch(string osName)
    {
        OS = OS.getInstance(osName);
    }
}

class OS // потокобезопасная реализация
{
    private static OS instance;

    public string Name { get; private set; }

    private static object syncRoot = new Object();

    protected OS(string name)
    {
        this.Name = name;
    }

    public static OS getInstance(string name)
    {
        if (instance == null)
        {
            lock (syncRoot)
            {
                if (instance == null)
                    instance = new OS(name);
            }
        }
        return instance;
    }
}
//Потокобезопасная реализация без использования lock
//public class Singleton
//{
//    private static readonly Singleton instance = new Singleton();

//    public string Name { get; private set; }

//    private Singleton()
//    {
//        Name = System.Guid.NewGuid().ToString();
//    }

//    public static Singleton GetInstance()
//    {
//        return instance;
//    }
//}

// потоконебезопасная реализация
//class OS
//{
//    private static OS instance;

//    public string Name { get; private set; }

//    protected OS(string name)
//    {
//        this.Name = name;
//    }

//    public static OS getInstance(string name)
//    {
//        if (instance == null)
//            instance = new OS(name);
//        return instance;
//    }
//}

//Lazy-реализация
//public class Singleton
//{
//    public string Name { get; private set; }

//    private Singleton()
//    {
//        Name = System.Guid.NewGuid().ToString();
//    }

//    public static Singleton GetInstance()
//    {
//        return Nested.instance;
//    }

//    private class Nested
//    {
//        internal static readonly Singleton instance = new Singleton();
//    }
//}

//Реализация через класс Lazy<T>
//    public class Singleton
//    {
//    private static readonly Lazy<Singleton> lazy =
//        new Lazy<Singleton>(() => new Singleton());

//    public string Name { get; private set; }

//    private Singleton()
//    {
//        Name = System.Guid.NewGuid().ToString();
//    }

//    public static Singleton GetInstance()
//    {
//        return lazy.Value;
//    }
//}