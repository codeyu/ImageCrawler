using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Filter
{
    public interface IPageFilter
    {
        /// <summary>
        /// Checks the given page, if it will should not be filtered. 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Boolean checkForValidPage(Page page);
    }
}
