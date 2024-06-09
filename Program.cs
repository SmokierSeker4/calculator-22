using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorCsharp
{
    public interface IOperation
    {
        string Name { get; }
        double Run(params double[] numbers);
    }

    public interface IOperationProvider
    {
        IEnumerable<IOperation> Get();
    }

    public interface IMenu<out T>
    {
        IMenu<T> Show();
        IMenuItemSelector<T> ItemSelector { get; }
    }

    public interface IMenuItemSelector<out T>
    {
        T Select();
    }

    public interface IOperationMenuItemSelector : IMenuItemSelector<IOperation>
    {
    }

    public interface IMenuItemSelectorProvider
    {
        int GetMenuItemId();
    }

    internal class LocalInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<Application>().LifestyleSingleton(),

                Component.For<IOperationMenuItemSelector>()
                    .ImplementedBy<OperationMenuItemSelector>()
                    .LifestyleTransient(),

                Component.For<IMenuItemSelectorProvider>()
                    .ImplementedBy<OperationMenuItemSelectorView>()
                    .LifestyleTransient(),

                Component.For<IOperationProvider>()
                    .ImplementedBy<OperationProvider>()
                    .LifestyleSingleton(),

                Component.For<IMenu<IOperation>>()
                    .ImplementedBy<OperationMenu>()
                    .LifestyleTransient(),

                Classes.FromThisAssembly()
                    .BasedOn<IOperation>()
                    .WithServiceBase()
                    .LifestyleTransient()
            );
        }
    }

    public class Program
    {
        private static IWindsorContainer _container = new WindsorContainer();

        public static void Main()
        {
            try
            {
                Start();
                var app = _container.Resolve<Application>();
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                _container?.Dispose();
            }
        }

        private static void Start()
        {
            _container.Kernel.AddFacility<StartableFacility>(f => f.DeferredStart());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            _container.Install(new LocalInstaller());
        }
    }

    public class Application
    {
        private readonly IMenu<IOperation> _menu;

        public Application(IMenu<IOperation> menu)
        {
            _menu = menu;
        }

        public void Run()
        {
            while (true)
            {
                var operation = _menu.Show().ItemSelector.Select();
                double result = operation.Run(10, 5); 
                Console.WriteLine($"Результат: {result}");
                Console.WriteLine("Нажмите 'q' для выхода или любую другую клавишу для продолжения...");
                if (Console.ReadLine().ToLower() == "q") break;
            }
        }
    }

    public class OperationProvider : IOperationProvider
    {
        private readonly IEnumerable<IOperation> _operations;

        public OperationProvider(IEnumerable<IOperation> operations)
        {
            _operations = operations;
        }

        public IEnumerable<IOperation> Get()
        {
            return _operations;
        }
    }

    public class OperationMenu : IMenu<IOperation>
    {
        private readonly IOperationProvider _operationProvider;

        public OperationMenu(IOperationProvider operationProvider, IOperationMenuItemSelector menuItemSelector)
        {
            _operationProvider = operationProvider;
            ItemSelector = menuItemSelector;
        }

        public IMenuItemSelector<IOperation> ItemSelector { get; }

        public IMenu<IOperation> Show()
        {
            Console.WriteLine("======== КАЛЬКУЛЯТОР ==========");
            int i = 1;
            foreach (var operation in _operationProvider.Get())
            {
                Console.WriteLine($"{i++}. ОПЕРАЦИЯ {operation.Name};");
            }
            return this;
        }
    }

    public class OperationMenuItemSelectorView : IMenuItemSelectorProvider
    {
        public int GetMenuItemId()
        {
            Console.Write("Выберите действие: ");
            return Convert.ToInt32(Console.ReadLine());
        }
    }

    public class OperationMenuItemSelector : IOperationMenuItemSelector
    {
        private readonly IMenuItemSelectorProvider _selector;
        private readonly IOperationProvider _operationProvider;

        public OperationMenuItemSelector(IMenuItemSelectorProvider selector, IOperationProvider operationProvider)
        {
            _selector = selector;
            _operationProvider = operationProvider;
        }

        public IOperation Select()
        {
            int id = _selector.GetMenuItemId();
            return _operationProvider.Get().ElementAt(id - 1);
        }
    }

    public abstract class Operation : IOperation
    {
        protected Operation(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract double Run(params double[] numbers);
    }

    public class Addition : Operation
    {
        public Addition() : base("Сложение") { }

        public override double Run(params double[] numbers)
        {
            return numbers.Sum();
        }
    }

    public class Substraction : Operation
    {
        public Substraction() : base("Вычитание") { }

        public override double Run(params double[] numbers)
        {
            double result = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                result -= numbers[i];
            }
            return result;
        }
    }

    public class Multiplication : Operation
    {
        public Multiplication() : base("Умножение") { }

        public override double Run(params double[] numbers)
        {
            double result = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                result *= numbers[i];
            }
            return result;
        }
    }

    public class Division : Operation
    {
        public Division() : base("Деление") { }

        public override double Run(params double[] numbers)
        {
            double result = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                result /= numbers[i];
            }
            return result;
        }
    }

    public class Sqrt : Operation
    {
        public Sqrt() : base("Квадратный корень") { }

        public override double Run(params double[] numbers)
        {
            return Math.Sqrt(numbers[0]);
        }
    }

    public class Cos : Operation
    {
        public Cos() : base("Косинус") { }

        public override double Run(params double[] numbers)
        {
            return Math.Cos(numbers[0]);
        }
    }

    public class Sin : Operation
    {
        public Sin() : base("Синус") { }

        public override double Run(params double[] numbers)
        {
            return Math.Sin(numbers[0]);
        }
    }

    public class Tg : Operation
    {
        public Tg() : base("Тангенс") { }

        public override double Run(params double[] numbers)
        {
            return Math.Tan(numbers[0]);
        }
    }
}
