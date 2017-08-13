using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Common;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for CrawlerJobPartDownloadTest and is intended
    ///to contain all CrawlerJobPartDownloadTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerJobPartDownloadTest
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
        ///A test for run
        ///</summary>
        [TestMethod()]
        public void runTest()
        {
            //
            CrawlerJobPartDownload crawlerJobPartDownload = new CrawlerJobPartDownload(); // TODO: Initialize to an appropriate value
            Page page = new Page();
            page.urlStr = "http://www.ftd.de";
            crawlerJobPartDownload.crawlerJobContext = new CrawlerJobContext();
            crawlerJobPartDownload.crawlerJobContext.page = page;

            double averagePercentage = 0;
            crawlerJobPartDownload.progressPercentageChangeEvent += new ImageCrawler.Crawler.Job.CrawlerJobPartProgressUpdate(delegate(double progressPercentage) {
                averagePercentage = (4.0 * averagePercentage + progressPercentage) / 5.0;
            });

            //
            List<ICrawlerJobPart> result = crawlerJobPartDownload.run();

            //
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result[0] is CrawlerJobPartAnalyzePage);

            //
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.pageContent);
            Assert.IsTrue(page.pageContent.Length > 1000);
            Assert.IsTrue(averagePercentage > 0.3);

        }
    }
}
