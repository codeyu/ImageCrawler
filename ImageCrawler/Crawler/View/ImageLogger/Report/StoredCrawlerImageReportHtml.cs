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

namespace ImageCrawler.Crawler.ImageLogger.Report
{
    public class StoredCrawlerImageReportHtml : StoredCrawlerImageReportAbstract
    {
        #region IStoredCrawlerImageReport Members

        public override void save(string filename)
        {
            if (filename != null)
            {
                try
                {
                    using (StreamWriter writer = File.CreateText(filename + ".html"))
                    {
                        //
                        writer.WriteLine("<html>");
                        writer.WriteLine("<head>");
                        writer.WriteLine("</head>");
                        writer.WriteLine("<body>");

                        //
                        List<String> sortedSrcUrlStr = new List<String>(this.srcUrlStrToFilenameDictionary.Keys);
                        sortedSrcUrlStr.Sort();

                        foreach (String srcUrlStr in sortedSrcUrlStr)
                        {
                            List<String> imageFilenameList;
                            Boolean valid = this.srcUrlStrToFilenameDictionary.TryGetValue(srcUrlStr, out imageFilenameList);
                            if (valid && imageFilenameList != null)
                            {
                                foreach (String imageFilename in imageFilenameList)
                                {
                                    writer.WriteLine("  <a href=\"" + srcUrlStr + "\">");
                                    writer.WriteLine("    <img src=\"" + imageFilename + "\"/>");
                                    writer.WriteLine("  </a>");
                                }
                            }
                        }

                        //
                        writer.WriteLine("</body>");
                        writer.WriteLine("</html>");
                    }
                }
                catch (Exception e) { }
            }
        }

        #endregion
        
    }
}
