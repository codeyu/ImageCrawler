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
using ImageCrawler.Crawler.Common;
namespace ImageCrawler.Crawler.ImageLogger
{
    /// <summary>
    /// Used to log images to a given folder. If no folder is set, no immediate logging will occure. If a directory is set later and images came in before, they are cached up to the size defined with crawlerImageCacheMaximumFilesize.
    /// </summary>
    public interface ICrawlerImageLogger
    {
        /// <summary>
        /// Defines the summed cached image filesize. As long as all cached files have less than this size together, new images will be cached, if no directory has been set for the logger.
        /// </summary>
        long crawlerImageCacheMaximumFilesize { get; set; }

        /// <summary>
        /// Writes an image to the logging directory or caches it, if no logging folder is set.
        /// </summary>
        /// <param name="crawlerImage"></param>
        void logCrawlerImage(CrawlerImage crawlerImage);

        /// <summary>
        /// Writes a list of images to the logging directory or caches them, if no logging folder is set.
        /// </summary>
        /// <param name="crawlerImageList"></param>
        void logCrawlerImageList(System.Collections.Generic.List<CrawlerImage> crawlerImageList);

        /// <summary>
        /// Sets the name of the logging directory. If images are logged before, without an directory has been set, the images have been possibly cached, so they are written to the given directory immediately. To stop logging set the parameter to null.
        /// </summary>
        /// <param name="foldername"></param>
        void setLoggerFoldername(string foldername);

        /// <summary>
        /// Writes a report of the file collection to the logging directory.
        /// </summary>
        void writeReportFile();

        /// <summary>
        /// Clears the internal image log.
        /// </summary>
        void clear();
    }
}
