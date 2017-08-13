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
using ImageCrawler.Crawler.Job;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Backlog;

namespace ImageCrawler.Crawler.View.GUI
{
    public delegate void CrawlerJobViewUpdateEventProcessInformation(CrawlerProcessInformation crawlerProcessInformation);
    public delegate void CrawlerJobViewUpdateEventImage(CrawlerImage crawlerImage);
    public delegate void CrawlerJobViewUpdateEventProcessInformationList(List<CrawlerProcessInformation> crawlerProcessInformationList, int pageBacklogUndonePagesCount, int pageBacklogAllPagesCount);
    public delegate void CrawlerJobViewUpdateEventAddedNewProcessInformation(CrawlerProcessInformation crawlerProcessInformation);

    /// <summary>
    /// View available by the CrawlerController to get actual informations about the crawling process. Should be used by any GUI.
    /// </summary>
    public interface ICrawlerJobView
    {
        /// <summary>
        /// Triggers if any crawler process is updatet or a new process is introduced.
        /// 
        /// </summary>
        event CrawlerJobViewUpdateEventProcessInformation updateProcessInformationEvent;
        event CrawlerJobViewUpdateEventImage updateImageEvent;
        event CrawlerJobViewUpdateEventProcessInformationList crawlerJobViewUpdateEventProcessInformationList;
        event CrawlerJobViewUpdateEventAddedNewProcessInformation crawlerJobViewUpdateEventAddedNewProcessInformation;

        /// <summary>
        /// Returns a list of all ever occurred process information objects.
        /// 
        /// </summary>
        /// <returns></returns>
        List<CrawlerProcessInformation> getAllCrawlerProcessInformationList();

        /// <summary>
        /// Returns a list of process information objects, which are based on an active crawling process. Every crawler thread will occur with an own entry within this list. The objects of the list will have events available, allowing to track the state changes.
        /// </summary>
        /// <returns></returns>
        List<CrawlerProcessInformation> getActiveCrawlerProcessInformationList();

        /// <summary>
        /// Registers a new processInformation object to the view.
        /// </summary>
        /// <param name="crawlerProcessInformation"></param>
        void registerCrawlerProcessInformation(CrawlerProcessInformation crawlerProcessInformation);

        /// <summary>
        /// Registers the page backlog. Used by the controller
        /// </summary>
        /// <param name="pageBacklog"></param>
        void registerPageBacklog(IPageBacklog pageBacklog);

        /// <summary>
        /// Registers a crawler job pool. Used by the controller.
        /// </summary>
        /// <param name="crawlerJobPool"></param>
        void registerCrawlerJobPool(ICrawlerJobPool crawlerJobPool);

        /// <summary>
        /// Returns the count of the pages which are not yet done by the crawler.
        /// </summary>
        /// <returns></returns>
        int getPageBacklogUndonePagesCount();

        /// <summary>
        /// Returns the count of all registered pages within the page backlog.
        /// </summary>
        /// <returns></returns>
        int getPageBacklogAllPagesCount();

        /// <summary>
        /// Returns the number of currently working jobs.
        /// </summary>
        /// <returns></returns>
        int getCurrentlyRunningWorkerJobCount();

        /// <summary>
        /// Clears all process information data available / resets the job view.
        /// </summary>
        void clear();

        /// <summary>
        /// Returns true as long as the crawling process is running and is not finished.
        /// </summary>
        /// <returns></returns>
        Boolean isCrawlingProcessActive();
        
    }
}
