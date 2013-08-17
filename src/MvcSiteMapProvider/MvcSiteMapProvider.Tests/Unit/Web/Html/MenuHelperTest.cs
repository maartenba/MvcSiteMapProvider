using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Html;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    [TestFixture]
    public class MenuHelperTest
    {
        #region SetUp / TearDown

        private HttpContextBase httpContext = null;
        private IViewDataContainer iView = null;
        private ViewContext viewContext = null;

        [SetUp]
        public void Init()
        { 
            // necessary for building the HTML helpers
            httpContext = new FakeHttpContext();
            iView = new FakeIView();
            viewContext = new ViewContext();
        }

        [TearDown]
        public void Dispose()
        {
            httpContext = null;
            iView = null;
            viewContext = null;
        }

        private ISiteMap CreateFakeSiteMapCase1()
        {
            bool securityTrimmingEnabled = true;
            var siteMap = new FakeSiteMap(securityTrimmingEnabled);

            var home = new FakeSiteMapNode(siteMap, "Home", "Home", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/", metaRobotsContentString: string.Empty);
            siteMap.AddNode(home);

            var about = new FakeSiteMapNode(siteMap, "About", "About", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/About", metaRobotsContentString: string.Empty);
            siteMap.AddNode(about, home);

            siteMap.SetCurrentNode(about);

            var contact = new FakeSiteMapNode(siteMap, "Contact", "Contact", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/Contact", metaRobotsContentString: string.Empty);
            siteMap.AddNode(contact, home);

            return siteMap;
        }

        private ISiteMap CreateFakeSiteMapCase2()
        {
            bool securityTrimmingEnabled = true;
            var siteMap = new FakeSiteMap(securityTrimmingEnabled);

            var home = new FakeSiteMapNode(siteMap, "Home", "Home", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/", metaRobotsContentString: string.Empty);
            siteMap.AddNode(home);

            // About
            var about = new FakeSiteMapNode(siteMap, "About", "About", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/About", metaRobotsContentString: string.Empty);
            siteMap.AddNode(about, home);

            var aboutMe = new FakeSiteMapNode(siteMap, "AboutMe", "About Me", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/AboutMe", metaRobotsContentString: string.Empty);
            siteMap.AddNode(aboutMe, about);

            var aboutYou = new FakeSiteMapNode(siteMap, "AboutYou", "About You", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/AboutYou", metaRobotsContentString: string.Empty);
            siteMap.AddNode(aboutYou, about);

            // Contact

            var contact = new FakeSiteMapNode(siteMap, "Contact", "Contact", isDynamic: false, isAccessibleToUser: false, isVisible: true, isClickable: true, url: "/Home/Contact", metaRobotsContentString: string.Empty);
            siteMap.AddNode(contact, home);

            var contactSomebody = new FakeSiteMapNode(siteMap, "ContactSomebody", "Contact Somebody", isDynamic: false, isAccessibleToUser: false, isVisible: true, isClickable: true, url: "/Home/Contact/ContactSomebody", metaRobotsContentString: string.Empty);
            siteMap.AddNode(contactSomebody, contact);

            // Categories

            var categories = new FakeSiteMapNode(siteMap, "Categories", "Categories", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: false, url: "/Categories", metaRobotsContentString: string.Empty);
            siteMap.AddNode(categories, home);

            // Categories/Cameras

            var cameras = new FakeSiteMapNode(siteMap, "Cameras", "Cameras", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras", metaRobotsContentString: string.Empty);
            siteMap.AddNode(cameras, categories);

            var nikonCoolpix200 = new FakeSiteMapNode(siteMap, "NikonCoolpix200", "Nikon Coolpix 200", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras/NikonCoolpix200", metaRobotsContentString: string.Empty);
            siteMap.AddNode(nikonCoolpix200, cameras);

            var canonIxus300 = new FakeSiteMapNode(siteMap, "CanonIxus300", "Canon Ixus 300", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras/CanonIxus300", metaRobotsContentString: string.Empty);
            siteMap.AddNode(canonIxus300, cameras);

            // Categories/MemoryCards

            var memoryCards = new FakeSiteMapNode(siteMap, "MemoryCards", "Memory Cards", isDynamic: false, isAccessibleToUser: true, isVisible: false, isClickable: true, url: "/Categories/MemoryCards", metaRobotsContentString: string.Empty);
            siteMap.AddNode(memoryCards, categories);

            var kingston256GBSD = new FakeSiteMapNode(siteMap, "Kingston256GBSD", "Kingston 256 GB SD", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Kingston256GBSD", metaRobotsContentString: string.Empty);
            siteMap.AddNode(kingston256GBSD, memoryCards);

            var sony256GBSD = new FakeSiteMapNode(siteMap, "Sony256GBSD", "Sony 256 GB SD", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Sony256GBSD", metaRobotsContentString: string.Empty);
            siteMap.AddNode(sony256GBSD, memoryCards);

            var sonySDCardReader = new FakeSiteMapNode(siteMap, "SonySDCardReader", "Sony SD Card Reader", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Sony256GBSD/SonySDCardReader", metaRobotsContentString: string.Empty);
            siteMap.AddNode(sonySDCardReader, sony256GBSD);


            // Current node is cameras category
            siteMap.SetCurrentNode(cameras);

            return siteMap;
        }

        #endregion

        #region Tests

        [Test]
        public void BuildModel_Case1DefaultMenu_ShouldReturnAllNodesAtRootLevel()
        {
            // @Html.MvcSiteMap().Menu()

            // Arrange
            var siteMap = this.CreateFakeSiteMapCase1();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext, this.iView);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = MenuHelper.BuildModel(
                helper: helperExtension, 
                sourceMetadata: new SourceMetadataDictionary(), 
                startingNode: startingNode, 
                startingNodeInChildLevel: true, 
                showStartingNode: true, 
                maxDepth: Int32.MaxValue, 
                drillDownToCurrent: false, 
                visibilityAffectsDescendants: true);

            // Assert
            // Flat structure - 3 nodes
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[1].Title);
            Assert.AreEqual("Contact", result.Nodes[2].Title);
        }

        [Test]
        public void BuildModel_Case1_StartingNodeNotInChildLevel_ShouldReturnHierarchicalNodes()
        {
            // @Html.MvcSiteMap().Menu(true, false, true)

            // Arrange
            var siteMap = this.CreateFakeSiteMapCase1();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext, this.iView);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = MenuHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                showStartingNode: true,
                maxDepth: Int32.MaxValue,
                drillDownToCurrent: false,
                visibilityAffectsDescendants: true);

            // Assert
            // Tree structure - 3 nodes
            Assert.AreEqual("Home", result.Nodes[0].Title);
            Assert.AreEqual("About", result.Nodes[0].Children[0].Title);
            Assert.AreEqual("Contact", result.Nodes[0].Children[1].Title);
        }

        [Test]
        public void BuildModel_Case1_DontShowStartingNode_ShouldReturnAllNodesAtRootLevelWithoutStartingNode()
        {
            // @Html.MvcSiteMap().Menu(false)

            // Arrange
            var siteMap = this.CreateFakeSiteMapCase1();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext, this.iView);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = MenuHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: true,
                showStartingNode: false,
                maxDepth: Int32.MaxValue,
                drillDownToCurrent: false,
                visibilityAffectsDescendants: true);

            // Assert
            Assert.AreEqual("About", result.Nodes[0].Title);
            Assert.AreEqual("Contact", result.Nodes[1].Title);
        }

        [Test]
        public void BuildModel_Case2_StartingNodeNotInChildLevel_ShouldReturnHierarchicalNodes()
        {
            // @Html.MvcSiteMap().Menu(true, false, true)

            // Arrange
            var siteMap = this.CreateFakeSiteMapCase2();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext, this.iView);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = MenuHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                showStartingNode: true,
                maxDepth: Int32.MaxValue,
                drillDownToCurrent: false,
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
            // @Html.MvcSiteMap().Menu(true, false, true, false)

            // Arrange
            var siteMap = this.CreateFakeSiteMapCase2();
            var startingNode = siteMap.RootNode;
            HtmlHelper helper = new HtmlHelper(this.viewContext, this.iView);
            MvcSiteMapHtmlHelper helperExtension = new MvcSiteMapHtmlHelper(helper, siteMap, false);

            // Act
            var result = MenuHelper.BuildModel(
                helper: helperExtension,
                sourceMetadata: new SourceMetadataDictionary(),
                startingNode: startingNode,
                startingNodeInChildLevel: false,
                showStartingNode: true,
                maxDepth: Int32.MaxValue,
                drillDownToCurrent: false,
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
        }

        #endregion
    }
}
