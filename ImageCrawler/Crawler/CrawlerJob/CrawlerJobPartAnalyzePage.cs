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
using System.Text.RegularExpressions;

namespace ImageCrawler.Crawler.Job
{
    /// <summary>
    /// Analyzes the content part of a page object. Determines all anker href urls and produces absolute url strings, as well as all image links.
    /// </summary>
    public class CrawlerJobPartAnalyzePage:ICrawlerJobPart
    {
        public CrawlerJobContext crawlerJobContext { get; set; }

        protected Boolean isAborting = false;

        #region ICrawlerJobPart Members

        public event CrawlerJobPartImageRetrievedEvent crawlerJobPartImageRetrievedEvent;
        public event CrawlerJobPartProgressUpdate progressPercentageChangeEvent;
        public event CrawlerJobPartSuspendEvent crawlerJobPartSuspendEvent;

        public List<ICrawlerJobPart> run()
        {
            //
            List<ICrawlerJobPart> crawlerJobPartList = new List<ICrawlerJobPart>();

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(0.0);
            }

            //
            if (this.crawlerJobContext != null && this.crawlerJobContext.page != null && this.crawlerJobContext.page.pageContent != null)
            {
                //
                Boolean metaTagDoesNotAllowFollowingLinks = false;
                Boolean metaTagDoesNotAllowIndexingLinks = false;
                if (!this.isAborting)
                {
                    //analyze if there are meta tags dissallowing to crawl the page
                    metaTagDoesNotAllowFollowingLinks = Regex.IsMatch(this.crawlerJobContext.page.pageContent, "(?i)<META[^>]*name[^>^=]*=[^>]*\"(ROBOTS|robots)\"[^>]*content[^>^=]*=[^>]*\"[a-zA-Z,\\s]*NOFOLLOW[a-zA-Z,\\s]*\"[^>]*>");
                    metaTagDoesNotAllowIndexingLinks = Regex.IsMatch(this.crawlerJobContext.page.pageContent, "(?i)<META[^>]*name[^>^=]*=[^>]*\"(ROBOTS|robots)\"[^>]*content[^>^=]*=[^>]*\"[a-zA-Z,\\s]*NOINDEX[a-zA-Z,\\s]*\"[^>]*>"); 
                }

                //anker links
                if (!this.isAborting && !metaTagDoesNotAllowFollowingLinks)
                {
                    //
                    this.crawlerJobContext.page.ankerHrefLinkList = new List<String>();

                    //anker links
                    const String tagAnkerAttributeValue = "\"[^\"^>]*\"";
                    const String tagAnkerHref = "href[^=^>]*=[^\"^>]*" + tagAnkerAttributeValue;
                    const String tagAnkerPrefix = "<a[^>]+";
                    const String tagAnkerSuffix = "[^>]*>";

                    const String tagFrameSrc = "src[^=^>]*=[^\"^>]*" + tagAnkerAttributeValue;
                    const String tagFramePrefix = "<.?frame[^>]+";
                    const String tagFrameSuffix = "[^>]*>";

                    List<String> ankerTokenList = new List<String>();
                    ankerTokenList.AddRange(this.determineMatchingTokensInStrContent(this.crawlerJobContext.page.pageContent, tagAnkerPrefix + tagAnkerHref + tagAnkerSuffix));
                    ankerTokenList.AddRange(this.determineMatchingTokensInStrContent(this.crawlerJobContext.page.pageContent, tagFramePrefix + tagFrameSrc + tagFrameSuffix));

                    foreach (String token in ankerTokenList)
                    {
                        //
                        if (this.isAborting)
                        {
                            break;
                        }

                        //
                        String anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(token, tagAnkerHref);
                        if (anyCurrentUrlStr != null)
                        {
                            anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(anyCurrentUrlStr, tagAnkerAttributeValue);
                        }

                        if (anyCurrentUrlStr == null)
                        {
                            anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(token, tagFrameSrc);
                            if (anyCurrentUrlStr != null)
                            {
                                anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(anyCurrentUrlStr, tagAnkerAttributeValue);
                            }
                        }

                        if (anyCurrentUrlStr != null)
                        {
                            anyCurrentUrlStr = Regex.Replace(anyCurrentUrlStr, "^\"|\"$", "");
                        }

                        String normalizedUrlStr = this.determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(this.crawlerJobContext.page.urlStr, anyCurrentUrlStr);

                        if (normalizedUrlStr != null)
                        {
                            if (!this.crawlerJobContext.page.ankerHrefLinkList.Contains(normalizedUrlStr))
                            {
                                this.crawlerJobContext.page.ankerHrefLinkList.Add(normalizedUrlStr);
                            }
                        }
                    }

                    //
                    if (this.progressPercentageChangeEvent != null)
                    {
                        this.progressPercentageChangeEvent(0.5);
                    }

                    //
                    if (this.crawlerJobPartSuspendEvent != null)
                    {
                        this.crawlerJobPartSuspendEvent();
                    }
                }

                //image links
                if (!this.isAborting && !metaTagDoesNotAllowIndexingLinks)
                {
                    //
                    this.crawlerJobContext.page.imageSrcLinkList = new List<String>();

                    //
                    const String tagImageAttributeValue = "\"[^\"]*\"";
                    const String tagImageSrc = "src[^=^>]*=[^\"^>]*" + tagImageAttributeValue;
                    const String tagImageSrcAlternative = "src=[^\\s\"]+";
                    const String tagImagePrefix = "<img[^>]+";
                    const String tagImageSuffix = "[^>]*>";
                    List<String> imageTokenList = this.determineMatchingTokensInStrContent(this.crawlerJobContext.page.pageContent, tagImagePrefix + "(" + tagImageSrc + "|" + tagImageSrcAlternative + ")" + tagImageSuffix);
                    foreach (String token in imageTokenList)
                    {
                        //
                        if (this.isAborting)
                        {
                            break;
                        }

                        //normal syntax
                        {
                            String anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(token, tagImageSrc);
                            anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(anyCurrentUrlStr, tagImageAttributeValue);
                            if (anyCurrentUrlStr != null)
                            {
                                anyCurrentUrlStr = Regex.Replace(anyCurrentUrlStr, "^[^\"]*\"|\"[^\"]*$", "");

                                String normalizedUrlStr = this.determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(this.crawlerJobContext.page.urlStr, anyCurrentUrlStr);

                                if (normalizedUrlStr != null)
                                {
                                    if (!this.crawlerJobContext.page.imageSrcLinkList.Contains(normalizedUrlStr) && (this.crawlerJobContext.imageBacklog == null || !this.crawlerJobContext.imageBacklog.containsImageSrcUrlStr(normalizedUrlStr)))
                                    {
                                        this.crawlerJobContext.page.imageSrcLinkList.Add(normalizedUrlStr);
                                    }
                                }
                            }
                        }

                        //alternative (wrong but occurring) syntax
                        {
                            String anyCurrentUrlStr = this.determineFirstMatchingTokenInStrContent(token, tagImageSrcAlternative);
                            if (anyCurrentUrlStr != null)
                            {
                                anyCurrentUrlStr = Regex.Replace(anyCurrentUrlStr, "^src=", "");
                                String normalizedUrlStr = this.determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(this.crawlerJobContext.page.urlStr, anyCurrentUrlStr);

                                if (normalizedUrlStr != null)
                                {
                                    if (!this.crawlerJobContext.page.imageSrcLinkList.Contains(normalizedUrlStr) && (this.crawlerJobContext.imageBacklog == null || !this.crawlerJobContext.imageBacklog.containsImageSrcUrlStr(normalizedUrlStr)))
                                    {
                                        this.crawlerJobContext.page.imageSrcLinkList.Add(normalizedUrlStr);
                                    }
                                }
                            }
                        }
                    }

                    //try to get every image related url link
                    if (this.crawlerJobContext.crawlerImageFilter == null || !this.crawlerJobContext.crawlerImageFilter.filterFreeFloatingImageUrls)
                    {
                        const String tagFloatingImageSrc = "[^\\s^=^\"^']*(jp.?g|gif|png|bmp)";
                        List<String> floatingImageTokenList = this.determineMatchingTokensInStrContent(this.crawlerJobContext.page.pageContent, tagFloatingImageSrc);
                        foreach (String token in floatingImageTokenList)
                        {
                            //
                            if (this.isAborting)
                            {
                                break;
                            }

                            //alternative (wrong but occurring) syntax
                            {
                                String anyCurrentUrlStr = token;
                                if (anyCurrentUrlStr != null)
                                {
                                    anyCurrentUrlStr = Regex.Replace(anyCurrentUrlStr, "src|[=]|[\"']", "");
                                    String normalizedUrlStr = this.determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(this.crawlerJobContext.page.urlStr, anyCurrentUrlStr);

                                    if (normalizedUrlStr != null)
                                    {
                                        if (!this.crawlerJobContext.page.imageSrcLinkList.Contains(normalizedUrlStr) && (this.crawlerJobContext.imageBacklog == null || !this.crawlerJobContext.imageBacklog.containsImageSrcUrlStr(normalizedUrlStr)))
                                        {
                                            this.crawlerJobContext.page.imageSrcLinkList.Add(normalizedUrlStr);
                                        }
                                    }
                                }
                            }
                        }
                    }


                    //
                    if (this.crawlerJobContext.page.ankerHrefLinkList != null && this.crawlerJobContext.page.ankerHrefLinkList.Count > 0)
                    {
                        crawlerJobPartList.Add(new CrawlerJobPartGenerateSubJobs());
                    }

                    //
                    if (this.crawlerJobContext.page.imageSrcLinkList != null && this.crawlerJobContext.page.imageSrcLinkList.Count > 0)
                    {
                        crawlerJobPartList.Add(new CrawlerJobPartRetrieveImages());
                    }
                }

                //
                this.crawlerJobContext.page.pageContent = "";
            }

            //
            if (this.progressPercentageChangeEvent != null)
            {
                this.progressPercentageChangeEvent(1.0);
            }

            //
            return crawlerJobPartList;
        }

        public void doAbort()
        {
            this.isAborting = true;
        }

        private List<String> determineMatchingTokensInStrContent(String content, String matcherExpression)
        {
            //
            List<String> tokenList = new List<String>();

            //
            if (content != null)
            {
                //
                Regex ankerRegex = new Regex(matcherExpression);
                MatchCollection matchCollection = ankerRegex.Matches(content);
                if (matchCollection != null)
                {
                    foreach (Match match in matchCollection)
                    {
                        tokenList.Add(match.Value);
                    }
                }
            }

            //
            return tokenList;
        }

        private String determineFirstMatchingTokenInStrContent(String content, String matcherExpression)
        {
            //
            String retval = null;

            //
            List<String> tokenList = this.determineMatchingTokensInStrContent(content,matcherExpression);
            if (tokenList.Count > 0)
            {
                retval = tokenList[0];
            }

            //
            return retval;
        }

        private String determineNormalizedToAbsoluteUrlStrFromAnyUrlStringAndParentPageUrlStr(String parentUrlStr,String  anyCurrentUrlStr)
        {
            //
            String retval = null;

            //
            if (parentUrlStr != null && anyCurrentUrlStr != null && anyCurrentUrlStr != "")
            {
                try
                {
                    Uri parentUri = new Uri(parentUrlStr);
                    Uri absoluteUri = new Uri(parentUri, anyCurrentUrlStr);
                    retval = absoluteUri.AbsoluteUri;
                }
                catch (Exception e)
                {
                }
            }

            //
            return retval;
        }

        public CrawlerJobProcessState getProcessState()
        {
            return CrawlerJobProcessState.Analyze;
        }

        #endregion
    }
}
