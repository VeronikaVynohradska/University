using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public interface ISparseMatrix
    {
        int this[int row, int col] { get; set; }

        int Get(int row, int col);

        void Set(int row, int col, int value);
    }
}
