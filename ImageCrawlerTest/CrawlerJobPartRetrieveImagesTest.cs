using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Job;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for CrawlerJobPartRetrieveImagesTest and is intended
    ///to contain all CrawlerJobPartRetrieveImagesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerJobPartRetrieveImagesTest
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
            CrawlerJobPartRetrieveImages crawlerJobPartRetrieveImages = new CrawlerJobPartRetrieveImages();

            //
            Page page = new Page();
            List<String> imageSrcLinkList = new List<String>();
            page.imageSrcLinkList = imageSrcLinkList;
            imageSrcLinkList.Add("http://static1.ftdcdn.de/images/ftd-logo.png");
            List<CrawlerImage> imageList = new List<CrawlerImage>();
            crawlerJobPartRetrieveImages.crawlerJobContext = new CrawlerJobContext();
            crawlerJobPartRetrieveImages.crawlerJobContext.page =page;

            //
            CrawlerImageFilter crawlerImageFilter = new CrawlerImageFilter();
            crawlerImageFilter.minimumFilesize = 1;
            crawlerJobPartRetrieveImages.crawlerJobContext.crawlerImageFilter = crawlerImageFilter;

            //
            crawlerJobPartRetrieveImages.crawlerJobPartImageRetrievedEvent+=new CrawlerJobPartImageRetrievedEvent(delegate(CrawlerImage crawlerImage){
                imageList.Add(crawlerImage);
            });

            //            
            List<ICrawlerJobPart> result = crawlerJobPartRetrieveImages.run();

            //
            Assert.IsNotNull(result);
            Assert.AreEqual(1,imageList.Count);
            Assert.IsNotNull(imageList[0].image);
            Assert.IsNotNull(imageList[0].type);
            Assert.IsNotNull(imageList[0].srcUrlStr);
            Assert.AreEqual("png",imageList[0].type);
            
        }
    }
}
