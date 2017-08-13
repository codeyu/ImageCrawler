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
    /// <summary>
    /// The image backlog is used to ensure, images are not loaded twice. Implementations should not store given images, only meta informations, to reduce large memory consumption.
    /// </summary>
    public interface ICrawlerImageBacklog
    {
        /// <summary>
        /// Adds an new image to the image backlog. If the image is already within the backlog, false is returned, otherwise true.
        /// </summary>
        /// <param name="crawlerImage"></param>
        /// <returns></returns>
        Boolean addImage(CrawlerImage crawlerImage);

        /// <summary>
        /// Returns true, if the image backlog already contains the given image.
        /// </summary>
        /// <param name="crawlerImage"></param>
        Boolean containsImage(CrawlerImage crawlerImage);

        /// <summary>
        /// Returns true, if the given url is already registered for an image.
        /// </summary>
        /// <param name="srcUrlStr"></param>
        /// <returns></returns>
        Boolean containsImageSrcUrlStr(String srcUrlStr);

        /// <summary>
        /// Returns true, if a image with the same filesize is already within the image backlog.
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        Boolean containsImageFilesize(long filesize);

        /// <summary>
        /// Makes the image backlog empty.
        /// </summary>
        void clear();
    }
}
