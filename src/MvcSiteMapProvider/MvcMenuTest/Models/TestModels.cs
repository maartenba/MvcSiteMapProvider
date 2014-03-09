using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MvcMenuTest.Models
{
    public class TestCaseCollection : List<TestCase>
    {
        public delegate MvcHtmlString Method_01(bool a, bool b, bool c);
        public delegate MvcHtmlString Method_02(int a, bool b, bool c, int d);
        public delegate MvcHtmlString Method_03(int a, bool b, bool c, int d, bool e, bool f);

        public Method_01 Method01 { get; set; }
        public Method_02 Method02 { get; set; }
        public Method_03 Method03 { get; set; }
        public TestCaseCollection()
        {
            new ArrayList().Add(Method01);
            this.Add(new TestCase()
            {
                Case = "@Html.MvcSiteMap().Menu(true, false, true)",
                MethodForTest = TestCase.Method.M1,
                Param = new object[] { true, false, true },
                ExpectedOutput = RenderOutput(
                new Node(null, "Root",
                    new Node("/Test", "Test",
                        new Node("/Home/A", "A",
                            new Node("/Home/A1", "A1"),
                            new Node("/Home/A2", "A2"),
                            new Node("/Home/A3", "A3",
                                new Node("/Home/A31", "A3.1"),
                                new Node("/Home/A32", "A3.2")
                            )
                        ),
                        new Node("/Home/B", "B",
                            new Node("/Home/B1", "B1"),
                            new Node("/Home/B2", "B2")
                        )
                    )
                ))
            });
            this.Add(new TestCase()
            {
                Case = "@Html.MvcSiteMap().Menu(false, true, false)",
                MethodForTest = TestCase.Method.M1,
                Param = new object[] { false, true, false },
                ExpectedOutput = RenderOutput(
                new Node(null, "Root",
                    new Node("/Home/About", "About"),
                    new Node("/Home/Contact", "Contact"),
                    new Node("/Test", "Test",
                        new Node("/Home/A", "A",
                            new Node("/Home/A1", "A1"),
                            new Node("/Home/A2", "A2"),
                            new Node("/Home/A3", "A3",
                                new Node("/Home/A31", "A3.1"),
                                new Node("/Home/A32", "A3.2")
                            )
                        ),
                        new Node("/Home/B", "B",
                            new Node("/Home/B1", "B1"),
                            new Node("/Home/B2", "B2")
                        )
                    )
                ))
            });
            this.Add(new TestCase()
            {
                Case = "@Html.MvcSiteMap().Menu(0, true, false, 2)",
                MethodForTest = TestCase.Method.M2,
                Param = new object[] { 0, true, false, 2 },
                ExpectedOutput = RenderOutput(
                new Node(null, "Root",
                    new Node("/Home/About", "About"),
                    new Node("/Home/Contact", "Contact"),
                    new Node("/Test", "Test",
                        new Node("/Home/A", "A"),
                        new Node("/Home/B", "B")
                    )
                ))
            });
            this.Add(new TestCase()
            {
                Case = "@Html.MvcSiteMap().Menu(0, false, true, 1)",
                MethodForTest = TestCase.Method.M2,
                Param = new object[] { 0, false, true, 1 },
                ExpectedOutput = RenderOutput(
                new Node(null, "Root",
                    new Node("/", "Home",
                        new Node("/Home/About", "About"),
                        new Node("/Home/Contact", "Contact"),
                        new Node("/Test", "Test")
                    )
                ))
            });
            this.Add(new TestCase()
            {
                Case = "@Html.MvcSiteMap().Menu(0, false, true, 2, true, true)",
                MethodForTest = TestCase.Method.M3,
                Param = new object[] { 0, false, true, 2, true, true },
                ExpectedOutput = RenderOutput(
                new Node(null, "Root",
                    new Node("/", "Home",
                        new Node("/Home/About", "About"),
                        new Node("/Home/Contact", "Contact"),
                        new Node("/Test", "Test",
                            new Node("/Home/A", "A"),
                            new Node("/Home/B", "B")
                        )
                    )
                ))
            });
        }
        public void ExecuteMethod()
        {
            foreach (var item in this)
            {
                switch (item.MethodForTest)
                {
                    case TestCase.Method.M1:
                        item.Output = Method01.Invoke((bool)item.Param[0], (bool)item.Param[1], (bool)item.Param[2]).ToHtmlString();
                        break;
                    case TestCase.Method.M2:
                        item.Output = Method02.Invoke((int)item.Param[0], (bool)item.Param[1], (bool)item.Param[2], (int)item.Param[3]).ToHtmlString();
                        break;
                    case TestCase.Method.M3:
                        item.Output = Method03.Invoke((int)item.Param[0], (bool)item.Param[1], (bool)item.Param[2], (int)item.Param[3], (bool)item.Param[4], (bool)item.Param[5]).ToHtmlString();
                        break;
                }
            }
        }

        private string RenderOutput(Node element)
        {
            string str = "";
            if (element.HasChild)
            {
                if (element.Title == "Root")
                {
                    str += "<ul id=\"menu\">";
                }
                else
                {
                    str += "<ul>";
                }
                foreach (var child in element.Children)
                {
                    str += "<li>";
                    str += "<a href=\"" + child.Href + "\" title=\"" + child.Title + "\">" + child.Title + "</a>";
                    str += RenderOutput(child);
                    str += "</li>";
                }
                str += "</ul>";
            }
            return str;
        }
    }
    public class Node
    {
        public string Href { get; set; }
        public string Title { get; set; }
        public List<Node> Children;
        public Node(string href, string title)
        {
            Children = new List<Node>();
            Href = href;
            Title = title;
        }
        public Node(string href, string title, params Node[] children)
        {
            Children = children.ToList();
            Href = href;
            Title = title;
        }
        public bool HasChild
        {
            get
            {
                return Children.Count > 0;
            }
        }
    }
    public class TestCase
    {
        public enum Method { M1, M2, M3 }
        public Method MethodForTest { get; set; }
        public string Case { get; set; }
        public object[] Param { get; set; }
        private string _output;
        public string Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value.Replace("\r\n", string.Empty);
                _output = Regex.Replace(_output, @"[ ]+(?=<)", string.Empty);
            }
        }
        public string ExpectedOutput { get; set; }
        public bool IsPassed
        {
            get { return Output == ExpectedOutput; }
        }
    }
}