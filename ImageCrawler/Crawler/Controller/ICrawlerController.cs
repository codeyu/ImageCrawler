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
using ImageCrawler.Crawler.View.GUI;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Controller
{

    public delegate void CrawlerControllerFinishedWorkingEvent();

    /// <summary>
    /// Controller for the whole crawling process. For a GUI a separate CrawlerJobView is available.
    /// </summary>
    public interface ICrawlerController
    {
        /// <summary>
        /// Defines the maximum number parallel crawler jobs.
        /// </summary>
        void setMaximumCrawlerJobThreadCount(int threadCount);

        /// <summary>
        /// Triggers if all jobs have been done by the workers. This does not depend on the fact, if the workes have been aborted or finished naturally.
        /// </summary>
        event CrawlerControllerFinishedWorkingEvent crawlerControllerFinishedWorkingEvent;

        /// <summary>
        /// Returns the view for the current crawling process.
        /// </summary>
        /// <returns></returns>
        ICrawlerJobView getCrawlerJobView();

        /// <summary>
        /// Starts the crawling process with the given root page. Every page added later have to be a subpage of this root page. If the given root page is null, no restriction for other pages is given, so external links will be crawler, too.
        /// Returns the actual view on the crawler process.
        /// </summary>
        /// <param name="page"></param>
        ICrawlerJobView startCrawling(Page page);

        /// <summary>
        /// Adds new pages to the crawler's to do list.
        /// </summary>
        /// <param name="pageList"></param>
        void addPageList(List<Page> pageList);

        /// <summary>
        /// Adds a new page to the crawler's to do list.
        /// </summary>
        /// <param name="page"></param>
        void addPage(Page page);

        /// <summary>
        /// This event is triggered when the crawling process has done all jobs.
        /// </summary>
        /// <returns></returns>
        ManualResetEvent getFinishedEvent();

        /// <summary>
        /// Resets the whole crawler to the initial state.
        /// </summary>
        void clear();

        /// <summary>
        /// Stops the crawling process. This cannot be resumed.
        /// </summary>
        void stopCrawling();

        /// <summary>
        /// Suspends the current crawling process. The process can be resumed.
        /// </summary>
        void suspendCrawling();

        /// <summary>
        /// Resumes a previously suspended crawler.
        /// </summary>
        void resumeCrawling();

        /// <summary>
        /// Returns true, if the crawling process has been suspende.        
        /// </summary>
        /// <returns></returns>
        Boolean isSuspended();

        /// <summary>
        /// Sets the crawlerImage filter.
        /// </summary>
        /// <param name="crawlerImageFilter"></param>
        void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter);

        /// <summary>
        /// If a folder is set, all retrieved images will be written to this folder. If null is set, logging of images will stop.
        /// </summary>
        /// <param name="foldername"></param>
        void setImageLoggingFolder(String foldername);

    }
}
