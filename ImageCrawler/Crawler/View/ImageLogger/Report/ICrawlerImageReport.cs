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

namespace ImageCrawler.Crawler.ImageLogger.Report
{
    /// <summary>
    /// Collects information about the stored images and creates a report from it.
    /// </summary>
    public interface IStoredCrawlerImageReport
    {
        /// <summary>
        /// Adds information about an stored image file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="pagelinkUrlStr"></param>
        void addCrawlerImageInformation(String filename, String pagelinkUrlStr);

        /// <summary>
        /// Saves the report to the given file. The filename should be without extension. The extension will be added by the implementation.
        /// </summary>
        /// <param name="filename"></param>
        void save(String filename);

        /// <summary>
        /// Cleans the saved report informations.
        /// </summary>
        void clear();
    }
}
