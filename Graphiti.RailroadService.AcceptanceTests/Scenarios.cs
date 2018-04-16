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
        [DataTestMethod]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "ABC", 9)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "AD", 5)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "ADC", 13)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "AEBCD", 22)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "AED", -1)]
        public void CalculateDistance(string graphSrt, string stopsStr, float expectedDistance)
        {
            Graph graph = new GraphLoader().Load(graphSrt);
            RouteQuery routeQuery = new RouteQuery(graph);
            var stops = stopsStr.ToCharArray().Select(x => x.ToString());
            float result;

            try
            {
                result = routeQuery.CalculateDistance(stops);
            }
            catch (RouteNotFoundException)
            {
                result = -1;
            }
            Assert.AreEqual(expectedDistance, result);
        }

        [DataTestMethod]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "C", "C", null, 3, 2)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "A", "C", 4, 4, 3)]
        public void FindTripsWithStops(
            string graphSrt,
            string from,
            string to,
            int? minStops,
            int maxStops,
            int expectedTripCount)
        {
            Graph graph = new GraphLoader().Load(graphSrt);
            RouteQuery routeQuery = new RouteQuery(graph);

            var routes = routeQuery.GetAllRoutes(from, to, minStops, maxStops);

            Assert.AreEqual(expectedTripCount, routes.Count());
        }

        [DataTestMethod]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "A", "C", 9)]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "B", "B", 9)]
        public void FindTripsWithMinStops(string graphSrt, string from, string to, float expectedDistance)
        {
            Graph graph = new GraphLoader().Load(graphSrt);
            RouteQuery routeQuery = new RouteQuery(graph);

            var route = routeQuery.GetRouteWithMinStops(from, to);

            Assert.AreEqual(expectedDistance, route.GetTotalWeight());
        }

        [DataTestMethod]
        [DataRow("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "C", "C", 29, 7)]
        public void FindTripsShorterThanGiventDistance(
            string graphSrt,
            string from,
            string to,
            float maxDistance,
            int expectedRouteCount)
        {
            Graph graph = new GraphLoader().Load(graphSrt);
            RouteQuery routeQuery = new RouteQuery(graph);

            var routes = routeQuery.GetRoutes(from, to, maxDistance);
            Assert.AreEqual(expectedRouteCount, routes.Count());
        }
    }
}
