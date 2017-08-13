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
    public class CrawlerJobPartInitialPageLock : ICrawlerJobPart
    {
        #region ICrawlerJobPart Members

        public CrawlerJobContext crawlerJobContext { get; set; }

        public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
        public event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;
        public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        
        public List<ICrawlerJobPart> run()
        {
            //
            List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.pageBacklog != null)
            {
                Page page = this.crawlerJobContext.pageBacklog.getAndRemovePageFromTopOfTheUndoneStack();
                if (page != null)
                {
                    this.crawlerJobContext.crawlerJob.addCrawlerJobPartAndSetPage(new CrawlerJobPartFilter(), page);
                }
            }

            //
            return crawlerJobPartList;
        }

        public void doAbort()
        {
        }

        public CrawlerJobProcessState getProcessState()
        {
            return CrawlerJobProcessState.Initialize;
        }


        #endregion


    }
}
