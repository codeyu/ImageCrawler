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

namespace ImageCrawler.Crawler.Job
{
    public delegate void CrawlerJobPartProgressUpdate(double processPercentage);
    public delegate void CrawlerJobPartImageRetrievedEvent(CrawlerImage crawlerImage);
    public delegate void CrawlerJobPartSuspendEvent();

    public interface ICrawlerJobPart
    {
        /// <summary>
        /// Holds the reference of the crawler job context.
        /// </summary>
        CrawlerJobContext crawlerJobContext { get; set; }

        /// <summary>
        /// Triggers if the progress of the work changes.
        /// </summary>
        event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;

        /// <summary>
        /// Triggers if an image was loaded.
        /// </summary>
        event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;

        /// <summary>
        /// Event the job part will execute at points where the process can be suspended. The wait will be called by the event handle provider.
        /// </summary>
        event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        /// <summary>
        /// Returns the process state of a crawler job, to which this part belongs to.
        /// </summary>
        /// <returns></returns>
        CrawlerJobProcessState getProcessState();

        /// <summary>
        /// Runs the workload of the job part.
        /// </summary>
        /// <returns></returns>
        List<ICrawlerJobPart> run();

        /// <summary>
        /// Aborts the running process.
        /// </summary>
        void doAbort();

    }
}
