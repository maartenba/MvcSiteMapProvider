using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Script.Serialization;

namespace MvcSiteMapProvider.Tests.Unit.Collections.Specialized
{
    [TestFixture]
    public class RouteValueDictionaryTest
    {
        #region SetUp / TearDown

        private Mock<ISiteMap> mSiteMap = null;
        private Mock<IRequestCache> mRequestCache = null;
        private Mock<IReservedAttributeNameProvider> mReservedAttributeNameProvider = null;
        private Mock<IJsonToDictionaryDeserializer> mjsonToDictionaryDeserializer = null;

        [SetUp]
        public void Init()
        {
            mSiteMap = new Mock<ISiteMap>();
            mRequestCache = new Mock<IRequestCache>();
            mReservedAttributeNameProvider = new Mock<IReservedAttributeNameProvider>();
            mjsonToDictionaryDeserializer = new Mock<IJsonToDictionaryDeserializer>();

            mReservedAttributeNameProvider
                .Setup(x => x.IsRouteAttribute(It.IsAny<string>()))
                .Returns(true);
        }

        [TearDown]
        public void Dispose()
        {
            mSiteMap = null;
            mRequestCache = null;
            mReservedAttributeNameProvider = null;
            mjsonToDictionaryDeserializer = null;
        }

        private IRouteValueDictionary NewRouteValueDictionaryInstance()
        {
            return new RouteValueDictionary("testNodeKey", "RouteValues", mSiteMap.Object, mReservedAttributeNameProvider.Object, mjsonToDictionaryDeserializer.Object, mRequestCache.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParams_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsNonMatchingCaseValues_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "home");
            routeValues.Add("action", "index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsNonMatchingCaseKeys_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("Controller", "Home");
            target.Add("Action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsAndArea_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("area", "Admin");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndNonMatchingArea_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("area", "Home");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndArea_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Test");
            target.Add("action", "Index");
            target.Add("area", "Admin");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParams_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Test");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndMissingArea_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("area", "Admin");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsAndMatchingID_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("id", "1234");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndNonMatchingID_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("id", "123");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsAndMissingID_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", "1234");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_NonMatchingRouteWith2StandardParamsEmptyID_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", "12345");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("id", "");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MatchesRoute_MatchingRouteWith2StandardParamsNullIDEmptyArea_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", null);
            routeValues.Add("area", "");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Ensures that all configured route keys with values are considered in the match")]
        public void MatchesRoute_MatchingRouteWith2StandardParamsEmptyIDEmptyArea_WithExtraConfiguredKeyAndValue_ShouldReturnFalse()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", null);
            routeValues.Add("area", "");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("something", "1234");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        [Test(Description = "Ensures that empty configured route keys are not considered in the match")]
        public void MatchesRoute_MatchingRouteWith2StandardParamsEmptyIDEmptyArea_WithExtraConfiguredKeyAndEmptyValue_ShouldReturnTrue()
        {
            // arrange
            var routeValues = new Dictionary<string, object>();
            routeValues.Add("controller", "Home");
            routeValues.Add("action", "Index");
            routeValues.Add("id", null);
            routeValues.Add("area", "");

            var target = NewRouteValueDictionaryInstance();
            target.Add("controller", "Home");
            target.Add("action", "Index");
            target.Add("something", "");

            // act
            var result = target.MatchesRoute(routeValues);

            // assert
            var actual = result;
            var expected = true;

            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
