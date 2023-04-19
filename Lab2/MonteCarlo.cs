using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace Lab2
{
    public class MonteCarlo
    {
        private static Random random = new Random();
        private static List<double> xIn = new List<double>();
        private static List<double> yIn = new List<double>();
        private static List<double> xOut = new List<double>();
        private static List<double> yOut = new List<double>();

        public static double Integrate(Func<double, double> f, double a, double b, int n, bool flag)
        {
            double maxVal = Enumerable.Range(0, (int)((b - a) / 0.1) + 1)
                .Select(i => a + i * 0.1)
                .Select(x => f(x))
                .Max();
            int pointsInside = 0;
            for (int i = 0; i < n; i++)
            {
                double X = GetRandomNumber(a, b);
                double Y = GetRandomNumber(0, maxVal);
                if(Y <= f(X))
                {
                    pointsInside++;
                }
                if (flag && Y <= f(X))
                {
                    xIn.Add(X);
                    yIn.Add(Y);
                }
                else if (flag)
                {
                    xOut.Add(X);
                    yOut.Add(Y);
                }
            }

            double average = (double)pointsInside / n;
            double result = (b - a) * maxVal / average;

            return result;
        }

        private static double GetRandomNumber(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
        public static void CreateChart(Func<double, double> f, double a, double b)
        {
            Chart chart = new Chart();

            chart.ChartAreas.Add(new ChartArea());
            chart.Series.Add(new Series("Series1"));

            chart.Series["Series1"].ChartType = SeriesChartType.Point;
            chart.Series["Series1"].MarkerSize = 5;
            chart.Series["Series1"].MarkerColor = Color.Green;
            for (int i = 0; i < xIn.Count; i++)
            {
                chart.Series["Series1"].Points.AddXY(xIn[i], yIn[i]);
            }

            chart.Series.Add(new Series("Series2"));
            chart.Series["Series2"].ChartType = SeriesChartType.Point;
            chart.Series["Series2"].MarkerSize = 5;
            chart.Series["Series2"].MarkerColor = Color.Red;
            for (int i = 0; i < xIn.Count; i++)
            {
                chart.Series["Series2"].Points.AddXY(xOut[i], yOut[i]);
            }

            chart.Series.Add(new Series("Series3"));
            chart.Series["Series3"].ChartType = SeriesChartType.Line;
            chart.Series["Series3"].BorderWidth = 3;

            for (double x = a; x < b; x += 0.01)
            {
                chart.Series["Series3"].Points.AddXY(x, f(x));
            }
            chart.Size = new Size(1920, 1080);
            chart.SaveImage("chart.png", ChartImageFormat.Png);
        
        }
    }
}
