using ImageCrawler.Crawler.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ImageCrawler.Crawler.Common;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for PageFilterTest and is intended
    ///to contain all PageFilterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageFilterTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for checkForValidPage
        ///</summary>
        [TestMethod()]
        public void checkForValidPageTest()
        {
            //
            PageFilter pageFilter;
            Page page;
            Boolean result;

            //
            pageFilter = new PageFilter();
            page = new Page("http://de.wikipedia.org/wiki/Robots_Exclusion_Standard");
            result = pageFilter.checkForValidPage(page);
            Assert.AreEqual(true, result);

            /*
             * invalid page
             */
            //pageFilter = new PageFilter();
            page = new Page("http://de.wikipedia.org/wiki/Especial:Search");
            result = pageFilter.checkForValidPage(page);
            Assert.AreEqual(false, result);

        }
    }
}
