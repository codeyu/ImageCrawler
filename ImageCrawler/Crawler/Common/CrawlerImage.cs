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
using System.Drawing;

namespace ImageCrawler.Crawler.Common
{
    /// <summary>
    /// Container for an downloaded image.
    /// </summary>
    public class CrawlerImage
    {
        public Image image { get; set; }
        public String srcUrlStr { get; set; }
        public String pageLinkUrlStr { get; set; }
        public String type { get; set; }
        public long filesize { get; set; }
        public String srcUrlPath
        {
            get
            {
                try
                {
                    Uri uri = new Uri(this.srcUrlStr);
                    return uri.AbsolutePath;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Returns an event args wrapper containing this crawler image object.
        /// </summary>
        /// <returns></returns>
        public EventArg asEventArg()
        {
            return new EventArg(this);
        }

        /// <summary>
        /// Wrapper for the event arg transport.
        /// </summary>
        public class EventArg : EventArgs
        {
            public CrawlerImage crawlerImage = null;

            public EventArg(CrawlerImage crawlerImage)
            {
                this.crawlerImage = crawlerImage;
            }
        }
    }
}
