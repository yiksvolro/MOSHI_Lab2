using System;
using System.Linq.Dynamic.Core;

namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Введіть ваше рівняння: ");
            string expression = Console.ReadLine();
            var lambda = DynamicExpressionParser.ParseLambda(typeof(double), typeof(double), expression);
            Func<double, double> func = (Func<double, double>)lambda.Compile();
            Console.WriteLine("Введіть нижню межу а = ");
            double a = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введіть верхню межу b = ");
            double b = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введіть кількість точок n = ");
            int n = Convert.ToInt32(Console.ReadLine());
            double result1 = MonteCarlo.Integrate(func, a, b, n,true);
            double result2 = MonteCarlo.Integrate(func, a, b, 100000000,false);
            Console.WriteLine("I = " + result1);
            GetErrors(result1, result2);
            MonteCarlo.CreateChart(func, a, b);
        }
        private static void GetErrors(double result, double resultExact)
        {
            double errorAbs = 0;
            double errorRel = 0;
            if(result != 0)
            {
                errorAbs = Math.Abs(resultExact - result);
                if(resultExact == 0)
                {
                    errorRel = 0;
                }
                else
                {
                    errorRel = errorAbs / Math.Abs(resultExact);    
                }

            }
            Console.WriteLine("Абсолютна похибка = " + errorAbs);
            Console.WriteLine("Відносна похибка = " + errorRel);


        }
    }
}
