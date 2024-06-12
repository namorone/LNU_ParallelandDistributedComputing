using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {

        Random random = new Random();
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch stopwatch2 = new Stopwatch();
        Stopwatch stopwatch3 = new Stopwatch();
        Stopwatch stopwatch4 = new Stopwatch();
        Stopwatch stopwatch5 = new Stopwatch();
        Stopwatch stopwatch6 = new Stopwatch();

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Введіть кількість рядків матриці: ");
        int rows = int.Parse(Console.ReadLine());
        Console.Write("Введіть кількість стопців матриці: ");
        int cols = int.Parse(Console.ReadLine());
        Console.Write("Введіть кількість потоків: ");
        int numThreads = int.Parse(Console.ReadLine());

        int[,] matrix_A = new int[rows, cols];

        int[,] matrix_B = new int[rows, cols];

        int[,] matrix_Add = new int[rows, cols];

        int[,] P_matrix_Add = new int[rows, cols];

        //---------            ГЕНЕРАЦІЯ ТА ВИВІД    --------

        int[,] Generate_matrix(int rows, int cols, int[,] matrix)
        {
            // Заповнюємо матрицю випадковими числами
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(1, 101); // Генеруємо випадкове число від 1 до 100
                }
            }
            return matrix;
        }

        void Write_matrix(int rows, int cols, int[,] matrix)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }

        }


        matrix_A = Generate_matrix(rows, cols, matrix_A);
        //Console.WriteLine("матриця А");
        //Write_matrix(rows, cols, matrix_A);

        matrix_B = Generate_matrix(rows, cols, matrix_B);
        //Console.WriteLine("матриця В");
        //Write_matrix(rows, cols, matrix_B);

        //---------            ДОДАВАННЯ    --------

        int[,] Add_matrix(int rows, int cols, int[,] matrix_a, int[,] matrix_b, int[,] matrix_add)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix_add[i, j] = matrix_a[i, j] + matrix_b[i, j];
                }

            }
            return matrix_add;
        }
        stopwatch.Start();

        matrix_Add = Add_matrix(rows, cols, matrix_A, matrix_B, matrix_Add);

        stopwatch.Stop();
        //double elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        Console.WriteLine("\nЧас виконання додавання послідовно: " + stopwatch.Elapsed.TotalMilliseconds.ToString() + "  мс");
        //Console.WriteLine("матриця A+В");
        //Write_matrix(rows, cols, matrix_Add);


        int[,] Parallel_Add_matrix(int numThreads, int rows, int[,] matrix_a, int[,] matrix_b, int[,] matrix_add)
        {
            Parallel.For(0, numThreads, threadNum =>
            {
                int startIndex = (threadNum * rows) / numThreads;
                int endIndex = ((threadNum + 1) * rows) / numThreads;

                for (int i = startIndex; i < endIndex; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix_add[i, j] = matrix_a[i, j] + matrix_b[i, j];
                    }
                }
            });
            return matrix_add;
        }

        stopwatch2.Start();
        P_matrix_Add = Parallel_Add_matrix(numThreads, rows, matrix_A, matrix_B, P_matrix_Add);
        stopwatch2.Stop();
        Console.WriteLine("Час виконання додавання паралельно: " + stopwatch2.Elapsed.TotalMilliseconds.ToString() + "  мс\n");
        
        double acceleration_add = stopwatch.Elapsed.TotalMilliseconds / stopwatch2.Elapsed.TotalMilliseconds;
        
        Console.WriteLine("Прискорення додавання : " + acceleration_add.ToString() );
        Console.WriteLine("Ефективність додавання : " + (acceleration_add/ numThreads).ToString() + "\n");
        //if (P_matrix_Add != matrix_Add) 
        //{
        //    Console.WriteLine(true);
        //}
        //Console.WriteLine("матриця P A+В");
        //Write_matrix(rows, cols, P_matrix_Add);
        //Console.WriteLine(P_matrix_Add.Length);
        //Console.WriteLine(matrix_Add.Length);

        //---------            ВІДНІМАННЯ    --------


        int[,] Subtraction_matrix(int rows, int cols, int[,] matrix_a, int[,] matrix_b, int[,] matrix_s)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix_s[i, j] = matrix_a[i, j] - matrix_b[i, j];
                }

            }
            return matrix_s;
        }

        int[,] Parallel_Subtraction_matrix(int numThreads, int rows, int[,] matrix_a, int[,] matrix_b, int[,] matrix_s)
        {
            Parallel.For(0, numThreads, threadNum =>
            {
                int startIndex = (threadNum * rows) / numThreads;
                int endIndex = ((threadNum + 1) * rows) / numThreads;

                for (int i = startIndex; i < endIndex; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix_s[i, j] = matrix_a[i, j] - matrix_b[i, j];
                    }
                }
            });
            return matrix_s;
        }

        stopwatch3.Start();

        matrix_Add = Subtraction_matrix(rows, cols, matrix_A, matrix_B, matrix_Add);

        stopwatch3.Stop();
        
        Console.WriteLine("Час виконання віднімання послідовно: " + stopwatch3.Elapsed.TotalMilliseconds.ToString() + "  мс");
        //Console.WriteLine("матриця A-В");
        //Write_matrix(rows, cols, matrix_Add);

        stopwatch4.Start();
        P_matrix_Add = Parallel_Subtraction_matrix(numThreads, rows, matrix_A, matrix_B, P_matrix_Add);
        stopwatch4.Stop();
        Console.WriteLine("Час виконання віднімання паралельно: " + stopwatch4.Elapsed.TotalMilliseconds.ToString() + "  мс\n");
        
        double acceleration_Subtraction = stopwatch3.Elapsed.TotalMilliseconds / stopwatch4.Elapsed.TotalMilliseconds;

        Console.WriteLine("Прискорення віднімання : " + acceleration_Subtraction.ToString() );
        Console.WriteLine("Ефективність віднімання : " + (acceleration_Subtraction / numThreads).ToString() + "\n");

        //Console.WriteLine("матриця P A-В");
        //Write_matrix(rows, cols, matrix_Add);

        //---------            МНОЖЕННЯ    --------

        int[,] Mult_matrix(int rows, int cols, int[,] matrix_a, int[,] matrix_b, int[,] matrix_m)
        {

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix_m[i, j] = 0; // Ініціалізуємо результат нулями

                    for (int k = 0; k < cols; k++)
                    {
                        matrix_m[i, j] += matrix_a[i, k] * matrix_b[k, j];
                    }
                }
            }
            return matrix_m;
        }

        stopwatch5.Start();

        matrix_Add = Mult_matrix(rows, cols, matrix_A, matrix_B, matrix_Add);

        stopwatch5.Stop();

        Console.WriteLine("Час виконання множення послідовно: " + stopwatch5.Elapsed.TotalMilliseconds.ToString() + "  мс");

        //Console.WriteLine("матриця A*В");
        //Write_matrix(rows, cols, matrix_Add);

        int[,] Parallel_Mult_matrix(int numThreads, int rows, int cols, int[,] matrix_a, int[,] matrix_b, int[,] matrix_m)
        {
            Parallel.For(0, numThreads, threadNum =>
            {
                int startIndex = (threadNum * rows) / numThreads;
                int endIndex = ((threadNum + 1) * rows) / numThreads;

                for (int i = startIndex; i < endIndex; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        matrix_m[i, j] = 0; // Ініціалізуємо результат нулями

                        for (int k = 0; k < cols; k++)
                        {
                            matrix_m[i, j] += matrix_a[i, k] * matrix_b[k, j];
                        }
                    }
                }
            });
            return matrix_m;
        }
        stopwatch6.Start();

        matrix_Add = Parallel_Mult_matrix(numThreads, rows, cols, matrix_A, matrix_B, matrix_Add);

        stopwatch6.Stop();

        Console.WriteLine("Час виконання множення паралельно: " + stopwatch6.Elapsed.TotalMilliseconds.ToString() + "  мс\n");

        double acceleration_Mult = stopwatch5.Elapsed.TotalMilliseconds / stopwatch6.Elapsed.TotalMilliseconds;

        Console.WriteLine("Прискорення множення : " + acceleration_Mult.ToString());
        Console.WriteLine("Ефективність множення : " + (acceleration_Mult / numThreads).ToString() + "\n");

        //Console.WriteLine("матриця P A*В");
        //Write_matrix(rows, cols, matrix_Add);
    }
};