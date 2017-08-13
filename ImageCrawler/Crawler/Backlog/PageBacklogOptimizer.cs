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
using System.Text.RegularExpressions;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Backlog
{
    /// <summary>
    /// Simple implementation of an optimizer, which starts a background thread that runs every 10seconds to optimize the current page backlog. As default a simple tripple gram analyzer is used.
    /// </summary>
    public class PageBacklogOptimizer : IPageBacklogOptimizer
    {
        protected Thread optimizerThread;
        protected IPageBacklog pageBacklog = null;

        private Boolean isAborting = false;
        private int aspiredOptimizingDurationForWholeBacklogInSeconds = 10;

        public IPageBacklogOptimizerAnalyzerAndClassifier analyzerAndClassifier { get; set; }

        public PageBacklogOptimizer()
        {
            if (this.analyzerAndClassifier == null)
            {
                this.analyzerAndClassifier = new PageBacklogOptimizerAnalyzerAndClassifierTrippleGram();
            }
        }

        #region IPageBacklogOptimizer Members

        public void startOptimizing()
        {
            if (this.optimizerThread == null)
            {
                this.optimizerThread = new Thread(new ThreadStart(run));
                this.optimizerThread.IsBackground = true;
                this.optimizerThread.Start();
            }
        }

        /// <summary>
        /// Thread method analyzing the page backlog. Should abort, if isAborting is set to true.
        /// </summary>
        public void run()
        {
            while (!this.isAborting && this.pageBacklog != null)
            {
                //make snapshot of the current backlog pages
                List<Page> pageList = this.pageBacklog.getAllBacklogPageList();

                //run over the snapshot and optimize pages
                double optimizerProgress = 0.0;
                int pageBacklogSize = pageList.Count();
                int pageCounter = 0;
                System.DateTime startTime = System.DateTime.Now;
                System.TimeSpan durationSinceOptimizingStart = System.TimeSpan.FromSeconds(0);
                foreach (Page page in pageList)
                {
                    //
                    if (page != null)
                    {
                        if (page.isCompleted)
                        {
                            this.analyzerAndClassifier.analyzePage(page);
                        }
                        else
                        {
                            this.analyzerAndClassifier.classifyPage(page);
                        }
                    }

                    //
                    pageCounter++;
                    optimizerProgress = pageCounter * 1.0 / pageBacklogSize;

                    int durationDifferenceBetweenActualAndEstimatedDuration;
                    do
                    {
                        durationSinceOptimizingStart = System.DateTime.Now.Subtract(startTime);
                        int estimatedDurationSinceOptimizingStartInSeconds = (int)Math.Round(this.aspiredOptimizingDurationForWholeBacklogInSeconds * optimizerProgress);
                        durationDifferenceBetweenActualAndEstimatedDuration = estimatedDurationSinceOptimizingStartInSeconds - durationSinceOptimizingStart.Seconds;

                        if (durationDifferenceBetweenActualAndEstimatedDuration > 0)
                        {
                            Thread.Sleep(100);
                        }
                    } while (durationDifferenceBetweenActualAndEstimatedDuration > 0 && !this.isAborting);

                    //
                    if (this.isAborting)
                    {
                        break;
                    }

                    //
                    if (pageCounter % 100 == 0)
                    {
                        this.analyzerAndClassifier.cleanupStatistics();
                    }
                }

                //force page backlog to recognize the new priorities
                this.pageBacklog.addPageList(pageList);

                //
                this.analyzerAndClassifier.cleanupStatistics();
            }
        }

        public void stopOptimizing()
        {
            this.isAborting = true;
        }

        public void setPageBacklog(IPageBacklog pageBacklog)
        {
            this.pageBacklog = pageBacklog;
        }        

        #endregion
    }
}
