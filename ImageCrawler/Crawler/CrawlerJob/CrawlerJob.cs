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
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Backlog;
using ImageCrawler.Crawler.Filter;

namespace ImageCrawler.Crawler.Job
{
    /// <summary>
    /// worker job used with ICrawlerJob
    /// </summary>
    public class CrawlerJob : ICrawlerJob
    {
        protected Boolean isRunning = false;
        protected Boolean isAborting = false;

        protected List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();
        
        protected CrawlerJobContext crawlerJobContext = new CrawlerJobContext();

        private ICrawlerJobPart currentlyActiveJobPart;

        #region ICrawlerJob Members

        public event CrawlerJobProgressPercentageChangeEvent progressPercentageChangeEvent;
        public event CrawlerJobProcessStateChangeEvent processStateChangeEvent;
        public event CrawlerJobImageRetrievedEvent crawlerJobImageRetrievedEvent;
        public event CrawlerJobSuspendEvent crawlerJobSuspendEvent;

        public CrawlerJob()
        {
            this.crawlerJobContext.crawlerJob = this;
        }

        public CrawlerJob(ICrawlerController crawlerController) :this()
        {
            this.setParentalCrawlerController(crawlerController);
        }

        public ICrawlerJob addCrawlerJobPartInitialPageLock(IPageBacklog pageBacklog)
        {
            //
            if (pageBacklog != null)
            {
                this.crawlerJobContext.pageBacklog = pageBacklog;
                this.addCrawlerJobPartAndSetPage(new CrawlerJobPartInitialPageLock(), null);
            }

            //
            return this;
        }

        /// <summary>
        /// Runns the crawler job. You have to set an initial crawler job part before.
        /// </summary>
        public void run()
        {
            //
            this.isRunning = true;
            this.triggerProcessStateChangeEvent( CrawlerJobProcessState.Initialize);
            this.triggerProgressPercentageChangeEvent(0.00000);

            //
            while (this.crawlerJobPartList.Count > 0)
            {
                //
                this.currentlyActiveJobPart = this.crawlerJobPartList[0];
                this.crawlerJobPartList.RemoveAt(0);

                //
                if (this.isAborting)
                {
                    break;
                }

                //
                if (this.currentlyActiveJobPart != null)
                {
                    //
                    this.triggerProcessStateChangeEvent(this.currentlyActiveJobPart.getProcessState());

                    //
                    if (this.crawlerJobSuspendEvent != null)
                    {
                        this.crawlerJobSuspendEvent();
                    }

                    //run the job part workload
                    List<ICrawlerJobPart> followingJobPartList = null;
                    try
                    {
                        followingJobPartList = this.currentlyActiveJobPart.run();
                    }
                    catch (Exception e)
                    {
                        //
                        this.triggerProcessStateChangeEvent(CrawlerJobProcessState.Erroneous);
                        Thread.Sleep(1000); //Keep in error state for some time
                    }

                    //
                    if (followingJobPartList != null)
                    {
                        followingJobPartList.ForEach(crawlerJobPartItem => this.attachCrawlerJobPart(crawlerJobPartItem));
                        this.crawlerJobPartList.AddRange(followingJobPartList);
                    }
                }
            }

            //
            if (this.crawlerJobContext.page != null)
            {
                this.crawlerJobContext.page.isCompleted = true;
            }

            //
            this.isRunning = false;
            this.triggerProcessStateChangeEvent(CrawlerJobProcessState.Finished);
            this.triggerProgressPercentageChangeEvent(1.00000);
        }

        public void doAbort()
        {
            this.isAborting = true;
            if (this.currentlyActiveJobPart != null)
            {
                this.currentlyActiveJobPart.doAbort();
            }
        }

        private void triggerProgressPercentageChangeEvent(double progressPercentage)
        {
            if (this.progressPercentageChangeEvent != null && !this.isAborting)
            {
                this.progressPercentageChangeEvent(this, progressPercentage);
            }
        }

        private void triggerProcessStateChangeEvent(CrawlerJobProcessState crawlerJobProcessState)
        {
            if (this.processStateChangeEvent != null && !this.isAborting)
            {
                this.processStateChangeEvent(this, crawlerJobProcessState);
            }
        }

        private void attachCrawlerJobPart(ICrawlerJobPart crawlerJobPart)
        {
            crawlerJobPart.crawlerJobContext = this.crawlerJobContext;
            crawlerJobPart.progressPercentageChangeEvent += new CrawlerJobPartProgressUpdate(crawlerJobPart_ProgressPercentageChangeEvent);
            crawlerJobPart.crawlerJobPartImageRetrievedEvent += new CrawlerJobPartImageRetrievedEvent(crawlerJobPart_crawlerJobPartImageRetrievedEvent);
            crawlerJobPart.crawlerJobPartSuspendEvent += new CrawlerJobPartSuspendEvent(crawlerJobPart_crawlerJobPartSuspendEvent);
        }

        private void crawlerJobPart_crawlerJobPartSuspendEvent()
        {
            if (this.crawlerJobSuspendEvent != null)
            {
                this.crawlerJobSuspendEvent();
            }
        }

        private void crawlerJobPart_crawlerJobPartImageRetrievedEvent(CrawlerImage crawlerImage)
        {
            if (crawlerImage != null && crawlerImage.image != null && this.crawlerJobImageRetrievedEvent != null && !this.isAborting)
            {
                if (this.crawlerJobContext.page != null)
                {
                    Page page = this.crawlerJobContext.page;
                    lock (page)
                    {
                        this.crawlerJobContext.page.imageFoundCounter++;
                        this.crawlerJobContext.page.imageFoundAreaSummation += crawlerImage.image.Width * crawlerImage.image.Height;
                    }
                }
                this.crawlerJobImageRetrievedEvent(this,crawlerImage);
            }
        }

        private void crawlerJobPart_ProgressPercentageChangeEvent(double progressPercentage)
        {
            if (this.progressPercentageChangeEvent != null && !this.isAborting)
            {
                this.progressPercentageChangeEvent(this, progressPercentage);
            }
        }

        public bool hasFinished()
        {
            return this.isRunning;
        }

        public ICrawlerJob addCrawlerJobPartAndSetPage(ICrawlerJobPart crawlerJobPart, Page page)
        {

            this.crawlerJobContext.page = page;

            if (crawlerJobPart != null && !this.crawlerJobPartList.Contains(crawlerJobPart))
            {
                this.crawlerJobPartList.Add(crawlerJobPart);
                this.attachCrawlerJobPart(crawlerJobPart);
            }

            //
            return this;
        }

        public ICrawlerJob setParentalCrawlerController(ICrawlerController crawlerController)
        {
            this.crawlerJobContext.crawlerController = crawlerController;
            return this;
        }

        public ICrawlerController getParentalCrawlerController()
        {
            return this.crawlerJobContext.crawlerController;
        }

        public Page getPage()
        {
            return this.crawlerJobContext.page;
        }

        public void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter)
        {
            this.crawlerJobContext.crawlerImageFilter = crawlerImageFilter;
        }

        public void setCrawlerImageBacklog(ICrawlerImageBacklog imageBacklog)
        {
            this.crawlerJobContext.imageBacklog = imageBacklog;
        }

        public void setPageFilter(IPageFilter pageFilter)
        {
            this.crawlerJobContext.pageFilter = pageFilter;
        }

        #endregion
    }
}
