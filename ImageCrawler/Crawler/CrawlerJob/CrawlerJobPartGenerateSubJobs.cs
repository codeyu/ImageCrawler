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
using System.Text.RegularExpressions;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Controller;

namespace ImageCrawler.Crawler.Job
{
    public class CrawlerJobPartGenerateSubJobs : ICrawlerJobPart
    {
        protected Boolean isAborting = false;

        #region ICrawlerJobPart Members

        public CrawlerJobContext crawlerJobContext { get; set; }

        public event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;
        public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
        public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        public List<ICrawlerJobPart> run()
        {
            //
            List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(0.000);
            }

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.page != null && this.crawlerJobContext.crawlerJob != null && !this.isAborting)
            {
                ICrawlerController crawlerController = this.crawlerJobContext.crawlerJob.getParentalCrawlerController();
                if (this.crawlerJobContext.crawlerJob.getParentalCrawlerController() != null)
                {
                    //
                    List<String> ankerHrefLinkList = new List<String>();
                    if (this.crawlerJobContext.page.ankerHrefLinkList != null)
                    {
                        ankerHrefLinkList.AddRange(this.crawlerJobContext.page.ankerHrefLinkList);
                    }

                    //
                    if (this.crawlerJobContext.page.redirectUrlStrList != null)
                    {
                        ankerHrefLinkList.AddRange(this.crawlerJobContext.page.redirectUrlStrList);
                    }

                    //                    
                    int counter = 0;
                    int counterMax = ankerHrefLinkList.Count;
                    foreach (String ankerHrefLink in ankerHrefLinkList)
                    {
                        //
                        if (this.isAborting)
                        {
                            break;
                        }

                        //
                        Page page = new Page();
                        //page.parent = this.page;
                        //this.page.childPageList.Add(page);
                        page.urlStr = ankerHrefLink;

                        //image is part of the url
                        if (Regex.IsMatch(ankerHrefLink, "image"))
                        {
                            page.priority = 10;
                        }

                        //image type is part of the url
                        if (Regex.IsMatch(ankerHrefLink, "jp.?g|gif|png|bmp"))
                        {
                            page.priority = 10;
                        }  

                        /*
                         * end special priority behavior
                         */

                        //
                        crawlerController.addPage(page);

                        //
                        counter++;
                        if (this.progressPercentageChangeEvent != null)
                        {
                            this.progressPercentageChangeEvent(counter * 1.0 / counterMax);
                        }

                        //
                        if (this.crawlerJobPartSuspendEvent != null)
                        {
                            this.crawlerJobPartSuspendEvent();
                        }
                    }

                    //
                    this.crawlerJobContext.page.ankerHrefLinkList = null;
                    this.crawlerJobContext.page.redirectUrlStrList = null;
                }
            }

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(1.000);
            }

            //
            return crawlerJobPartList;
        }        

        public void doAbort()
        {
            this.isAborting = true;
        }

        public CrawlerJobProcessState getProcessState()
        {
            return CrawlerJobProcessState.GenerateSubJobs;
        }


        #endregion
    }
}
