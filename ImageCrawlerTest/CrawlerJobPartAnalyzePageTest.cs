using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for CrawlerJobPartAnalyzePageTest and is intended
    ///to contain all CrawlerJobPartAnalyzePageTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerJobPartAnalyzePageTest
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
        public void runTestSimplePage()
        {
            //
            Page page = new Page();
            page.urlStr = "http://www.google.de";
            const String ankerUrlStr = "http://www.google.de/some_sub_path/";
            const String imageUrlStr = "/bilder/bild1.jpg";
            String imageUrlStrAbsolut = page.urlStr + imageUrlStr;

            String ankerTag = "<a \n   href=\"" + ankerUrlStr + "\" attr2 = \"value2\">skjfk</a>";
            String imageTag = "<img \r  src\r\n=\n\r   \"" + imageUrlStr + "\" some=\"other\" flags=\"flag\">";
            page.pageContent = "jsdlkfjösjf ksjfj jdsflkjas f " + ankerTag + "sjlkfjsf" + imageTag+ "sjdfkkdsjflk";

            //
            CrawlerJobPartAnalyzePage crawlerJobPartAnalyzePage = new CrawlerJobPartAnalyzePage();
            CrawlerJobContext crawlerJobContext = new CrawlerJobContext();
            crawlerJobContext.page = page;
            crawlerJobContext.imageBacklog = new CrawlerImageBacklog();
            crawlerJobPartAnalyzePage.crawlerJobContext = crawlerJobContext;


            //
            List<ICrawlerJobPart> result = crawlerJobPartAnalyzePage.run();

            //
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result[0] is CrawlerJobPartGenerateSubJobs);
            Assert.IsTrue(result[1] is CrawlerJobPartRetrieveImages);

            //
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.pageContent);
            Assert.IsNotNull(page.ankerHrefLinkList);
            Assert.AreEqual(1, page.ankerHrefLinkList.Count);
            Assert.AreEqual(ankerUrlStr, page.ankerHrefLinkList[0]);

            Assert.IsNotNull(page.imageSrcLinkList);
            Assert.AreEqual(1, page.imageSrcLinkList.Count);
            Assert.AreEqual(imageUrlStrAbsolut, page.imageSrcLinkList[0]);
        }

        /// <summary>
        ///A test for run
        ///</summary>
        [TestMethod()]
        public void runTestMetataggedPage()
        {
            //
            Page page = new Page();
            page.urlStr = "http://www.google.de";
            const String ankerUrlStr = "http://www.google.de/some_sub_path/";
            const String imageUrlStr = "/bilder/bild1.jpg";
            String imageUrlStrAbsolut = page.urlStr + imageUrlStr;

            String ankerTag = "<a \n   href=\"" + ankerUrlStr + "\" attr2 = \"value2\">skjfk</a>";
            String imageTag = "<img \r  src\r\n=\n\r   \"" + imageUrlStr + "\" some=\"other\" flags=\"flag\">";
            page.pageContent = "<mEta name=\"roBots\" content=\"ebc,noFollow,lala\">js<mEta name=\"roBots\" content=\"ebc,noIndEx,lala\">dlkfjösjf ksjfj jdsflkjas f " + ankerTag + "sjlkfjsf" + imageTag + "sjdfkkdsjflk";

            //
            CrawlerJobPartAnalyzePage crawlerJobPartAnalyzePage = new CrawlerJobPartAnalyzePage();
            CrawlerJobContext crawlerJobContext = new CrawlerJobContext();
            crawlerJobContext.page = page;
            crawlerJobContext.imageBacklog = new CrawlerImageBacklog();
            crawlerJobPartAnalyzePage.crawlerJobContext = crawlerJobContext;

            //
            List<ICrawlerJobPart> result = crawlerJobPartAnalyzePage.run();

            //
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);

            //
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.pageContent);
            Assert.IsNull(page.ankerHrefLinkList);
            Assert.IsNull(page.imageSrcLinkList);
        }
    }
}
