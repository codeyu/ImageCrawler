/*
  Copyright 2010 Danny Kunz

    This file is part of the Image Crawler project.

    Image Crawler is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Image Crawler is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Image Crawler.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.View.GUI;
using ImageCrawler.Crawler.Backlog;
using ImageCrawler.Crawler.ImageLogger;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Filter;

namespace ImageCrawler.Crawler.Controller
{
    /// <summary>
    /// Default implementation of a crawler controller.
    /// </summary>
    public class CrawlerController : ICrawlerController
    {
        //
        protected ICrawlerJobPool crawlerJobPool = new CrawlerJobPool();
        protected ICrawlerJobView crawlerJobView = new CrawlerJobView();
        protected IPageBacklog pageBacklog = new PageBacklog();
        protected IPageBacklogOptimizer pageBacklogOptimizer = new PageBacklogOptimizer();
        protected ICrawlerImageBacklog crawlerImageBacklog = new CrawlerImageBacklog();
        protected IPageFilter pageFilter = new PageFilter();
        protected ICrawlerImageLogger crawlerImageLogger = new CrawlerImageLogger();

        protected CrawlerJobMonitor crawlerJobMonitor = new CrawlerJobMonitor();
        protected InitialJobFactory initialJobFactory = new InitialJobFactory();

        protected CrawlerImageFilter crawlerImageFilter = new CrawlerImageFilter();
        protected Uri baseUri = null;

        //
        public CrawlerController()
        {
            this.crawlerJobView.registerPageBacklog(this.pageBacklog);
            this.crawlerJobView.registerCrawlerJobPool(this.crawlerJobPool);
            this.crawlerJobPool.crawlerJobPoolFinishedWorkingEvent += new CrawlerJobPoolFinishedWorkingEvent(crawlerJobPool_crawlerJobPoolFinishedWorkingEvent);
            this.crawlerJobPool.crawlerJobPoolFinishedCrawlerJob += new CrawlerJobPoolFinishedCrawlerJob(delegate(ICrawlerJob crawlerJob)
            {
                this.crawlerJobMonitor.unregisterCrawlerJob(crawlerJob);
            });
            this.crawlerJobMonitor.imageRetrievedEvent += new EventHandler<CrawlerImage.EventArg>(delegate(object sender, CrawlerImage.EventArg crawlerImageEventArg)
            {
                this.crawlerImageLogger.logCrawlerImage(crawlerImageEventArg.crawlerImage);
            });
            this.pageBacklogOptimizer.setPageBacklog(this.pageBacklog);
            this.crawlerImageLogger.crawlerImageCacheMaximumFilesize = 10000000; //set to 10MB
        }

        public void crawlerJobPool_crawlerJobPoolFinishedWorkingEvent()
        {
            //
            if (this.crawlerControllerFinishedWorkingEvent != null)
            {
                this.crawlerControllerFinishedWorkingEvent();
            }

            //
            this.pageBacklogOptimizer.stopOptimizing();
        }

        #region ICrawlerController Members

        public event CrawlerControllerFinishedWorkingEvent crawlerControllerFinishedWorkingEvent;

        public ICrawlerJobView startCrawling(Page page)
        {
            //
            if (page != null)
            {
                this.normalizePageUrl(page);
                this.determineBaseUrlFromPage(page, true);
                this.addPage(page);
            }

            //
            if (this.pageBacklog.getBacklogSizeForUndonePages() > 0)
            {
                this.crawlerJobPool.startCrawlerJobs();
            }
            else
            {
                this.crawlerJobPool.getFinishedEvent().Set();
            }

            //
            this.pageBacklogOptimizer.startOptimizing();

            //
            return this.crawlerJobView;
        }

        public ICrawlerJobView getCrawlerJobView()
        {
            return this.crawlerJobView;
        }

        public void addPageList(List<Page> pageList)
        {
            foreach (Page page in pageList)
            {
                this.addPage(page);
            }
        }

        public void addPage(Page page)
        {
            if (page != null)
            {
                //
                this.normalizePageUrl(page);

                //
                if (this.isValidPage(page))
                {
                    //if pagelog does not yet contain page add a job for the page.
                    if (this.pageBacklog.addPage(page))
                    {
                        this.generateAndRegisterInitialCrawlerJob();
                    }
                }
            }
        }

        private Boolean isValidPage(Page page)
        {
            //
            Boolean retval = true;

            //
            try
            {
                retval &= page != null;
                retval &= page.urlStr != null;
                if (this.baseUri != null)
                {
                    retval &= this.baseUri == null || (page.urlStr != null && this.baseUri.IsBaseOf(new Uri(page.urlStr)));
                }
            }
            catch (Exception e)
            {
                retval = false;
            }

            //
            return retval;
        }

        private void normalizePageUrl(Page page)
        {
            if (page != null)
            {
                String pageUrlStr = page.urlStr;
                try
                {
                    UriBuilder pageUriBuilder = new UriBuilder(page.urlStr);
                    pageUriBuilder.Fragment = "";
                    page.urlStr = pageUriBuilder.Uri.AbsoluteUri;
                }
                catch (Exception e)
                {
                }
            }
        }

        /// <summary>
        /// Determines the uri of the base page. If force is true, the current base uri will be overriden always, otherwise it will be only set as long no other base uri is set yet.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="force"></param>
        private void determineBaseUrlFromPage(Page page, Boolean force)
        {
            if (page != null && (force || this.baseUri == null))
            {
                try
                {
                    this.baseUri = new Uri(page.urlStr);
                }
                catch (Exception e)
                {
                }
            }
        }

        private void generateAndRegisterInitialCrawlerJob()
        {
            ICrawlerJob crawlerJob = this.initialJobFactory.generateInitialCrawlerJob(this, this.pageBacklog);
            crawlerJob.setCrawlerImageFilter(this.crawlerImageFilter);
            crawlerJob.setCrawlerImageBacklog(this.crawlerImageBacklog);
            crawlerJob.setPageFilter(this.pageFilter);
            this.crawlerJobPool.addCrawlerJob(crawlerJob);
            CrawlerProcessInformation crawlerProcessInformation = this.crawlerJobMonitor.registerCrawlerJob(crawlerJob);
            this.crawlerJobView.registerCrawlerProcessInformation(crawlerProcessInformation);
        }

        public void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter)
        {
            this.crawlerImageFilter = crawlerImageFilter;
        }

        public void setMaximumCrawlerJobThreadCount(int threadCount)
        {
            this.crawlerJobPool.setMaximumThreadCount(threadCount);
        }

        public ManualResetEvent getFinishedEvent()
        {
            return this.crawlerJobPool.getFinishedEvent();
        }

        public void clear()
        {
            this.crawlerJobView.clear();
            this.pageBacklog.clear();
            this.crawlerImageBacklog.clear();
            this.crawlerImageLogger.clear();
        }

        public void stopCrawling()
        {
            this.crawlerJobPool.stopCrawlerJobs();
            this.pageBacklogOptimizer.stopOptimizing();
            this.crawlerImageLogger.writeReportFile();
            this.crawlerJobMonitor.setDecoupleEvents(true);
            this.clear();
        }

        public void suspendCrawling()
        {
            this.crawlerImageLogger.writeReportFile();
            this.crawlerJobPool.suspendCrawlerJobs();
        }

        public void resumeCrawling()
        {
            this.crawlerJobPool.resumeCrawlerJobs();
        }

        public Boolean isSuspended()
        {
            return this.crawlerJobPool.isSuspended;
        }

        public void setImageLoggingFolder(String foldername)
        {
            this.crawlerImageLogger.setLoggerFoldername(foldername);
        }

        #endregion

        public void setInitialJobFactory(InitialJobFactory initialJobFactory)
        {
            this.initialJobFactory = initialJobFactory;
        }

        /// <summary>
        /// This factory produces the first initial crawler job. This job should use the page back log to retrieve a page and trigger the next jobs.
        /// </summary>
        public class InitialJobFactory
        {
            public virtual ICrawlerJob generateInitialCrawlerJob(ICrawlerController crawlerController, IPageBacklog pageBacklog)
            {
                return new CrawlerJob(crawlerController).addCrawlerJobPartInitialPageLock(pageBacklog);
            }
        }

        /// <summary>
        /// Used to manage the crawler job events and produce corresponding processInformation objects.
        /// </summary>
        protected class CrawlerJobMonitor
        {
            protected Dictionary<ICrawlerJob, CrawlerProcessInformation> crawlerJobToProcessInformationDictionary = new Dictionary<ICrawlerJob, CrawlerProcessInformation>();
            private Boolean decoupleEvents = false;

            /// <summary>
            /// Triggers once for every new image has been retrieved by the crawler.
            /// </summary>
            public event EventHandler<CrawlerImage.EventArg> imageRetrievedEvent;

            /// <summary>
            /// Decouples the events from the crawler jobs from the monitor. If set to false, coupling will be reactivated.
            /// </summary>
            public void setDecoupleEvents(Boolean decoupleEvents)
            {
                this.decoupleEvents = decoupleEvents;
            }

            /// <summary>
            /// Removes a crawler job from the monitor.
            /// </summary>
            /// <param name="crawlerJob"></param>
            public void unregisterCrawlerJob(ICrawlerJob crawlerJob)
            {
                //
                lock (this.crawlerJobToProcessInformationDictionary)
                {
                    this.crawlerJobToProcessInformationDictionary.Remove(crawlerJob);
                }
            }

            public CrawlerProcessInformation registerCrawlerJob(ICrawlerJob crawlerJob)
            {
                //
                CrawlerProcessInformation crawlerProcessInformation = new CrawlerProcessInformation();

                //
                if (crawlerJob != null)
                {
                    //
                    crawlerJob.processStateChangeEvent += new CrawlerJobProcessStateChangeEvent(this.crawlerJobProcessStateChangeEvent);
                    crawlerJob.progressPercentageChangeEvent += new CrawlerJobProgressPercentageChangeEvent(this.crawlerJobProgressPercentageChangeEvent);
                    crawlerJob.crawlerJobImageRetrievedEvent += new CrawlerJobImageRetrievedEvent(crawlerJobImageRetrievedEvent);

                    //
                    lock (this.crawlerJobToProcessInformationDictionary)
                    {
                        this.crawlerJobToProcessInformationDictionary.Add(crawlerJob, crawlerProcessInformation);
                    }
                }

                return crawlerProcessInformation;
            }

            private void crawlerJobImageRetrievedEvent(ICrawlerJob crawlerJob, CrawlerImage crawlerImage)
            {
                if (!this.decoupleEvents && crawlerImage != null)
                {
                    //processinformation -> inside the monitor
                    CrawlerProcessInformation crawlerProcessInformation = this.determineCrawlerProcessInformationForCrawlerJob(crawlerJob);
                    if (crawlerProcessInformation != null)
                    {
                        crawlerProcessInformation.triggerCrawlerProcessInformationNewImageEvent(crawlerImage);
                    }

                    //some other components of the controller -> outside of the monitor
                    if (this.imageRetrievedEvent != null)
                    {
                        this.imageRetrievedEvent(crawlerJob, crawlerImage.asEventArg());
                    }
                }
            }

            private void crawlerJobProcessStateChangeEvent(ICrawlerJob crawlerJob, CrawlerJobProcessState processState)
            {
                if (!this.decoupleEvents)
                {
                    CrawlerProcessInformation crawlerProcessInformation = this.determineCrawlerProcessInformationForCrawlerJob(crawlerJob);
                    if (crawlerProcessInformation != null)
                    {
                        //
                        crawlerProcessInformation.processState = this.determineProcessInformationProcessStateFromCrawlerJobProcessState(processState);
                        crawlerProcessInformation.pageUrlStr = this.determinePageUrlStrFromCrawlerJob(crawlerJob);
                        crawlerProcessInformation.triggerCrawlerProcessInformationUpdateEvent();
                    }
                }
            }

            private String determinePageUrlStrFromCrawlerJob(ICrawlerJob crawlerJob)
            {
                //
                String retval = "";

                //
                if (crawlerJob != null)
                {
                    Page page = crawlerJob.getPage();
                    if (page != null)
                    {
                        retval = page.urlStr;
                    }
                }

                //
                return retval;
            }

            private CrawlerProcessInformation.ProcessState determineProcessInformationProcessStateFromCrawlerJobProcessState(CrawlerJobProcessState jobProcessState)
            {
                //
                CrawlerProcessInformation.ProcessState processState = CrawlerProcessInformation.ProcessState.NotStarted;

                //                        
                if (jobProcessState == CrawlerJobProcessState.Initialize)
                {
                    processState = CrawlerProcessInformation.ProcessState.Initialize;
                }
                else if (jobProcessState == CrawlerJobProcessState.LockPage)
                {
                    processState = CrawlerProcessInformation.ProcessState.LockPage;
                }
                else if (jobProcessState == CrawlerJobProcessState.Download)
                {
                    processState = CrawlerProcessInformation.ProcessState.Download;
                }
                else if (jobProcessState == CrawlerJobProcessState.Analyze)
                {
                    processState = CrawlerProcessInformation.ProcessState.Analyze;
                }
                else if (jobProcessState == CrawlerJobProcessState.RetrieveImages)
                {
                    processState = CrawlerProcessInformation.ProcessState.RetrieveImages;
                }
                else if (jobProcessState == CrawlerJobProcessState.GenerateSubJobs)
                {
                    processState = CrawlerProcessInformation.ProcessState.GenerateSubJobs;
                }
                else if (jobProcessState == CrawlerJobProcessState.Erroneous)
                {
                    processState = CrawlerProcessInformation.ProcessState.Erroneous;
                }
                else if (jobProcessState == CrawlerJobProcessState.Finished)
                {
                    processState = CrawlerProcessInformation.ProcessState.Finished;
                }
                else if (jobProcessState == CrawlerJobProcessState.Aborting)
                {
                    processState = CrawlerProcessInformation.ProcessState.Aborting;
                }
                else if (jobProcessState == CrawlerJobProcessState.Filtering)
                {
                    processState = CrawlerProcessInformation.ProcessState.Filtering;
                }

                //
                return processState;
            }

            private void crawlerJobProgressPercentageChangeEvent(ICrawlerJob crawlerJob, double progressPercentage)
            {
                if (!this.decoupleEvents)
                {
                    CrawlerProcessInformation crawlerProcessInformation = this.determineCrawlerProcessInformationForCrawlerJob(crawlerJob);
                    if (crawlerProcessInformation != null)
                    {
                        crawlerProcessInformation.progressPercentage = progressPercentage;
                        crawlerProcessInformation.triggerCrawlerProcessInformationUpdateEvent();
                    }
                }
            }

            private CrawlerProcessInformation determineCrawlerProcessInformationForCrawlerJob(ICrawlerJob crawlerJob)
            {
                //
                CrawlerProcessInformation crawlerProcessInformation = null;

                //
                if (crawlerJob != null)
                {
                    lock (this.crawlerJobToProcessInformationDictionary)
                    {
                        this.crawlerJobToProcessInformationDictionary.TryGetValue(crawlerJob, out crawlerProcessInformation);
                    }
                }

                //
                return crawlerProcessInformation;
            }

        }
    }
}
