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
using ImageCrawler;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Common;

namespace ImageCrawlerConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] != null && new DirectoryInfo(args[0]).Exists)
            {
                //
                ICrawlerController crawlerController = new CrawlerController();
                crawlerController.setImageLoggingFolder(args[0]);

                //
                crawlerController.startCrawling(new Page(args[1]));

                //
                Console.WriteLine("Crawling process is running. Press any key to stop crawling...");
                Console.ReadLine();

                //
                crawlerController.stopCrawling();
            }
            else
            {
                Program.renderHelp();
            }
        }

        private static void renderHelp()
        {
            Console.WriteLine("####################################################################");
            Console.WriteLine("#                   Image Crawler Console Application              #");
            Console.WriteLine("####################################################################");
            Console.WriteLine("#                                                                  #");
            Console.WriteLine("#  This application crawls website images and writes the pictures  #");
            Console.WriteLine("#  into a given folder.                                            #");
            Console.WriteLine("#                                                                  #");
            Console.WriteLine("#  Use as following:                                               #");
            Console.WriteLine("#    ImageCrawlerConsole [existing log folder path] [webpage url]  #");
            Console.WriteLine("#                                                                  #");
            Console.WriteLine("####################################################################");

            Console.WriteLine("Press any key too quit...");
            Console.ReadLine();
        }
    }
}
