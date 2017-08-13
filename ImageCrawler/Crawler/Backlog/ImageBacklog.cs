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
    public class CrawlerImageBacklog:ICrawlerImageBacklog
    {
        protected Dictionary<String, bool> srcUrlStrToBooleanDictionary = new Dictionary<String,bool>();
        protected Dictionary<long, bool> filesizeToBooleanDictionary = new Dictionary<long, bool>();

        #region IImageBacklog Members

        public Boolean addImage(CrawlerImage crawlerImage)
        {
            //
            Boolean retval = false;

            //
            if (!this.containsImage(crawlerImage))
            {
                //
                String srcUrlStr = crawlerImage.srcUrlStr;
                if ( srcUrlStr != null)
                {
                    //
                    lock (this.srcUrlStrToBooleanDictionary)
                    {
                        this.srcUrlStrToBooleanDictionary.Add(srcUrlStr, true);
                    }

                    //
                    lock (this.filesizeToBooleanDictionary)
                    {
                        this.filesizeToBooleanDictionary.Add(crawlerImage.filesize, true);
                    }

                    retval = true;
                }
            }

            //
            return retval;
        }

        public Boolean containsImage(CrawlerImage crawlerImage)
        {
            return crawlerImage != null && this.containsImageSrcUrlStr(crawlerImage.srcUrlStr);
        }

        public Boolean containsImageFilesize(long filesize)
        {
            //
            Boolean retval = false;

            //            
            lock (this.srcUrlStrToBooleanDictionary)
            {
                retval = this.filesizeToBooleanDictionary.ContainsKey(filesize);
            }

            //
            return retval;
        }

        public Boolean containsImageSrcUrlStr(string srcUrlStr)
        {
            //
            Boolean retval = false;

            //            
            if (srcUrlStr != null)
            {
                lock (this.srcUrlStrToBooleanDictionary)
                {
                    retval = this.srcUrlStrToBooleanDictionary.ContainsKey(srcUrlStr);
                }
            }


            //
            return retval;
        }

        public void clear()
        {
            lock (this.srcUrlStrToBooleanDictionary)
            {
                this.srcUrlStrToBooleanDictionary.Clear();
            }
        }


        #endregion
    }
}
