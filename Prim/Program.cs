using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

// Розмірність матриці брала 10000*10000
// Час у мілісекундах.
//
//    |головний потік   | 2 потоки            |головний потік  | 4 потоки           |головний потік  | 8 потоків    
//    | --------------------------            | --------------------------          | ---------------------------
//    |      1160       |    740              |      1027      |    604             |      1054      |    637       
//    |      1047       |    739              |      1031      |    628             |      1019      |    673    



namespace Prim
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
                    int elem = random.Next(0, 100);
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

        public static void PrintResult(int[] parent, int[,] matrix)
        {
            Console.WriteLine("Edge \tWeight");
            for (int i = 0; i < n; i++)
            {
                if (i != startVertex)
                {
                    Console.WriteLine(parent[i] + " - " + i + "\t   " + matrix[i, parent[i]]);
                }
            }
        }

        // знаходить вершину(з тих вершин, які ще не зафіксовані), до якої мінімальна вага ребра
        public static int FindMin(int[] dist, bool[] checkVertex)
        {
            int min = int.MaxValue;
            int min_index = -1;

            while (min_index == -1)
            {
                for (int vertex = 0; vertex < n; vertex++)
                {
                    if (!checkVertex[vertex] && dist[vertex] < min) // якщо значення ще не фіксоване і менше за мін, то воно стає мін
                    {
                        min = dist[vertex];
                        min_index = vertex;
                    }
                }
            }

            return min_index; // повертаю індекс мін значення
        }


        public static void PrimsAlgorithm(int begin, int stop, int[,] matrix, int[] parent, int[] dist, bool[] checkVertex)
        {
            for (int count = begin; count < stop; count++)
            {
                int u = FindMin(dist, checkVertex); // шукаю мінімальну вагу


                checkVertex[u] = true; // фіксую вершину, ставлю позначку, що вже проклали шлях до цієї вершини

                for (int vertex = 0; vertex < n; vertex++)
                {
                    // міняємо dist[vertex] тоді, коли немає позначки, що ця вершина фіксована,
                    // якщо є шлях від u до vertex та  matrix[u, vertex] менша, за поточне dist[vertex]
                    if (!checkVertex[vertex] && matrix[u, vertex] != 0 && matrix[u, vertex] < dist[vertex])
                    {
                        parent[vertex] = u;
                        dist[vertex] = matrix[u, vertex];
                    }
                }
            }
        }
        public static void Calculate(int[,] matrix, int[] parent, int[] dist)
        {
            // масив булівських значень, щоб фіксувати вершини
            bool[] checkVertex = new bool[n];

            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                checkVertex[i] = false;
                parent[i] = 0;
            }


            dist[startVertex] = 0;
            parent[startVertex] = -1;


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



                Thread thread = new Thread(() => PrimsAlgorithm(begin, stop, matrix, parent, dist, checkVertex));
                threads.Add(thread);
                thread.Start();

            }

            threads.ForEach(x => x.Join());
        }

        static void Main(string[] args)
        {
            Console.Write("Введіть кількість вершин\n n = ");
            n = Convert.ToInt32(Console.ReadLine());

            Console.Write($"Введіть вершину старту (від 0 до {n - 1}): "); // з якої вершини починати
            startVertex = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введіть число потоків: ");
            numberOfThreads = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            int[,] matrix = new int[n, n];
            FillMatrix(matrix);

            int[] parent = new int[n]; // вершини
            int[] dist = new int[n]; // вага ребра між вершинами

            Stopwatch sw = new Stopwatch();

            sw.Start();
            bool[] checkVertex = new bool[n];
            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                checkVertex[i] = false;
            }
            dist[startVertex] = 0;
            parent[startVertex] = -1;
            PrimsAlgorithm(0, n, matrix, parent, dist, checkVertex);
            sw.Stop();
            Console.WriteLine($"Стандартно: {sw.ElapsedMilliseconds}\n");
            var t_1 = sw.Elapsed.TotalMilliseconds;

            sw.Reset();

            sw.Start();
            Calculate(matrix, parent, dist);
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
