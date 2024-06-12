using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


// Розмірність матриці брала 10000*10000
// Час у мілісекундах.
//
//    |головний потік   | 2 потоки            |головний потік  | 4 потоки           |головний потік  | 8 потоків    
//    | --------------------------            | --------------------------          | ---------------------------
//    |      1510       |    1021             |      1765      |    844             |      1663      |    962       
//    |      1626       |    1029             |      1673      |    855             |      1687      |    948    


namespace Dijkstra
{
    class Program
    {
        static int n;
        static int numberOfThreads;
        static int startVertex;

        public static void FillMatrix(int[,] matrix)
        {
            int l = 0;
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = l; j < n; j++)
                {
                    int elem = random.Next(0, 10);
                    if (i != j)  // якщо i=j, то лишаємо нулі
                    {
                        matrix[i, j] = elem;
                        matrix[j, i] = elem; // заповнюємо матрицю
                    }
                    
                }
                l++;
            }
        }

        public static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintResult(int[] dist)
        {
            Console.Write("Vertex     Distance " + $"from Vertex {startVertex} \n");
            for (int i = 0; i < n; i++)
                Console.Write(i + " \t\t " + dist[i] + "\n");
        }

        public static int FindMin(int[] dist, bool[] checkVertex)
        {
            int min = int.MaxValue;
            int min_index = -1;

            for (int i = 0; i < n; i++)
                if (checkVertex[i] == false && dist[i] <= min) // якщо значення ще не постійне і менше за мін, то воно стає мін
                {
                    min = dist[i];
                    min_index = i;
                }

            return min_index; // повертаю індекс мін значення
        }


        public static void DijkstraAlgorithm(int begin, int stop, int[,] matrix, int[] resDist, bool[] checkVertex)
        {
            for (int count = begin; count < stop; count++)
            {
                int u = FindMin(resDist, checkVertex); // шукаю мінімальну вагу

                
                checkVertex[u] = true; // ставлю позначку, що це значення вже постійне

                for (int vertex = 0; vertex < n; vertex++)
                {
                    // міняємо resDist[vertex] тоді, коли немає позначки, що це значення постійне,
                    // якщо є шлях від u до vertex та сума шляху менша, за поточне значення
                    if (!checkVertex[vertex] && matrix[u, vertex] != 0 && resDist[u] != int.MaxValue && resDist[u] + matrix[u, vertex] < resDist[vertex])
                    {
                        resDist[vertex] = resDist[u] + matrix[u, vertex];
                    }
                }                    
            }
        }
        public static int[] Calculate(int[,] matrix, int numberOfThreads, int[] resDist)
        {
            // масив булівських значень, щоб відмічати вже постійні значення для вершини
            bool[] checkVertex = new bool[n];

            // заповнюю результуючий масив великими числами(так потрібно за алгоритмом Дейкстри)
            // та булівський масив заповеюю значеннями false
            for (int i = 0; i < n; i++)
            {
                resDist[i] = int.MaxValue;
                checkVertex[i] = false;
            }

            // шлях від початкової вершини до неї самої ж дорівнює 0
            resDist[startVertex] = 0;


            if (numberOfThreads > n)
                numberOfThreads = n;

            int move = n / numberOfThreads;      // крок з яким матриця буде ділитися на частини
            List<Thread> threads = new List<Thread>();

            for (int t = 0; t < numberOfThreads; t++)
            {
                int begin = t * move;            // рахує з якого рядка поток починатиме обчислювати   
                int stop = 0;
                if (t + 1 == numberOfThreads)    // і до якого рядка. Якщо запускається останній потік, то він обчислює всі
                    stop = n;                    // рядки, що залишилися, щоб не загубити рядки у випадку, коли їх кількість не ділиться 
                else                             // без остачі на кількість потоків
                    stop = begin + move;



                Thread thread = new Thread(() => DijkstraAlgorithm(begin, stop, matrix, resDist, checkVertex));
                threads.Add(thread);
                thread.Start();

            }

            threads.ForEach(x => x.Join());
            return resDist;
        }

        static void Main(string[] args)
        {
            Console.Write("Введіть кількість вершин\n n = ");
            n = Convert.ToInt32(Console.ReadLine());

            Console.Write($"Введіть вершину старту (від 0 до {n - 1}): "); // з якої вершини шукати найкоротший шлях до усіх інших вершин
            startVertex = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введіть число потоків: ");
            numberOfThreads = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            int [,] matrix = new int[n, n];
            FillMatrix(matrix);

            //PrintMatrix(matrix);

            int[] resDist = new int[n];

            Stopwatch sw = new Stopwatch();

            bool[] checkVertex = new bool[n];
            for (int i = 0; i < n; i++)
            {
                resDist[i] = int.MaxValue;
                checkVertex[i] = false;
            }
            resDist[startVertex] = 0;

            sw.Start();


            DijkstraAlgorithm(0, n, matrix, resDist, checkVertex);

            sw.Stop();
            Console.WriteLine($"Стандартно: {sw.ElapsedMilliseconds}\n");
            var t_1 = sw.Elapsed.TotalMilliseconds;

            sw.Reset();

            int[] res2 = new int[n];

            sw.Start();
            res2 = Calculate(matrix, numberOfThreads, resDist);
            sw.Stop();
            Console.WriteLine($"Час парадельно на  {numberOfThreads} потоках: {sw.ElapsedMilliseconds}\n");

            var t_2 = sw.Elapsed.TotalMilliseconds;
            double acceleration_add = t_1 / t_2;

            Console.WriteLine("Прискорення : " + acceleration_add.ToString());
            Console.WriteLine("Ефективність  : " + (acceleration_add / numberOfThreads).ToString() + "\n");
            Console.ReadKey();
        }
    }
}