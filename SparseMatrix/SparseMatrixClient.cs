using Lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class SparseMatrixClient
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sparse Matrix Client\n");

            int[,] denseArrayA =
            {
                {5, 0, 0, 0},
                {0, 8, 0, 0},
                {0, 0, 3, 0},
                {0, 0, 0, 6},
            };

            SparseMatrix matrixA = new SparseMatrix(denseArrayA);

            Console.WriteLine("Matrix A:");
            Console.WriteLine(matrixA.ToString());

            int[,] denseArrayB =
            {
                {0, 0, 2, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            };

            SparseMatrix matrixB = new SparseMatrix(denseArrayB);

            Console.WriteLine("Matrix B:");
            Console.WriteLine(matrixB.ToString());

            SparseMatrix sumMatrix = matrixA + matrixB;
            Console.WriteLine("Matrix A + Matrix B:");
            Console.WriteLine(sumMatrix.ToString());

            SparseMatrix productMatrix = matrixA * matrixB;
            Console.WriteLine("Matrix A * Matrix B:");
            Console.WriteLine(productMatrix.ToString());

            SparseMatrix transposeMatrix = !matrixA;
            Console.WriteLine("Transpose of Matrix A:");
            Console.WriteLine(transposeMatrix.ToString());

            SparseMatrix scalarMatrix = matrixA * 3;
            Console.WriteLine("Matrix A * 3:");
            Console.WriteLine(scalarMatrix.ToString());

            Console.ReadKey();
        }
    }
}
