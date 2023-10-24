using Lab2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace MatrixTests
{
    [TestClass]
    public class SparseMatrixTests
    {
        [TestMethod]
        public void MatrixAddition_SameSize_ReturnAdditionOfTwoMatrixes()
        {
            var matrixA = new SparseMatrix(3, 3);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[0, 2] = 3;
            matrixA[1, 1] = 4;
            matrixA[2, 0] = 5;

            var matrixB = new SparseMatrix(3, 3);
            matrixB[0, 0] = 5;
            matrixB[0, 1] = 4;
            matrixB[1, 1] = 3;
            matrixB[1, 2] = 2;
            matrixB[2, 2] = 1;

            var result = matrixA + matrixB;

            Assert.AreEqual(6, result[0, 0]);
            Assert.AreEqual(6, result[0, 1]);
            Assert.AreEqual(3, result[0, 2]);
            Assert.AreEqual(7, result[1, 1]);
            Assert.AreEqual(2, result[1, 2]);
            Assert.AreEqual(5, result[2, 0]);
            Assert.AreEqual(1, result[2, 2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MatrixAddition_DifferentSizes_ThrowException() 
        {
            var matrixA = new SparseMatrix(3, 3);
            matrixA[0, 1] = 3;
            matrixA[0, 2] = 5;
            matrixA[1, 1] = 7;
            matrixA[2, 0] = 9;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 5;
            matrixB[0, 1] = 4;

            var result = matrixA + matrixB;
        }

        [TestMethod]
        public void MatrixMultiplication_SameSize_ValidResult()
        {
            var matrixA = new SparseMatrix(3, 3);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[1, 1] = 3;
            matrixA[2, 2] = 4;

            var matrixB = new SparseMatrix(3, 3);
            matrixB[0, 0] = 2;
            matrixB[0, 1] = 3;
            matrixB[1, 1] = 4;
            matrixB[2, 0] = 1;
            matrixB[2, 2] = 5;

            var result = matrixA * matrixB;

            Assert.AreEqual(2, result[0, 0]);
            Assert.AreEqual(11, result[0, 1]);
            Assert.AreEqual(12, result[1, 1]);
            Assert.AreEqual(4, result[2, 0]);
            Assert.AreEqual(20, result[2, 2]);
        }

        [TestMethod]
        public void MatrixMultiplication_NumberOfColumnsInMatrixAIsEqualToNumberOfRowsInMatrixB_ValidResult()
        {
            var matrixA = new SparseMatrix(3, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[1, 1] = 3;
            matrixA[2, 1] = 4;

            var matrixB = new SparseMatrix(2, 3);
            matrixB[0, 0] = 2;
            matrixB[0, 1] = 3;
            matrixB[1, 0] = 4;
            matrixB[1, 2] = 5;

            var result = matrixA * matrixB;

            Assert.AreEqual(10, result[0, 0]);
            Assert.AreEqual(3, result[0, 1]);
            Assert.AreEqual(15, result[1, 2]);
            Assert.AreEqual(16, result[2, 0]);
            Assert.AreEqual(20, result[2, 2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MatrixMultiplication_NumberOfColumnsInMatrixAIsDifferentFromNumberOfRowsInMatrixB_ThrowException()
        {
            var matrixA = new SparseMatrix(3, 3);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[1, 1] = 3;
            matrixA[2, 1] = 4;

            var matrixB = new SparseMatrix(2, 3);
            matrixB[0, 0] = 2;
            matrixB[0, 1] = 3;
            matrixB[1, 0] = 4;
            matrixB[1, 2] = 5;

            var result = matrixA * matrixB;
        }

        [TestMethod]
        public void MatrixTranspose_ReturnTransposedMatrix()
        {
            var matrixA = new SparseMatrix(3, 2);
            matrixA[0, 1] = 3;
            matrixA[1, 0] = 6;
            matrixA[2, 0] = 9;
            matrixA[2, 1] = 12;

            var result = !matrixA;

            Assert.AreEqual(3, result[1, 0]);
            Assert.AreEqual(6, result[0, 1]);
            Assert.AreEqual(9, result[0, 2]);
            Assert.AreEqual(12, result[1, 2]);
        }

        [TestMethod]
        public void MultiplicationWithScalar_ReturnMatrixInWhichNonZeroElementsAreMultipliedByScalar()
        {
            var matrix = new SparseMatrix(3, 3);
            matrix[0, 0] = 1;
            matrix[1, 0] = 2;
            matrix[2, 2] = 3;

            int scalar = 2;

            var expected = new SparseMatrix(3, 3);
            expected[0, 0] = 2;
            expected[1, 0] = 4;
            expected[2, 2] = 6;

            var result = matrix * scalar;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void MultiplicationWithScalar_ScalarIsEqualToZero_ReturnZeroMatrix()
        {
            var matrix = new SparseMatrix(3, 3);
            matrix[0, 0] = 5;
            matrix[1, 1] = 10;
            matrix[2, 2] = 15;

            int scalar = 0;

            var expected = new SparseMatrix(3, 3);

            var result = matrix * scalar;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(expected[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void SparseMatrix_TrueOperator_MatrixWithNonZeroElements_ReturnsTrue()
        {
            var matrix = new SparseMatrix(3, 3);
            matrix[1, 1] = 5;

            Assert.IsTrue(matrix ? true : false);
        }

        [TestMethod]
        public void SparseMatrix_FalseOperator_MatrixWithNonZeroElements_ReturnsFalse()
        {
            var matrix = new SparseMatrix(3, 3);
            matrix[1, 1] = 5;

            Assert.IsTrue(!matrix ? true : false);
        }

        [TestMethod]
        public void SparseMatrix_TrueOperator_EmptyMatrix_ReturnsFalse()
        {
            var matrix = new SparseMatrix(3, 3);

            Assert.IsFalse(matrix ? true : false);
        }

        [TestMethod]
        public void SparseMatrix_FalseOperator_EmptyMatrix_ReturnsTrue()
        {
            var matrix = new SparseMatrix(3, 3);

            Assert.IsFalse(!matrix ? true : false);
        }

        [TestMethod]
        public void ExplicitConversion_FromDenseMatrixToSparseMatrix_ReturnSparseMatrixFromDenseMatrix()
        {
            int[,] denseMatrix =
            {
            { 1, 0, 3 },
            { 0, 0, 0 },
            { 4, 0, 6 }
            };

            SparseMatrix result = (SparseMatrix)denseMatrix;

            Assert.AreEqual(1, result[0, 0]);
            Assert.AreEqual(3, result[0, 2]);
            Assert.AreEqual(4, result[2, 0]);
            Assert.AreEqual(6, result[2, 2]);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ExplicitConversion_NullDenseMatrix_ThrowsNullReferenceException()
        {
            int[,] nullDenseMatrix = null;

            SparseMatrix result = (SparseMatrix)nullDenseMatrix;
        }

        [TestMethod]
        public void NotEqualsOperator_DifferentMatrices_ReturnsTrue()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 3;
            matrixB[0, 1] = 4;

            Assert.IsTrue(matrixA != matrixB);
        }

        [TestMethod]
        public void LessThanOperator_MatrixAHasFewerNonZeroElements_ReturnsTrue()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 1;
            matrixB[0, 1] = 2;

            Assert.IsTrue(matrixA < matrixB);
        }

        [TestMethod]
        public void GreaterThanOperator_MatrixAHasMoreNonZeroElements_ReturnsTrue()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;
            matrixA[1, 1] = 3;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 1;
            matrixB[0, 1] = 2;

            Assert.IsTrue(matrixA > matrixB);
        }

        [TestMethod]
        public void ToString_ValidMatrix_ReturnsExpectedString()
        {
            var matrix = new SparseMatrix(2, 2);
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;

            var expectedString = new StringBuilder();
            expectedString.AppendLine("1\t2\t");
            expectedString.AppendLine("0\t0\t");

            Assert.AreEqual(expectedString.ToString(), matrix.ToString());
        }

        [TestMethod]
        public void Equals_ValidMatrix_ReturnsTrue()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 1;
            matrixB[0, 1] = 2;

            Assert.IsTrue(matrixA.Equals(matrixB));
            Assert.IsTrue(matrixA == matrixB);
        }

        [TestMethod]
        public void Equals_ForDifferentMatrixes_ReturnsFalse() 
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 3;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 1;
            matrixB[0, 1] = 2;

            Assert.IsFalse(matrixA == matrixB);
        }

        [TestMethod]
        public void GetHashCode_TwoIdenticalMatrices_HaveSameHashCode()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 1;
            matrixB[0, 1] = 2;

            Assert.IsTrue(matrixA == matrixB);
            
            Assert.AreEqual(matrixA.GetHashCode(), matrixB.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentMatrices_HaveDifferentHashCodes()
        {
            var matrixA = new SparseMatrix(2, 2);
            matrixA[0, 0] = 1;
            matrixA[0, 1] = 2;

            var matrixB = new SparseMatrix(2, 2);
            matrixB[0, 0] = 2;
            matrixB[0, 1] = 3;

            Assert.AreNotEqual(matrixA.GetHashCode(), matrixB.GetHashCode());
        }
    }
}
