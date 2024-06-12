#include <stdlib.h>
#include <stdio.h>
#include "mpi.h"
#include <time.h>

MPI_Status status;

#define MAX_N 4000

double matrix_a[MAX_N][MAX_N], matrix_b[MAX_N][MAX_N], matrix_c[MAX_N][MAX_N];



int main(int argc, char** argv)
{
    int processCount, processId, slaveTaskCount, source, dest, rows, offset, N;

    double start, stop;

    MPI_Init(&argc, &argv);

    MPI_Comm_rank(MPI_COMM_WORLD, &processId);

    MPI_Comm_size(MPI_COMM_WORLD, &processCount);

    if (argc != 2) {
        if (processId == 0) {
            fprintf(stderr, "Usage: %s N\n", argv[0]);
        }
        MPI_Finalize();
        return 1;
    }

    N = atoi(argv[1]);

    if (N <= 0 || N > MAX_N) {
        if (processId == 0) {
            fprintf(stderr, "N is out of bounds (1-%d)\n", MAX_N);
        }
        MPI_Finalize();
        return 1;
    }

    slaveTaskCount = processCount - 1;

    if (processId == 0) {
        

        srand(time(NULL));
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                matrix_a[i][j] = rand() % 10;
                matrix_b[i][j] = rand() % 10;
            }
        }

        printf("\n\t\tMultiply matrix with MPI\n\n");

        start = MPI_Wtime();

        rows = N / slaveTaskCount;
        offset = 0;

        for (dest = 1; dest <= slaveTaskCount; dest++)
        {
            MPI_Send(&offset, 1, MPI_INT, dest, 1, MPI_COMM_WORLD);
            MPI_Send(&rows, 1, MPI_INT, dest, 1, MPI_COMM_WORLD);
            MPI_Send(&matrix_a[offset][0], rows * N, MPI_DOUBLE, dest, 1, MPI_COMM_WORLD);
            MPI_Send(&matrix_b, N * N, MPI_DOUBLE, dest, 1, MPI_COMM_WORLD);

            offset = offset + rows;
        }

        for (int i = 1; i <= slaveTaskCount; i++)
        {
            source = i;
            MPI_Recv(&offset, 1, MPI_INT, source, 2, MPI_COMM_WORLD, &status);
            MPI_Recv(&rows, 1, MPI_INT, source, 2, MPI_COMM_WORLD, &status);
            MPI_Recv(&matrix_c[offset][0], rows * N, MPI_DOUBLE, source, 2, MPI_COMM_WORLD, &status);
        }

        stop = MPI_Wtime();

        printf("MPI Ellapsed time: %.6f s\n", stop - start);
    }

    if (processId > 0) {
        source = 0;

        MPI_Recv(&offset, 1, MPI_INT, source, 1, MPI_COMM_WORLD, &status);
        MPI_Recv(&rows, 1, MPI_INT, source, 1, MPI_COMM_WORLD, &status);
        MPI_Recv(&matrix_a, rows * N, MPI_DOUBLE, source, 1, MPI_COMM_WORLD, &status);
        MPI_Recv(&matrix_b, N * N, MPI_DOUBLE, source, 1, MPI_COMM_WORLD, &status);

        for (int k = 0; k < N; k++) {
            for (int i = 0; i < rows; i++) {
                matrix_c[i][k] = 0.0;
                for (int j = 0; j < N; j++)
                    matrix_c[i][k] = matrix_c[i][k] + matrix_a[i][j] * matrix_b[j][k];
            }
        }

        MPI_Send(&offset, 1, MPI_INT, 0, 2, MPI_COMM_WORLD);
        MPI_Send(&rows, 1, MPI_INT, 0, 2, MPI_COMM_WORLD);
        MPI_Send(&matrix_c, rows * N, MPI_DOUBLE, 0, 2, MPI_COMM_WORLD);
    }

    MPI_Finalize();
    return 0;
}
