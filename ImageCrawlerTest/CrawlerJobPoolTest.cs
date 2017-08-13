using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for CrawlerJobPoolTest and is intended
    ///to contain all CrawlerJobPoolTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerJobPoolTest
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
        ///A test for startCrawler
        ///</summary>
        [TestMethod()]
        public void startCrawlerJobsTest()
        {
            //
            int threadCount = 20;
            int jobCount = 200;

            //
            int processStateChangeEventCounter = 0;
            int progressPercentageChangeEventCounter = 0;
            int finishedJobEventCounter = 0;

            //
            CrawlerJobPool crawlerJobPool = new CrawlerJobPool();
            CrawlerJobMock.maximumAllowedSimultanRunningThreadsCount = threadCount;
            for (int ii = 1; ii <= jobCount; ii++)
            {
                ICrawlerJob crawlerJobMock = new CrawlerJobMock();
                crawlerJobMock.processStateChangeEvent += new CrawlerJobProcessStateChangeEvent(delegate(ICrawlerJob job, CrawlerJobProcessState processState)
                {
                    Interlocked.Increment(ref processStateChangeEventCounter);

                    if (processState == CrawlerJobProcessState.Finished)
                    {
                        Interlocked.Increment(ref finishedJobEventCounter);
                    }

                });
                crawlerJobMock.progressPercentageChangeEvent += new CrawlerJobProgressPercentageChangeEvent(delegate(ICrawlerJob job, double percentage)
                {
                    Interlocked.Increment(ref progressPercentageChangeEventCounter);
                });

                crawlerJobPool.addCrawlerJob(crawlerJobMock);
            }

            //
            crawlerJobPool.setMaximumThreadCount(threadCount);
            crawlerJobPool.startCrawlerJobs();
            crawlerJobPool.getFinishedEvent().WaitOne(Timeout.Infinite);

            //
            Assert.AreEqual(jobCount, finishedJobEventCounter);
            Assert.AreEqual(jobCount * (10 + 2), processStateChangeEventCounter);
            Assert.AreEqual(jobCount * (10 + 2), progressPercentageChangeEventCounter);
            Console.WriteLine(CrawlerJobMock.averageRunningThreadCount);
            Assert.IsTrue(CrawlerJobMock.averageRunningThreadCount > threadCount / 8);
        }

        private class CrawlerJobMock : ICrawlerJob
        {
            public static int simultanRunningThreadsCount = 0;
            public static double averageRunningThreadCount = 0;
            public static int maximumAllowedSimultanRunningThreadsCount { get; set; }

            #region ICrawlerJob Members

            public event CrawlerJobImageRetrievedEvent crawlerJobImageRetrievedEvent;
            public event CrawlerJobProgressPercentageChangeEvent progressPercentageChangeEvent;
            public event CrawlerJobProcessStateChangeEvent processStateChangeEvent;

            public void run()
            {
                //
                CrawlerJobMock.simultanRunningThreadsCount++;
                CrawlerJobMock.averageRunningThreadCount = (CrawlerJobMock.simultanRunningThreadsCount + (CrawlerJobMock.maximumAllowedSimultanRunningThreadsCount - 1) * CrawlerJobMock.averageRunningThreadCount) / CrawlerJobMock.maximumAllowedSimultanRunningThreadsCount;

                //
                //Console.WriteLine(CrawlerJobMock.averageRunningThreadCount);

                //
                Assert.IsTrue(CrawlerJobMock.maximumAllowedSimultanRunningThreadsCount >= CrawlerJobMock.simultanRunningThreadsCount);

                //simulating normal crawler job events.
                if (this.processStateChangeEvent != null)
                {
                    this.processStateChangeEvent(this,CrawlerJobProcessState.Initialize);
                }

                if (this.progressPercentageChangeEvent != null)
                {
                    this.progressPercentageChangeEvent(this, 0.0);
                }

                for (int ii = 1; ii <= 10; ii++)
                {
                    if (this.progressPercentageChangeEvent != null)
                    {
                        this.progressPercentageChangeEvent(this, 0.5);
                    }
                    if (this.processStateChangeEvent != null)
                    {
                        this.processStateChangeEvent(this,CrawlerJobProcessState.Download);
                    }

                    if (ii == 9)
                    {
                        if (this.crawlerJobImageRetrievedEvent != null)
                        {
                            this.crawlerJobImageRetrievedEvent(this, new CrawlerImage());
                        }
                    }

                }

                //
                Thread.Sleep(5);

                //
                if (this.progressPercentageChangeEvent != null)
                {
                    this.progressPercentageChangeEvent(this, 1.0000);
                }
                if (this.processStateChangeEvent != null)
                {
                    this.processStateChangeEvent(this,CrawlerJobProcessState.Finished);
                }

                //
                CrawlerJobMock.simultanRunningThreadsCount--;
            }

            public bool hasFinished()
            {
                return true;
            }

            ICrawlerJob ICrawlerJob.addCrawlerJobPartAndSetPage(ICrawlerJobPart crawlerJobPart, Page page)
            {
                return this;
            }

            ICrawlerJob ICrawlerJob.addCrawlerJobPartInitialPageLock(IPageBacklog pageBacklog)
            {
                return this;
            }

            ICrawlerJob ICrawlerJob.setParentalCrawlerController(ICrawlerController crawlerController)
            {
                return this;
            }

            public Page getPage()
            {
                return null;
            }

            public ICrawlerController getParentalCrawlerController()
            {
                throw new NotImplementedException();
            }

            #endregion



            #region ICrawlerJob Members


            public event CrawlerJobSuspendEvent crawlerJobSuspendEvent;

            #endregion

            #region ICrawlerJob Members


            public void doAbort()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerJob Members


            public void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerJob Members


            public void setCrawlerImageBacklog(ICrawlerImageBacklog crawlerImageBacklog)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerJob Members


            public void setPageFilter(ImageCrawler.Crawler.Filter.IPageFilter pageFilter)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
