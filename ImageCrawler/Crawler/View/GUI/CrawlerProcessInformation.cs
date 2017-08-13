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

namespace ImageCrawler.Crawler.View.GUI
{
    public delegate void CrawlerProcessInformationUpdateEvent(CrawlerProcessInformation crawlerProcessInformation);
    public delegate void CrawlerProcessInformationNewImageEvent(CrawlerImage crawlerImage);

    /// <summary>
    /// Makes informations about an crawler process available. A crawler process has to 
    /// </summary>
    public class CrawlerProcessInformation
    {
        public event CrawlerProcessInformationUpdateEvent crawlerProcessInformationUpdateEvent;
        public event CrawlerProcessInformationNewImageEvent crawlerProcessInformationNewImageEvent;

        public double progressPercentage {get;set;}
        public ProcessState processState { get; set; }
        public String pageUrlStr { get; set; }
        public System.DateTime createdTime = System.DateTime.Now;
        public TimeSpan runningTime { get { return System.DateTime.Now.Subtract(this.createdTime); } }

        public CrawlerProcessInformation()
        {
            this.processState = ProcessState.NotStarted;
            this.progressPercentage = 0.000000;
            this.pageUrlStr = "";
        }

        /// <summary>
        /// Representations of states of a process.
        /// </summary>
        public enum ProcessState
        {
            NotStarted, Download, Analyze, RetrieveImages, GenerateSubJobs, Finished, Initialize, LockPage, Aborting, Erroneous, Filtering
        }

        public void triggerCrawlerProcessInformationUpdateEvent()
        {
            if (this.crawlerProcessInformationUpdateEvent != null)
            {
                this.crawlerProcessInformationUpdateEvent(this);
            }
        }

        public void triggerCrawlerProcessInformationNewImageEvent(CrawlerImage crawlerImage)
        {
            if (this.crawlerProcessInformationNewImageEvent != null)
            {
                this.crawlerProcessInformationNewImageEvent(crawlerImage);
            }
        }

        /// <summary>
        /// Returns true, if the process information object is based on an active crawler.
        /// </summary>
        /// <returns></returns>
        public Boolean isActiveProcess()
        {
            return this.processState != ProcessState.NotStarted && this.processState != ProcessState.Finished;
        }
    }
}
