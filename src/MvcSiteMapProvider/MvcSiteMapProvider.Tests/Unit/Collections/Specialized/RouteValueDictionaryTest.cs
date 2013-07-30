using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Tests.Unit.Collections.Specialized
{
    [TestFixture]
    public class RouteValueDictionaryTest
    {
        #region SetUp / TearDown

        private Mock<ISiteMap> mSiteMap = null;
        private Mock<IRequestCache> mRequestCache = null;

        [SetUp]
        public void Init()
        {
            mSiteMap = new Mock<ISiteMap>();
            mRequestCache = new Mock<IRequestCache>();
        }

        [TearDown]
        public void Dispose()
        {
            mSiteMap = null;
            mRequestCache = null;
        }

        private IRouteValueDictionary NewRouteValueDictionaryInstance()
        {
            return new RouteValueDictionary(mSiteMap.Object, mRequestCache.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParams_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsAndArea_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("Area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("Area", "Admin");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndNonMatchingArea_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("Area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("Area", "Home");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndArea_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("Area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Test");
            target.Add("Action", "Index");
            target.Add("Area", "Admin");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParams_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Test");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndMissingArea_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("Area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsAndMatchingID_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("id", "1234");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndNonMatchingID_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("id", "123");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndNonExistantID_ShouldReturnFalse()
        {
            // arrange
            var actionParameters = new List<string>();

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsAndNonMatchingActionParameterID_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();
            actionParameters.Add("id");

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "12345");
            
            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("id", "1");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsEmptyIDAndNonMatchingActionParameterID_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();
            actionParameters.Add("id");

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "12345");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");
            target.Add("id", "");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsMissingIDAndNonMatchingActionParameterID_ShouldReturnTrue()
        {
            // arrange
            var actionParameters = new List<string>();
            actionParameters.Add("id");

            var routeValues = new Dictionary<string, object>();
            routeValues.Add("Controller", "Home");
            routeValues.Add("Action", "Index");
            routeValues.Add("id", "12345");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(actionParameters, routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
