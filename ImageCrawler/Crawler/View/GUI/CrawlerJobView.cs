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
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawler.Crawler.View.GUI
{
    public class CrawlerJobView : ICrawlerJobView
    {
        protected List< CrawlerProcessInformation> activeCrawlerProcessInformationList = new List<CrawlerProcessInformation>();
        protected List<CrawlerProcessInformation> allOccurredCrawlerProcessInformationList = new List<CrawlerProcessInformation>();
        protected IPageBacklog pageBacklog;
        protected int currentlyRunningWorkerJobCount = 0;

        #region ICrawlerJobView Members

        public event CrawlerJobViewUpdateEventProcessInformation updateProcessInformationEvent;
        public event CrawlerJobViewUpdateEventImage updateImageEvent;
        public event CrawlerJobViewUpdateEventProcessInformationList crawlerJobViewUpdateEventProcessInformationList;
        public event CrawlerJobViewUpdateEventAddedNewProcessInformation crawlerJobViewUpdateEventAddedNewProcessInformation;

        public List<CrawlerProcessInformation> getAllCrawlerProcessInformationList()
        {
            List<CrawlerProcessInformation> crawlerProcessInformationList = new List<CrawlerProcessInformation>();
            lock (this.allOccurredCrawlerProcessInformationList)
            {
                crawlerProcessInformationList.AddRange(this.allOccurredCrawlerProcessInformationList);
            }
            return crawlerProcessInformationList;
        }

        public List<CrawlerProcessInformation> getActiveCrawlerProcessInformationList()
        {
            List<CrawlerProcessInformation> crawlerProcessInformationList = new List<CrawlerProcessInformation>();
            lock (this.activeCrawlerProcessInformationList)
            {
                crawlerProcessInformationList.AddRange(this.activeCrawlerProcessInformationList);
            }
            return crawlerProcessInformationList;
        }

        public void registerCrawlerProcessInformation(CrawlerProcessInformation crawlerProcessInformation)
        {
            if (crawlerProcessInformation != null)
            {
                crawlerProcessInformation.crawlerProcessInformationUpdateEvent += new CrawlerProcessInformationUpdateEvent(this.crawlerProcessInformation_UpdateEvent);
                crawlerProcessInformation.crawlerProcessInformationNewImageEvent += new CrawlerProcessInformationNewImageEvent(this.crawlerProcessInformation_crawlerProcessInformationNewImageEvent);
                this.determineProcessInformationActiveState(crawlerProcessInformation);
            }
        }

        public void registerPageBacklog(IPageBacklog pageBacklog)
        {
            this.pageBacklog = pageBacklog;
        }

        private void determineProcessInformationActiveState(CrawlerProcessInformation crawlerProcessInformation)
        {
            if (crawlerProcessInformation != null)
            {
                lock (this.activeCrawlerProcessInformationList)
                {
                    if (crawlerProcessInformation.isActiveProcess())
                    {
                        if (!this.activeCrawlerProcessInformationList.Contains(crawlerProcessInformation))
                        {
                            //
                            this.activeCrawlerProcessInformationList.Add(crawlerProcessInformation);
                            this.triggerCrawlerJobViewUpdateEventAddedNewProcessInformation(crawlerProcessInformation);
                            this.triggerCrawlerJobViewUpdateEventProcessInformationList();
                        }
                    }
                    else
                    {
                        if (this.activeCrawlerProcessInformationList.Contains(crawlerProcessInformation))
                        {
                            //
                            this.activeCrawlerProcessInformationList.Remove(crawlerProcessInformation);
                            this.triggerCrawlerJobViewUpdateEventProcessInformationList();
                        }
                    }
                }

                lock (this.allOccurredCrawlerProcessInformationList)
                {
                    if (!this.allOccurredCrawlerProcessInformationList.Contains(crawlerProcessInformation))
                    {
                        this.allOccurredCrawlerProcessInformationList.Add(crawlerProcessInformation);
                    }
                }
            }
        }

        private void triggerCrawlerJobViewUpdateEventAddedNewProcessInformation(CrawlerProcessInformation crawlerProcessInformation)
        {
            if (crawlerJobViewUpdateEventAddedNewProcessInformation != null)
            {
                lock (crawlerProcessInformation)
                {
                    crawlerJobViewUpdateEventAddedNewProcessInformation(crawlerProcessInformation);
                }
            }
        }

        private void triggerCrawlerJobViewUpdateEventProcessInformationList()
        {
            if (this.crawlerJobViewUpdateEventProcessInformationList != null )
            {
                int undonePagesCount = this.getPageBacklogUndonePagesCount();
                int allPagesCount = this.getPageBacklogAllPagesCount();
                this.crawlerJobViewUpdateEventProcessInformationList(this.activeCrawlerProcessInformationList, undonePagesCount, allPagesCount);
            }
        }

        private void crawlerProcessInformation_UpdateEvent(CrawlerProcessInformation crawlerProcessInformation)
        {
            this.determineProcessInformationActiveState(crawlerProcessInformation);
            lock (crawlerProcessInformation)
            {
                if (this.updateProcessInformationEvent != null)
                {
                    this.updateProcessInformationEvent(crawlerProcessInformation);
                }
            }
            
        }

        private void crawlerProcessInformation_crawlerProcessInformationNewImageEvent(CrawlerImage crawlerImage)
        {
            if (crawlerImage != null)
            {
                if (this.updateImageEvent != null)
                {
                    this.updateImageEvent(crawlerImage);
                }
            }
        }

        public void clear()
        {
            this.activeCrawlerProcessInformationList.Clear();
            this.allOccurredCrawlerProcessInformationList.Clear();
        }

        public Boolean isCrawlingProcessActive()
        {
            lock (this.activeCrawlerProcessInformationList)
            {
                return this.activeCrawlerProcessInformationList.Count > 0;
            }
        }


        public int getPageBacklogUndonePagesCount()
        {
            return this.pageBacklog != null ? this.pageBacklog.getBacklogSizeForUndonePages() : 0;
        }

        public int getPageBacklogAllPagesCount()
        {
            return this.pageBacklog != null ? this.pageBacklog.getBacklogSizeForAllRegisteredPages() : 1;
        }

        public int getCurrentlyRunningWorkerJobCount()
        {
            return this.currentlyRunningWorkerJobCount;
        }

        public void registerCrawlerJobPool(ICrawlerJobPool crawlerJobPool)
        {
            if (crawlerJobPool != null)
            {
                crawlerJobPool.crawlerJobPoolWorkerUpdate += new CrawlerJobPoolWorkerUpdate(delegate(int runningWorkerJobCount){
                    this.currentlyRunningWorkerJobCount = runningWorkerJobCount;
                });
            }
        }

        #endregion
    }
}
