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
    /// Defines the filter data used to determine, which images are allowed to be retrieved or stored.
    /// </summary>
    public class CrawlerImageFilter
    {
        public int minimumWidth { get; set; }
        public int minimumHeight { get; set; }
        public int minimumArea { get; set; }
        public int minimumFilesize { get; set; }
        public Boolean onlyAnkerLinkedImages { get; set; }
        public Boolean filterDummyRequest { get; set; }
        public Boolean filterImagesWithSameSize { get; set; }
        public Boolean filterFreeFloatingImageUrls { get; set; }

        public CrawlerImageFilter()
        {
            this.minimumHeight = 10;
            this.minimumWidth = 10;
            this.minimumArea = 10;
            this.minimumFilesize = 10000;
            this.onlyAnkerLinkedImages = false;
            this.filterDummyRequest = false;
            this.filterFreeFloatingImageUrls = false;
        }
    }
}
