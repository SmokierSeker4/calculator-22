using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace калькулятор_22
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Выберите операцию:");
                Console.WriteLine("1. Сложение");
                Console.WriteLine("2. Вычитание");
                Console.WriteLine("3. Умножение");
                Console.WriteLine("4. Деление");
                Console.WriteLine("5. Возведение в степень");
                Console.WriteLine("6. Синус");
                Console.WriteLine("7. Косинус");
                Console.WriteLine("8. Тангенс");
                Console.WriteLine("9. Натуральный логарифм");
                Console.WriteLine("10. Выход");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 11)
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                double result = 0;

                switch (choice)
                {
                    case 1:
                        result = Add();
                        break;
                    case 2:
                        result = Subtract();
                        break;
                    case 3:
                        result = Multiply();
                        break;
                    case 4:
                        result = Divide();
                        break;
                    case 5:
                        result = Power();
                        break;
                    case 6:
                        result = Sin();
                        break;
                    case 7:
                        result = Cos();
                        break;
                    case 8:
                        result = Tan();
                        break;
                    case 9:
                        result = Log();
                        break;
                    case 10:
                        result = Exp();
                        break;
                    default:
                        Console.WriteLine("Ошибка: некорректный выбор операции.");
                        continue;
                }

                Console.WriteLine($"Результат: {result}");
            }
        }

        static double Add()
        {
            Console.Write("Введите первое число: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите второе число: ");
            double b = double.Parse(Console.ReadLine());
            return a + b;
        }

        static double Subtract()
        {
            Console.Write("Введите уменьшаемое число: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите вычитаемое число: ");
            double b = double.Parse(Console.ReadLine());
            return a - b;
        }

        static double Multiply()
        {
            Console.Write("Введите первое число: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите второе число: ");
            double b = double.Parse(Console.ReadLine());
            return a * b;
        }

        static double Divide()
        {
            Console.Write("Введите делимое число: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите делитель: ");
            double b = double.Parse(Console.ReadLine());
            if (b == 0)
            {
                Console.WriteLine("Ошибка: деление на ноль.");
                return 0;
            }
            return a / b;
        }

        static double Power()
        {
            Console.Write("Введите число: ");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите степень: ");
            double b = double.Parse(Console.ReadLine());
            return Math.Pow(a, b);
        }

        static double Sin()
        {
            Console.Write("Введите угол в радианах: ");
            double angle = double.Parse(Console.ReadLine());
            return Math.Sin(angle);
        }

        static double Cos()
        {
            Console.Write("Введите угол в радианах: ");
            double angle = double.Parse(Console.ReadLine());
            return Math.Cos(angle);
        }

        static double Tan()
        {
            Console.Write("Введите угол в радианах: ");
            double angle = double.Parse(Console.ReadLine());
            return Math.Tan(angle);
        }

        static double Log()
        {
            Console.Write("Введите число для вычисления натурального логарифма: ");
            double number = double.Parse(Console.ReadLine());
            return Math.Log(number);
        }

        static double Exp()
        {
            Console.Write("Введите число для вычисления экспоненты: ");
            double number = double.Parse(Console.ReadLine());
            return Math.Exp(number);
        }
    }
}
    
