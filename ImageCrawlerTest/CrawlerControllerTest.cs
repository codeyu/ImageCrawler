using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Backlog;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.View.GUI;
using System.Drawing;

namespace ImageCrawlerTest
{    
    /// <summary>
    ///This is a test class for CrawlerControllerTest and is intended
    ///to contain all CrawlerControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerControllerTest
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
        ///A test for startCrawling
        ///</summary>
        [TestMethod()]
        public void startCrawlingFtdTest()
        {
            Page page = new Page();
            page.urlStr = "http://www.ftd.de";
            CrawlerController crawlerController = new CrawlerController();
            ICrawlerJobView crawlerJobView = crawlerController.getCrawlerJobView();

            //
            int peekProcessInformationCount = 0;
            int updateEventCounter = 0;
            int imageEventCounter = 0;

            int trackedProcessInformationUpdateCounter = 0;
            int trackedProcessInformationImageUpdateCounter = 0;
            double trackedProcessInformationPercentagePeek = 0.0000;

            //
            CrawlerProcessInformation trackedProcessInformation = null;
            crawlerJobView.updateProcessInformationEvent += new CrawlerJobViewUpdateEventProcessInformation(delegate(CrawlerProcessInformation crawlerProcessInformation)
            {
                List<CrawlerProcessInformation> crawlerProcessInformationList = crawlerJobView.getActiveCrawlerProcessInformationList();
                peekProcessInformationCount = Math.Max(peekProcessInformationCount, crawlerProcessInformationList.Count);
                updateEventCounter++;

                //
                if (trackedProcessInformation == null && crawlerProcessInformation != null)
                {
                    trackedProcessInformation = crawlerProcessInformation;
                    trackedProcessInformation.crawlerProcessInformationUpdateEvent += new CrawlerProcessInformationUpdateEvent(delegate(CrawlerProcessInformation information)
                    {
                        trackedProcessInformationUpdateCounter++;
                        trackedProcessInformationPercentagePeek = Math.Max(trackedProcessInformationPercentagePeek, information.progressPercentage);
                    });
                    trackedProcessInformation.crawlerProcessInformationNewImageEvent += new CrawlerProcessInformationNewImageEvent(delegate(CrawlerImage image)
                    {
                        trackedProcessInformationImageUpdateCounter++;
                    });
                }
            });
            crawlerJobView.updateImageEvent += new CrawlerJobViewUpdateEventImage(delegate(CrawlerImage crawlerImage)
            {
                imageEventCounter++;
            });

            //
            crawlerController.startCrawling(page);
            crawlerController.getFinishedEvent().WaitOne(System.TimeSpan.FromSeconds(5), false);

            //
            Assert.IsTrue(peekProcessInformationCount > 0);
            Assert.IsTrue(updateEventCounter > 10);

            Assert.IsTrue(trackedProcessInformationUpdateCounter > 10);
            Assert.IsTrue(trackedProcessInformationPercentagePeek > 0.3);
        }

        /// <summary>
        ///A test for startCrawling
        ///</summary>
        [TestMethod()]
        public void startCrawlingTest()
        {
            //
            Page page = new Page();
            page.urlStr = "http://www.google.de";
            CrawlerController crawlerController = new CrawlerController();
            ICrawlerJobView crawlerJobView = crawlerController.getCrawlerJobView();

            //
            Assert.IsNotNull(crawlerJobView);

            //
            int peekProcessInformationCount = 0;
            int updateEventCounter = 0;
            int imageEventCounter = 0;

            int trackedProcessInformationUpdateCounter  = 0;
            int trackedProcessInformationImageUpdateCounter = 0;
            double trackedProcessInformationPercentagePeek = 0.0000;
            //
            CrawlerProcessInformation trackedProcessInformation=null;
            crawlerJobView.updateProcessInformationEvent += new CrawlerJobViewUpdateEventProcessInformation(delegate(CrawlerProcessInformation crawlerProcessInformation)
            {
                List<CrawlerProcessInformation> crawlerProcessInformationList = crawlerJobView.getActiveCrawlerProcessInformationList();
                peekProcessInformationCount = Math.Max(peekProcessInformationCount, crawlerProcessInformationList.Count);
                updateEventCounter++;

                //
                if (trackedProcessInformation == null && crawlerProcessInformation != null)
                {
                    trackedProcessInformation = crawlerProcessInformation;
                    trackedProcessInformation.crawlerProcessInformationUpdateEvent += new CrawlerProcessInformationUpdateEvent(delegate(CrawlerProcessInformation information) {
                        trackedProcessInformationUpdateCounter++;
                        trackedProcessInformationPercentagePeek = Math.Max(trackedProcessInformationPercentagePeek, information.progressPercentage);
                    });
                    trackedProcessInformation.crawlerProcessInformationNewImageEvent += new CrawlerProcessInformationNewImageEvent(delegate(CrawlerImage image){
                        trackedProcessInformationImageUpdateCounter++;
                    });
                }
            });
            crawlerJobView.updateImageEvent += new CrawlerJobViewUpdateEventImage(delegate(CrawlerImage crawlerImage){
                imageEventCounter++;
            });

            //
            int crawlerJobCount = 100;
            int crawlerJobPartCount = 10;
            crawlerController.setInitialJobFactory(new InitialJobFactoryMock(crawlerJobCount, crawlerJobPartCount));
            crawlerController.setMaximumCrawlerJobThreadCount(20);
            crawlerController.startCrawling(page);
            crawlerController.getFinishedEvent().WaitOne(Timeout.Infinite, false);

            //
            Assert.IsTrue(peekProcessInformationCount > 1);
            Assert.IsTrue(updateEventCounter > crawlerJobCount * 10);
            Assert.AreEqual(crawlerJobCount * crawlerJobPartCount, imageEventCounter);

            Assert.IsTrue(trackedProcessInformationUpdateCounter > 10);
            Assert.IsTrue(trackedProcessInformationPercentagePeek > 0.3);
            Assert.AreEqual(crawlerJobPartCount, trackedProcessInformationImageUpdateCounter);
        }

        /// <summary>
        /// Mock class to provide another crawler job workflow. 
        /// </summary>
        private class InitialJobFactoryMock : CrawlerController.InitialJobFactory
        {
            private int jobCount;
            private int jobPartCount;

            /// <summary>
            /// Use thread count to determine how many jobs should be introduced.
            /// </summary>
            /// <param name="jobCount"></param>
            public InitialJobFactoryMock(int jobCount, int jobPartCount)
            {
                this.jobCount = jobCount;
                this.jobPartCount = jobPartCount;
            }

            public override ICrawlerJob generateInitialCrawlerJob(ICrawlerController crawlerController, IPageBacklog pageBacklog)
            {
                return new CrawlerJob().setParentalCrawlerController(crawlerController).addCrawlerJobPartAndSetPage(new CrawlerJobPartMock(this.jobPartCount, this.jobCount-1), new Page("http://www.google.de"));
            }
        }

        /// <summary>
        /// Mock job part, which will introduce itself as many times, as the given constructor counter.
        /// </summary>
        private class CrawlerJobPartMock : ICrawlerJobPart
        {
            private int jobPartCounter = 0;
            private int spawnJobsCounter = 0;

            public CrawlerJobPartMock(int jobPartCounter, int spawnJobsCounter)
            {
                this.jobPartCounter = jobPartCounter;
                this.spawnJobsCounter = spawnJobsCounter;
            }            

            #region ICrawlerJobPart Members

            public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
            public event ImageCrawler.Crawler.Job.CrawlerJobPartProgressUpdate progressPercentageChangeEvent;

            public CrawlerJobContext crawlerJobContext { get; set; }

            public List<ICrawlerJobPart> run()
            {
                //
                List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

                //
                if (this.jobPartCounter > 1)
                {
                    crawlerJobPartList.Add(new CrawlerJobPartMock(this.jobPartCounter - 1, 0));
                }

                //
                for (int ii = 1; ii <= 10; ii++)
                {
                    if (this.progressPercentageChangeEvent != null)
                    {
                        this.progressPercentageChangeEvent(ii * 0.1);
                    }

                    if (ii == 9)
                    {
                        if (this.crawlerJobPartImageRetrievedEvent != null)
                        {
                            CrawlerImage crawlerImage = new CrawlerImage();
                            crawlerImage.image = new Bitmap(10,10);
                            this.crawlerJobPartImageRetrievedEvent(crawlerImage);
                        }
                    }
                }

                for (int ii = 1; ii <= this.spawnJobsCounter; ii++)
                {
                    CrawlerController crawlerController = (CrawlerController)this.crawlerJobContext.crawlerController;
                    crawlerController.setInitialJobFactory(new InitialJobFactoryMock(0, this.jobPartCounter));
                    crawlerController.addPage(new Page("http://www.google.de/" + ii));
                }

                //
                return crawlerJobPartList;
            }

            public CrawlerJobProcessState getProcessState()
            {
                if (this.jobPartCounter > 1)
                {
                    return CrawlerJobProcessState.Analyze;
                }
                else
                {
                    return CrawlerJobProcessState.GenerateSubJobs;
                }
            }
            #endregion




            #region ICrawlerJobPart Members


            public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

            #endregion

            #region ICrawlerJobPart Members


            public void doAbort()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerJobPart Members


            public void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter)
            {
            }

            #endregion

            #region ICrawlerJobPart Members


            public void setCrawlerImageBacklog(ICrawlerImageBacklog crawlerImageBacklog)
            {
            }

            #endregion

            
        }
    }
}
