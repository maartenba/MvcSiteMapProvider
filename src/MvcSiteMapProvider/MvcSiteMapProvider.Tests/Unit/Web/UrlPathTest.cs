using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Tests.Unit.Web
{
    [TestFixture]
    public class UrlPathTest
    {
        #region SetUp / TearDown

        private Mock<IMvcContextFactory> mvcContextFactory = null;
        private Mock<IBindingProvider> bindingProvider = null;

        [SetUp]
        public void Init()
        {
            mvcContextFactory = new Mock<IMvcContextFactory>();
            bindingProvider = new Mock<IBindingProvider>();
        }

        [TearDown]
        public void Dispose()
        {
            mvcContextFactory = null;
            bindingProvider = null;
        }

        private UrlPath NewUrlPath()
        {
            return new UrlPath(
                this.mvcContextFactory.Object, 
                this.bindingProvider.Object);
        }

        #endregion

        #region Tests

        [Test]
        public void IsAbsoluteUrl_WithTildeUrl_ShouldReturnFalse()
        {
            // arrange
            var target = this.NewUrlPath();

            // act
            var result = target.IsAbsoluteUrl("~/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsAbsoluteUrl_WithRelativeUrl_ShouldReturnFalse()
        {
            // arrange
            var target = this.NewUrlPath();

            // act
            var result = target.IsAbsoluteUrl("/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsAbsoluteUrl_WithAbsoluteUrl_ShouldReturnTrue()
        {
            // arrange
            var target = this.NewUrlPath();

            // act
            var result = target.IsAbsoluteUrl("http://somewhere.com/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsAbsoluteUrl_WithAbsoluteUrlAsQueryParameter_ShouldReturnFalse()
        {
            // arrange
            var target = this.NewUrlPath();

            // act
            var result = target.IsAbsoluteUrl(@"/error/pagenotfound?404;http://somewhere.com/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Return false when the virtual application name (and host name) match.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithMatchingApplication_ShouldReturnFalse()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/matching-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://someinternalhost/matching-application/some-page", context.Object);

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Return true when the virtual application name doesn't match.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithNonMatchingApplication_ShouldReturnTrue()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/matching-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://someinternalhost/non-matching-application/some-page", context.Object);

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Return false when the host name matches the internet facing host name in a local
        /// environment.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithMatchingLocalHost_ShouldReturnFalse()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            //request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://someinternalhost/some-page", context.Object);

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Return true when the host name matches the internet facing host name in a local
        /// environment.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithExternalLocalHost_ShouldReturnTrue()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://someotherhost/some-page", context.Object);

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Return false when the host name matches the internet facing host name in a cloud or web farm
        /// environment.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithMatchingHost_ShouldReturnFalse()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection { { "HTTP_HOST", "somehost" } });
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://somehost/some-page", context.Object);

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Return true when the host name doesn't match the internet facing host name in a cloud or web farm
        /// environment.
        /// </summary>
        [Test]
        public void IsExternalUrl_WithExternalHost_ShouldReturnTrue()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection { { "HTTP_HOST", "somehost" } });
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsExternalUrl("http://someotherhost/some-page", context.Object);

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Checks whether the host name matches the local host name if the HTTP_HOST is 
        /// not provided by the environment.
        /// </summary>
        [Test]
        public void IsPublicHostName_WithMatchingLocalHost_ShouldReturnTrue()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsPublicHostName("someinternalhost", context.Object);

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Ensures if the host name doesn't match the internet facing host name in a cloud or web farm
        /// environment, will return false.
        /// </summary>
        [Test]
        public void IsPublicHostName_WithNonMatchingPublicHost_ShouldReturnFalse()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection { { "HTTP_HOST", "somehost" } });
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsPublicHostName("somehost2", context.Object);

            // assert
            var actual = result;
            var expected = false;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Checks whether the host name matches the internet facing host name in a cloud or web farm
        /// environment.
        /// </summary>
        [Test]
        public void IsPublicHostName_WithPublicHost_ShouldReturnTrue()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection { { "HTTP_HOST", "somehost" } });
            request.Setup(x => x.Url).Returns(new Uri("https://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.IsPublicHostName("somehost", context.Object);

            // assert
            var actual = result;
            var expected = true;
            Assert.AreEqual(expected, actual);
        }


        ////[Test]
        ////public void MakeRelativeUrlAbsolute_WithAbsoluteUrl_ShouldReturnSpecifiedAbsoluteUrl()
        ////{
        ////    // arrange
        ////    Mock<HttpContextBase> context = new Mock<HttpContextBase>();
        ////    Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        ////    context.Setup(x => x.Request).Returns(request.Object);
        ////    request.Setup(x => x.ApplicationPath).Returns("/some-application");
        ////    request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
        ////    request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
        ////    request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
        ////    this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
        ////    var target = this.NewUrlPath();

        ////    // act
        ////    var result = target.MakeRelativeUrlAbsolute("http://testing.com:333/directory/subdirectory/page.aspx?a=b", context.Object);

        ////    // assert
        ////    var actual = result;
        ////    var expected = "http://testing.com:333/directory/subdirectory/page.aspx?a=b";
        ////    Assert.AreEqual(expected, actual);
        ////}

        ////[Test]
        ////public void MakeRelativeUrlAbsolute_WithTildeUrl_ShouldReturnAbsoluteUrlWithVirtualPath()
        ////{
        ////    // arrange
        ////    Mock<HttpContextBase> context = new Mock<HttpContextBase>();
        ////    Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        ////    context.Setup(x => x.Request).Returns(request.Object);
        ////    request.Setup(x => x.ApplicationPath).Returns("/some-application");
        ////    request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
        ////    request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
        ////    request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
        ////    this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
        ////    var target = this.NewUrlPath();

        ////    // act
        ////    var result = target.MakeRelativeUrlAbsolute("~/directory/subdirectory/page.aspx?a=b", context.Object);

        ////    // assert
        ////    var actual = result;
        ////    var expected = "https://somehost/some-application/directory/subdirectory/page.aspx?a=b";
        ////    Assert.AreEqual(expected, actual);
        ////}

        /////// <summary>
        /////// The ResolveUrl method is responsible for ensuring the correct virtual application name is 
        /////// returned. MakeRelativeUrlAbsolute still must respect the ~ symbol - in this case it is missing,
        /////// so we must return the root relative path, not the virtual application relative path.
        /////// </summary>
        ////[Test]
        ////public void MakeRelativeUrlAbsolute_WithRelativeUrl_ShouldReturnAbsoluteUrlWithoutVirtualApplication()
        ////{
        ////    // arrange
        ////    Mock<HttpContextBase> context = new Mock<HttpContextBase>();
        ////    Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        ////    context.Setup(x => x.Request).Returns(request.Object);
        ////    request.Setup(x => x.ApplicationPath).Returns("/some-application");
        ////    request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
        ////    request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
        ////    request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
        ////    this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
        ////    var target = this.NewUrlPath();

        ////    // act
        ////    var result = target.MakeRelativeUrlAbsolute("/directory/subdirectory/page.aspx?a=b", context.Object);

        ////    // assert
        ////    var actual = result;
        ////    var expected = "https://somehost/directory/subdirectory/page.aspx?a=b";
        ////    Assert.AreEqual(expected, actual);
        ////}

        ////[Test]
        ////public void MakeRelativeUrlAbsolute_WithRootRelativeUrl_ShouldReturnAbsoluteUrlWithVirtualPath()
        ////{
        ////    // arrange
        ////    Mock<HttpContextBase> context = new Mock<HttpContextBase>();
        ////    Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
        ////    context.Setup(x => x.Request).Returns(request.Object);
        ////    request.Setup(x => x.ApplicationPath).Returns("/some-application");
        ////    request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
        ////    request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
        ////    request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
        ////    this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
        ////    var target = this.NewUrlPath();

        ////    // act
        ////    var result = target.MakeRelativeUrlAbsolute("/some-application/directory/subdirectory/page.aspx?a=b", context.Object);

        ////    // assert
        ////    var actual = result;
        ////    var expected = "https://somehost/some-application/directory/subdirectory/page.aspx?a=b";
        ////    Assert.AreEqual(expected, actual);
        ////}

        /// <summary>
        /// Verifies that an application hosted in a virtual directory will return an absolute URL if an
        /// absolute URL is passed in.
        /// </summary>
        [Test]
        public void ResolveVirtualApplicationToRootRelativeUrl_WithVirtualDirectoryAndAbsoluteUrl_ShouldReturnAbsoluteUrl()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveVirtualApplicationToRootRelativeUrl("http://www.somewhere.com/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = "http://www.somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies that an application hosted in a virtual directory will return the virtual directory qualified URL
        /// when the url starts with a "~".
        /// </summary>ApplicationRelative
        [Test]
        public void ResolveVirtualApplicationToRootRelativeUrl_WithVirtualDirectoryAndTildeUrl_ShouldReturnRootRelativeUrl()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveVirtualApplicationToRootRelativeUrl("~/directory/subdirectory/page.aspx?a=b");

            // assert
            var actual = result;
            var expected = "/some-application/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithTildeUrl_ShouldReturnRootRelativeUrl()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("~/directory/subdirectory/page.aspx?a=b", null, null, context.Object);

            // assert
            var actual = result;
            var expected = "/some-application/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithRelativeUrl_ShouldReturnRelativeUrl()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("/directory/subdirectory/page.aspx?a=b", null, null, context.Object);

            // assert
            var actual = result;
            var expected = "/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithRelativeUrlHostAndNonStandardPort_ShouldReturnAbsoluteUrlWithHttpProtocolSpecifiedHostAndDefaultPort()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost:999/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("/directory/subdirectory/page.aspx?a=b", null, "somewhere.com", context.Object);

            // assert
            var actual = result;
            var expected = "http://somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithRelativeUrlAndHost_ShouldReturnAbsoluteUrlWithHttpProtocolAndSpecifiedHost()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("/directory/subdirectory/page.aspx?a=b", null, "somewhere.com", context.Object);

            // assert
            var actual = result;
            var expected = "http://somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithRelativeUrlAndProtocol_ShouldReturnAbsoluteUrlWithSpecifiedProtocolAndPublicHost()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("http://somehost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("/directory/subdirectory/page.aspx?a=b", "http", null, context.Object);

            // assert
            var actual = result;
            var expected = "http://somehost/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveUrl_WithRelativeUrlProtocolAndHost_ShouldReturnAbsoluteUrlWithSpecifiedProtocolAndHost()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("http://somehost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("/directory/subdirectory/page.aspx?a=b", "http", "somewhere.com", context.Object);

            // assert
            var actual = result;
            var expected = "http://somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Ensures the request protocol is used when the configured protocol is *
        /// </summary>
        [Test]
        public void ResolveUrl_WithTildeUrlAndAsteriskProtocol_ShouldReturnRootRelativeUrlAndRequestProtocol()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveUrl("~/directory/subdirectory/page.aspx?a=b", "*", null, context.Object);

            // assert
            var actual = result;
            var expected = "https://somehost/some-application/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveContentUrl_WithRelativeUrlAndHost_ShouldReturnAbsoluteUrlWithRequestProtocolAndSpecifiedHost()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveContentUrl("/directory/subdirectory/page.aspx?a=b", null, "somewhere.com", context.Object);

            // assert
            var actual = result;
            var expected = "https://somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveContentUrl_WithRelativeUrlHostAndNonStandardPort_ShouldReturnAbsoluteUrlWithRequestProtocolSpecifiedHostAndDefaultPort()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ApplicationPath).Returns("/some-application");
            request.Setup(x => x.ServerVariables).Returns(new NameValueCollection());
            request.Setup(x => x.Url).Returns(new Uri("https://somehost:999/a.aspx?a=b"));
            request.Setup(x => x.RawUrl).Returns("/a.aspx?a=b");
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.ResolveContentUrl("/directory/subdirectory/page.aspx?a=b", null, "somewhere.com", context.Object);

            // assert
            var actual = result;
            var expected = "https://somewhere.com/directory/subdirectory/page.aspx?a=b";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies SSL forwarders are correctly handled when they supply X_FORWARDED_PROTO and HOST
        /// </summary>
        [Test]
        public void GetPublicFacingUrl_WithExternalSSLForwarder_ShouldReturnSSLUrl()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(
                new NameValueCollection { 
                    { "HTTP_X_FORWARDED_PROTO", "https" }, 
                    { "HTTP_HOST",  "somehost" } 
                });
            request.Setup(x => x.Url).Returns(new Uri("http://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.GetPublicFacingUrl(context.Object);

            // assert
            Uri actual = result;
            Uri expected = new Uri("https://somehost/a.aspx?a=b");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies SSL forwarders are correctly handled when they supply X_FORWARDED_PROTO and HOST:port
        /// </summary>
        [Test]
        public void GetPublicFacingUrl_WithExternalSSLForwarderAndNonStandardPort_ShouldReturnSSLUrlWithNonStandardPort()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(
                new NameValueCollection { 
                    { "HTTP_X_FORWARDED_PROTO", "https" }, 
                    { "HTTP_HOST", "somehost:999" } 
                });
            request.Setup(x => x.Url).Returns(new Uri("http://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.GetPublicFacingUrl(context.Object);

            // assert
            Uri actual = result;
            Uri expected = new Uri("https://somehost:999/a.aspx?a=b");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies SSL forwarders are correctly handled when they supply just HOST
        /// </summary>
        [Test]
        public void GetPublicFacingUrl_WithExternalHost_ShouldReturnExternalHost()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(
                new NameValueCollection { 
                    { "HTTP_HOST", "somehost" } 
                });
            request.Setup(x => x.Url).Returns(new Uri("http://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.GetPublicFacingUrl(context.Object);

            // assert
            Uri actual = result;
            Uri expected = new Uri("http://somehost/a.aspx?a=b");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Verifies SSL forwarders are correctly handled when they supply just HOST:port
        /// </summary>
        [Test]
        public void GetPublicFacingUrl_WithExternalHostAndPort_ShouldReturnExternalHostAndPort()
        {
            // arrange
            Mock<HttpContextBase> context = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> request = new Mock<HttpRequestBase>();
            context.Setup(x => x.Request).Returns(request.Object);
            request.Setup(x => x.ServerVariables).Returns(
                new NameValueCollection { 
                    { "HTTP_HOST", "somehost:79" } 
                });
            request.Setup(x => x.Url).Returns(new Uri("http://someinternalhost/a.aspx?a=b"));
            this.mvcContextFactory.Setup(x => x.CreateHttpContext()).Returns(context.Object);
            var target = this.NewUrlPath();

            // act
            var result = target.GetPublicFacingUrl(context.Object);

            // assert
            Uri actual = result;
            Uri expected = new Uri("http://somehost:79/a.aspx?a=b");
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
