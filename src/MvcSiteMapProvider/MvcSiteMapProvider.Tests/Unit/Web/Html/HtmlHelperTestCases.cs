using System;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    class HtmlHelperTestCases
    {
        public static ISiteMap CreateFakeSiteMapCase1()
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

        public static ISiteMap CreateFakeSiteMapCase2()
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
    }
}
