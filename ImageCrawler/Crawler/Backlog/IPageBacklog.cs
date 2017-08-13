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
    /// A backlog of pages provides two kinds of functions
    /// </summary>
    public interface IPageBacklog
    {
        /// <summary>
        /// Returns the page object, which corresponds to the given absolute url string.
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        Page getPageByUrlStr(String urlStr);

        /// <summary>
        /// Adds a new page to the backlog. Returns true, if the backlog does not contain the page already. If a page is already within the page backlog and it is still part of the undone pages, it will be reinserted with the right priority.
        /// </summary>
        /// <param name="page"></param>
        Boolean addPage(Page page);

        /// <summary>
        /// Adds a list of pages to the backlog. Returns all pages, which were added to the backlog successfully.
        /// </summary>
        /// <param name="pageList"></param>
        List<Page> addPageList(List<Page> pageList);

        /// <summary>
        /// Removes a page from the backlog.
        /// </summary>
        /// <param name="page"></param>
        void removePage(Page page);

        /// <summary>
        /// Gets the first entry of the page backlog and removes it from the backlog. Returns null, if no page is within the backlog.
        /// </summary>
        /// <returns></returns>
        Page getAndRemovePageFromTopOfTheUndoneStack();

        /// <summary>
        /// Returns the number of pages within the page backlog which are not yet done.
        /// </summary>
        /// <returns></returns>
        int getBacklogSizeForUndonePages();

        /// <summary>
        /// Returns all pages ever successfully registered to the backlog.
        /// </summary>
        /// <returns></returns>
        int getBacklogSizeForAllRegisteredPages();

        /// <summary>
        /// Returns true, if at least one page is within the page undone backlog.
        /// </summary>
        /// <returns></returns>
        Boolean isBacklogForUndonePagesNotEmpty();

        /// <summary>
        /// Returns a new list containing references of all pages within the page backlog.
        /// </summary>
        /// <returns></returns>
        List<Page> getAllBacklogPageList();

        /// <summary>
        /// Clears the whole page backlog.
        /// </summary>
        void clear();
    }
}
