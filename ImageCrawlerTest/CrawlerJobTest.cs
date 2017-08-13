using ImageCrawler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.View.GUI;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawlerTest
{
    
    
    /// <summary>
    ///This is a test class for CrawlerJobTest and is intended
    ///to contain all CrawlerJobTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CrawlerJobTest
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
            Page page = new Page();

            //
            CrawlerJob crawlerJob = new CrawlerJob();
            crawlerJob.setParentalCrawlerController(new CrawlerControllerMock());
            crawlerJob.addCrawlerJobPartAndSetPage(new CrawlerJobPartMock(5), page);
            
            //
            int processFinishedCounter = 0;
            int processStateChangedCounter = 0;
            crawlerJob.processStateChangeEvent += new CrawlerJobProcessStateChangeEvent(delegate(ICrawlerJob job, CrawlerJobProcessState processState)
            {
                processStateChangedCounter++;

                if (processState == CrawlerJobProcessState.Finished)
                {
                    processFinishedCounter++;
                }
            });

            int progressPercentageChangeCounter = 0;
            crawlerJob.progressPercentageChangeEvent += new CrawlerJobProgressPercentageChangeEvent(delegate(ICrawlerJob job, double percentage)
            {
                progressPercentageChangeCounter++;
            });

            //
            crawlerJob.run();

            //
            Assert.AreEqual(1, processFinishedCounter);
            Assert.AreEqual(5 + 2, processStateChangedCounter);
            Assert.AreEqual(5 * 10 + 2, progressPercentageChangeCounter);

        }


        /// <summary>
        /// Mock crawler controller.
        /// </summary>
        private class CrawlerControllerMock : ICrawlerController
        {

            #region ICrawlerController Members

            public ICrawlerJobView getCrawlerJobView()
            {
                return null;
            }

            public ICrawlerJobView startCrawling(Page page)
            {
                return null;
            }

            public void addPageList(List<Page> pageList)
            {
            }

            public void addPage(Page page)
            {
            }

            public ManualResetEvent getFinishedEvent()
            {
                return null;
            }

            public void clear()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerController Members


            public void stopCrawling()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerController Members


            public void suspendCrawling()
            {
                throw new NotImplementedException();
            }

            public void resumeCrawling()
            {
                throw new NotImplementedException();
            }

            public bool isSuspended()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerController Members

            public event CrawlerControllerFinishedWorkingEvent crawlerControllerFinishedWorkingEvent;

            #endregion

            #region ICrawlerController Members


            public void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerController Members

            public void setMaximumCrawlerJobThreadCount(int threadCount)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region ICrawlerController Members


            public void setImageLoggingFolder(string foldername)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Mock job part, which will introduce itself as many times, as the given constructor counter.
        /// </summary>
        private class CrawlerJobPartMock : ICrawlerJobPart
        {
            private int toZeroCounter = 0;

            public CrawlerJobPartMock(int toZeroCounter)
            {
                this.toZeroCounter = toZeroCounter;
            }

            #region ICrawlerJobPart Members

            public CrawlerJobContext crawlerJobContext { get; set; }

            public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
            public event ImageCrawler.Crawler.Job.CrawlerJobPartProgressUpdate progressPercentageChangeEvent;

            public List<ICrawlerJobPart> run()
            {
                //
                List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

                //
                if (this.toZeroCounter > 1)
                {
                    crawlerJobPartList.Add(new CrawlerJobPartMock(this.toZeroCounter - 1));
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
                            this.crawlerJobPartImageRetrievedEvent(new CrawlerImage());
                        }
                    }
                }

                //
                return crawlerJobPartList;
            }

            public CrawlerJobProcessState getProcessState()
            {
                if (this.toZeroCounter > 1)
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

            

        }
    }
}
