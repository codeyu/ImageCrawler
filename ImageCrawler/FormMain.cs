using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Resources;
using ImageCrawler.Crawler.Controller;
using ImageCrawler.Crawler.Common;
using ImageCrawler.Crawler.View.GUI;
using ImageCrawler.Crawler.Job;

namespace ImageCrawler
{
    public partial class FormMain : Form
    {
        protected ICrawlerController crawlerController = null;
        protected List<CrawlerImage> collectingCrawlerImageList = new List<CrawlerImage>();
        protected List<CrawlerImage> shownCrawlerImageList = new List<CrawlerImage>();

        //drag & drop
        private String lastDraggedFilename = null;
        private long lastDraggedFileCounter = 0;

        /// <summary>
        /// start pause stop state control
        /// </summary>
        private CrawlerButtonStateController crawlerButtonStateController = new CrawlerButtonStateController();

        /// <summary>
        /// picture detail
        /// </summary>
        private DialogImageDetailView dialogImageDetailView = new DialogImageDetailView();

        /// <summary>
        /// folder for image log
        /// </summary>
        private String imageLoggingFolder = null;

        /// <summary>
        /// Used to determine, if the image list view should be updated. Contains the last update time.
        /// </summary>
        private System.DateTime timeOfLastImageListViewUpdate = System.DateTime.Now;
        private string paypalDonationTempHtmlFilename = Application.UserAppDataPath + "\\image_crawler_paypal_donation.html";

        public FormMain()
        {
            //
            InitializeComponent();
            this.initializeListView();
            this.initializeCrawlerButtonStateController();
            this.initializeTooltip();

            //
            this.FormClosed += new FormClosedEventHandler(delegate(object sender, FormClosedEventArgs args)
            {
                //
                if (!this.crawlerButtonStateController.isStopped() && this.crawlerController != null)
                {
                    this.crawlerController.stopCrawling();
                }

                //
                try
                {
                    FileInfo fileInfo = new FileInfo(this.paypalDonationTempHtmlFilename);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
                catch (Exception e) { }
            });
        }

        private void initializeTooltip()
        {
            this.toolTip.SetToolTip(this.imageListView, "To save a picture drag and drop it to a windows explorer window.\nTo get a larger preview doubleclick the picture.");
            this.toolTip.SetToolTip(this.buttonImageLogFolder, "If a directory is set with this button, \nall incoming images are written into this directory as files.\nBe aware, that the directory has to be set before the crawling starts!");

            const String minimumAreaTooltip = "Sets the area an image have to have at least. Area means height multiplicated with width.\nIts possible to give a simple number like 10000 or use the 100x100 syntax. Important is only the area, not the width or height.";
            this.toolTip.SetToolTip(this.labelMinimumArea, minimumAreaTooltip);
            this.toolTip.SetToolTip(this.comboBoxMinimumArea, minimumAreaTooltip);

            const String minimumFilesizeTooltip = "Sets the minimum size a file has to have. Allows to prevent loading of some images, if the server allows their size to be estimated in advance.";
            this.toolTip.SetToolTip(this.labelMinimumFilesize, minimumFilesizeTooltip);
            this.toolTip.SetToolTip(this.comboBoxMinimumFilesize, minimumFilesizeTooltip);

            const String filterDummyResponseTooltip =  "Forces two requests on pages which redirect. This allows on some pages to get the intended link and not some dummy link.";
            this.toolTip.SetToolTip(this.labelFilterDummyResponse,filterDummyResponseTooltip);
            this.toolTip.SetToolTip(this.checkBoxFilterDummyResponse,filterDummyResponseTooltip);

            const String onlyAnkerImagesTooltip = "Only retrieves images included within the page as ordinary 'links' and not included as images.";
            this.toolTip.SetToolTip(this.labelOnlyAnkerImages,onlyAnkerImagesTooltip);
            this.toolTip.SetToolTip(this.checkBoxOnlyAnkerImages, onlyAnkerImagesTooltip);

            const String threadCountTooltip = "Sets the number of concurrently working threads.";
            this.toolTip.SetToolTip(this.labelThreadCount, threadCountTooltip);
            this.toolTip.SetToolTip(this.comboBoxThreadCount, threadCountTooltip);

            const String imagesCountTooltip = "Sets the number of shown images. If images are coming in too fast, some images will not be shown to prevent flickering.";
            this.toolTip.SetToolTip(this.labelImagesCount, imagesCountTooltip);
            this.toolTip.SetToolTip(this.comboBoxImageCount, imagesCountTooltip);

            const String filterImagesWithSameSizeTooltip = "Filters all images which have the same size, as an image which was already loaded before.";
            this.toolTip.SetToolTip(this.labelFilterImagesWithSameSize, filterImagesWithSameSizeTooltip);
            this.toolTip.SetToolTip(this.checkBoxFilterImagesWithSameSize, filterImagesWithSameSizeTooltip);

            const String filterFreeFloatingUrlsTooltip = "Filters urls which do not correspond to valid html tags and are floating free within the webpage. (For example within JavaScript)";
            this.toolTip.SetToolTip(this.labelFilterFreeFloatingUrls, filterFreeFloatingUrlsTooltip);
            this.toolTip.SetToolTip(this.checkBoxFilterFreeFloatingUrls, filterFreeFloatingUrlsTooltip);

        }

        private void initializeCrawlerButtonStateController()
        {
            this.crawlerButtonStateController.buttonPlay = this.buttonStartCrawling;
            this.crawlerButtonStateController.buttonPause = this.buttonSuspendCrawling;
            this.crawlerButtonStateController.buttonStop = this.buttonStopCrawling;
            this.crawlerButtonStateController.form = this;
            this.crawlerButtonStateController.initButtonState();
        }

        private CrawlerImage determineInListViewSelectedCrawlerImage()
        {
            lock (this.shownCrawlerImageList)
            {
                //
                CrawlerImage crawlerImage = null;

                //
                System.Windows.Forms.ListView.SelectedIndexCollection indexList = this.imageListView.SelectedIndices;
                if (indexList.Count > 0)
                {
                    int index = indexList[0];
                    if (this.shownCrawlerImageList.Count > index)
                    {
                        crawlerImage = this.shownCrawlerImageList[index];
                    }
                }

                //
                return crawlerImage;
            }
        }

        private String generateImageFilename(CrawlerImage crawlerImage)
        {
            String directoryPathName = Application.UserAppDataPath;
            Boolean fileExistsAlready = true;
            String filename = "";
            while (fileExistsAlready)
            {
                filename = directoryPathName + "\\" + crawlerImage.pageLinkUrlStr.Length + "image" + this.lastDraggedFileCounter++ + "." + crawlerImage.type;
                fileExistsAlready = new FileInfo(filename).Exists;
            }
            return filename;
        }

        

        /// <summary>
        /// Parses the given panel information and creates a filter object, which can be eased by the controller
        /// </summary>
        /// <returns></returns>
        private CrawlerImageFilter generateCrawlerImageFilterFromFilterPanel()
        {
            CrawlerImageFilter crawlerImageFilter = new CrawlerImageFilter();
            if (crawlerImageFilter != null)
            {
                crawlerImageFilter.minimumWidth = this.defaultIntValue(this.comboBoxMinimumWidth.Text, 10);
                crawlerImageFilter.minimumHeight = this.defaultIntValue(this.comboBoxMinimumHeight.Text, 10);
                crawlerImageFilter.minimumArea = this.calculatedDefaultIntValue(this.comboBoxMinimumArea.Text, 10);
                crawlerImageFilter.minimumFilesize = this.calculatedDefaultIntValue(this.comboBoxMinimumFilesize.Text, 10000);
                crawlerImageFilter.onlyAnkerLinkedImages = this.checkBoxOnlyAnkerImages.Checked;
                crawlerImageFilter.filterDummyRequest = this.checkBoxFilterDummyResponse.Checked;
                crawlerImageFilter.filterImagesWithSameSize = this.checkBoxFilterImagesWithSameSize.Checked;
                crawlerImageFilter.filterFreeFloatingImageUrls = this.checkBoxFilterFreeFloatingUrls.Checked;
            }
            return crawlerImageFilter;
        }

        private int calculatedDefaultIntValue(String calculatableIntValue, int defaultValue)
        {
            //
            int retval = defaultValue;

            //
            if (calculatableIntValue != null && calculatableIntValue != "")
            {
                //replace of kb and mb
                calculatableIntValue = Regex.Replace(calculatableIntValue, "k.", "000");
                calculatableIntValue = Regex.Replace(calculatableIntValue, "m.", "000000");
                calculatableIntValue = Regex.Replace(calculatableIntValue, "\\s", "");

                //
                if (calculatableIntValue.Contains('x'))
                {
                    String[] tokens = calculatableIntValue.Split('x');
                    if (tokens.Length == 2)
                    {
                        retval = this.defaultIntValue(tokens[0], defaultValue) * this.defaultIntValue(tokens[1], 1);
                    }
                }
                else
                {
                    retval = this.defaultIntValue(calculatableIntValue, defaultValue);
                }
            }

            //
            return retval;
        }

        /// <summary>
        /// Tries to convert the given string into an integer value. If it fails, it returns the default value.
        /// </summary>
        /// <param name="intValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private int defaultIntValue(String intValue, int defaultValue)
        {
            //
            int retval = defaultValue;

            //
            try
            {
                int.TryParse(intValue, out retval);
            }
            catch (Exception e) { }

            //
            return retval;
        }

        private void initializeListView()
        {
            this.imageListView.ItemDrag += new ItemDragEventHandler(delegate(object sender, ItemDragEventArgs args)
            {
                lock (this.collectingCrawlerImageList)
                {
                    //
                    CrawlerImage crawlerImage = this.determineInListViewSelectedCrawlerImage();
                    if (crawlerImage != null)
                    {
                        //
                        String filename = this.generateImageFilename(crawlerImage);
                        crawlerImage.image.Save(filename);

                        //
                        DataObject dataObject = new DataObject(DataFormats.FileDrop, new String[] { filename }); 
                        this.imageListView.DoDragDrop(dataObject, DragDropEffects.All);

                        this.lastDraggedFilename = filename;
                    }
                }
            });

            this.imageListView.DragDrop += new DragEventHandler(delegate(object dragdropsender, DragEventArgs dragdropargs)
            {
                if (this.lastDraggedFilename != null)
                {
                    FileInfo fileInfo = new FileInfo(this.lastDraggedFilename);
                    try
                    {
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                    }
                    catch (Exception e1)
                    {
                    }
                }
            });

        }

        /// <summary>
        /// Manages the behavior if a new image have been loaded.
        /// </summary>
        /// <param name="crawlerImage"></param>
        private void retrieveImageEvent(CrawlerImage crawlerImage)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    //
                    lock (this.collectingCrawlerImageList)
                    {
                        //
                        int maxImagesCount = this.determineMaxImageToViewCount();

                        /*
                         * only refresh the images, if there are enough new images
                         * 
                         * or
                         * 
                         * if there is no picture shown in the gui at all, add the pictures dirctly to the gui
                         */
                        if (this.imageList.Images.Count < maxImagesCount)
                        {
                            //
                            this.shownCrawlerImageList.Add(crawlerImage);

                            //
                            this.imageList.Images.Add(crawlerImage.image);

                            //
                            ListViewItem item = new ListViewItem();
                            item.ImageIndex = this.imageList.Images.Count - 1;
                            item.Text = crawlerImage.srcUrlPath;
                            this.imageListView.Items.Add(item);

                        }
                        else
                        {
                            //
                            this.collectingCrawlerImageList.Add(crawlerImage);

                            //
                            System.DateTime currentTime = System.DateTime.Now;
                            TimeSpan timeDifference = currentTime.Subtract(this.timeOfLastImageListViewUpdate);

                            //if images does come in too fast, throw them away.
                            const int updateDelayInSeconds = 10;
                            if (this.collectingCrawlerImageList.Count >= maxImagesCount && timeDifference.Seconds > updateDelayInSeconds)
                            {
                                //
                                this.timeOfLastImageListViewUpdate = currentTime;

                                //
                                lock (this.collectingCrawlerImageList)
                                {
                                    lock (this.shownCrawlerImageList)
                                    {
                                        //
                                        this.imageListView.Items.Clear();
                                        this.imageList.Images.Clear();
                                        this.shownCrawlerImageList.Clear();

                                        //
                                        this.shownCrawlerImageList.AddRange(this.collectingCrawlerImageList.GetRange(0, maxImagesCount));
                                        this.collectingCrawlerImageList.Clear();

                                        //move all images from the crawler image list to the GUI
                                        int index = 0;
                                        foreach (CrawlerImage iCrawlerImage in this.shownCrawlerImageList)
                                        {
                                            //
                                            this.imageList.Images.Add(iCrawlerImage.image);

                                            //
                                            ListViewItem item = new ListViewItem();
                                            item.ImageIndex = index++;
                                            item.Text = iCrawlerImage.srcUrlPath;
                                            this.imageListView.Items.Add(item);
                                        }
                                    }
                                }
                            }//end if timedifference
                        }
                    }
                }));
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// Interface for CrawlerButtonStateController
        /// </summary>
        private interface IButtonStateAction
        {
            Button buttonPlay { get; set; }
            Button buttonPause { get; set; }
            Button buttonStop { get; set; }
            FormMain form { get; set; }

            IButtonStateAction clickPlay();
            IButtonStateAction clickPause();
            IButtonStateAction clickStop();
        }

        /// <summary>
        /// Manages the play, pause, stop buttons state and properties. Delegate pattern.
        /// </summary>
        private class CrawlerButtonStateController : IButtonStateAction
        {
            public Button buttonPlay { get; set; }
            public Button buttonPause { get; set; }
            public Button buttonStop { get; set; }
            public FormMain form { get; set; }

            private IButtonStateAction currentButtonStateAction;

            public void initButtonState()
            {
                this.currentButtonStateAction = new ButtonStateActionStopped(this);
            }

            public IButtonStateAction clickPlay()
            {
                return this.currentButtonStateAction = this.currentButtonStateAction.clickPlay();
            }

            public IButtonStateAction clickPause()
            {
                return this.currentButtonStateAction = this.currentButtonStateAction.clickPause();
            }

            public IButtonStateAction clickStop()
            {
                return this.currentButtonStateAction = this.currentButtonStateAction.clickStop();
            }

            public Boolean isRunning()
            {
                return this.currentButtonStateAction is ButtonStateActionPlaying;
            }

            public Boolean isSuspended()
            {
                return this.currentButtonStateAction is ButtonStateActionSuspended;
            }

            public Boolean isStopped()
            {
                return this.currentButtonStateAction is ButtonStateActionStopped;
            }

            private class ButtonStateActionStopped : IButtonStateAction
            {
                #region IButtonStateAction Members

                public Button buttonPlay { get; set; }
                public Button buttonPause { get; set; }
                public Button buttonStop { get; set; }
                public FormMain form { get; set; }

                public ButtonStateActionStopped(IButtonStateAction buttonStateAction)
                {
                    //
                    this.buttonPlay = buttonStateAction.buttonPlay;
                    this.buttonPause = buttonStateAction.buttonPause;
                    this.buttonStop = buttonStateAction.buttonStop;
                    this.form = buttonStateAction.form;

                    //
                    this.buttonPlay.Enabled = true;
                    this.buttonPause.Enabled = false;
                    this.buttonStop.Enabled = false;
                    this.form.buttonImageFilter.Enabled = true;
                    this.form.comboBoxUrl.Enabled = true;
                    this.form.checkBoxIncludeExternalPages.Enabled = true;
                    this.form.buttonImageLogFolder.Enabled = true;
                }

                public IButtonStateAction clickPlay()
                {
                    //
                    if (this.form.crawlerController != null && this.form.crawlerController.getCrawlerJobView().isCrawlingProcessActive())
                    {
                        this.form.crawlerController.stopCrawling();
                    }

                    //
                    this.form.clearFlowLayoutPanelProcessStatus();
                    this.form.crawlerController = new CrawlerController();
                    ICrawlerJobView crawlerJobView = this.form.crawlerController.getCrawlerJobView();
                    this.form.crawlerController.setCrawlerImageFilter(this.form.generateCrawlerImageFilterFromFilterPanel());
                    this.form.synchronizeComboBoxThreadCountToCrawlerController();

                    //
                    this.form.imageListView.Clear();
                    this.form.imageList.Images.Clear();
                    lock (this.form.collectingCrawlerImageList)
                    {
                        this.form.collectingCrawlerImageList.Clear();
                        this.form.shownCrawlerImageList.Clear();
                    }

                    //
                    this.form.synchronizeImageLoggerFolder();

                    //
                    this.form.crawlerController.crawlerControllerFinishedWorkingEvent += new CrawlerControllerFinishedWorkingEvent(delegate()
                    {
                        try
                        {
                            if (this.form.crawlerButtonStateController.isStopped())
                            {
                                this.form.Invoke(new MethodInvoker(delegate()
                                {
                                    this.form.imageList.Images.Clear();
                                    this.form.imageListView.Clear();
                                }));
                            }

                            this.form.Invoke(new MethodInvoker(delegate()
                            {
                                this.form.crawlerButtonStateController.currentButtonStateAction = new ButtonStateActionStopped(this);
                            }));
                        }
                        catch (Exception e) { }
                    });

                    //
                    if (crawlerJobView != null)
                    {
                        crawlerJobView.crawlerJobViewUpdateEventAddedNewProcessInformation += new CrawlerJobViewUpdateEventAddedNewProcessInformation(delegate(CrawlerProcessInformation crawlerProcessInformation)
                        {
                            try
                            {
                                if (crawlerJobView.isCrawlingProcessActive())
                                {
                                    this.form.Invoke(new MethodInvoker(delegate()
                                    {
                                        Panel panel = new ProcessInformationPanel(crawlerProcessInformation);
                                        this.form.panelCrawlerStatus.Controls.Add(panel);
                                    }));
                                }
                            }
                            catch (Exception e) { }
                        });
                        crawlerJobView.updateProcessInformationEvent += new CrawlerJobViewUpdateEventProcessInformation(delegate(CrawlerProcessInformation crawlerProcessInformation)
                        {
                            try
                            {
                                if (crawlerJobView.isCrawlingProcessActive())
                                {
                                    //
                                    int undonePagesCount = crawlerJobView.getPageBacklogUndonePagesCount();
                                    int allPagesCount = crawlerJobView.getPageBacklogAllPagesCount();
                                    int runningJobsCount = Math.Max(0, Math.Min(allPagesCount - undonePagesCount,  crawlerJobView.getCurrentlyRunningWorkerJobCount()));
                                    int progressValue = ((allPagesCount - (undonePagesCount + runningJobsCount)) * 100) / Math.Max(1, allPagesCount);
                                    String backlogStatus = "Remaining pages: " + (undonePagesCount + runningJobsCount) + "/" + allPagesCount;

                                    if (!this.form.labelBacklogStatus.Text.Equals(backlogStatus))
                                    {
                                        //
                                        this.form.Invoke(new MethodInvoker(delegate()
                                        {
                                            //
                                            this.form.labelBacklogStatus.Text = backlogStatus;
                                            this.form.progressBarPageBacklog.Value = progressValue;
                                        }));
                                    }
                                }
                                else
                                {
                                    this.form.Invoke(new MethodInvoker(delegate()
                                    {
                                        this.form.resetPageBacklogStatus();
                                        this.form.clearFlowLayoutPanelProcessStatus();
                                    }));
                                }
                            }
                            catch (Exception e) { }
                        });
                        crawlerJobView.updateImageEvent += new CrawlerJobViewUpdateEventImage(this.form.retrieveImageEvent);
                    }

                    //         
                    if (this.form.checkBoxIncludeExternalPages.Checked)
                    {
                        this.form.crawlerController.addPage(new Page(this.form.comboBoxUrl.Text));
                        this.form.crawlerController.startCrawling(null);
                    }
                    else
                    {
                        this.form.crawlerController.startCrawling(new Page(this.form.comboBoxUrl.Text));
                    }

                    //
                    return new ButtonStateActionPlaying(this);
                }

                public IButtonStateAction clickPause()
                {
                    throw new NotImplementedException();
                }

                public IButtonStateAction clickStop()
                {
                    throw new NotImplementedException();
                }

                #endregion
            }

            private class ButtonStateActionPlaying : IButtonStateAction
            {
                public Button buttonPlay { get; set; }
                public Button buttonPause { get; set; }
                public Button buttonStop { get; set; }
                public FormMain form { get; set; }

                public ButtonStateActionPlaying(IButtonStateAction buttonStateAction)
                {
                    //
                    this.buttonPlay = buttonStateAction.buttonPlay;
                    this.buttonPause = buttonStateAction.buttonPause;
                    this.buttonStop = buttonStateAction.buttonStop;
                    this.form = buttonStateAction.form;

                    //
                    this.buttonPlay.Enabled = false;
                    this.buttonPause.Enabled = true;
                    this.buttonStop.Enabled = true;
                    this.form.buttonImageFilter.Enabled = false;
                    this.form.panelImageFilter.Visible = false;
                    this.form.comboBoxUrl.Enabled = false;
                    this.form.checkBoxIncludeExternalPages.Enabled = false;
                    this.form.buttonImageLogFolder.Enabled = false;
                }


                public IButtonStateAction clickPlay()
                {
                    throw new NotImplementedException();
                }

                public IButtonStateAction clickPause()
                {
                    //                                               
                    if (this.form.crawlerController != null && !this.form.crawlerController.isSuspended())
                    {
                        this.form.crawlerController.suspendCrawling();
                    }

                    //
                    return new ButtonStateActionSuspended(this);
                }

                public IButtonStateAction clickStop()
                {
                    //
                    if (this.form.crawlerController != null)
                    {
                        this.form.crawlerController.stopCrawling();
                    }
                    this.form.clearFlowLayoutPanelProcessStatus();
                    this.form.resetPageBacklogStatus();

                    //
                    return new ButtonStateActionStopped(this);
                }

            }

            private class ButtonStateActionSuspended : IButtonStateAction
            {
                public Button buttonPlay { get; set; }
                public Button buttonPause { get; set; }
                public Button buttonStop { get; set; }
                public FormMain form { get; set; }

                public ButtonStateActionSuspended(IButtonStateAction buttonStateAction)
                {
                    //
                    this.buttonPlay = buttonStateAction.buttonPlay;
                    this.buttonPause = buttonStateAction.buttonPause;
                    this.buttonStop = buttonStateAction.buttonStop;
                    this.form = buttonStateAction.form;

                    //
                    this.buttonPlay.Enabled = true;
                    this.buttonPause.Enabled = false;
                    this.buttonStop.Enabled = true;
                    this.form.buttonImageFilter.Enabled = false;
                    this.form.comboBoxUrl.Enabled = false;
                    this.form.checkBoxIncludeExternalPages.Enabled = false;
                    this.form.buttonImageLogFolder.Enabled = true;
                }

                public IButtonStateAction clickPlay()
                {
                    //
                    if (this.form.crawlerController.isSuspended())
                    {
                        this.form.crawlerController.resumeCrawling();
                    }

                    //
                    return new ButtonStateActionPlaying(this);
                }

                public IButtonStateAction clickPause()
                {
                    throw new NotImplementedException();
                }

                public IButtonStateAction clickStop()
                {
                    //
                    if (this.form.crawlerController != null)
                    {
                        this.form.crawlerController.stopCrawling();
                        this.form.crawlerController.clear();
                    }
                    this.form.clearFlowLayoutPanelProcessStatus();
                    this.form.resetPageBacklogStatus();

                    //
                    return new ButtonStateActionStopped(this);
                }
            }
        }

        private int determineMaxImageToViewCount()
        {
            //
            int maxImagesCount = 5;
            int maxImagesCountUpperLimit = 200;
            int maxImagesCountLowerLimit = 1;

            //limit the images count given by the user
            try
            {
                maxImagesCount = int.Parse(this.comboBoxImageCount.Text);
            }
            catch (Exception e)
            { }
            maxImagesCount = Math.Max(maxImagesCountLowerLimit, Math.Min(maxImagesCount, maxImagesCountUpperLimit));

            //
            return maxImagesCount;
        }

        private void clearFlowLayoutPanelProcessStatus()
        {
            this.panelCrawlerStatus.Controls.Clear();
        }

        private class ProcessInformationPanel : FlowLayoutPanel
        {
            protected ProgressBar progressBar = new ProgressBar();
            protected Label labelProcessState = new Label();
            protected LinkLabel labelUrl = new LinkLabel();
            protected Label labelStartTimestamp = new Label();
            protected Label labelRunningTimestamp = new Label();

            private System.DateTime lastUpdateTime = System.DateTime.Now.Subtract(TimeSpan.FromMilliseconds(500));

            public ProcessInformationPanel(CrawlerProcessInformation crawlerProcessInformation)
            {
                //
                this.Controls.Add(this.labelStartTimestamp);
                this.Controls.Add(this.labelRunningTimestamp);
                this.Controls.Add(this.labelProcessState);
                this.Controls.Add(this.labelUrl);
                this.Controls.Add(this.progressBar);

                //
                this.labelStartTimestamp.Text = "";
                this.labelStartTimestamp.AutoSize = true;
                this.labelStartTimestamp.Padding = new Padding(2);

                //
                this.labelRunningTimestamp.Text = "";
                this.labelRunningTimestamp.AutoSize = true;
                this.labelRunningTimestamp.Padding = new Padding(2);

                //
                this.labelUrl.Text = "";
                this.labelUrl.AutoSize = true;
                this.labelUrl.Padding = new Padding(2);
                this.labelUrl.LinkClicked += new LinkLabelLinkClickedEventHandler(delegate(object sender, LinkLabelLinkClickedEventArgs args)
                {
                    System.Diagnostics.Process.Start(this.labelUrl.Text);
                    this.labelUrl.LinkVisited = true;
                });

                this.labelProcessState.Text = "";
                this.labelProcessState.AutoSize = true;
                this.labelProcessState.Padding = new Padding(2);

                this.progressBar.Height = 12;

                //
                this.BackColor = Color.GhostWhite;
                this.BorderStyle = BorderStyle.FixedSingle;
                this.AutoSize = true;
                this.Dock = DockStyle.Top;
                this.Margin = new Padding(5);

                //
                if (crawlerProcessInformation != null)
                {
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 1;
                    timer.Tick += new EventHandler(delegate(object sender, EventArgs args)
                    {
                        //
                        timer.Interval = 200;

                        //
                        if (crawlerProcessInformation.isActiveProcess())
                        {
                            //time throttle for process update
                            if (System.DateTime.Now.Subtract(this.lastUpdateTime).Milliseconds > 100)
                            {
                                try
                                {
                                    this.Invoke(new MethodInvoker(delegate()
                                    {
                                        //
                                        String startedAt = "Started at: " + crawlerProcessInformation.createdTime.ToLongTimeString();
                                        String runningDuration = "Running: " + String.Format("{0:d3}s", crawlerProcessInformation.runningTime.Seconds);
                                        String pageUrl = crawlerProcessInformation.pageUrlStr;
                                        String processState = Enum.GetName(typeof(CrawlerJobProcessState), crawlerProcessInformation.processState);
                                        int progressValue = (int)Math.Min(100, Math.Max(0, Math.Round(crawlerProcessInformation.progressPercentage * 100)));

                                        //
                                        if (!this.labelStartTimestamp.Text.Equals(startedAt))
                                        {
                                            this.labelStartTimestamp.Text = startedAt;
                                        }
                                        if (!this.labelRunningTimestamp.Text.Equals(runningDuration))
                                        {
                                            this.labelRunningTimestamp.Text = runningDuration;
                                        }
                                        if (!this.labelUrl.Text.Equals(pageUrl))
                                        {
                                            this.labelUrl.Text = pageUrl;
                                        }
                                        if (!this.labelProcessState.Text.Equals(processState))
                                        {
                                            this.labelProcessState.Text = processState;
                                        }
                                        if (this.progressBar.Value != progressValue)
                                        {
                                            this.progressBar.Value = progressValue;
                                        }
                                    }));
                                }
                                catch (Exception e) { }

                                //
                                this.lastUpdateTime = System.DateTime.Now;
                            }
                        }
                        else
                        {
                            try
                            {
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    //
                                    if (this.Parent != null)
                                    {
                                        this.Parent.Controls.Remove(this);
                                    }

                                    //
                                    timer.Stop();
                                }));
                            }
                            catch (Exception e) { }
                        }
                    });

                    //
                    timer.Start();
                }
            }
        }

        private void buttonStopCrawling_Click(object sender, EventArgs e)
        {
            this.crawlerButtonStateController.clickStop();
        }

        private void buttonStartCrawling_Click(object sender, EventArgs e)
        {
            this.crawlerButtonStateController.clickPlay();
        }

        private void resetPageBacklogStatus()
        {
            this.labelBacklogStatus.Text = "Remaining pages: 0/0";
            this.progressBarPageBacklog.Value = 0;
        }

        private void buttonSuspendCrawling_Click(object sender, EventArgs e)
        {
            this.crawlerButtonStateController.clickPause();
        }

        private void imageListView_DoubleClick(object sender, EventArgs e)
        {
            CrawlerImage crawlerImage = this.determineInListViewSelectedCrawlerImage();
            if (crawlerImage != null && crawlerImage.image != null)
            {
                try
                {
                    this.dialogImageDetailView.setImage(crawlerImage.image, this.generateImageFilename(crawlerImage), crawlerImage.pageLinkUrlStr);
                    this.dialogImageDetailView.ShowDialog();
                }
                catch (Exception e1) { }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.buttonStartCrawling_Click(sender, null);
            }
        }

        private void buttonImageLogFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialogImageLogDirectory.ShowDialog();
            this.imageLoggingFolder = this.folderBrowserDialogImageLogDirectory.SelectedPath;
            this.synchronizeImageLoggerFolder();
        }

        private void synchronizeImageLoggerFolder()
        {
            if (this.crawlerController != null)
            {
                this.crawlerController.setImageLoggingFolder(this.imageLoggingFolder);
            }
        }

        private void synchronizeComboBoxThreadCountToCrawlerController()
        {
            if (this.crawlerController != null)
            {
                const int maxThreadCount = 50;
                const int defaultThreadCount = 10;
                this.crawlerController.setMaximumCrawlerJobThreadCount(Math.Min(maxThreadCount, this.defaultIntValue(this.comboBoxThreadCount.Text, defaultThreadCount)));
            }
        }

        private void comboBoxThreadCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.synchronizeComboBoxThreadCountToCrawlerController();
        }
        

        private void buttonImageFilter_Click(object sender, EventArgs e)
        {
            this.panelImageFilter.Visible = !this.panelImageFilter.Visible;
        }

        private void buttonDonate_Click(object sender, EventArgs e)
        {
            //generate donation html file from resource
            FileInfo fileInfo = new FileInfo(this.paypalDonationTempHtmlFilename);
            try
            {
                String htmlPageContent = global::ImageCrawler.Properties.Resources.paypal_donation;
                if (!fileInfo.Exists)
                {
                    using (StreamWriter writer = File.CreateText(fileInfo.FullName))
                    {
                        writer.WriteLine(htmlPageContent);
                    }
                }
            }
            catch (Exception e1) { }

            //
            System.Diagnostics.Process.Start(fileInfo.FullName);
        }

        private void linkLabelCopyrightAndLicence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                new AboutBoxImageCrawler().Show();
            }
            catch (Exception e1) { }
        }

        
    }
}
