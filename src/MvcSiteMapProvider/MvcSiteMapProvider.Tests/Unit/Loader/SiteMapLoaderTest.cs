using System;
using NUnit;
using NUnit.Framework;
using Moq;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider.Tests.Unit
{
    [TestFixture]
    public class SiteMapLoaderTest
    {
        #region Setup / Teardown

        private Mock<ISiteMapCache> siteMapCache = null;
        private Mock<ISiteMapCacheKeyGenerator> siteMapCacheKeyGenerator = null;
        private Mock<ISiteMapCreator> siteMapCreator = null;

        [SetUp]
        public void Setup()
        {
            siteMapCache = new Mock<ISiteMapCache>();
            siteMapCacheKeyGenerator = new Mock<ISiteMapCacheKeyGenerator>();
            siteMapCreator = new Mock<ISiteMapCreator>();
        }

        [TearDown]
        public void TearDown()
        {

        }

        private ISiteMapLoader NewSiteMapLoader()
        {
            return new SiteMapLoader(
                siteMapCache.Object,
                siteMapCacheKeyGenerator.Object,
                siteMapCreator.Object);
        }

        #endregion

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetSiteMap_EmptySiteMapCacheKey_ShouldThrowArgumentNullException()
        {
            // arrange
            var siteMapCacheKey = "";
            var target = NewSiteMapLoader();

            // act
            var result = target.GetSiteMap(siteMapCacheKey);

            // assert
        }

        [Test]
        public void GetSiteMap_WithSiteMapCacheKey_ShouldCallGetOrAddWithSiteMapCacheKey()
        {
            // arrange
            var siteMapCacheKey = "theKey";
            var target = NewSiteMapLoader();
            siteMapCache
                .Setup(x => x.GetOrAdd(siteMapCacheKey, It.IsAny<Func<ISiteMap>>(), It.IsAny<Func<ICacheDetails>>()))
                .Returns((string key, Func<ISiteMap> loadFunction, Func<ICacheDetails> getCacheDetailsFunction) => new Mock<ISiteMap>().Object);

            // act
            var result = target.GetSiteMap(siteMapCacheKey);

            // assert
            siteMapCache
                .Verify(x => x.GetOrAdd(siteMapCacheKey, It.IsAny<Func<ISiteMap>>(), It.IsAny<Func<ICacheDetails>>()), 
                Times.Once());
        }
    }
}
