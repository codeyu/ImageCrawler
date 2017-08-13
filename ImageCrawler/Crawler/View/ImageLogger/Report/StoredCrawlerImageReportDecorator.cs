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
    /// Decorator implementation, to provide a single object point for all added further implementations of the same interface. For this purpose an internal list of implementations is hold, where new instances can be added to and removed from.
    /// </summary>
    public class StoredCrawlerImageReportDecorator : IStoredCrawlerImageReport
    {
        protected List<IStoredCrawlerImageReport> storedCrawlerImageReportList = new List<IStoredCrawlerImageReport>();

        #region IStoredCrawlerImageReport Members

        public void addCrawlerImageInformation(string filename, string pagelinkUrlStr)
        {
            foreach (IStoredCrawlerImageReport storedCrawlerImageReport in this.storedCrawlerImageReportList)
            {
                storedCrawlerImageReport.addCrawlerImageInformation(filename, pagelinkUrlStr);
            }
        }

        public void save(string filename)
        {
            foreach (IStoredCrawlerImageReport storedCrawlerImageReport in this.storedCrawlerImageReportList)
            {
                storedCrawlerImageReport.save(filename);
            }
        }

        public void clear()
        {
            foreach (IStoredCrawlerImageReport storedCrawlerImageReport in this.storedCrawlerImageReportList)
            {
                storedCrawlerImageReport.clear();
            }
        }

        public StoredCrawlerImageReportDecorator addStoredCrawlerImageReport(IStoredCrawlerImageReport storedCrawlerImageReport)
        {
            //
            if (storedCrawlerImageReport != null)
            {
                this.storedCrawlerImageReportList.Add(storedCrawlerImageReport);
            }

            //
            return this;
        }

        public void removeStoredCrawlerImageReport(IStoredCrawlerImageReport storedCrawlerImageReport)
        {
            //
            if (storedCrawlerImageReport != null)
            {
                this.storedCrawlerImageReportList.Remove(storedCrawlerImageReport);
            }
        }

        #endregion
    }
}
