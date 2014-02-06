using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Moq;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Html;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    [TestFixture]
    public class SiteMapHelperTest
    {
        #region SetUp / TearDown

        private Mock<IViewDataContainer> iView = null;
        private Mock<ViewContext> viewContext = null;

        [SetUp]
        public void Init()
        {
            iView = new Mock<IViewDataContainer>();
            viewContext = new Mock<ViewContext>();
        }

        [TearDown]
        public void Dispose()
        {
            iView = null;
            viewContext = null;
        }

        #endregion

        #region Tests

        [Test]
        public void BuildModel_Case1_Default_ShouldReturnAllNodesAtRootLevel()
        {
            // @Html.MvcSiteMap().SiteMap()

            // Arrange
            var siteMap = HtmlHelperTestCases.CreateFakeSiteMapCase1();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext.Object, this.iView.Object);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = SiteMapHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode, 
                startingNodeInChildLevel: true, 
                visibilityAffectsDescendants: true);

            // Assert
            // Flat structure - 3 nodes
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[1].Title);
            Assert.AreEqual("Contact", result.Nodes[2].Title);

            // Check counts
            Assert.AreEqual(3, result.Nodes.Count);
            Assert.AreEqual(0, result.Nodes[0].Children.Count);
            Assert.AreEqual(0, result.Nodes[1].Children.Count);
            Assert.AreEqual(0, result.Nodes[2].Children.Count);
        }

        [Test]
        public void BuildModel_Case1_StartingNodeNotInChildLevel_ShouldReturnHierarchicalNodes()
        {
            // @Html.MvcSiteMap().SiteMap(false)

            // Arrange
            var siteMap = HtmlHelperTestCases.CreateFakeSiteMapCase1();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext.Object, this.iView.Object);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = SiteMapHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                visibilityAffectsDescendants: true);

            // Assert
            // Tree structure - 3 nodes
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[0].Children[0].Title);
            Assert.AreEqual("Contact", result.Nodes[0].Children[1].Title);

            // Check Counts
            Assert.AreEqual(1, result.Nodes.Count);
            Assert.AreEqual(2, result.Nodes[0].Children.Count);
            Assert.AreEqual(0, result.Nodes[0].Children[0].Children.Count);
            Assert.AreEqual(0, result.Nodes[0].Children[1].Children.Count);
        }

        [Test]
        public void BuildModel_Case2_StartingNodeNotInChildLevel_ShouldReturnHierarchicalNodes()
        {
            // @Html.MvcSiteMap().Menu(false)

            // Arrange
            var siteMap = HtmlHelperTestCases.CreateFakeSiteMapCase2();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext.Object, this.iView.Object);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = SiteMapHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                visibilityAffectsDescendants: true);

            // Assert
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[0].Children[0].Title);
            Assert.AreEqual("About Me", result.Nodes[0].Children[0].Children[0].Title);
            Assert.AreEqual("About You", result.Nodes[0].Children[0].Children[1].Title);

            // "Contact" is inaccessible - should be skipped. So should its child node "ContactSomebody".
            Assert.AreEqual("Categories", result.Nodes[0].Children[1].Title);

            Assert.AreEqual("Cameras", result.Nodes[0].Children[1].Children[0].Title);
            Assert.AreEqual("Nikon Coolpix 200", result.Nodes[0].Children[1].Children[0].Children[0].Title);
            Assert.AreEqual("Canon Ixus 300", result.Nodes[0].Children[1].Children[0].Children[1].Title);

            // "Memory Cards" is not visible. None of its children should be visible.
            Assert.AreEqual(1, result.Nodes[0].Children[1].Children.Count);
        }

        [Test]
        public void BuildModel_Case2_StartingNodeNotInChildLevel_VisibilyDoesntAffectDescendants_ShouldReturnHierarchialNodes()
        {
            // @Html.MvcSiteMap().SiteMap(null, MvcSiteMapProvider.SiteMaps.Current.RootNode, false, false)

            // Arrange
            var siteMap = HtmlHelperTestCases.CreateFakeSiteMapCase2();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext.Object, this.iView.Object);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = SiteMapHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                visibilityAffectsDescendants: false);

            // Assert
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[0].Children[0].Title);
            Assert.AreEqual("About Me", result.Nodes[0].Children[0].Children[0].Title);
            Assert.AreEqual("About You", result.Nodes[0].Children[0].Children[1].Title);

            // "Contact" is inaccessible - should be skipped. So should its child node "ContactSomebody".
            Assert.AreEqual("Categories", result.Nodes[0].Children[1].Title);

            Assert.AreEqual("Cameras", result.Nodes[0].Children[1].Children[0].Title);
            Assert.AreEqual("Nikon Coolpix 200", result.Nodes[0].Children[1].Children[0].Children[0].Title);
            Assert.AreEqual("Canon Ixus 300", result.Nodes[0].Children[1].Children[0].Children[1].Title);

            // "Memory Cards" is not visible. However its children should be in its place.
            Assert.AreEqual("Kingston 256 GB SD", result.Nodes[0].Children[1].Children[1].Title);
            Assert.AreEqual("Sony 256 GB SD", result.Nodes[0].Children[1].Children[2].Title);
            Assert.AreEqual("Sony SD Card Reader", result.Nodes[0].Children[1].Children[2].Children[0].Title);

            // Check counts
            Assert.AreEqual(1, result.Nodes.Count);
            Assert.AreEqual(2, result.Nodes[0].Children.Count); // Home
            Assert.AreEqual(2, result.Nodes[0].Children[0].Children.Count); // About
            Assert.AreEqual(0, result.Nodes[0].Children[0].Children[0].Children.Count); // About Me
            Assert.AreEqual(0, result.Nodes[0].Children[0].Children[1].Children.Count); // About You
            Assert.AreEqual(3, result.Nodes[0].Children[1].Children.Count); // Categories
            Assert.AreEqual(2, result.Nodes[0].Children[1].Children[0].Children.Count); // Cameras
            Assert.AreEqual(0, result.Nodes[0].Children[1].Children[0].Children[0].Children.Count); // Nikon Coolpix 200
            Assert.AreEqual(0, result.Nodes[0].Children[1].Children[0].Children[1].Children.Count); // Canon Ixus 300
            Assert.AreEqual(0, result.Nodes[0].Children[1].Children[1].Children.Count); // Kingston 256 GB SD
            Assert.AreEqual(1, result.Nodes[0].Children[1].Children[2].Children.Count); // Sony 256 GB SD
            Assert.AreEqual(0, result.Nodes[0].Children[1].Children[2].Children[0].Children.Count); // Sony SD Card Reader
        }



        #endregion
    }
}
