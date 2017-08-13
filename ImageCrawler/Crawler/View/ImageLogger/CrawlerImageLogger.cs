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
using System.IO;
using ImageCrawler.Crawler.ImageLogger.Report;
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.ImageLogger
{
    public class CrawlerImageLogger : ICrawlerImageLogger
    {
        protected IStoredCrawlerImageReport storedCrawlerImageReport = new StoredCrawlerImageReportDecorator().addStoredCrawlerImageReport(new StoredCrawlerImageReportHtml()).addStoredCrawlerImageReport(new StoredCrawlerImageReportXml());
        protected String foldername = null;

        private long fileId = System.DateTime.Now.ToBinary() % 10000;
        private int imageFileCounter = 0;

        private List<CrawlerImage> cachedCrawlerImageList = new List<CrawlerImage>();

        /// <summary>
        /// Defines the sum of all image files, which are cached, if no directory is set. If a directory will be set later, theses files will be written to the log, too. Default is 0mb, so noch caching will occure.
        /// </summary>
        public long crawlerImageCacheMaximumFilesize { get; set; }

        public CrawlerImageLogger()
        {
            this.crawlerImageCacheMaximumFilesize = 0; //0 is default, no caching.
        }


        public void logCrawlerImage(CrawlerImage crawlerImage)
        {
            if (this.foldername != null)
            {
                //
                String filename = this.generateImageFilename(crawlerImage);
                crawlerImage.image.Save(filename);
                this.storedCrawlerImageReport.addCrawlerImageInformation(filename, crawlerImage.pageLinkUrlStr);

                //auto log
                if (this.imageFileCounter % 50 == 0)
                {
                    this.writeReportFile();
                }
            }
            else
            {
                this.doCacheCrawlerImage(crawlerImage);
            }
        }

        public void logCrawlerImageList(List<CrawlerImage> crawlerImageList)
        {
            if (crawlerImageList != null)
            {
                foreach (CrawlerImage crawlerImage in crawlerImageList)
                {
                    this.logCrawlerImage(crawlerImage);
                }
            }
        }

        private void doCacheCrawlerImage(CrawlerImage crawlerImage)
        {
            lock (this.cachedCrawlerImageList)
            {
                if (this.determineCurrentlyCachedCrawlerImageFilesizeSum() + crawlerImage.filesize < this.crawlerImageCacheMaximumFilesize)
                {
                    this.cachedCrawlerImageList.Add(crawlerImage);
                }
            }
        }

        private void writeCachedCrawlerImagesIntoLog()
        {
            if (this.foldername != null)
            {
                lock (this.cachedCrawlerImageList)
                {
                    this.logCrawlerImageList(this.cachedCrawlerImageList);
                }
            }
        }

        private long determineCurrentlyCachedCrawlerImageFilesizeSum()
        {
            //
            long retval = 0;

            //
            lock (this.cachedCrawlerImageList)
            {
                foreach (CrawlerImage crawlerImage in this.cachedCrawlerImageList)
                {
                    retval += crawlerImage.filesize;
                }
            }

            //
            return retval;
        }

        public void writeReportFile()
        {
            //
            if (this.foldername != null)
            {
                this.storedCrawlerImageReport.save(this.generateReportFilename());
            }
        }

        /// <summary>
        /// Generates the filename for the report file.
        /// </summary>
        /// <returns></returns>
        private String generateReportFilename()
        {
            //
            String filename = "";

            //
            try
            {
                if (this.foldername != null && new DirectoryInfo(this.foldername).Exists)
                {
                    filename = this.foldername + "\\index" + this.fileId; //fileextension will be set by the report implementation
                }
            }
            catch (Exception e) { }

            //
            return filename;
        }

        /// <summary>
        /// Generates the filename for the report file.
        /// </summary>
        /// <returns></returns>
        private String generateImageFilename(CrawlerImage crawlerImage)
        {
            //
            String filename = "";

            //
            try
            {
                if (this.foldername != null && new DirectoryInfo(this.foldername).Exists)
                {
                    Boolean fileExistsAlready = true;
                    while (fileExistsAlready)
                    {
                        filename = this.foldername + "\\" + this.fileId + crawlerImage.pageLinkUrlStr.Length + "image" + this.imageFileCounter++ + "." + crawlerImage.type;
                        fileExistsAlready = new FileInfo(filename).Exists;
                    }
                }
            }
            catch (Exception e) { }

            //
            return filename;
        }

        public void setLoggerFoldername(String foldername)
        {
            if (this.foldername == null)
            {
                this.foldername = foldername;
                this.writeCachedCrawlerImagesIntoLog();
            }
            else if (!this.foldername.Equals(foldername))
            {
                this.writeReportFile();
                this.clear();
                this.foldername = foldername;
            }            
        }

        public void clear()
        {
            this.storedCrawlerImageReport.clear();
            this.cachedCrawlerImageList.Clear();
        }
    }
}
