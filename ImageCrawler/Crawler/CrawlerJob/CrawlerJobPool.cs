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
    public class CrawlerJobPool : ICrawlerJobPool
    {
        #region ICrawlerController Members

        protected List<ICrawlerJob> availableCrawlerJobList = new List<ICrawlerJob>();
        protected List<ICrawlerJob> runningCrawlerJobList = new List<ICrawlerJob>();

        protected Thread launcherThread;
        protected int threadCount = 1;
        private int workerThreadId = 0;

        public Boolean isSuspended { get; set; }
        public Boolean isRunning { get; set; }
        private Boolean doAbort = false;

        private ManualResetEvent launcherThreadResetEvent = new ManualResetEvent(false);
        private ManualResetEvent finishedEvent = new ManualResetEvent(false);
        private ManualResetEvent suspendEvent = new ManualResetEvent(false);

        public event CrawlerJobPoolFinishedWorkingEvent crawlerJobPoolFinishedWorkingEvent;
        public event CrawlerJobPoolWorkerUpdate crawlerJobPoolWorkerUpdate;
        public event CrawlerJobPoolLaunchingCrawlerJob crawlerJobPoolLaunchingCrawlerJob;
        public event CrawlerJobPoolFinishedCrawlerJob crawlerJobPoolFinishedCrawlerJob;

        public void addCrawlerJob(ICrawlerJob crawlerJob)
        {
            lock (this.availableCrawlerJobList)
            {
                this.availableCrawlerJobList.Add(crawlerJob);
                crawlerJob.processStateChangeEvent += new CrawlerJobProcessStateChangeEvent(crawlerJob_finishedJobDelegate);
                crawlerJob.crawlerJobSuspendEvent += new CrawlerJobSuspendEvent(crawlerJob_crawlerJobSuspendEvent);
            }
        }

        private void crawlerJob_crawlerJobSuspendEvent()
        {
            while (this.isSuspended && !this.doAbort)
            {
                this.suspendEvent.WaitOne(5000);
            }
        }

        private void crawlerJob_finishedJobDelegate(ICrawlerJob sender, CrawlerJobProcessState processState)
        {
            if (sender != null && processState == CrawlerJobProcessState.Finished)
            {
                //
                lock (this.runningCrawlerJobList)
                {
                    //
                    this.runningCrawlerJobList.Remove(sender);

                    //
                    if (this.crawlerJobPoolWorkerUpdate != null)
                    {
                        this.crawlerJobPoolWorkerUpdate(this.runningCrawlerJobList.Count);
                    }
                }

                //
                this.launcherThreadResetEvent.Set();
            }
        }

        public void removeCrawlerJob(ICrawlerJob crawlerJob)
        {
            lock (this.availableCrawlerJobList)
            {
                this.availableCrawlerJobList.Remove(crawlerJob);
            }
            if (this.crawlerJobPoolFinishedCrawlerJob != null)
            {
                this.crawlerJobPoolFinishedCrawlerJob(crawlerJob);
            }
        }

        /// <summary>
        /// tries to launch a new thread for another available worker. If successful returns true, otherwise false.
        /// </summary>
        /// <returns></returns>        
        protected Boolean launchNewThreadIfPossible()
        {
            //
            Boolean retval = false;

            //
            ICrawlerJob crawlerJob = null;
            lock (this.availableCrawlerJobList)
            {
                lock (this.runningCrawlerJobList)
                {
                    try
                    {
                        if (this.availableCrawlerJobList.Count > 0 && this.runningCrawlerJobList.Count < this.threadCount)
                        {
                            //
                            crawlerJob = this.availableCrawlerJobList.ElementAt(0);
                            this.availableCrawlerJobList.Remove(crawlerJob);
                            this.runningCrawlerJobList.Add(crawlerJob);

                            //
                            Thread thread = new Thread(new ThreadStart(crawlerJob.run));
                            thread.Name = "worker" + ++workerThreadId;
                            thread.IsBackground = true;
                            thread.Start();

                            //
                            retval = true;

                            //
                            if (this.crawlerJobPoolLaunchingCrawlerJob != null)
                            {
                                this.crawlerJobPoolLaunchingCrawlerJob(crawlerJob);
                            }

                            //
                            if (this.crawlerJobPoolWorkerUpdate != null)
                            {
                                this.crawlerJobPoolWorkerUpdate(this.runningCrawlerJobList.Count);
                            }
                        }
                    }
                    catch (Exception e)
                    { }
                }
            }

            //
            return retval;
        }

        public void setMaximumThreadCount(int threadCount)
        {
            this.threadCount = Math.Max(1, threadCount);
        }

        public void startCrawlerJobs()
        {
            //
            this.isSuspended = false;
            this.doAbort = false;
            this.isRunning = true;       
     
            //
            this.threadCount = Math.Max(1, this.threadCount);

            //
            lock (this.runningCrawlerJobList)
            {
                this.runningCrawlerJobList.Clear();
            }

            //
            this.launcherThread = new Thread(new ThreadStart(delegate()
            {
                //
                Thread.CurrentThread.Name = "LauncherThread";

                //repeat as long any thread is or was running.
                while (this.availableCrawlerJobList.Count > 0 && this.isRunning && !this.doAbort)
                {
                    //
                    while (this.launchNewThreadIfPossible()) ;

                    //wait till every worker stopped.
                    while (this.runningCrawlerJobList.Count > 0 && this.isRunning)
                    {
                        while (this.launchNewThreadIfPossible()) ;

                        this.launcherThreadResetEvent.WaitOne(System.TimeSpan.FromMilliseconds(500), false);
                    }
                }

                //
                this.isRunning = false;
                this.isSuspended = false;

                //
                this.finishedEvent.Set();

                //
                if (this.crawlerJobPoolFinishedWorkingEvent != null)
                {
                    this.crawlerJobPoolFinishedWorkingEvent();
                }

                //
                lock (this.runningCrawlerJobList)
                {
                    this.runningCrawlerJobList.Clear();
                }

            }));

            launcherThread.Start();
        }

        public ManualResetEvent getFinishedEvent()
        {
            return this.finishedEvent;
        }

        public void stopCrawlerJobs()
        {
            //
            this.doAbort = true;

            //
            lock (this.runningCrawlerJobList)
            {
                foreach (CrawlerJob crawlerJob in this.runningCrawlerJobList)
                {
                    crawlerJob.doAbort();
                }
            }

            //
            this.runningCrawlerJobList.Clear();
            this.availableCrawlerJobList.Clear();
        }

        public void suspendCrawlerJobs()
        {
            this.isSuspended = true;
            this.suspendEvent.Reset();
        }

        public void resumeCrawlerJobs()
        {
            this.isSuspended = false;
            this.suspendEvent.Set();
        }


        #endregion
    }
}
