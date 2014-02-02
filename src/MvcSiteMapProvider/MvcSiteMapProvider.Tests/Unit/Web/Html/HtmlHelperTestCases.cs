using System;

namespace MvcSiteMapProvider.Tests.Unit.Web.Html
{
    class HtmlHelperTestCases
    {
        public static ISiteMap CreateFakeSiteMapCase1()
        {
            // All nodes are accessible and not dynamic
            // (+) means visible, (-) means not visible

            //  /Home/Index(+) (or /)
            //      /Home/About(+)
            //      /Home/Contact(+)
            
            var siteMap = new FakeSiteMap(securityTrimmingEnabled: true, visibilityAffectsDescendants: true);

            var home = new FakeSiteMapNode(siteMap, "Home", "Home", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/", metaRobotsContentString: string.Empty);
            var about = new FakeSiteMapNode(siteMap, "About", "About", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/About", metaRobotsContentString: string.Empty);
            var contact = new FakeSiteMapNode(siteMap, "Contact", "Contact", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/Contact", metaRobotsContentString: string.Empty);
            
            siteMap.AddNode(home);
            siteMap.AddNode(about, home);
            siteMap.AddNode(contact, home);

            siteMap.SetCurrentNode(about);

            return siteMap;
        }

        public static ISiteMap CreateFakeSiteMapCase2()
        {
            // All nodes are not dynamic
            // (+) means visible, (-) means not visible
            // (a) means accessible, (p) means protected

            //  /Home/Index(+a) (or /)
            //      /Home/About(+a)
            //          /Home/About/AboutMe(+a)
            //          /Home/About/AboutYou(+a)
            //      /Home/Contact(+)
            //          /Home/Contact/ContactSomebody(+p)
            //      /Categories(+a)
            //          /Categories/Cameras(+a)
            //          /Categories/Cameras/NikonCoolpix200(+p)
            //          /Categories/Cameras/CanonIxus300(+a)
            //      /Categories/MemoryCards(+a)
            //          /Categories/MemoryCards/Kingston256GBSD(+a)
            //          /Categories/MemoryCards/Sony256GBSD(+a)
            //              /Categories/MemoryCards/Sony256GBSD/SonySDCardReader(+a)


            var siteMap = new FakeSiteMap(securityTrimmingEnabled: true, visibilityAffectsDescendants: true);

            // Home
            var home = new FakeSiteMapNode(siteMap, "Home", "Home", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/", metaRobotsContentString: string.Empty);
            
            // About
            var about = new FakeSiteMapNode(siteMap, "About", "About", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/About", metaRobotsContentString: string.Empty);
            var aboutMe = new FakeSiteMapNode(siteMap, "AboutMe", "About Me", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/AboutMe", metaRobotsContentString: string.Empty);
            var aboutYou = new FakeSiteMapNode(siteMap, "AboutYou", "About You", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Home/AboutYou", metaRobotsContentString: string.Empty);
            
            // Contact
            var contact = new FakeSiteMapNode(siteMap, "Contact", "Contact", isDynamic: false, isAccessibleToUser: false, isVisible: true, isClickable: true, url: "/Home/Contact", metaRobotsContentString: string.Empty);
            var contactSomebody = new FakeSiteMapNode(siteMap, "ContactSomebody", "Contact Somebody", isDynamic: false, isAccessibleToUser: false, isVisible: true, isClickable: true, url: "/Home/Contact/ContactSomebody", metaRobotsContentString: string.Empty);

            // Categories
            var categories = new FakeSiteMapNode(siteMap, "Categories", "Categories", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: false, url: "/Categories", metaRobotsContentString: string.Empty);
            
            // Categories/Cameras
            var cameras = new FakeSiteMapNode(siteMap, "Cameras", "Cameras", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras", metaRobotsContentString: string.Empty);
            var nikonCoolpix200 = new FakeSiteMapNode(siteMap, "NikonCoolpix200", "Nikon Coolpix 200", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras/NikonCoolpix200", metaRobotsContentString: string.Empty);
            var canonIxus300 = new FakeSiteMapNode(siteMap, "CanonIxus300", "Canon Ixus 300", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/Cameras/CanonIxus300", metaRobotsContentString: string.Empty);
            
            // Categories/MemoryCards
            var memoryCards = new FakeSiteMapNode(siteMap, "MemoryCards", "Memory Cards", isDynamic: false, isAccessibleToUser: true, isVisible: false, isClickable: true, url: "/Categories/MemoryCards", metaRobotsContentString: string.Empty);
            var kingston256GBSD = new FakeSiteMapNode(siteMap, "Kingston256GBSD", "Kingston 256 GB SD", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Kingston256GBSD", metaRobotsContentString: string.Empty);
            var sony256GBSD = new FakeSiteMapNode(siteMap, "Sony256GBSD", "Sony 256 GB SD", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Sony256GBSD", metaRobotsContentString: string.Empty);
            var sonySDCardReader = new FakeSiteMapNode(siteMap, "SonySDCardReader", "Sony SD Card Reader", isDynamic: false, isAccessibleToUser: true, isVisible: true, isClickable: true, url: "/Categories/MemoryCards/Sony256GBSD/SonySDCardReader", metaRobotsContentString: string.Empty);
            
            // Home
            siteMap.AddNode(home);

            // About
            siteMap.AddNode(about, home);
            siteMap.AddNode(aboutMe, about);
            siteMap.AddNode(aboutYou, about);

            // Contact
            siteMap.AddNode(contact, home);
            siteMap.AddNode(contactSomebody, contact);

            // Categories
            siteMap.AddNode(categories, home);

            // Categories/Cameras
            siteMap.AddNode(cameras, categories);
            siteMap.AddNode(nikonCoolpix200, cameras);
            siteMap.AddNode(canonIxus300, cameras);

            // Categories/MemoryCards
            siteMap.AddNode(memoryCards, categories);
            siteMap.AddNode(kingston256GBSD, memoryCards);
            siteMap.AddNode(sony256GBSD, memoryCards);
            siteMap.AddNode(sonySDCardReader, sony256GBSD);


            // Current node is cameras category
            siteMap.SetCurrentNode(cameras);

            return siteMap;
        }

        public static ISiteMap CreateFakeSiteMapCase3()
        {
            // All nodes are accessible and not dynamic
            // (+) means visible, (-) means not visible

            // /(+)
            //  /Root/A(+)
            //      /Root/AA(-)
            //          /Root/AAA(+)
            //              /Root/AAAA(+)
            //          /Root/AAB(+)
            //          /Root/AAC(+)
            //      /Root/AB(-)
            //          /Root/ABA(-)
            //              /Root/ABAA(+)
            //              /Root/ABAB(+)
            //              /Root/ABAC(-)
            //          /Root/ABB(+)
            //          /Root/ABC(+)
            //      /Root/AC(-)
            //          /Root/ACA(-)
            //              /Root/ACAA(+)
            //              /Root/ACAB(+)
            //              /Root/ACAC(-)
            //          /Root/ACB(-)
            //          /Root/ACC(-)
            //      /Root/AD(-)
            //          /Root/ADA(-)
            //              /Root/ADAA(-)
            //                  /Root/ADAAA(+)
            //              /Root/ADAB(-)
            //              /Root/ADAC(-)
            //          /Root/ADB(+)
            //          /Root/ADC(+)
            //  /Root/B(+)
            //  /Root/C(+)

            var siteMap = new FakeSiteMap(securityTrimmingEnabled: true, visibilityAffectsDescendants: true);

            var root = new FakeSiteMapNode(siteMap, "Root", "Root", url: "/", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var a = new FakeSiteMapNode(siteMap, "A", "A", url: "/Root/A", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aa = new FakeSiteMapNode(siteMap, "AA", "AA", url: "/Root/AA", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aaa = new FakeSiteMapNode(siteMap, "AAA", "AAA", url: "/Root/AAA", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aaaa = new FakeSiteMapNode(siteMap, "AAAA", "AAAA", url: "/Root/AAAA", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aab = new FakeSiteMapNode(siteMap, "AAB", "AAB", url: "/Root/AAB", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aac = new FakeSiteMapNode(siteMap, "AAC", "AAC", url: "/Root/AAC", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var ab = new FakeSiteMapNode(siteMap, "AB", "AB", url: "/Root/AB", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aba = new FakeSiteMapNode(siteMap, "ABA", "ABA", url: "/Root/ABA", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var abaa = new FakeSiteMapNode(siteMap, "ABAA", "ABAA", url: "/Root/ABAA", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var abab = new FakeSiteMapNode(siteMap, "ABAB", "ABAB", url: "/Root/ABAB", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var abac = new FakeSiteMapNode(siteMap, "ABAC", "ABAC", url: "/Root/ABAC", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var abb = new FakeSiteMapNode(siteMap, "ABB", "ABB", url: "/Root/ABB", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var abc = new FakeSiteMapNode(siteMap, "ABC", "ABC", url: "/Root/ABC", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var ac = new FakeSiteMapNode(siteMap, "AC", "AC", url: "/Root/AC", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var aca = new FakeSiteMapNode(siteMap, "ACA", "ACA", url: "/Root/ACA", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var acaa = new FakeSiteMapNode(siteMap, "ACAA", "ACAA", url: "/Root/ACAA", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var acab = new FakeSiteMapNode(siteMap, "ACAB", "ACAB", url: "/Root/ACAB", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var acac = new FakeSiteMapNode(siteMap, "ACAC", "ACAC", url: "/Root/ACAC", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var acb = new FakeSiteMapNode(siteMap, "ACB", "ACB", url: "/Root/ACB", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var acc = new FakeSiteMapNode(siteMap, "ACC", "ACC", url: "/Root/ACC", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var ad = new FakeSiteMapNode(siteMap, "AD", "AD", url: "/Root/AD", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var ada = new FakeSiteMapNode(siteMap, "ADA", "ADA", url: "/Root/ADA", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adaa = new FakeSiteMapNode(siteMap, "ADAA", "ADAA", url: "/Root/ADAA", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adaaa = new FakeSiteMapNode(siteMap, "ADAAA", "ADAAA", url: "/Root/ADAAA", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adab = new FakeSiteMapNode(siteMap, "ADAB", "ADAB", url: "/Root/ADAB", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adac = new FakeSiteMapNode(siteMap, "ADAC", "ADAC", url: "/Root/ADAC", isVisible: false, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adb = new FakeSiteMapNode(siteMap, "ADB", "ADB", url: "/Root/ADB", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var adc = new FakeSiteMapNode(siteMap, "ADC", "ADC", url: "/Root/ADC", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var b = new FakeSiteMapNode(siteMap, "B", "B", url: "/Root/B", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);
            var c = new FakeSiteMapNode(siteMap, "C", "C", url: "/Root/C", isVisible: true, isDynamic: false, isAccessibleToUser: true, isClickable: true, metaRobotsContentString: string.Empty);

            siteMap.AddNode(root);
            siteMap.AddNode(a, root);
            siteMap.AddNode(aa, a);
            siteMap.AddNode(aaa, aa);
            siteMap.AddNode(aaaa, aaa);
            siteMap.AddNode(aab, aa);
            siteMap.AddNode(aac, aa);
            siteMap.AddNode(ab, a);
            siteMap.AddNode(aba, ab);
            siteMap.AddNode(abaa, aba);
            siteMap.AddNode(abab, aba);
            siteMap.AddNode(abac, aba);
            siteMap.AddNode(abb, ab);
            siteMap.AddNode(abc, ab);
            siteMap.AddNode(ac, a);
            siteMap.AddNode(aca, ac);
            siteMap.AddNode(acaa, aca);
            siteMap.AddNode(acab, aca);
            siteMap.AddNode(acac, aca);
            siteMap.AddNode(acb, ac);
            siteMap.AddNode(acc, ac);
            siteMap.AddNode(ad, a);
            siteMap.AddNode(ada, ad);
            siteMap.AddNode(adaa, ada);
            siteMap.AddNode(adaaa, adaa);
            siteMap.AddNode(adab, ada);
            siteMap.AddNode(adac, ada);
            siteMap.AddNode(adb, ad);
            siteMap.AddNode(adc, ad);
            siteMap.AddNode(b, root);
            siteMap.AddNode(c, root);
            
            return siteMap;
        }
    }
}
