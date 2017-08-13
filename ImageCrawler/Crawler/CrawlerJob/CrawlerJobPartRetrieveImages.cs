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
using System.Net;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Job
{
    public class CrawlerJobPartRetrieveImages : ICrawlerJobPart
    {
        
        protected Boolean isAborting = false;
        
        private HttpWebRequest webRequest = null;
        private HttpWebResponse webResponse = null;

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
            if (this.crawlerJobContext != null && this.crawlerJobContext.crawlerImageFilter == null)
            {
                this.crawlerJobContext.crawlerImageFilter = new CrawlerImageFilter();
            }

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(0.000);
            }

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.page != null && this.crawlerJobContext.page.imageSrcLinkList != null && this.crawlerJobContext.page.imageSrcLinkList.Count > 0 && !this.isAborting && (this.crawlerJobContext.crawlerImageFilter == null || !this.crawlerJobContext.crawlerImageFilter.onlyAnkerLinkedImages))
            {
                List<String> imageSrcLinkList = this.crawlerJobContext.page.imageSrcLinkList;

                int counter = 0;
                int counterMax = imageSrcLinkList.Count - 1;
                foreach (String imageSrcLink in imageSrcLinkList)
                {
                    //
                    if (this.isAborting)
                    {
                        break;
                    }

                    //
                    try
                    {
                        //
                        CrawlerImage crawlerImage = new CrawlerImage();

                        //
                        try
                        {
                            this.webRequest = (HttpWebRequest)HttpWebRequest.Create(imageSrcLink);
                            this.webRequest.AllowAutoRedirect = true;
                            this.webRequest.MaximumAutomaticRedirections = 5;
                            this.webResponse = (HttpWebResponse)this.webRequest.GetResponse();
                        }
                        catch (Exception e)
                        {
                            //
                            if (this.webResponse != null)
                            {
                                this.webResponse.Close();
                            }
                        }

                        //
                        if (this.webResponse != null && (this.crawlerJobContext.imageBacklog == null || !this.crawlerJobContext.imageBacklog.containsImageSrcUrlStr(this.webResponse.ResponseUri.AbsoluteUri)))
                        {
                            if (this.crawlerJobContext.crawlerImageFilter == null || (
                                (this.webResponse.ContentLength < 0 || this.webResponse.ContentLength > this.crawlerJobContext.crawlerImageFilter.minimumFilesize)
                                && (!this.crawlerJobContext.crawlerImageFilter.filterImagesWithSameSize || !this.crawlerJobContext.imageBacklog.containsImageFilesize(this.webResponse.ContentLength))
                                ))
                            {
                                //
                                Image image = null;
                                using (Stream webResponseStream = this.webResponse.GetResponseStream())
                                {
                                    image = Image.FromStream(webResponseStream);
                                }

                                //
                                String contentType = webResponse.ContentType;
                                String contentTypeImage = null;
                                if (contentType != null)
                                {
                                    String[] contentTypeTokens = contentType.Split('/');
                                    if (contentTypeTokens.Length == 2)
                                    {
                                        contentTypeImage = contentTypeTokens[0];
                                        crawlerImage.type = contentTypeTokens[1];
                                    }
                                }

                                if (contentTypeImage == "image" && (this.crawlerJobContext.crawlerImageFilter == null || (image.Width > this.crawlerJobContext.crawlerImageFilter.minimumWidth && image.Height > this.crawlerJobContext.crawlerImageFilter.minimumHeight && image.Width * image.Height > this.crawlerJobContext.crawlerImageFilter.minimumArea)))
                                {
                                    //
                                    crawlerImage.image = image;
                                    crawlerImage.srcUrlStr = this.webResponse.ResponseUri.AbsoluteUri;
                                    crawlerImage.pageLinkUrlStr = this.crawlerJobContext.page.urlStr;
                                    crawlerImage.filesize = this.webResponse.ContentLength;

                                    //
                                    Boolean valid = this.crawlerJobContext.imageBacklog == null || this.crawlerJobContext.imageBacklog.addImage(crawlerImage);

                                    //
                                    if (valid && this.crawlerJobPartImageRetrievedEvent != null && !this.isAborting)
                                    {
                                        this.crawlerJobPartImageRetrievedEvent(crawlerImage);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        if (webResponse != null)
                        {
                            webResponse.Close();
                        }
                    }

                    //
                    counter++;
                    if (this.progressPercentageChangeEvent != null)
                    {
                        this.progressPercentageChangeEvent((counter * 1.0) / counterMax);
                    }

                    //
                    if (this.crawlerJobPartSuspendEvent != null)
                    {
                        this.crawlerJobPartSuspendEvent();
                    }

                    //
                    if (this.crawlerJobPartSuspendEvent != null)
                    {
                        this.crawlerJobPartSuspendEvent();
                    }
                }

                //
                this.crawlerJobContext.page.imageSrcLinkList.Clear();
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
            if (this.webResponse != null)
            {
                this.webResponse.Close();
            }
        }

        public CrawlerJobProcessState getProcessState()
        {
            return CrawlerJobProcessState.RetrieveImages;
        }

        
        #endregion
    }
}
