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
using ImageCrawler.Crawler.Backlog;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Filter;

namespace ImageCrawler.Crawler.Job
{
    public delegate void CrawlerJobProgressPercentageChangeEvent(ICrawlerJob sender, Double processPercentage);
    public delegate void CrawlerJobProcessStateChangeEvent(ICrawlerJob sender, CrawlerJobProcessState processState);
    public delegate void CrawlerJobImageRetrievedEvent(ICrawlerJob sender, CrawlerImage crawlerImage);
    public delegate void CrawlerJobSuspendEvent();

    /// <summary>
    /// Defines a set of process states a crawler job can be in.
    /// </summary>
    public enum CrawlerJobProcessState
    {
        NotStarted, Download, Analyze, RetrieveImages, GenerateSubJobs, Finished, Initialize, LockPage, Erroneous, Aborting, Filtering
    }

    /// <summary>
    /// Defines a single job for the CrawlerJobPool. An CrawlerJob has several CrawlerJobPart's which defines the partly actions.
    /// </summary>
    public interface ICrawlerJob
    {
        /// <summary>
        /// Triggers if the process changes the percentage of progress.
        /// </summary>
        event CrawlerJobProgressPercentageChangeEvent progressPercentageChangeEvent;

        /// <summary>
        /// Triggers if the process state changes. For example from download to analyze.
        /// </summary>
        event CrawlerJobProcessStateChangeEvent processStateChangeEvent;

        /// <summary>
        /// Triggers if a new image has been loaded.
        /// </summary>
        event CrawlerJobImageRetrievedEvent crawlerJobImageRetrievedEvent;

        /// <summary>
        /// Executes the suspend event.
        /// </summary>
        event CrawlerJobSuspendEvent crawlerJobSuspendEvent;

        /// <summary>
        /// starts the current workload process. You have to initialize the job by setting an initial CrawlerJobPart. Use addCrawlerJobPartDownload(...) or addCrawlerJobPartAndSetPage(...) to do this.
        /// </summary>
        void run();

        /// <summary>
        /// returns true, if the run method has been executed and has ended.
        /// </summary>
        /// <returns></returns>
        Boolean hasFinished();

        /// <summary>
        /// Sets the initial crawler job part. Is usually the CrawlerJobPartDownload. Regularly as default you should use addCrawlerJobPartDownload instead. You have to set the page manually.
        /// </summary>
        /// <param name="crawlerJobPart"></param>
        ICrawlerJob addCrawlerJobPartAndSetPage(ICrawlerJobPart crawlerJobPart, Page page);

        /// <summary>
        /// Sets the initial crawler job part as CrawlerJobPartDownload. As page the first page is used from the page backlog, from which the page is removed, too. The page will be set, too.
        /// </summary>
        /// <param name="pageBacklog"></param>
        ICrawlerJob addCrawlerJobPartInitialPageLock(IPageBacklog pageBacklog);

        /// <summary>
        /// Sets the crawler controller which owns this job.
        /// </summary>
        /// <param name="crawlerController"></param>
        ICrawlerJob setParentalCrawlerController(ICrawlerController crawlerController);

        /// <summary>
        /// Returns the crawlerController of the current job.
        /// </summary>
        /// <returns></returns>
        ICrawlerController getParentalCrawlerController();

        /// <summary>
        /// Returns the page, the job is working on.
        /// </summary>
        /// <returns></returns>
        Page getPage();

        /// <summary>
        /// Sets the image filter.
        /// </summary>
        /// <param name="crawlerImageFilter"></param>
        void setCrawlerImageFilter(CrawlerImageFilter crawlerImageFilter);

        /// <summary>
        /// Sets the image backlog.
        /// </summary>
        /// <param name="crawlerImageBacklog"></param>
        void setCrawlerImageBacklog(ICrawlerImageBacklog crawlerImageBacklog);

        /// <summary>
        /// Sets the page filter object.
        /// </summary>
        /// <param name="pageFilter"></param>
        void setPageFilter(IPageFilter pageFilter);

        /// <summary>
        /// Aborts the current job work.
        /// </summary>
        void doAbort();
    }
}
