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
    public class PageBacklog : IPageBacklog
    {
        protected Dictionary<String, Page> urlStrToPageDicitionary = new Dictionary<String,Page>();
        protected List<Dictionary<Page,bool>> undonePageDictionaryList = new List<Dictionary<Page,bool>>();


        public PageBacklog()
        {
        }

        #region IPageBacklog Members

        public Page getPageByUrlStr(string urlStr)
        {
            //
            Page page = null;

            //
            lock (this.urlStrToPageDicitionary)
            {
                this.urlStrToPageDicitionary.TryGetValue(urlStr, out page);
            }

            //
            return page;
        }

        public Boolean addPage(Page page)
        {
            //
            Boolean retval = false;

            //
            lock (this.urlStrToPageDicitionary)
            {
                if (page != null && page.urlStr != null)
                {
                    //
                    if (!this.urlStrToPageDicitionary.ContainsKey(page.urlStr))
                    {
                        //
                        this.urlStrToPageDicitionary.Add(page.urlStr, page);

                        //
                        this.includePageIntoUndonePageList(page);

                        //
                        retval = true;
                    }
                    else
                    {
                        //reinsert if the page is still in the undone page list. This ensures the page being correlated to the right priority.
                        lock(this.undonePageDictionaryList)
                        {
                            if (this.removePageFromUndonePageList(page))
                            {
                                this.includePageIntoUndonePageList(page);
                            }
                        }
                    }
                }
            }

            //
            return retval;
        }

        private void includePageIntoUndonePageList(Page page)
        {
            //
            lock (this.undonePageDictionaryList)
            {
                //
                int priority = this.defaultPriority(page.priority);
                this.ensureUndonePageListToHavePlacesUpToTheGivenPriorityIndex(priority);
                this.undonePageDictionaryList[priority].Add(page,true);
            }
        }

        /// <summary>
        /// Returns a valid priority
        /// </summary>
        /// <param name="priority"></param>
        /// <returns></returns>
        private int defaultPriority(int priority)
        {
            return Math.Max(0, Math.Min(100, priority));
        }

        private void ensureUndonePageListToHavePlacesUpToTheGivenPriorityIndex(int priorityIndex)
        {
            while (priorityIndex >= this.undonePageDictionaryList.Count())
            {
                this.undonePageDictionaryList.Add(new Dictionary<Page,bool>());
            }
        }

        

        public List<Page> addPageList(List<Page> pageList)
        {
            //
            List<Page> retlist = new List<Page>();

            //
            if (pageList != null)
            {
                foreach (Page page in pageList)
                {
                    if (this.addPage(page))
                    {
                        retlist.Add(page);
                    }
                }
            }

            //
            return retlist;
        }

        public void removePage(Page page)
        {
            lock (this.urlStrToPageDicitionary)
            {
                lock (this.undonePageDictionaryList)
                {
                    if (page != null && page.urlStr != null && this.urlStrToPageDicitionary.ContainsKey(page.urlStr))
                    {
                        this.urlStrToPageDicitionary.Remove(page.urlStr);
                        this.removePageFromUndonePageList(page);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the page from the undone page list and returns true, if the page was found and removed.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private Boolean removePageFromUndonePageList(Page page)
        {
            //
            Boolean retval = false;

            //
            lock (this.undonePageDictionaryList)
            {
                foreach (Dictionary<Page,bool> pageDictionary in this.undonePageDictionaryList)
                {
                    if (pageDictionary != null && pageDictionary.Remove(page))
                    {
                        retval = true;
                        break;
                    }
                }
            }

            //
            return retval;
        }

        public int getBacklogSizeForUndonePages()
        {
            lock (this.undonePageDictionaryList)
            {
                //
                int size = 0;

                //
                foreach (Dictionary<Page,bool> pageDictionary in this.undonePageDictionaryList)
                {
                    size += pageDictionary.Count();
                }

                //
                return size;
            }
        }

        public int getBacklogSizeForAllRegisteredPages()
        {
            lock (this.urlStrToPageDicitionary)
            {
                return this.urlStrToPageDicitionary.Count;
            }
        }

        public bool isBacklogForUndonePagesNotEmpty()
        {
            return this.getBacklogSizeForUndonePages() > 0;
        }

        public Page getAndRemovePageFromTopOfTheUndoneStack()
        {
            //
            Page page = null;

            //
            lock (this.undonePageDictionaryList)
            {
                if (this.undonePageDictionaryList.Count > 0)
                {
                    for (int ii = this.undonePageDictionaryList.Count - 1; ii >= 0; ii--)
                    {
                        //
                        Dictionary<Page,bool> pageDictionary = this.undonePageDictionaryList[ii];

                        //
                        if (pageDictionary != null && pageDictionary.Count > 0)
                        {
                            //
                            page = pageDictionary.Keys.First();
                            if (page != null)
                            {
                                pageDictionary.Remove(page);
                            }

                            //
                            break;
                        }
                    }
                }
            }

            //
            return page;
        }

        public List<Page> getAllBacklogPageList()
        {
            lock (this.urlStrToPageDicitionary)
            {
                return new List<Page>(this.urlStrToPageDicitionary.Values);
            }
        }

        public void clear()
        {
            lock (this.urlStrToPageDicitionary)
            {
                lock (this.undonePageDictionaryList)
                {
                    this.undonePageDictionaryList.Clear();
                    this.urlStrToPageDicitionary.Clear();
                }
            }
        }

        #endregion
    }
}
