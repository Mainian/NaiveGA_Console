using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveGA_Console
{
    public class Point
    {
        public Point(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    class Program
    {
        static double acceptableCost = 2.0;
        static int popultionCount = 24;
        Point start = new Point(1, 1, 1,);
        Point target = new Point(11, 11, 11);

        void Main(string[] args)
        {
            //Point pointA = new Point(1, 1, 1);
            //Point pointB = new Point(11, 11, 11);
            Point pointD, pointN;

            double min = -20.0;
            double max = -20.0;

            List<Point> population = initialPopulation(min, max);
            population.Sort(ComparePoints);
        }

        double cost(Point position, Point target)
        {
            return Math.Sqrt(Math.Pow(position.X-target.X,2.0) + Math.Pow(position.Y - target.Y,2.0) + Math.Pow(position.Z - target.Z,2.0));
        }

        List<Point> initialPopulation(double min, double max)
        {
            List<Point> pop = new List<Point>();
            Random random = new Random();

            for (int i = 0; i < popultionCount; i++)
            {
                double randomX = random.NextDouble() * (max - min) + min;
                double randomY = random.NextDouble() * (max - min) + min;
                double randomZ = random.NextDouble() * (max - min) + min;

                pop.Add(new Point(randomX, randomY, randomZ));
            }

            return pop;
        }

        //void sortList(ref List<Point> list)
        //{
        //    list.Sort(ComparePoints);
        //}

        private int ComparePoints(Point pointOne, Point pointTwo)
        {
            //smaller cost is best
            if(pointOne == null)
            {
                if(pointTwo == null) //they are equal
                    return 0;
                else //pointOne is null and pointTwo is not null. PointTwo is greater
                    return -1;
            }
            else
            {
                //If x is not null
                if(pointTwo == null) //y is null, x is greater
                    return 1;
                else
                {
                    double cost_one = cost(pointOne, target);
                    double cost_two = cost(pointTwo, target);

                    if(cost_one > cost_two) // lower cost is better
                        return -1; //pointTwo is better
                    else if(cost_two > cost_one)
                        return 1; //pointOne is better
                    else // they are the same
                        return 0;
                }
            }
        }
    }
}
