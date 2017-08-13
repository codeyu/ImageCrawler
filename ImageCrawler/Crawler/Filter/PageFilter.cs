using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace ImageCrawler.Crawler.Filter
{
   public class PageFilter:IPageFilter
    {
       /// <summary>
       /// Holds 
       /// </summary>
       protected Dictionary<String, HostContext> hostStringToHostContextDicitonary = new Dictionary<String, HostContext>();

        #region IPageFilter Members

        public Boolean checkForValidPage(Common.Page page)
        {
            //
            Boolean retval = true;

            //
            try
            {
                Uri pageUri = new Uri(page.urlStr);
                String hostStr = pageUri.Host;
                if (hostStr != null)
                {
                    //
                    HostContext hostContext = null;

                    //
                    lock (this.hostStringToHostContextDicitonary)
                    {
                        //                    
                        if (!this.hostStringToHostContextDicitonary.ContainsKey(hostStr))
                        {
                            HostContext generatedHostContext = this.generateHostContextForPageUri(pageUri);
                            if (generatedHostContext != null)
                            {
                                this.hostStringToHostContextDicitonary.Add(hostStr, generatedHostContext);
                            }
                        }

                        //
                        this.hostStringToHostContextDicitonary.TryGetValue(hostStr, out hostContext);
                    }

                    if (hostContext != null)
                    {
                        foreach (Uri disallowedUri in hostContext.disallowedUriList)
                        {                            
                            retval &= !(disallowedUri.IsBaseOf(pageUri) && pageUri.AbsolutePath.StartsWith(disallowedUri.AbsolutePath) );
                            if (!retval)
                            {
                                break;
                            }
                        }                        
                    }
                }
            }
            catch (Exception e) { }

            //
            return retval;
        }

        #endregion

        private HostContext generateHostContextForPageUri(Uri pageUri)
        {
            //
            HostContext hostContext = new HostContext();

            //
            if (pageUri != null)
            {
                //
                RobotsTxt.RobotsTxt robotsTxt = null;

                //
                UriBuilder host = new UriBuilder(pageUri.Scheme, pageUri.Host);

                //make a request to the host to retrieve the robots.txt information file
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    //
                    Uri robotsTxtUri = new Uri(host.Uri, "/robots.txt");

                    request = (HttpWebRequest) HttpWebRequest.Create(robotsTxtUri);
                    request.AllowAutoRedirect = true;
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        String robotsTxtContent = reader.ReadToEnd();
                        robotsTxt = new RobotsTxt.RobotsTxt(robotsTxtContent);
                    }
                }
                catch (Exception e) { }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }

                //
                List<String> disallowedUrlPathList = new List<String>();
                if (robotsTxt != null)
                {
                    //
                    List<String> tempDisallowedUrlPathList = null;

                    //
                    String crawlerName = "ImageCrawler";
                    if (robotsTxt.botnameToDisallowedUrlsDictionary.TryGetValue(crawlerName, out tempDisallowedUrlPathList) && tempDisallowedUrlPathList != null)
                    {
                        disallowedUrlPathList.AddRange(tempDisallowedUrlPathList);
                    }
                    else
                    {
                        crawlerName = "*";
                        if (robotsTxt.botnameToDisallowedUrlsDictionary.TryGetValue(crawlerName, out tempDisallowedUrlPathList) && tempDisallowedUrlPathList != null)
                        {
                            disallowedUrlPathList.AddRange(tempDisallowedUrlPathList);
                        }
                    }                   
                }

                //
                if (disallowedUrlPathList.Count > 0)
                {
                    foreach (String disallowedUrlPath in disallowedUrlPathList)
                    {
                        try
                        {
                            //
                            Uri disallowedUri = new Uri(host.Uri, disallowedUrlPath);

                            //
                            hostContext.disallowedUriList.Add(disallowedUri);
                        }
                        catch (Exception e) { }
                    }
                }
            }

            //
            return hostContext;
        }

        protected class HostContext
        {
            public Uri hostUri { get; set; }
            public List<Uri> disallowedUriList { get; set; }

            public HostContext()
            {
                this.disallowedUriList = new List<Uri>();
            }
        }
    }
}
