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
using ImageCrawler.Crawler.Common;

namespace ImageCrawler.Crawler.Backlog
{
    /// <summary>
    /// Simple analyzer implementation which uses a tripple n-gram analyzer.
    /// </summary>
    public class PageBacklogOptimizerAnalyzerAndClassifierTrippleGram : IPageBacklogOptimizerAnalyzerAndClassifier
    {
        private int tokenNumberMax = 1000;
        private int defaultPagePriority = 5;

        private double averageSuccessCount = 0;
        private int successCountMomentum = 20;

        private SortedDictionary<String, double> tokenToAverageSuccessCountDictionary = new SortedDictionary<string, double>();

        #region IAnalyzerAndClassifier Members

        public void analyzePage(Page page)
        {
            //
            if (page != null && page.urlStr != null)
            {
                //extract all token combination with length of three characters. -> n-gram analyzer
                List<String> tokenList = this.extractTokenListFromPage(page);

                //
                double averageImageAreaPerImage;
                lock (page)
                {
                    averageImageAreaPerImage = page.imageFoundAreaSummation / Math.Max(1.0, page.imageFoundCounter);
                }
                foreach (String token in tokenList)
                {
                    double successCount = this.averageSuccessCount;
                    this.tokenToAverageSuccessCountDictionary.TryGetValue(token, out successCount);

                    //make an approximated average calculation
                    successCount = ((this.successCountMomentum - 1) * successCount + averageImageAreaPerImage) / (1.0 * this.successCountMomentum);

                    this.tokenToAverageSuccessCountDictionary.Remove(token);
                    this.tokenToAverageSuccessCountDictionary.Add(token, successCount);
                }
            }
        }

        public void classifyPage(Page page)
        {
            //
            if (page != null && page.urlStr != null)
            {
                //extract all token combination with length of three characters. -> n-gram analyzer
                List<String> tokenList = this.extractTokenListFromPage(page);

                //
                double successFactor = 1.0;
                foreach (String token in tokenList)
                {
                    double successCount = 0;
                    Boolean found = this.tokenToAverageSuccessCountDictionary.TryGetValue(token, out successCount);
                    if (found)
                    {
                        successFactor *= successCount / Math.Max(1.0, this.averageSuccessCount);
                    }
                }

                //
                int priority = (int)Math.Round((this.defaultPagePriority + 9 * page.priority) / 10.0 * successFactor);
                page.priority = priority;
            }
        }

        public void cleanupStatistics()
        {
            //
            this.averageSuccessCount = this.determineAverageSuccessCount();

            //
            if (this.tokenToAverageSuccessCountDictionary.Count() > this.tokenNumberMax)
            {
                //get the least valuable token
                List<KeyValuePair<String, double>> tokenAverageSuccessCountList = this.tokenToAverageSuccessCountDictionary.ToList<KeyValuePair<String, double>>();
                tokenAverageSuccessCountList.Sort(new TokenAverageSuccessCountComparer());

                while (this.tokenToAverageSuccessCountDictionary.Count() > this.tokenNumberMax)
                {
                    //
                    KeyValuePair<String, double> leastValuableTokenAndSuccessorCountPair = tokenAverageSuccessCountList.First();
                    tokenAverageSuccessCountList.Remove(leastValuableTokenAndSuccessorCountPair);

                    //remove it from the dictionaries
                    this.tokenToAverageSuccessCountDictionary.Remove(leastValuableTokenAndSuccessorCountPair.Key);
                }
            }
        }

        private double determineAverageSuccessCount()
        {
            //
            double retval = 0;

            //
            int size = this.tokenToAverageSuccessCountDictionary.Count();
            double successCountSum = 0;
            foreach (double successCount in this.tokenToAverageSuccessCountDictionary.Values)
            {
                successCountSum += successCount;
            }

            //
            retval = successCountSum / Math.Max(1.0, size);

            //
            return retval;
        }

        private List<String> extractTokenListFromPage(Page page)
        {
            //
            List<String> tokenList = new List<String>();

            //
            if (page != null && page.urlStr != null)
            {
                //
                String urlStr = page.urlStr;

                //extract all token combination with length of three characters. -> n-gram analyzer
                for (int ii = 1; ii <= 3; ii++)
                {
                    Match match = Regex.Match(urlStr, "[a-zA-Z]{3,3}");
                    while (match.Success)
                    {
                        //
                        String token = match.Value;
                        tokenList.Add(token);

                        //
                        match = match.NextMatch();
                    }

                    //
                    urlStr = urlStr.Substring(1);
                }
            }

            //
            return tokenList;
        }

        private class TokenAverageSuccessCountComparer : IComparer<KeyValuePair<String, double>>
        {
            public int Compare(KeyValuePair<string, double> x, KeyValuePair<string, double> y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        #endregion
    }
}
