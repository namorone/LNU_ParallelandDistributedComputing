using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        // Зчитуємо кількість потоків та розмір матриці з консолі
        Console.Write("Введіть кількість потоків: ");
        int numberOfThreads = int.Parse(Console.ReadLine());

        Console.Write("Введіть розмір матриці (n x n): ");
        int matrixSize = int.Parse(Console.ReadLine());

        // Створюємо матрицю коефіцієнтів та вектор вільних членів
        double[,] matrix = new double[matrixSize, matrixSize];
        double[] vector = new double[matrixSize];
        // Заповнюємо матрицю та вектор даними (для прикладу)
        matrix = FillMatrix(matrix, matrixSize, matrixSize);
        vector = FillVector(vector, matrixSize);
        double[,] copiedMatrix = new double[matrixSize, matrixSize];
        copiedMatrix = Copy_matrix(matrixSize, matrix, copiedMatrix);
        double[] vectorcopy = new double[matrixSize];
        vectorcopy = Vector_copy(matrixSize, vector, vectorcopy);
        // Виведення результатів (при потребі)
        //Console.WriteLine("матриця");
        //Write_matrix(matrixSize, matrixSize, matrix);
        //Console.WriteLine("\tвектор");
        //Write(vector, matrixSize);

        // Послідовне розв'язання
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        double[] sequentialResult = SequentialSolve(matrix, vector);
        stopwatch.Stop();
        Console.WriteLine($"Послідовний час виконання: {stopwatch.ElapsedMilliseconds} мс");
        var t_1 = stopwatch.Elapsed.TotalMilliseconds;
        //Console.WriteLine("матриця2222");
        //Write_matrix(matrixSize, matrixSize, copiedMatrix);
        //Console.WriteLine("\tвектор");
        //Console.WriteLine("матриця");
        //Write_matrix(matrixSize, matrixSize, matrix);
        //Write(vectorcopy, matrixSize);

        // Паралельне розв'язання
        stopwatch.Reset();
        stopwatch.Start();
        double[] parallelResult = ParallelSolve(copiedMatrix, vectorcopy, numberOfThreads);
        stopwatch.Stop();
        Console.WriteLine($"Паралельний час виконання ({numberOfThreads} потоків): {stopwatch.ElapsedMilliseconds} мс");
        var t_2= stopwatch.Elapsed.TotalMilliseconds;
        double acceleration_add = t_1/t_2;

        Console.WriteLine("Прискорення : " + acceleration_add.ToString());
        Console.WriteLine("Ефективність  : " + (acceleration_add / numberOfThreads).ToString() + "\n");

        // Порівняння результатів з бібліотечною функцією
        bool resultsMatch = sequentialResult.SequenceEqual(parallelResult);
        Console.WriteLine($"Результати співпадають ? {resultsMatch}");
        //Console.WriteLine("матриця3333");
        //Write_matrix(matrixSize, matrixSize, matrix);
        //Console.WriteLine("\tвектор");
        //Write(vector, matrixSize);
        ////Console.WriteLine("\tвихідний вектор пос");
        ////Write(sequentialResult, matrixSize);
        ////Console.WriteLine("\tвихідний вектор пap");
        ////Write(parallelResult, matrixSize);
        bool t;
        t = AreArraysEqual(parallelResult, sequentialResult);
        Console.WriteLine($"Рівність вихідних векторів: {t}");

    }
    static double[] Vector_copy(int rows, double[] original, double[] copied)
    {

        for (int i = 0; i < rows; i++)
        {
            
              copied[i] = original[i];
            
        }
        return copied;
    }
    static double[,] Copy_matrix(int rows, double[,] originalMatrix, double[,] copiedMatrix)
    {

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                copiedMatrix[i, j] = originalMatrix[i, j];
            }
        }
        return copiedMatrix;
    }    
    static void Write_matrix(int rows, int cols, double[,] matrix)
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
    static void Write(double[] vector, int cols)
    {
        
        for (int j = 0; j < cols; j++)
        {
            Console.Write(vector[j] + " ");
        }
            

    }
    static double[,] FillMatrix(double[,] matrix, int rows, int cols)
    {
        Random random = new Random();

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

    static double[] FillVector(double[] vector, int cols)
    {
        Random random = new Random();

        for (int j = 0; j < cols; j++)
        {
            vector[j] = random.Next(1, 101); // Генеруємо випадкове число від 1 до 100
        }
        return vector;
    }
    static bool AreArraysEqual(double[] array1, double[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
    static double[] SequentialSolve(double[,] matrix, double[] vector)
    {
        int n = matrix.GetLength(0);
        double[] result = new double[n];

        // Реалізація методу розв'язання СЛАР для послідовного розв'язання
        for (int k = 0; k < n - 1; k++)
        {
            for (int i = k + 1; i < n; i++)
            {
                double factor = matrix[i, k] / matrix[k, k];
                for (int j = k + 1; j < n; j++)
                {
                    matrix[i, j] -= factor * matrix[k, j];
                }
                vector[i] -= factor * vector[k];
            }
        }

        for (int i = n - 1; i >= 0; i--)
        {
            result[i] = vector[i];
            for (int j = i + 1; j < n; j++)
            {
                result[i] -= matrix[i, j] * result[j];
            }
            result[i] /= matrix[i, i];
        }

        return result;
    }

    static double[] ParallelSolve(double[,] matrix, double[] vector, int numberOfThreads)
    {
        int n = vector.Length;
        double[] result = new double[n];

        // Пряма частина методу Гаусса (розділення обчислень)
        for (int k = 0; k < n - 1; k++)
        {
            Parallel.For(k + 1, n, new ParallelOptions { MaxDegreeOfParallelism = numberOfThreads }, i =>
            {
                double factor = matrix[i, k] / matrix[k, k];
                for (int j = k + 1; j < n; j++)
                {
                    matrix[i, j] -= factor * matrix[k, j];
                }
                vector[i] -= factor * vector[k];
            });
        }

        // Зворотня підстановка
        for (int i = n - 1; i >= 0; i--)
        {
            result[i] = vector[i];
            for (int j = i + 1; j < n; j++)
            {
                result[i] -= matrix[i, j] * result[j];
            }
            result[i] /= matrix[i, i];
        }

        return result;
    }
}

