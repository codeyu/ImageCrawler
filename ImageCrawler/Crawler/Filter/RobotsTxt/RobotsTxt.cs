using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ImageCrawler.Crawler.Filter.RobotsTxt
{
    /// <summary>
    /// Representation of the robots.txt file found on many webpages to determine which parts of the web presence is allowed to be crawled.
    /// </summary>
    public class RobotsTxt
    {
        /// <summary>
        /// After a robots.txt content has been parsed, this dictionary holds information about disallowed url pathes for different bots (user agents)
        /// </summary>
        public Dictionary<String, List<String>> botnameToDisallowedUrlsDictionary { get; set; }

        /// <summary>
        /// Creates an object where the given robots txt content is parsed.
        /// </summary>
        /// <param name="robotsTxtContent"></param>
        public RobotsTxt(String robotsTxtContent)
        {
            this.botnameToDisallowedUrlsDictionary = new Dictionary<String, List<String>>();
            this.parseRobotsTxtContent(robotsTxtContent);
        }

        /// <summary>
        /// Parses the content of the robots.txt text file.
        /// </summary>
        /// <param name="content"></param>
        public void parseRobotsTxtContent(String robotsTxtContent)
        {
            if (robotsTxtContent != null)
            {
                try
                {
                    using (StringReader reader = new StringReader(robotsTxtContent))
                    {
                        //
                        String currentBotAgentName = "*";

                        //
                        List<String> disallowedUrlPathList = new List<String>();
                        String line = null;
                        while ((line = reader.ReadLine()) != null)
                        {
                            //
                            Match match;
                            match = Regex.Match(line, "(?i)(User-agent:)([^#]*)(#*.*)");
                            if (match.Success && match.Groups.Count >= 3)
                            {
                                //
                                this.addDisallowedUrlPathListToDictionary(currentBotAgentName, disallowedUrlPathList);

                                //
                                currentBotAgentName = match.Groups[2].Value;
                                currentBotAgentName = (currentBotAgentName != null) ? currentBotAgentName.Trim() : null;

                                //
                                disallowedUrlPathList.Clear();
                            }

                            //
                            match = Regex.Match(line, "(?i)(Disallow:)([^#]*)(#*.*)");
                            if (match.Success && match.Groups.Count >= 3)
                            {
                                //
                                String disallowPath = match.Groups[2].Value;

                                if (disallowPath != null)
                                {
                                    disallowPath = disallowPath.Trim();
                                    if (disallowPath != "")
                                    {
                                        disallowedUrlPathList.Add(disallowPath);
                                    }
                                }
                            }
                        }
                        
                        //
                        this.addDisallowedUrlPathListToDictionary(currentBotAgentName, disallowedUrlPathList);
                    }
                }
                catch (Exception e) { }
            }
        }


        private void addDisallowedUrlPathListToDictionary(String botname, List<String> disallowedUrlPathList)
        {
            //
            if (botname != null && disallowedUrlPathList != null)
            {
                //
                List<String> currentDisallowedUrlPathList = null;

                //
                this.botnameToDisallowedUrlsDictionary.TryGetValue(botname, out currentDisallowedUrlPathList);
                if (currentDisallowedUrlPathList == null)
                {
                    currentDisallowedUrlPathList = new List<String>();
                    this.botnameToDisallowedUrlsDictionary.Add(botname, currentDisallowedUrlPathList);
                }

                //
                foreach (String disallowedUrlPath in disallowedUrlPathList)
                {
                    if (!currentDisallowedUrlPathList.Contains(disallowedUrlPath))
                    {
                        currentDisallowedUrlPathList.Add(disallowedUrlPath);
                    }
                }
            }
        }
    }
}
