using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Lab2
{
    public class SparseMatrix : ISparseMatrix
    {
        private int rows;
        private int cols;
        private List<SparseMatrixElement> nonZeroElements;

        public class SparseMatrixElement
        {
            public int row { get; }
            public int col { get; }
            public int val { get; }

            public SparseMatrixElement(int row, int col, int val)
            {
                this.row = row;
                this.col = col;
                this.val = val;
            }
        }

        public SparseMatrix(int rows, int cols) 
        {
            this.rows = rows;
            this.cols = cols;
            nonZeroElements = new List<SparseMatrixElement>();
        }

        public SparseMatrix(int rows, int cols, List<SparseMatrixElement> elements)
        {
            this.rows = rows;
            this.cols = cols;
            nonZeroElements = new List<SparseMatrixElement>(elements);
        }

        public SparseMatrix(int[,] denseMatrix)
        {
            rows = denseMatrix.GetLength(0);
            cols = denseMatrix.GetLength(1);
            nonZeroElements = new List<SparseMatrixElement>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (denseMatrix[i, j] != 0)
                    {
                        nonZeroElements.Add(new SparseMatrixElement(i, j, denseMatrix[i, j]));
                    }
                }
            }
        }

        public SparseMatrix(SparseMatrix otherMatrix)
        {
            this.rows = otherMatrix.rows;
            this.cols = otherMatrix.cols;
            nonZeroElements = otherMatrix.nonZeroElements.Select(e => new SparseMatrixElement(e.row, e.col, e.val)).ToList();
        }

        public SparseMatrix(SparseMatrix other, bool transferOwnership)
        {
            this.rows = other.rows;
            this.cols = other.cols;

            if (transferOwnership)
            {
                this.nonZeroElements = other.nonZeroElements;
            }
            else
            {
                this.nonZeroElements = other.nonZeroElements.Select(e => new SparseMatrixElement(e.row, e.col, e.val)).ToList();
            }
        }

        public int this[int row, int col]
        {
            get => Get(row, col);
            set => Set(row, col, value);
        }

        public int Get(int row, int col)
        {
            SparseMatrixElement element = nonZeroElements.FirstOrDefault(e => e.row == row && e.col == col);
            return element?.val ?? 0;
        }

        public void Set(int row, int col, int value)
        {
            SparseMatrixElement element = nonZeroElements.FirstOrDefault(e => e.row == row && e.col == col);
            if (element != null)
            {
                nonZeroElements.Remove(element);
            }
            if (value != 0)
            {
                var newElement = new SparseMatrixElement(row, col, value);
                int index = nonZeroElements.FindIndex(e => e.row > row || (e.row == row && e.col > col));
                if (index != -1)
                {
                    nonZeroElements.Insert(index, newElement);
                }
                else
                {
                    nonZeroElements.Add(newElement);
                }
            }
        }

        public static SparseMatrix operator +(SparseMatrix a, SparseMatrix b)
        {
            if (a.rows != b.rows || a.cols != b.cols)
                throw new InvalidOperationException("Matrix dimensions must match for addition");

            SparseMatrix result = new SparseMatrix(a.rows, a.cols);

            foreach (var elem in a.nonZeroElements)
                result.Set(elem.row, elem.col, elem.val);

            foreach (var elem in b.nonZeroElements)
                result.Set(elem.row, elem.col, result.Get(elem.row, elem.col) + elem.val);

            return result;
        }

        public static SparseMatrix operator *(SparseMatrix a, SparseMatrix b)
        {
            if (a.cols != b.rows)
                throw new InvalidOperationException("Matrix A columns must match Matrix B rows for multiplication");

            SparseMatrix result = new SparseMatrix(a.rows, b.cols);
            foreach (var elemA in a.nonZeroElements)
            {
                foreach (var elemB in b.nonZeroElements)
                {
                    if (elemA.col == elemB.row)
                    {
                        int resVal = elemA.val * elemB.val;
                        result.Set(elemA.row, elemB.col, result.Get(elemA.row, elemB.col) + resVal);
                    }
                }
            }
            return result;
        }

        public static SparseMatrix operator *(SparseMatrix a, int scalar)
        {
            SparseMatrix result = new SparseMatrix(a.rows, a.cols);
            foreach (var elem in a.nonZeroElements)
                result.Set(elem.row, elem.col, elem.val * scalar);

            return result;
        }

        public static SparseMatrix operator !(SparseMatrix a)
        {
            SparseMatrix result = new SparseMatrix(a.cols, a.rows);
            foreach (var elem in a.nonZeroElements)
                result.Set(elem.col, elem.row, elem.val);

            return result;
        }

        public static bool operator true(SparseMatrix a) => a.nonZeroElements.Count > 0;
        public static bool operator false(SparseMatrix a) => a.nonZeroElements.Count == 0;

        public static explicit operator SparseMatrix(int[,] arr) => new SparseMatrix(arr);

        public static implicit operator int[,](SparseMatrix sm)
        {
            int[,] dense = new int[sm.rows, sm.cols];
            foreach (var elem in sm.nonZeroElements)
                dense[elem.row, elem.col] = elem.val;

            return dense;
        }

        public override bool Equals(object obj)
        {
            return obj is SparseMatrix matrix && this == matrix;
        }

        public static bool operator ==(SparseMatrix a, SparseMatrix b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            if (a.rows != b.rows || a.cols != b.cols) return false;

            foreach (var elementA in a.nonZeroElements)
            {
                var valueB = b.Get(elementA.row, elementA.col);
                if (elementA.val != valueB) return false;
            }

            foreach (var elementB in b.nonZeroElements)
            {
                var valueA = a.Get(elementB.row, elementB.col);
                if (elementB.val != valueA) return false;
            }

            return true;
        }

        public static bool operator !=(SparseMatrix a, SparseMatrix b) => !(a == b);

        public static bool operator <(SparseMatrix a, SparseMatrix b) => a.nonZeroElements.Count < b.nonZeroElements.Count;
        public static bool operator >(SparseMatrix a, SparseMatrix b) => a.nonZeroElements.Count > b.nonZeroElements.Count;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    sb.Append(Get(i, j) + "\t");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + rows.GetHashCode();
            hash = hash * 23 + cols.GetHashCode();

            foreach (var element in nonZeroElements.OrderBy(e => e.row).ThenBy(e => e.col))
            {
                hash = hash * 23 + element.row.GetHashCode();
                hash = hash * 23 + element.col.GetHashCode();
                hash = hash * 23 + element.val.GetHashCode();
            }

            return hash;
        }
    }
}
