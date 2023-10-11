using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;
using System;

namespace Lab1.Tests
{
    [TestClass]
    public class RingTests
    {
        [TestMethod]
        public void AddToEmptyRing_SingleElement_RingSizeIsOne()
        {
            var ring = new Ring();
            ring.Add(5);
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        public void AddToExistingRing_NewElement_ReadReturnsCorrectElement()
        {
            var ring = new Ring(new int[] { 5 });
            ring.Add(10);
            ring.MoveClockwise();
            Assert.AreEqual(10, ring.ReadElement());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadFromEmptyRing_ThrowsInvalidOperationException()
        {
            var ring = new Ring();
            ring.ReadElement();
        }

        [TestMethod]
        public void ReadFromValidRing_ReturnsCurrentElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadWithExtractFromEmptyRing_ThrowsException()
        {
            var ring = new Ring();
            ring.ReadWithExtract();
        }

        [TestMethod]
        public void ReadWithExtractFromRingWithOneElement_RingBecomesEmpty()
        {
            var ring = new Ring(new int[] { 5 });
            int value = ring.ReadWithExtract();
            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void ReadWithExtractFromRingWithMultipleElements_ReturnsAndRemovesCurrentElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            int value = ring.ReadWithExtract();
            Assert.AreEqual(5, value);
            Assert.AreEqual(10, ring.ReadElement());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveClockwiseInEmptyRing_ThrowsException()
        {
            var ring = new Ring();
            ring.MoveClockwise();
        }

        [TestMethod]
        public void MoveClockwiseWithOneElement_StaysOnSameElement()
        {
            var ring = new Ring(new int[] { 5 });
            ring.MoveClockwise();
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        public void MoveClockwiseWithMultipleElements_MovesToNextElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            ring.MoveClockwise();
            Assert.AreEqual(10, ring.ReadElement());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveCounterclockwiseInEmptyRing_ThrowsException()
        {
            var ring = new Ring();
            ring.MoveCounterclockwise();
        }

        [TestMethod]
        public void MoveCounterclockwiseWithOneElement_StaysOnSameElement()
        {
            var ring = new Ring(new int[] { 5 });
            ring.MoveCounterclockwise();
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        public void RotateConsistency_MoveClockwiseThenCounterClockwise_ReturnsToOriginalElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            ring.MoveClockwise();
            ring.MoveCounterclockwise();
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        public void CompleteRotation_MovingClockwiseNTimes_ReturnsToOriginalElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            for (int i = 0; i < 3; i++)
            {
                ring.MoveClockwise();
            }
            Assert.AreEqual(5, ring.ReadElement());
        }

        [TestMethod]
        public void MoveCounterclockwiseWithMultipleElements_MovesToPreviousElement()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            ring.MoveCounterclockwise();
            Assert.AreEqual(15, ring.ReadElement());
        }

        [TestMethod]
        public void StrongComparisonUsingEqualsMethod_DifferentSizeRings_ReturnsFalse()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 5, 10 });

            bool result = ring1.Equals(ring2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StrongComparisonUsingEqualsMethod_IdenticalRings_ReturnsTrue()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 5, 10, 15 });

            bool result = ring1.Equals(ring2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StrongComparisonUsingEqualsMethod_SameRingElementsButInDifferentOrder_ReturnsFalse()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 15, 5, 10 });

            bool result = ring1.Equals(ring2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StrongComparisonAfterModificationUsingEqualsMethod_DifferentRings_ReturnsFalse()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 5, 10, 15 });

            ring1.Add(20);

            bool result = ring1.Equals(ring2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WeakComparison_EmptyRing_ReturnsFalse()
        {
            var ring1 = new Ring();
            var ring2 = new Ring(new int[] { 5, 10, 15 });
            Assert.IsFalse(ring1.WeakComparison(ring2));
        }

        [TestMethod]
        public void WeakComparison_WithMisalignedButEqualRings_ReturnsTrue()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 15, 5, 10 });
            Assert.IsTrue(ring1.WeakComparison(ring2));
        }

        [TestMethod]
        public void HashCodeComparison_DifferentRings_DifferentHashCodes()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 5, 10 });
            Assert.AreNotEqual(ring1.GetHashCode(), ring2.GetHashCode());
        }

        [TestMethod]
        public void HashCodeComparison_SimilarRings_SameHashCodes()
        {
            var ring1 = new Ring(new int[] { 5, 10, 15 });
            var ring2 = new Ring(new int[] { 5, 10, 15 });
            Assert.AreEqual(ring1.GetHashCode(), ring2.GetHashCode());
        }

        [TestMethod]
        public void ToStringRepresentationOfRing_ChecksForCorrectString()
        {
            var ring = new Ring(new int[] { 5, 10, 15 });
            string expected = "5 -> 10 -> 15 -> (back to start)";
            Assert.AreEqual(expected, ring.ToString());
        }

        [TestMethod]
        public void ToStringOfEmptyRing_ShouldReturnEmptyString()
        {
            var ring = new Ring();
            string expected = "Empty Ring";
            Assert.AreEqual(expected, ring.ToString());
        }
    }
}