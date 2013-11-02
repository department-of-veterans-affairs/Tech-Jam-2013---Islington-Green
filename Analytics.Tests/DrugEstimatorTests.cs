using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IG.Analyitcs;

namespace Analytics.Tests
{
    [TestClass]
    public class DrugEstimatorTests
    {
        [TestMethod]
        public void GetDateDrugsAreEmpty_ReturnsExpected()
        {
            DrugEstimator estimator = new DrugEstimator();
            DateTime fillDate = new DateTime(2013,1,1);

            DateTime expected = new DateTime(2013, 4, 1);
            DateTime actual = estimator.GetDateDrugsAreEmpty(fillDate, 90);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WeightComparer_GetCohort_ReturnsExpected()
        {
            WeightComparer comparer = new WeightComparer();
            Int32[] sampleGroup = { 100, 200, 300, 400 };

            double expected = 250.0;
            double actual = comparer.GetCohort(sampleGroup);
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void WeightComparer_GetDifferentToCohort_ReturnsExpected()
        {
            WeightComparer comparer = new WeightComparer();

            double expected = -5.0;
            double actual = comparer.GetDifferenceToCohort(120, 125.0);
            Assert.AreEqual(expected, actual);

        }
    }
}
