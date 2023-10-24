using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lab2
{
    public class Node
    {
        public int Row { get; }
        public int Col { get; }
        public int Val { get; set; }
        public Node Next { get; set; }

        public Node(int row, int col, int val)
        {
            this.Row = row;
            this.Col = col;
            this.Val = val;
        }
    }

    public class SparseMatrix : ISparseMatrix
    {
        private int rows;
        private int cols;
        private Node head;

        public SparseMatrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }

        public SparseMatrix(int[,] denseMatrix)
        {
            rows = denseMatrix.GetLength(0);
            cols = denseMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (denseMatrix[i, j] != 0)
                    {
                        SetElement(i, j, denseMatrix[i, j]);
                    }
                }
            }
        }

        public SparseMatrix(SparseMatrix otherMatrix)
        {
            this.rows = otherMatrix.rows;
            this.cols = otherMatrix.cols;

            Node currentNode = otherMatrix.head;
            while (currentNode != null)
            {
                SetElement(currentNode.Row, currentNode.Col, currentNode.Val);
                currentNode = currentNode.Next;
            }
        }

        private Node Find(int row, int col)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Row == row && current.Col == col)
                {
                    return current;
                }
                current = current.Next;
            }
            return null;
        }

        private void AddElement(Node element)
        {
            if (head == null)
            {
                head = element;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = element;
            }
        }

        private void RemoveElement(int row, int col)
        {
            if (head == null) return;

            if (head.Row == row && head.Col == col)
            {
                head = head.Next;
                return;
            }

            Node current = head;
            while (current.Next != null &&
                   (current.Next.Row != row || current.Next.Col != col))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
            }
        }

        private int GetElement(int row, int col)
        {
            Node element = Find(row, col);
            return element?.Val ?? 0;
        }

        private void SetElement(int row, int col, int value)
        {
            RemoveElement(row, col);
            if (value != 0)
            {
                AddElement(new Node(row, col, value));
            }
        }

        public int this[int row, int col]
        {
            get => GetElement(row, col);
            set => SetElement(row, col, value);
        }

        private int CountNonZeroElements()
        {
            int count = 0;
            Node current = head;
            while (current != null)
            {
                count++;
                current = current.Next;
            }
            return count;
        }

        public static bool operator true(SparseMatrix a) => a.CountNonZeroElements() > 0;
        public static bool operator false(SparseMatrix a) => a.CountNonZeroElements() == 0;

        public static SparseMatrix operator +(SparseMatrix a, SparseMatrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols)
                throw new InvalidOperationException("Matrix dimensions must match for addition");

            SparseMatrix result = new SparseMatrix(a.rows, a.cols);

            Node currentA = a.head;
            while (currentA != null)
            {
                result.SetElement(currentA.Row, currentA.Col, currentA.Val);
                currentA = currentA.Next;
            }

            Node currentB = b.head;
            while (currentB != null)
            {
                result.SetElement(currentB.Row, currentB.Col, result.GetElement(currentB.Row, currentB.Col) + currentB.Val);
                currentB = currentB.Next;
            }

            return result;
        }

        public static SparseMatrix operator *(SparseMatrix a, SparseMatrix b)
        {
            if (a.cols != b.rows)
                throw new InvalidOperationException("The number of columns in the first matrix must be equal to the number of rows in the second matrix for multiplication.");

            SparseMatrix result = new SparseMatrix(a.rows, b.cols);

            Node currentA = a.head;
            while (currentA != null)
            {
                Node currentB = b.head;
                while (currentB != null)
                {
                    if (currentA.Col == currentB.Row)
                    {
                        int product = currentA.Val * currentB.Val;
                        int currentValue = result.GetElement(currentA.Row, currentB.Col);
                        result.SetElement(currentA.Row, currentB.Col, currentValue + product);
                    }
                    currentB = currentB.Next;
                }
                currentA = currentA.Next;
            }

            return result;
        }

        public static SparseMatrix operator *(SparseMatrix a, int scalar)
        {
            SparseMatrix result = new SparseMatrix(a.rows, a.cols);

            Node currentNode = a.head;
            while (currentNode != null)
            {
                result.SetElement(currentNode.Row, currentNode.Col, currentNode.Val * scalar);
                currentNode = currentNode.Next;
            }

            return result;
        }

        public static SparseMatrix operator !(SparseMatrix a)
        {
            SparseMatrix transposed = new SparseMatrix(a.cols, a.rows);

            Node currentNode = a.head;
            while (currentNode != null)
            {
                transposed.SetElement(currentNode.Col, currentNode.Row, currentNode.Val);
                currentNode = currentNode.Next;
            }

            return transposed;
        }

        public static implicit operator SparseMatrix(int[,] denseMatrix)
        {
            return new SparseMatrix(denseMatrix);
        }

        public static explicit operator int[,](SparseMatrix sparseMatrix)
        {
            int[,] dense = new int[sparseMatrix.rows, sparseMatrix.cols];
            Node currentNode = sparseMatrix.head;
            while (currentNode != null)
            {
                dense[currentNode.Row, currentNode.Col] = currentNode.Val;
                currentNode = currentNode.Next;
            }
            return dense;
        }
        public override bool Equals(object obj)
        {
            if (obj is SparseMatrix otherMatrix)
            {
                if (this.rows != otherMatrix.rows || this.cols != otherMatrix.cols)
                    return false;

                Node currentNodeA = this.head;
                Node currentNodeB = otherMatrix.head;

                while (currentNodeA != null && currentNodeB != null)
                {
                    if (currentNodeA.Row != currentNodeB.Row ||
                        currentNodeA.Col != currentNodeB.Col ||
                        currentNodeA.Val != currentNodeB.Val)
                    {
                        return false;
                    }

                    currentNodeA = currentNodeA.Next;
                    currentNodeB = currentNodeB.Next;
                }

                return currentNodeA == null && currentNodeB == null;
            }
            return false;
        }

        public static bool operator ==(SparseMatrix a, SparseMatrix b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }
            return a.Equals(b);
        }

        public static bool operator !=(SparseMatrix a, SparseMatrix b)
        {
            return !(a == b);
        }

        public static bool operator <(SparseMatrix a, SparseMatrix b)
        {
            if (a is null || b is null)
                throw new ArgumentNullException("Matrices cannot be null");

            return a.CountNonZeroElements() < b.CountNonZeroElements();
        }

        public static bool operator >(SparseMatrix a, SparseMatrix b)
        {
            if (a is null || b is null)
                throw new ArgumentNullException("Matrices cannot be null");

            return a.CountNonZeroElements() > b.CountNonZeroElements();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sb.Append(GetElement(i, j)).Append("\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + rows.GetHashCode();
            hash = hash * 23 + cols.GetHashCode();

            Node currentNode = head;
            while (currentNode != null)
            {
                hash = hash * 23 + currentNode.Row.GetHashCode();
                hash = hash * 23 + currentNode.Col.GetHashCode();
                hash = hash * 23 + currentNode.Val.GetHashCode();

                currentNode = currentNode.Next;
            }
            return hash;
        }
    }
}
