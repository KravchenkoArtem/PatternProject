using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Паттерн "Команда" (Command) позволяет инкапсулировать запрос на выполнение 
/// определенного действия в виде отдельного объекта. Этот объект запроса на 
/// действие и называется командой. При этом объекты, инициирующие запросы на 
/// выполнение действия, отделяются от объектов, которые выполняют это действие.
/// 
/// Команды могут использовать параметры, которые передают ассоциированную с 
/// командой информацию.Кроме того, команды могут ставиться в очередь и также 
/// могут быть отменены.
/// </summary>

///Когда использовать команды?

///-Когда надо передавать в качестве параметров определенные действия, вызываемые в ответ на другие действия.То есть когда необходимы функции обратного действия в ответ на определенные действия.
///-Когда необходимо обеспечить выполнение очереди запросов, а также их возможную отмену.
///-Когда надо поддерживать логгирование изменений в результате запросов. Использование логов может помочь восстановить состояние системы - для этого необходимо будет использовать последовательность запротоколированных команд.

//  Формальное определение на языке C# выглядеть следующим образом:
namespace Command
{
    ///Command: интерфейс, представляющий команду.Обычно определяет метод Execute() 
    ///для выполнения действия, а также нередко включает метод Undo(), реализация 
    ///которого должна заключаться в отмене действия команды
    abstract class Commands
    {
        public abstract void Execute();
        public abstract void Undo();
    }
    //конкретная комманда
    /// <summary>
    /// ConcreteCommand: конкретная реализация команды, реализует метод Execute(), 
    /// в котором вызывается определенный метод, определенный в классе Receiver
    /// </summary>
    class ConcreteCommand : Commands
    {
        Receiver receiver;
        public ConcreteCommand(Receiver r)
        {
            receiver = r;
        }
        public override void Execute()
        {
            receiver.Operation();
        }
        public override void Undo() { }
    }

    // получатель команды
    /// <summary>
    /// Receiver: получатель команды. Определяет действия, которые должны 
    /// выполняться в результате запроса.
    /// </summary>
    class Receiver
    {
        public void Operation() { }
    }

    // инициатор команды
    /// <summary>
    /// Invoker: инициатор команды - вызывает команду для выполнения 
    /// определенного запроса
    /// </summary>
    class Invoker
    {
        Commands command;
        public void SetCommand(Commands c)
        {
            command = c;
        }
        public void Run()
        {
            command.Execute();
        }
        public void Cancel()
        {
            command.Undo();
        }
    }
    /// <summary>
    /// Client: клиент - создает команду и устанавливает ее получателя с 
    /// помощью метода SetCommand()
    /// </summary>
    class Client
    {
        void Main()
        {
            Invoker invoker = new Invoker();
            Receiver receiver = new Receiver();
            ConcreteCommand command = new ConcreteCommand(receiver);
            invoker.SetCommand(command);
            invoker.Run();
        }
    }
    ///Таким образом, инициатор, отправляющий запрос, ничего не знает о получателе, 
    ///который и будет выполнять команду. Кроме того, если нам потребуется применить 
    ///какие-то новые команды, мы можем просто унаследовать классы от абстрактного 
    ///класса Command и реализовать его методы Execute и Undo.


    ///Нередко в роли инициатора команд выступают панели управления или кнопки 
    ///интерфейса.Самая простая ситуация - надо программно организовать включение и 
    ///выключение прибора, например, телевизора.Решение данной задачи с помощью 
    ///команд могло бы выглядеть так:

    public class Command : MonoBehaviour
    {
        MultiPult pult = new MultiPult();
        TV tv = new TV();
        Volume volume = new Volume();
        /// <summary>
        /// Здесь два получателя команд - классы TV и Volume. Volume управляет 
        /// уровнем звука и сохраняет текущий уровень в переменной level. 
        /// Также есть две команды TVOnCommand и VolumeCommand.
        /// </summary>
        private void Start()
        {
            pult.SetCommand(0, new TVOnCommand(tv));
            pult.SetCommand(1, new VolumeCommand(volume));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // on tv
                pult.PressButton(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                // off tv
                pult.PressUndo();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                // up volume
                pult.PressButton(1);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                // undo volume
                pult.PressUndo();
            }
        }
    }
    interface ICommand
    {
        void Execute();
        void Undo();
    }
    // Receiver - Получатель
    class TV
    {
        public void On()
        {
            Debug.Log("TV is on");
        }

        public void Off()
        {
            Debug.Log("TV is off");
        }
    }

    class TVOnCommand : ICommand
    {
        TV tv;
        bool b;
        public TVOnCommand(TV tvSet)
        {
            tv = tvSet;
        }
        public void Execute()
        {
            if (b == false)
            {
                b = true;
                tv.On();
            }
        }
        public void Undo()
        {
            if (b == true)
            {
                b = false;
                tv.Off();
            }
        }
    }

    class VolumeCommand : ICommand
    {
        Volume volume;
        public VolumeCommand(Volume v)
        {
            volume = v;
        }

        public void Execute()
        {
            volume.RaiseLevel();
        }
        public void Undo()
        {
            volume.DropLevel();
        }
    }

    class Volume
    {
        public const int OFF = 0;
        public const int HIGH = 20;
        private int level;

        public Volume()
        {
            level = OFF;
        }

        public void RaiseLevel()
        {
            if (level < HIGH)
                level++;
            Debug.Log(string.Format("Уровень звука {0}", level));
        }

        public void DropLevel()
        {
            if (level > OFF)
                level--;
            Debug.Log(string.Format("Уровень звука {0}", level));
        }
    }

    // Invoker - инициатор
    /// <summary>
    /// При этом пульт ничего не знает об объекте TV. Он только знает, 
    /// как отправить команду. В итоге мы получаем гибкую систему, в которой 
    /// мы легко можем заменять одни команды на другие, создавать последовательности 
    /// команд. Например, в нашей программе кроме телевизора появилась микроволновка, 
    /// которой тоже неплохо было бы управлять с помощью одного интерфейса. 
    /// Для этого достаточно добавить соответствующие классы и установить команду:
    /// 
    /// Инициатор - MultiPult имеет две кнопки в виде массива commands: первая 
    /// предназначена для TV, а вторая - для увеличения уровня звука. Чтобы 
    /// сохранить историю команд используется стек. При отправке команды в стек 
    /// добавляется новый элемент, а при ее отмене, наоборот, происходит удаление 
    /// из стека. В данном случае стек выполняет роль примитивного лога команд.
    /// </summary>
    class MultiPult
    {
        ICommand[] commands;
        Stack<ICommand> commandsHistory;

        public MultiPult()
        {
            commands = new ICommand[2];
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = new NoCommand();
            }
            commandsHistory = new Stack<ICommand>();
        }

        public void SetCommand(int number, ICommand com)
        {
            commands[number] = com;
        }

        /// <summary>
        /// Правда, в вышеописанной системе есть один изъян: если мы попытаемся 
        /// выполнить команду до ее назначения, то программа выдаст исключение, 
        /// так как команда будет не установлена. Эту проблему мы могли бы решить, 
        /// проверяя команду на значение null в классе инициатора:
        /// </summary>
        public void PressButton(int number)
        {
            //if (command != null)
            commands[number].Execute();
            // добавляем выполненную команду в историю команд
            commandsHistory.Push(commands[number]);
        }
        public void PressUndo()
        {
            //if (command != null)
            if (commandsHistory.Count > 0)
            {
                ICommand undoCommand = commandsHistory.Pop();
                undoCommand.Undo();
            }
        }
    }
    ///Либо можно определить класс пустой команды, которая будет устанавливаться 
    ///по умолчанию:
    class NoCommand : ICommand
    {
        public void Execute()
        { }
        public void Undo()
        { }
    }
}

//https://metanit.com/sharp/patterns/3.3.php - Макрокоманды
