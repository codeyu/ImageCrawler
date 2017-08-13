using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageCrawler.Crawler.Job
{
    /// <summary>
    /// Implements a filtering part, where the page url is validated against the page filter.
    /// </summary>
    public class CrawlerJobPartFilter:ICrawlerJobPart
    {

        #region ICrawlerJobPart Members

        public CrawlerJobContext crawlerJobContext {get;set;}

        public event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;

        public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;

        public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        public CrawlerJobProcessState getProcessState()
        {
            return CrawlerJobProcessState.Filtering;
        }

        public List<ICrawlerJobPart> run()
        {
            //
            List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.page != null && this.crawlerJobContext.pageFilter != null)
            {
                Boolean isValidPage = this.crawlerJobContext.pageFilter.checkForValidPage(this.crawlerJobContext.page);
                if (isValidPage)
                {
                    this.crawlerJobContext.crawlerJob.addCrawlerJobPartAndSetPage(new CrawlerJobPartDownload(), this.crawlerJobContext.page);
                }
            }

            //
            return crawlerJobPartList;
        }

        public void doAbort()
        {            
        }

        #endregion
    }
}
