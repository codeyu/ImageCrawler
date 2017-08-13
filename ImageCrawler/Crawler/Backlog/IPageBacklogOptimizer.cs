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

namespace ImageCrawler.Crawler.Backlog
{
    /// <summary>
    /// Used to optimize the existing page backlog. Uses a simple analyzer algorithm running in a seperate thread. The optimizer uses an internal IAnalyzerAndClassifier object to analyze and classify pages.
    /// </summary>
    public interface IPageBacklogOptimizer
    {
        /// <summary>
        /// Starts the optimizing of the page backlog
        /// </summary>
        void startOptimizing();

        /// <summary>
        /// Stops the optimizing.
        /// </summary>
        void stopOptimizing();

        /// <summary>
        /// Sets the page backlog to be optimized.
        /// </summary>
        /// <param name="pageBacklog"></param>
        void setPageBacklog(IPageBacklog pageBacklog);


    }

    /// <summary>
    /// Defines an analyzer, which collects data from full processed pages and generates statistical data. And defines a classifying method, which allows classifying pages which are still in the page backlog as undone pages.
    /// </summary>
    public interface IPageBacklogOptimizerAnalyzerAndClassifier
    {
        /// <summary>
        /// Collects data from the page to update internal statistics.
        /// </summary>
        /// <param name="page"></param>
        void analyzePage(Page page);

        /// <summary>
        /// Classifies a page corresponding to the internal statistics.
        /// </summary>
        /// <param name="page"></param>
        void classifyPage(Page page);


        /// <summary>
        /// Triggers a cleanup of internal statistic data. Should not be triggered for every page.
        /// </summary>
        void cleanupStatistics();
    }
}
