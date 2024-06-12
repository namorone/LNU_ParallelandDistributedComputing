using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCLTemplate;




// 
// Матриці брав  n = m = p = q для зручності вводу. Але в програмі передбачено і різні розмірності, 
// головне, щоб m = p. Результати(у секундах) : 
// 
//  Розмірність: | 500   | 1000  | 2000   | 4000
//               | -----------------------------------
//               | 0.117 | 01.817 | 13.883 | 02:23.627
//               | -----------------------------------
//               | 0.090 | 01.588 | 13.718 |02:39.577
//               | -----------------------------------
//
//Для порівняння на 2 лабораторній роботі множення матриць 2000*2000 виконувалось за такий час:
// 1 потік - 188 - 203сек
// 2 потоки - 95 сек
// 4 потоки - 95сек
// 8 потоків - 112сек

namespace OpenCL
{
    class Program
    {
        static CLCalc.Program.Kernel matrixMultiplication;

        // multiplyMatrices - текст програми, що буде виконуватись. Саме ця програма буде виконувати паралельні обчислення.
        // Сюди ми передаємо не матрицю, а вектори(перед тим я конвертую матрицю з двовимірного
        // масиву до одновимірного в методі ConvertToArray)

        // "__global " - глобальна пам'ять

        // get_global_id(0) та  get_global_id(1) - ідентифікатори процесів. Нам треба їх 2, тому що ми працюємо 
        // з матрицями(двовимірний простір)

        // get_global_size(0) та get_global_size(1) - ідентифікатори для отримання розмірності простору
        static string multiplyMatrices = @"__kernel void 
        MultiplyMatrices(__global float * result,      
                        __global float * matrix1,
                        __global float * matrix2,
                        __global int * q)
            {

                int i = get_global_id(0);
                int j = get_global_id(1);

                int p = get_global_size(0);
                int r = get_global_size(1);
                result[i + p * j] = 0;
                int QQ = q[0];
                for (int k = 0; k < QQ; k++)
                {
                    result[i + p * j] += matrix1[i + p * k] * matrix2[k + QQ * j];
                }
        }";


        // генерація матриці
        public static void GenerateMatrix(float[,] matrix, Random rnd, int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = rnd.Next(100);
                }
            }
        }

        // вивести матрицю на консоль
        public static void PrintMatrix(float[,] matrix, int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // двовимірний масив до одновимірного
        public static float[] ConvertToArray(float[,] matrix, int n, int m)
        {
            float[] converted = new float[n * m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    converted[i + n * j] = matrix[i, j];

            return converted;
        }

        // одновимірний масив до двовимірного
        public static float[,] ConvertToMatrix(float[] array, int n, int m)
        {
            float[,] converted = new float[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    converted[i, j] = array[i + n * j];

            return converted;
        }

        static void Main(string[] args)
        {
            CLCalc.InitCL();   // Ініціалізація OpenCL

            CLCalc.Program.Compile(multiplyMatrices);   //Компіляція програми multiplyMatrices
            matrixMultiplication = new CLCalc.Program.Kernel("MultiplyMatrices"); //Присвоювання назви скомпільовапній програмі, її загрузка.

            Console.Write("Введіть розмір першої матриці\n\tn = ");
            int n = Convert.ToInt32(Console.ReadLine());
            Console.Write("\tm = ");
            int m = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.Write("\nВведіть розмір другої матриці\n\tp = ");
            int p = Convert.ToInt32(Console.ReadLine());
            Console.Write("\tq = ");
            int q = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            if (m != p)
            {
                Console.WriteLine("Множення матриць не можливе");
                Console.Read();
                return;
            }

            float[,] matrix1 = new float[n, m];
            float[,] matrix2 = new float[p, q];

            Random rnd = new Random();
            GenerateMatrix(matrix1, rnd, n, m);
            GenerateMatrix(matrix2, rnd, p, q);

            //PrintMatrix(matrix1, n, m);
            //PrintMatrix(matrix2, p, q);

            Stopwatch watch = Stopwatch.StartNew();

            // конвертую матриці до векторів(одновимірних масивів), щоб потім передати в kernel
            float[] arrayOfMatrix1 = ConvertToArray(matrix1, n, m);
            float[] arrayOfMatrix2 = ConvertToArray(matrix2, p, q);

            float[] arrayResult = new float[n * q];
            int[] arrayQ = new int[1] { m }; // q - межі рядків матриці в одновимірному масиві

            //Загружаем всі 3 матриці(у вигляді одновимірних масивів) та q в пам'ять пристрою
            CLCalc.Program.Variable varResult = new CLCalc.Program.Variable(arrayResult);
            CLCalc.Program.Variable varMatrix1 = new CLCalc.Program.Variable(arrayOfMatrix1);
            CLCalc.Program.Variable varMatrix2 = new CLCalc.Program.Variable(arrayOfMatrix2);
            CLCalc.Program.Variable varQ = new CLCalc.Program.Variable(arrayQ);

            // Аргументи для MultiplyMatrices kernel
            CLCalc.Program.Variable[] arguments = new CLCalc.Program.Variable[4] { varResult, varMatrix1, varMatrix2, varQ };
            int[] workers = new int[2] { n, q }; // global_work_size

            //Виконуємо MultiplyMatrices з аргументами arguments і global_work_size "workers"
            matrixMultiplication.Execute(arguments, workers);

            varResult.ReadFromDeviceTo(arrayResult);   //вигружаємо з пам'яті
            varResult.Dispose();

            // конвертуємо результат з вектора до матриці
            float[,] matrixResult = ConvertToMatrix(arrayResult, n, q);

            watch.Stop();
            Console.WriteLine("Time: " + watch.Elapsed);

            //PrintMatrix(matrixResult, n, q);

            Console.ReadKey();
        }
    }
}



