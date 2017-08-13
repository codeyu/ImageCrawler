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
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.Backlog;
using ImageCrawler.Crawler.Filter;

namespace ImageCrawler.Crawler.Job
{
    /// <summary>
    /// Holds object references for the context of a crawler job.
    /// </summary>
    public class CrawlerJobContext
    {
        public ICrawlerController crawlerController { get; set; }
        public Page page { get; set; }
        public CrawlerImageFilter crawlerImageFilter { get; set; }
        public ICrawlerImageBacklog imageBacklog { get; set; }
        public ICrawlerJob crawlerJob { get; set; }
        public IPageBacklog pageBacklog { get; set; }
        public IPageFilter pageFilter { get; set; }
    }
}
