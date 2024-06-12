using System;
class Program
{
    static void Main(string[] args)
    {
        //int[] arr = { 10, 80, 30, 90, 40, 50, 70 };
        //int[] arr = { 10,30,40,50,70,80,90};
        int[] arr = { 90, 80, 70, 50, 40, 30, 10 };

        QuickSort(arr, 0, arr.Length - 1);
        for (int i = 0; i < arr.Length; i++)
        {
            Console.WriteLine(arr[i]);
        }
    }
    static void QuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right); //r 5
            QuickSort(arr, left, pivotIndex - 1);  // r 4
            QuickSort(arr, pivotIndex + 1, right); //r7
        }
    }
    // 1,5,9,3,5
    // 5I,1J,9,3,5P
    //5,1I,9J,3,5P
    //5,9J,1I,3,5P
    //5,9,1IJ,3,5P
    //5,9,1J,3I,5P
    //5,9,1J,5P,3I,
    //10j, 80, 30, 90, 40, 50, 70P   i =-1
    //10ji, 80, 30, 90, 40, 50, 70P    s
    //10i, 80j, 30, 90, 40, 50, 70P
    //10i, 80, 30j, 90, 40, 50, 70P   
    //10, 80i, 30j, 90, 40, 50, 70P   s
    //10, 30i, 80j, 90, 40, 50, 70P
    //10, 30i, 80, 90j, 40, 50, 70P 
    //10, 30, 80i, 90, 40j, 50, 70P    s
    //10, 30, 40i, 90, 80j, 50, 70P
    //10, 30, 40, 90i, 80, 50j, 70P    s
    //10, 30, 40, 50i, 80, 90j, 70P
    //10, 30, 40, 50, 80i, 90j, 70P   ssss
    //10, 30, 40, 50, 70P, 90ij, 80 
    // r  5
    // q
    // 10j, 30, 40, 50, 70p, 90, 80   i= -1
    // 10ji, 30, 40, 50, 70p, 90, 80   s 
    // 10, 30ij, 40, 50, 70p, 90, 80   s
    // 10, 30, 40ij, 50, 70p, 90, 80   s
    // 10, 30, 40, 50ij, 70p, 90, 80   sss
    // 10, 30, 40, 50j, 70ip, 90, 80
    // 10, 30, 40, 50ji, 70p, 90, 80
    // r 4
    //q
    //10, 30, 40, 50, 70i, 90j, 80p   l = 5     r=6 
    //10, 30, 40, 50, 70i, 90, 80pj
    //10, 30, 40, 50, 70i, 80, 90pij    sssss
    // r 6





    static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right];
        //int pivot = arr[(left + right) / 2];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (arr[j] <= pivot)
            {
                Console.WriteLine($"{arr[j]}\t");
                i++;
                Swap(arr, i, j);

            }
            Console.WriteLine($"{arr[j]}\t ");
        }

        Swap(arr, i + 1, right);


        return i + 1;
    }

    static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
        Console.WriteLine("iteration");
    }
}