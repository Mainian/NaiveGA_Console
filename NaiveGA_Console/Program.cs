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

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }

    public class Naive
    {
        static double acceptableCost = 1.1;
        static int popultionCount = 24;

        static double minStart = -20.0;
        static double maxStart = 20.0;

        static double minMutation = -0.5;
        static double maxMuation = 0.5;

        Point start = new Point(1, 1, 1);
        Point target = new Point(11, 11, 11);

        public void FindSolution()
        {
            Point answer = new Point(0, 0, 0);
            List<Point> population = initialPopulation(minStart, maxStart);
            int population_count = 1;

            while (!foundAnswer(population, out answer))
            {
                population = mutatePopulation(newPopulation(getPairings(parentPopulation(population))));
                population_count++;
            }

            Console.Out.WriteLine("Solution found in " + population_count + " populations.\n" + answer.ToString() + ". Cost = " + cost(answer, target));
            Console.Read();
            //return solution;
        }

        bool foundAnswer(List<Point> points, out Point point)
        {
            foreach (Point pt in points)
                if (cost(pt, target) <= acceptableCost)
                {
                    point = pt;
                    return true;
                }

            point = new Point(0, 0, 0);
            return false;
        }

        double cost(Point position, Point target)
        {
            return Math.Sqrt(Math.Pow(position.X - target.X, 2.0) + Math.Pow(position.Y - target.Y, 2.0) + Math.Pow(position.Z - target.Z, 2.0));
        }



        Dictionary<Point, Point> getPairings(List<Point> points)
        {
            Dictionary<Point, Point> pairings = new Dictionary<Point, Point>();

            for (int i = 0; i < points.Count(); i += 2)
            {
                pairings.Add(points[i], points[i + 1]);
            }

            return pairings;
        }

        List<Point> newPopulation(Dictionary<Point, Point> pairings)
        {
            List<Point> population = new List<Point>();
            Random random = new Random();

            foreach (KeyValuePair<Point, Point> pairing in pairings)
            {
                Point parent_1 = pairing.Key;
                Point parent_2 = pairing.Value;
                Point baby1; Point baby2;

                switch (random.Next(0, 2))
                {
                    case 0:
                        baby1 = new Point(parent_1.X, parent_1.Y, parent_2.Z);
                        baby2 = new Point(parent_2.X, parent_2.Y, parent_1.Z);
                        break;
                    case 1:
                        baby1 = new Point(parent_1.X, parent_2.Y, parent_2.Z);
                        baby2 = new Point(parent_2.Y, parent_1.Y, parent_1.Z);
                        break;
                    default:
                        baby1 = new Point(parent_2.X, parent_1.Y, parent_2.Z);
                        baby2 = new Point(parent_1.X, parent_2.Y, parent_1.Z);
                        break;
                }

                population.Add(parent_1);
                population.Add(parent_2);
                population.Add(baby1);
                population.Add(baby2);
            }

            return population;
        }

        List<Point> mutatePopulation(List<Point> points)
        {
            List<Point> temp_population = new List<Point>();
            List<Point> rtrnPopulation = new List<Point>();

            temp_population.AddRange(points);
            temp_population.Sort(comparePoints);

            if (temp_population.Count >= 1)
            {
                rtrnPopulation.Add(temp_population[0]);

                //debug stuff
                double best = cost(temp_population[0], target);
                double worst = cost(temp_population[temp_population.Count - 1], target);

                temp_population.RemoveAt(0);
            }

            Random random = new Random();

            int mutations = random.Next(3, temp_population.Count());
            for (int i = 1; i <= mutations; i++)
            {
                int index = random.Next(0, temp_population.Count());
                Point mutate = temp_population[index];
                temp_population.RemoveAt(index);

                mutate.X += random.NextDouble() * (maxMuation - minMutation) * (minMutation);
                mutate.Y += random.NextDouble() * (maxMuation - minMutation) * (minMutation);
                mutate.Z += random.NextDouble() * (maxMuation - minMutation) * (minMutation);

                rtrnPopulation.Add(mutate);
            }

            rtrnPopulation.AddRange(temp_population);

            return rtrnPopulation;
        }

        List<Point> parentPopulation(List<Point> points)
        {
            points.Sort(comparePoints);

            List<Point> parents = new List<Point>();
            for (int i = 0; i < points.Count / 2; i++)
                parents.Add(points[i]);

            return parents;
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

        private int comparePoints(Point pointOne, Point pointTwo)
        {
            //smaller cost is best
            if (pointOne == null)
            {
                if (pointTwo == null) //they are equal
                    return 0;
                else //pointOne is null and pointTwo is not null. PointTwo is greater
                    return 1;
            }
            else
            {
                //If x is not null
                if (pointTwo == null) //y is null, x is greater
                    return -1;
                else
                {
                    double cost_one = cost(pointOne, target);
                    double cost_two = cost(pointTwo, target);

                    if (cost_one > cost_two) // lower cost is better
                        return 1; //pointTwo is better
                    else if (cost_two > cost_one)
                        return -1; //pointOne is better
                    else // they are the same
                        return 0;
                }
            }
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            Naive naive = new Naive();
            naive.FindSolution();
        }


    }
}
