using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    internal interface IRing
    {
        void Add(int value);
        int ReadElement();
        int ReadWithExtract();
        void Write(int el);
        void MoveClockwise();
        void MoveCounterclockwise();
        bool StrongComparison(Ring other);
        bool WeakComparison(Ring other);
    }
}