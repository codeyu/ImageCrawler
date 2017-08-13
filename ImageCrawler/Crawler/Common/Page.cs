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

namespace ImageCrawler.Crawler.Common
{
    /// <summary>
    /// Data object for page information collected by the crawler step for step.
    /// </summary>
    public class Page
    {
        public String urlStr { get; set; }

        public List<String> redirectUrlStrList { get; set; }
        public String pageContent { get; set; }
        public List<String> imageSrcLinkList { get; set; }
        public List<String> ankerHrefLinkList { get; set; }

        public long imageFoundCounter { get; set; }
        public long imageFoundAreaSummation { get; set; }
        public Boolean requestFailure { get; set; }

        public Boolean isCompleted { get; set; }

        //priority -> higher numbers will force the crawler to catch the page more likely.
        private int _priority = 5;
        public int priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = Math.Max(0, Math.Min(16, value));
            }
        }

        public Page()
        {
            this.requestFailure = false;
            this.imageFoundCounter = 0;
            this.imageFoundAreaSummation = 0;
            this.isCompleted = false;
        }

        public Page(String urlStr):this()
        {
            this.urlStr = urlStr;
        }

    }
}
