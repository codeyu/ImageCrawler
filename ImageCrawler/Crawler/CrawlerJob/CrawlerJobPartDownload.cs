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
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using ImageCrawler.Util;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Job
{
    public class CrawlerJobPartDownload : ICrawlerJobPart
    {
        protected Boolean isAborting = false;
        public CrawlerJobContext crawlerJobContext { get; set; }

        /// <summary>
        /// Identifies requests, which are not redirected to another url.
        /// </summary>
        private Boolean isNotRedirect = false;

        private HttpWebRequest webRequest = null;
        private HttpWebResponse webResponse = null;

        #region ICrawlerJobPart Members

        public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
        public event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;
        public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        /// <summary>
        /// Determines if the resonse includes a redirect to another url. If yes, the redirect url is added to the page information.
        /// </summary>
        private void analyzeAndSetRedirectInformationForPage()
        {
            if (this.webResponse != null && this.webResponse.ResponseUri != null)
            {
                try
                {
                    Uri redirectUri = this.webResponse.ResponseUri;
                    Uri pageUri = new Uri(this.crawlerJobContext.page.urlStr);
                    if (pageUri != null)
                    {
                        if (!pageUri.Equals(redirectUri))
                        {
                            if (this.crawlerJobContext.page.redirectUrlStrList == null)
                            {
                                this.crawlerJobContext.page.redirectUrlStrList = new List<String>();
                            }

                            this.crawlerJobContext.page.redirectUrlStrList.Add(redirectUri.AbsoluteUri);
                        }
                        else
                        {
                            this.isNotRedirect = true;
                        }
                    }
                }
                catch (Exception e) { }
            }
        }

        public List<ICrawlerJobPart> run()
        {
            //
            List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(0.0);
            }

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.page != null && this.crawlerJobContext.page.urlStr != null)
            {
                //
                try
                {
                    //fire request
                    this.webRequest = (HttpWebRequest)HttpWebRequest.Create(this.crawlerJobContext.page.urlStr);
                    this.webRequest.AllowAutoRedirect = true;
                    this.webRequest.AllowWriteStreamBuffering = true;
                    this.webRequest.MaximumAutomaticRedirections = 5;

                    //catch response                    
                    this.webResponse = (HttpWebResponse)webRequest.GetResponse();

                    //
                    this.analyzeAndSetRedirectInformationForPage();

                    //second request if filter settings want to avoid dummy pages
                    if (this.crawlerJobContext.crawlerImageFilter != null && this.crawlerJobContext.crawlerImageFilter.filterDummyRequest && !this.isNotRedirect)
                    {
                        //
                        Thread.Sleep(5000);

                        //
                        this.webRequest = (HttpWebRequest)HttpWebRequest.Create(this.crawlerJobContext.page.urlStr);
                        this.webRequest.AllowAutoRedirect = true;
                        this.webRequest.AllowWriteStreamBuffering = true;
                        this.webRequest.MaximumAutomaticRedirections = 5;

                        //
                        this.webResponse = (HttpWebResponse)webRequest.GetResponse();

                        //
                        this.analyzeAndSetRedirectInformationForPage();
                    }

                    //process response stream
                    if (this.isNotRedirect && this.webResponse != null)
                    {
                        //
                        String contentType = this.webResponse.ContentType;

                        //
                        Boolean isHtmlPage = contentType == null || contentType.IndexOf("html") >= 0;
                        Boolean isImage = contentType != null && contentType.IndexOf("image") >= 0;

                        //
                        if (isHtmlPage)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                //
                                using (Stream webResponseInputStream = this.webResponse.GetResponseStream())
                                {
                                    long estimatedContentSize = this.webResponse.ContentLength;
                                    StreamConnector.connect(webResponseInputStream, memoryStream, 8000, true, delegate(long currentlyReadBytes, out Boolean doAbort)
                                    {
                                        if (this.progressPercentageChangeEvent != null)
                                        {
                                            double progressPercentage = estimatedContentSize > 0 ? (currentlyReadBytes * 1.0) / (estimatedContentSize * 1.0) : 1.0 - 1.0 / Math.Max(currentlyReadBytes, 1.0);
                                            this.progressPercentageChangeEvent(progressPercentage);
                                        }

                                        if (this.crawlerJobPartSuspendEvent != null)
                                        {
                                            this.crawlerJobPartSuspendEvent();
                                        }

                                        doAbort = this.isAborting;
                                    });

                                    webResponseInputStream.Close();
                                }

                                //
                                memoryStream.Position = 0;
                                using (StreamReader reader = new StreamReader(memoryStream, true))
                                {
                                    this.crawlerJobContext.page.pageContent = reader.ReadToEnd();
                                }
                            }
                        }
                        else if (isImage && (this.crawlerJobContext.imageBacklog == null || !this.crawlerJobContext.imageBacklog.containsImageSrcUrlStr(this.crawlerJobContext.page.urlStr)))
                        {
                            if (this.crawlerJobContext.crawlerImageFilter == null || (
                                (this.webResponse.ContentLength < 0 || this.webResponse.ContentLength > this.crawlerJobContext.crawlerImageFilter.minimumFilesize)
                                && (!this.crawlerJobContext.crawlerImageFilter.filterImagesWithSameSize || !this.crawlerJobContext.imageBacklog.containsImageFilesize(this.webResponse.ContentLength))
                                ))
                            {
                                //
                                CrawlerImage crawlerImage = new CrawlerImage();
                                String[] contentTypeTokens = contentType.Split('/');
                                if (contentTypeTokens.Length == 2)
                                {
                                    //
                                    crawlerImage.type = contentTypeTokens[1];

                                    //
                                    using (Stream webResponseInputStream = this.webResponse.GetResponseStream())
                                    {
                                        ImageCrawler.Util.StreamConnector.ProgressReportStream progressReportStream = new ImageCrawler.Util.StreamConnector.ProgressReportStream(webResponseInputStream);
                                        long estimatedContentSize = this.webResponse.ContentLength;
                                        progressReportStream.progressUpdateEvent += new EventHandler<StreamConnector.ProgressReportStream.EventArgs>(delegate(object sender, StreamConnector.ProgressReportStream.EventArgs args)
                                        {
                                            if (this.progressPercentageChangeEvent != null)
                                            {
                                                double progressPercentage = estimatedContentSize > 0 ? (args.readBytesCounter * 1.0) / (estimatedContentSize * 1.0) : 1.0 - 1.0 / Math.Max(args.readBytesCounter, 1.0);
                                                this.progressPercentageChangeEvent(progressPercentage);
                                            }

                                            if (this.crawlerJobPartSuspendEvent != null)
                                            {
                                                this.crawlerJobPartSuspendEvent();
                                            }
                                        });

                                        Image image = Image.FromStream(progressReportStream);

                                        // 
                                        if (image.Width > this.crawlerJobContext.crawlerImageFilter.minimumWidth && image.Height > this.crawlerJobContext.crawlerImageFilter.minimumHeight && image.Width * image.Height > this.crawlerJobContext.crawlerImageFilter.minimumArea)
                                        {
                                            //
                                            crawlerImage.image = image;
                                            crawlerImage.srcUrlStr = this.crawlerJobContext.page.urlStr;
                                            crawlerImage.pageLinkUrlStr = this.crawlerJobContext.page.urlStr;

                                            //
                                            if (this.crawlerJobPartImageRetrievedEvent != null && !this.isAborting)
                                            {
                                                this.crawlerJobPartImageRetrievedEvent(crawlerImage);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //
                    if (this.crawlerJobContext.page.pageContent != null && this.crawlerJobContext.page.pageContent.Length > 0)
                    {
                        crawlerJobPartList.Add(new CrawlerJobPartAnalyzePage());
                    }
                    else if (this.crawlerJobContext.page.redirectUrlStrList != null)
                    {
                        crawlerJobPartList.Add(new CrawlerJobPartGenerateSubJobs());
                    }

                }
                catch (Exception e)
                {
                    if (this.crawlerJobContext.page != null)
                    {
                        this.crawlerJobContext.page.requestFailure = true;
                    }
                }
                finally
                {
                    if (this.webResponse != null)
                    {
                        this.webResponse.Close();
                    }
                }
            }

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(1.0);
            }

            //
            return crawlerJobPartList;
        }

        private String determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(String parentUrlStr, String anyCurrentUrlStr)
        {
            //
            String retval = null;

            //
            if (parentUrlStr != null && anyCurrentUrlStr != null)
            {
                try
                {
                    Uri parentUri = new Uri(parentUrlStr);
                    Uri absoluteUri = new Uri(parentUri, anyCurrentUrlStr);
                    retval = absoluteUri.AbsoluteUri;
                }
                catch (Exception e)
                {
                }
            }

            //
            return retval;
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
            return CrawlerJobProcessState.Download;
        }

        

        #endregion

    }
}
