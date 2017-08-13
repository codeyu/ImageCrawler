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

namespace ImageCrawler.Crawler.Job
{
    public delegate void CrawlerJobPoolFinishedWorkingEvent();
    public delegate void CrawlerJobPoolWorkerUpdate(int runningWorkerCount);
    public delegate void CrawlerJobPoolLaunchingCrawlerJob(ICrawlerJob crawlerJob);
    public delegate void CrawlerJobPoolFinishedCrawlerJob(ICrawlerJob crawlerJob);

    /// <summary>
    /// Manages the crawler job threads.
    /// </summary>
    public interface ICrawlerJobPool
    {

        /// <summary>
        /// Is triggered for a crawler job beeing launched.
        /// </summary>
        event CrawlerJobPoolLaunchingCrawlerJob crawlerJobPoolLaunchingCrawlerJob;

        /// <summary>
        /// Is triggered if a crawler job is finished.
        /// </summary>
        event CrawlerJobPoolFinishedCrawlerJob crawlerJobPoolFinishedCrawlerJob;

        /// <summary>
        /// Triggers if the workers are all finished. Does not depend on the fact, if the workers have been aborted manually or by natural end of work.
        /// </summary>
        event CrawlerJobPoolFinishedWorkingEvent crawlerJobPoolFinishedWorkingEvent;

        /// <summary>
        /// Triggers if a new worker starts work, or a worker is ended.
        /// </summary>
        event CrawlerJobPoolWorkerUpdate crawlerJobPoolWorkerUpdate;

        /// <summary>
        /// Return true, if the job pool has suspended his jobs.
        /// </summary>
        Boolean isSuspended { get; set; }

        /// <summary>
        /// Adds a new crawler job as available. This does not mean, that a job will immediately run.
        /// </summary>
        /// <param name="crawlerJob"></param>
        void addCrawlerJob(ICrawlerJob crawlerJob);

        /// <summary>
        /// Removes the given job from the pool, if the job has not started yet.
        /// 
        /// </summary>
        /// <param name="crawlerJob"></param>
        void removeCrawlerJob(ICrawlerJob crawlerJob);

        /// <summary>
        /// Starts the crawler using the given maximum number of threads at the same time. But runs through all given jobs.
        /// </summary>
        void startCrawlerJobs();

        /// <summary>
        /// Sets the maximum number of parallel running jobs allowed.
        /// </summary>
        /// <param name="threadCount"></param>
        void setMaximumThreadCount(int threadCount);

        /// <summary>
        /// Stops all jobs immediately.
        /// </summary>
        void stopCrawlerJobs();

        /// <summary>
        /// Suspends all crawler jobs until they are resumed.
        /// </summary>
        void suspendCrawlerJobs();

        /// <summary>
        /// Resumes the suspended crawler jobs.
        /// </summary>
        void resumeCrawlerJobs();

        /// <summary>
        /// The signals if all jobs are done.
        /// </summary>
        /// <returns></returns>
        ManualResetEvent getFinishedEvent();

    }

    
}
