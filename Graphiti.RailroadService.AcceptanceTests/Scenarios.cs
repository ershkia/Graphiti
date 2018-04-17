using System;
using System.IO;
using System.Linq;
using Graphiti.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Graphiti.RailroadService.AcceptanceTests
{
    [TestClass]
    public class Scenarios
    {
        private static MapQuery m_mapQuery;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Graph map = new GraphLoader().Load("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
            m_mapQuery = new MapQuery(map);

        }

        [DataTestMethod]
        [DataRow("ABC", 9)]
        [DataRow("AD", 5)]
        [DataRow("ADC", 13)]
        [DataRow("AEBCD", 22)]
        [DataRow("AED", -1)]
        public void CalculateDistance(string stopsStr, float expectedDistance)
        {
            var stops = stopsStr.ToCharArray().Select(x => x.ToString());
            float result;

            try
            {
                result = m_mapQuery.GetTotalDistance(stops);
            }
            catch (RouteNotFoundException)
            {
                result = -1;
            }
            Assert.AreEqual(expectedDistance, result);
        }

        [DataTestMethod]
        [DataRow("C", "C", null, 3, 2)]
        [DataRow("A", "C", 4, 4, 3)]
        public void FindTripsWithGivenNumberOfStops(
            string from,
            string to,
            int? minStops,
            int maxStops,
            int expectedTripCount)
        {
            var routes = m_mapQuery.GetRoutesByStopCount(from, to, minStops, maxStops);

            Assert.AreEqual(expectedTripCount, routes.Count());
        }

        [DataTestMethod]
        [DataRow("A", "C", 9)]
        [DataRow("B", "B", 9)]
        public void FindTripsWithMinStops(string from, string to, float expectedDistance)
        {
            var route = m_mapQuery.GetRouteWithMinStops(from, to);

            Assert.AreEqual(expectedDistance, route.GetTotalWeight());
        }

        [DataTestMethod]
        [DataRow("C", "C", 29, 7)]
        public void FindTripsShorterThanGiventDistance(
            string from,
            string to,
            float maxDistance,
            int expectedRouteCount)
        {
            var routes = m_mapQuery.GetRoutesByDistance(from, to, maxDistance);
            Assert.AreEqual(expectedRouteCount, routes.Count());
        }
    }
}
