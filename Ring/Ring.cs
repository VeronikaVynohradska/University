using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab1.Ring;

namespace Lab1
{
    public class Ring : IRing
    {
        private class Node
        {
            public int value { get; set; }
            public Node next { get; set; }
            public Node prev { get; set; }

            public Node(int val)
            {
                value = val;
                next = this;
                prev = this;
            }
        }

        private Node current;
        private int size;

        // default constructor
        public Ring() { }

        // copy constructor
        public Ring(Ring other)
        {
            current = null;
            size = other.size;
            if (other.current == null)
                return;

            Node temp = other.current;
            do
            {
                Add(temp.value);
                temp = temp.next;
            } while (temp != other.current);
        }

        // move constructor
        public Ring(Ring other, bool transferOwnership)
        {
            if (transferOwnership)
            {
                TransferOwnershipFrom(other);
            }
            else
            {
                DeepCopyFrom(other);
            }
        }

        private void TransferOwnershipFrom(Ring other)
        {
            current = other.current;
            size = other.size;
            other.current = null; // remove ownership from the other Ring
            other.size = 0;
        }

        private void DeepCopyFrom(Ring other)
        {
            current = null;
            size = other.size;
            if (other.current == null)
                return;

            Node temp = other.current;
            do
            {
                Add(temp.value);
                temp = temp.next;
            } while (temp != other.current);
        }

        // parameterized constructor
        public Ring(int[] ringValues)
        {
            if (ringValues == null || ringValues.Length == 0)
                return;

            current = new Node(ringValues[0]);

            Node prev = current;
            for (int i = 1; i < ringValues.Length; i++)
            {
                Node newNode = new Node(ringValues[i]);
                newNode.prev = prev;
                prev.next = newNode;

                prev = newNode;
            }

            // closing the ring (making it circular)
            prev.next = current;
            current.prev = prev;
        }

        public void Add(int value)
        {
            Node newNode = new Node(value);

            if (current != null)
            {
                Node tail = current.prev;
                tail.next = newNode;
                newNode.prev = tail;
                newNode.next = current;
                current.prev = newNode;
            }

            current = current ?? newNode;
            size++;
        }

        // returns the value of the current node
        public int ReadElement()
        {
            if (current == null)
                throw new InvalidOperationException("The ring is empty.");

            return current.value;
        }

        // extracts and returns the value of the current node, removing it from the ring
        public int ReadWithExtract()
        {
            if (current == null)
                throw new InvalidOperationException("The ring is empty.");

            int extractedValue = current.value;

            Node nextNode = current.next;

            current.prev.next = nextNode;
            nextNode.prev = current.prev;

            if (size - 1 == 0) // after decrementing, if there are no nodes left
            {
                current = null;
            }
            else
            {
                current = nextNode;
            }


            size--;
            return extractedValue;
        }

        // inserts a new node with the given value after the current node
        public void Write(int el)
        {
            Node node = new Node(el);

            if (current == null)
            {
                current = node;
                size++;
                return;
            }

            node.next = current.next;
            node.prev = current;
            current.next.prev = node;
            current.next = node;

            size++;
        }

        // moves the current pointer to the next node in a clockwise direction
        public void MoveClockwise()
        {
            if (current == null)
            {
                throw new InvalidOperationException("Cannot move in an empty ring.");
            }

            current = current.next;
        }

        // moves the current pointer to the previous node in a counter-clockwise direction
        public void MoveCounterclockwise()
        {
            if (current == null)
            {
                throw new InvalidOperationException("Cannot move in an empty ring.");
            }

            current = current.prev;
        }

        private bool IsRingEqualFromNode(Node startNode, Ring other)
        {
            Node thisRunner = startNode;
            Node otherRunner = other.current;

            do
            {
                if (thisRunner.value != otherRunner.value) return false;

                thisRunner = thisRunner.next;
                otherRunner = otherRunner.next;

            } while (thisRunner != startNode && otherRunner != other.current);

            return thisRunner == startNode && otherRunner == other.current;
        }

        // performs strong comparison of two rings
        public bool StrongComparison(Ring other)
        {
            if (other == null || current == null || other.current == null) return false;
            return IsRingEqualFromNode(current, other);
        }

        // performs weak comparison of two rings
        public bool WeakComparison(Ring other)
        {
            if (other == null || current == null || other.current == null) return false;

            Node thisRunner = current;

            do
            {
                if (IsRingEqualFromNode(thisRunner, other)) return true;

                thisRunner = thisRunner.next;
            } while (thisRunner != current);

            return false;
        }

        //  traverses the ring
        public override string ToString()
        {
            if (current == null)
                return "Empty Ring";

            StringBuilder sb = new StringBuilder();
            Node runner = current;
            do
            {
                sb.Append(runner.value + " -> ");
                runner = runner.next;
            } while (runner != current);

            sb.Append("(back to start)");

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
                return false;

            Ring otherRing = obj as Ring;
            return StrongComparison(otherRing);
        }

        // provides a way to get a hash code for the object
        public override int GetHashCode()
        {
            int hash = 19;
            Node runner = current;

            if (runner == null) return hash;

            do
            {
                hash = hash * 31 + runner.value.GetHashCode();
                runner = runner.next;
            } while (runner != current);

            return hash;
        }
    }
}
