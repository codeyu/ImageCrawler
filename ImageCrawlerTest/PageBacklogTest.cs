using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for PageBacklogTest and is intended
    ///to contain all PageBacklogTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PageBacklogTest
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
        ///A test for getAndRemovePageFromTopOfTheStack
        ///</summary>
        [TestMethod()]
        public void getAndRemovePageFromTopOfTheStackTest()
        {
            //
            PageBacklog target = new PageBacklog();
            Page page = new Page();
            page.urlStr = "http://www.google.de";
            target.addPage(page);

            Assert.AreEqual(1, target.getBacklogSizeForUndonePages());

            //
            Page actual = target.getAndRemovePageFromTopOfTheUndoneStack();

            Assert.AreEqual(page, actual);
            Assert.AreEqual(0, target.getBacklogSizeForUndonePages());
            
        }

        /// <summary>
        ///A test for getPageByUrlStr
        ///</summary>
        [TestMethod()]
        public void getPageByUrlStrTest()
        {
            //
            PageBacklog target = new PageBacklog();
            Page page = new Page();
            String urlStr = "http://www.google.de";
            page.urlStr = urlStr;
            target.addPage(page);

            //
            Page actual = target.getPageByUrlStr(urlStr);
            Assert.AreEqual(page, actual);
            Assert.AreEqual(1, target.getBacklogSizeForUndonePages());
            Assert.AreEqual(1, target.getBacklogSizeForAllRegisteredPages());
            Assert.AreEqual(true, target.isBacklogForUndonePagesNotEmpty());

            //
            target.getAndRemovePageFromTopOfTheUndoneStack();
            Assert.AreEqual(0, target.getBacklogSizeForUndonePages());
            Assert.AreEqual(1, target.getBacklogSizeForAllRegisteredPages());

            //
            target.removePage(page);
            Assert.AreEqual(0, target.getBacklogSizeForUndonePages());
            Assert.AreEqual(0, target.getBacklogSizeForAllRegisteredPages());
            Assert.AreEqual(false, target.isBacklogForUndonePagesNotEmpty());

            //
            target.addPage(page);
            target.removePage(page);
            Assert.AreEqual(0, target.getBacklogSizeForUndonePages());
            Assert.AreEqual(0, target.getBacklogSizeForAllRegisteredPages());
            Assert.AreEqual(false, target.isBacklogForUndonePagesNotEmpty());

            //
            target.addPage(page);
            target.getAndRemovePageFromTopOfTheUndoneStack();
            target.addPage(page);
            Assert.AreEqual(0, target.getBacklogSizeForUndonePages());
            Assert.AreEqual(1, target.getBacklogSizeForAllRegisteredPages());
            Assert.AreEqual(false, target.isBacklogForUndonePagesNotEmpty());
        }        
    }
}
