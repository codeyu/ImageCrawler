namespace ImageCrawler
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.comboBoxUrl = new System.Windows.Forms.ComboBox();
            this.labelUrl = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxIncludeExternalPages = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialogImageLogDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainerImageViewCrawlerStatus = new System.Windows.Forms.SplitContainer();
            this.panelImageFilter = new System.Windows.Forms.Panel();
            this.labelFilterFreeFloatingUrls = new System.Windows.Forms.Label();
            this.labelFilterImagesWithSameSize = new System.Windows.Forms.Label();
            this.checkBoxFilterFreeFloatingUrls = new System.Windows.Forms.CheckBox();
            this.labelFilterDummyResponse = new System.Windows.Forms.Label();
            this.checkBoxFilterImagesWithSameSize = new System.Windows.Forms.CheckBox();
            this.labelOnlyAnkerImages = new System.Windows.Forms.Label();
            this.checkBoxFilterDummyResponse = new System.Windows.Forms.CheckBox();
            this.checkBoxOnlyAnkerImages = new System.Windows.Forms.CheckBox();
            this.labelMinimumFilesize = new System.Windows.Forms.Label();
            this.labelMinimumArea = new System.Windows.Forms.Label();
            this.labelMinimumHeight = new System.Windows.Forms.Label();
            this.labelMinimumWidth = new System.Windows.Forms.Label();
            this.comboBoxMinimumFilesize = new System.Windows.Forms.ComboBox();
            this.comboBoxMinimumHeight = new System.Windows.Forms.ComboBox();
            this.comboBoxMinimumArea = new System.Windows.Forms.ComboBox();
            this.comboBoxMinimumWidth = new System.Windows.Forms.ComboBox();
            this.imageListView = new System.Windows.Forms.ListView();
            this.panelCrawlerStatus = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPageBacklogStatus = new System.Windows.Forms.Panel();
            this.comboBoxThreadCount = new System.Windows.Forms.ComboBox();
            this.labelThreadCount = new System.Windows.Forms.Label();
            this.labelBacklogStatus = new System.Windows.Forms.Label();
            this.progressBarPageBacklog = new System.Windows.Forms.ProgressBar();
            this.panelImageControl = new System.Windows.Forms.Panel();
            this.buttonImageFilter = new System.Windows.Forms.Button();
            this.comboBoxImageCount = new System.Windows.Forms.ComboBox();
            this.labelImagesCount = new System.Windows.Forms.Label();
            this.buttonDonate = new System.Windows.Forms.Button();
            this.buttonImageLogFolder = new System.Windows.Forms.Button();
            this.buttonStopCrawling = new System.Windows.Forms.Button();
            this.buttonSuspendCrawling = new System.Windows.Forms.Button();
            this.buttonStartCrawling = new System.Windows.Forms.Button();
            this.linkLabelCopyrightAndLicence = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageViewCrawlerStatus)).BeginInit();
            this.splitContainerImageViewCrawlerStatus.Panel1.SuspendLayout();
            this.splitContainerImageViewCrawlerStatus.Panel2.SuspendLayout();
            this.splitContainerImageViewCrawlerStatus.SuspendLayout();
            this.panelImageFilter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPageBacklogStatus.SuspendLayout();
            this.panelImageControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Chrysanthemum.jpg");
            this.imageList.Images.SetKeyName(1, "Desert.jpg");
            this.imageList.Images.SetKeyName(2, "Hydrangeas.jpg");
            // 
            // comboBoxUrl
            // 
            this.comboBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxUrl.FormattingEnabled = true;
            this.comboBoxUrl.Items.AddRange(new object[] {
            "http://www.zeit.de",
            "http://www.ftd.de",
            "http://www.yahoo.de",
            "http://www.bild.de",
            "http://www.web.de",
            "http://www.news.de",
            "http://www.n-tv.de",
            "http://www.kino.de",
            "http://www.flickr.com",
            "http://www.querbilder.de/",
            "http://www.fotosearch.de/",
            "http://www.bildwoerterbuch.com/mensch/anatomie/",
            "http://www.google.de/news",
            "http://de.wikipedia.org/wiki/Albert_Einstein",
            "http://de.wikipedia.org/wiki/Deutschland",
            "http://de.wikipedia.org/wiki/Amerika",
            "http://de.wikipedia.org/wiki/Welt"});
            this.comboBoxUrl.Location = new System.Drawing.Point(49, 21);
            this.comboBoxUrl.Name = "comboBoxUrl";
            this.comboBoxUrl.Size = new System.Drawing.Size(658, 21);
            this.comboBoxUrl.TabIndex = 2;
            this.comboBoxUrl.Text = "http://www.zeit.de";
            this.comboBoxUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxUrl_KeyPress);
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(12, 25);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(29, 13);
            this.labelUrl.TabIndex = 3;
            this.labelUrl.Text = "URL";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 300;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 300;
            this.toolTip.ReshowDelay = 60;
            // 
            // buttonClose
            // 
            this.buttonClose.AutoSize = true;
            this.buttonClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonClose.Location = new System.Drawing.Point(0, 465);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(929, 23);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // checkBoxIncludeExternalPages
            // 
            this.checkBoxIncludeExternalPages.AutoSize = true;
            this.checkBoxIncludeExternalPages.Location = new System.Drawing.Point(49, 48);
            this.checkBoxIncludeExternalPages.Name = "checkBoxIncludeExternalPages";
            this.checkBoxIncludeExternalPages.Size = new System.Drawing.Size(133, 17);
            this.checkBoxIncludeExternalPages.TabIndex = 6;
            this.checkBoxIncludeExternalPages.Text = "Include external pages";
            this.checkBoxIncludeExternalPages.UseVisualStyleBackColor = true;
            // 
            // splitContainerImageViewCrawlerStatus
            // 
            this.splitContainerImageViewCrawlerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerImageViewCrawlerStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainerImageViewCrawlerStatus.Location = new System.Drawing.Point(12, 89);
            this.splitContainerImageViewCrawlerStatus.Name = "splitContainerImageViewCrawlerStatus";
            this.splitContainerImageViewCrawlerStatus.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerImageViewCrawlerStatus.Panel1
            // 
            this.splitContainerImageViewCrawlerStatus.Panel1.Controls.Add(this.panelImageFilter);
            this.splitContainerImageViewCrawlerStatus.Panel1.Controls.Add(this.imageListView);
            this.splitContainerImageViewCrawlerStatus.Panel1MinSize = 210;
            // 
            // splitContainerImageViewCrawlerStatus.Panel2
            // 
            this.splitContainerImageViewCrawlerStatus.Panel2.Controls.Add(this.panelCrawlerStatus);
            this.splitContainerImageViewCrawlerStatus.Panel2.Controls.Add(this.panel1);
            this.splitContainerImageViewCrawlerStatus.Panel2MinSize = 36;
            this.splitContainerImageViewCrawlerStatus.Size = new System.Drawing.Size(905, 370);
            this.splitContainerImageViewCrawlerStatus.SplitterDistance = 225;
            this.splitContainerImageViewCrawlerStatus.SplitterWidth = 10;
            this.splitContainerImageViewCrawlerStatus.TabIndex = 14;
            // 
            // panelImageFilter
            // 
            this.panelImageFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelImageFilter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelImageFilter.Controls.Add(this.labelFilterFreeFloatingUrls);
            this.panelImageFilter.Controls.Add(this.labelFilterImagesWithSameSize);
            this.panelImageFilter.Controls.Add(this.checkBoxFilterFreeFloatingUrls);
            this.panelImageFilter.Controls.Add(this.labelFilterDummyResponse);
            this.panelImageFilter.Controls.Add(this.checkBoxFilterImagesWithSameSize);
            this.panelImageFilter.Controls.Add(this.labelOnlyAnkerImages);
            this.panelImageFilter.Controls.Add(this.checkBoxFilterDummyResponse);
            this.panelImageFilter.Controls.Add(this.checkBoxOnlyAnkerImages);
            this.panelImageFilter.Controls.Add(this.labelMinimumFilesize);
            this.panelImageFilter.Controls.Add(this.labelMinimumArea);
            this.panelImageFilter.Controls.Add(this.labelMinimumHeight);
            this.panelImageFilter.Controls.Add(this.labelMinimumWidth);
            this.panelImageFilter.Controls.Add(this.comboBoxMinimumFilesize);
            this.panelImageFilter.Controls.Add(this.comboBoxMinimumHeight);
            this.panelImageFilter.Controls.Add(this.comboBoxMinimumArea);
            this.panelImageFilter.Controls.Add(this.comboBoxMinimumWidth);
            this.panelImageFilter.Location = new System.Drawing.Point(650, 14);
            this.panelImageFilter.Name = "panelImageFilter";
            this.panelImageFilter.Size = new System.Drawing.Size(253, 209);
            this.panelImageFilter.TabIndex = 15;
            this.panelImageFilter.Visible = false;
            // 
            // labelFilterFreeFloatingUrls
            // 
            this.labelFilterFreeFloatingUrls.AutoSize = true;
            this.labelFilterFreeFloatingUrls.Location = new System.Drawing.Point(6, 186);
            this.labelFilterFreeFloatingUrls.Name = "labelFilterFreeFloatingUrls";
            this.labelFilterFreeFloatingUrls.Size = new System.Drawing.Size(109, 13);
            this.labelFilterFreeFloatingUrls.TabIndex = 5;
            this.labelFilterFreeFloatingUrls.Text = "Filter free floating urls:";
            // 
            // labelFilterImagesWithSameSize
            // 
            this.labelFilterImagesWithSameSize.AutoSize = true;
            this.labelFilterImagesWithSameSize.Location = new System.Drawing.Point(6, 163);
            this.labelFilterImagesWithSameSize.Name = "labelFilterImagesWithSameSize";
            this.labelFilterImagesWithSameSize.Size = new System.Drawing.Size(139, 13);
            this.labelFilterImagesWithSameSize.TabIndex = 5;
            this.labelFilterImagesWithSameSize.Text = "Filter images with same size:";
            // 
            // checkBoxFilterFreeFloatingUrls
            // 
            this.checkBoxFilterFreeFloatingUrls.AutoSize = true;
            this.checkBoxFilterFreeFloatingUrls.Checked = true;
            this.checkBoxFilterFreeFloatingUrls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilterFreeFloatingUrls.Location = new System.Drawing.Point(152, 186);
            this.checkBoxFilterFreeFloatingUrls.Name = "checkBoxFilterFreeFloatingUrls";
            this.checkBoxFilterFreeFloatingUrls.Size = new System.Drawing.Size(15, 14);
            this.checkBoxFilterFreeFloatingUrls.TabIndex = 4;
            this.checkBoxFilterFreeFloatingUrls.UseVisualStyleBackColor = true;
            // 
            // labelFilterDummyResponse
            // 
            this.labelFilterDummyResponse.AutoSize = true;
            this.labelFilterDummyResponse.Location = new System.Drawing.Point(6, 139);
            this.labelFilterDummyResponse.Name = "labelFilterDummyResponse";
            this.labelFilterDummyResponse.Size = new System.Drawing.Size(114, 13);
            this.labelFilterDummyResponse.TabIndex = 5;
            this.labelFilterDummyResponse.Text = "Filter dummy response:";
            // 
            // checkBoxFilterImagesWithSameSize
            // 
            this.checkBoxFilterImagesWithSameSize.AutoSize = true;
            this.checkBoxFilterImagesWithSameSize.Checked = true;
            this.checkBoxFilterImagesWithSameSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilterImagesWithSameSize.Location = new System.Drawing.Point(152, 163);
            this.checkBoxFilterImagesWithSameSize.Name = "checkBoxFilterImagesWithSameSize";
            this.checkBoxFilterImagesWithSameSize.Size = new System.Drawing.Size(15, 14);
            this.checkBoxFilterImagesWithSameSize.TabIndex = 4;
            this.checkBoxFilterImagesWithSameSize.UseVisualStyleBackColor = true;
            // 
            // labelOnlyAnkerImages
            // 
            this.labelOnlyAnkerImages.AutoSize = true;
            this.labelOnlyAnkerImages.Location = new System.Drawing.Point(7, 115);
            this.labelOnlyAnkerImages.Name = "labelOnlyAnkerImages";
            this.labelOnlyAnkerImages.Size = new System.Drawing.Size(127, 13);
            this.labelOnlyAnkerImages.TabIndex = 5;
            this.labelOnlyAnkerImages.Text = "Only direct linked images:";
            // 
            // checkBoxFilterDummyResponse
            // 
            this.checkBoxFilterDummyResponse.AutoSize = true;
            this.checkBoxFilterDummyResponse.Location = new System.Drawing.Point(152, 139);
            this.checkBoxFilterDummyResponse.Name = "checkBoxFilterDummyResponse";
            this.checkBoxFilterDummyResponse.Size = new System.Drawing.Size(15, 14);
            this.checkBoxFilterDummyResponse.TabIndex = 4;
            this.checkBoxFilterDummyResponse.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnlyAnkerImages
            // 
            this.checkBoxOnlyAnkerImages.AutoSize = true;
            this.checkBoxOnlyAnkerImages.Location = new System.Drawing.Point(152, 115);
            this.checkBoxOnlyAnkerImages.Name = "checkBoxOnlyAnkerImages";
            this.checkBoxOnlyAnkerImages.Size = new System.Drawing.Size(15, 14);
            this.checkBoxOnlyAnkerImages.TabIndex = 4;
            this.checkBoxOnlyAnkerImages.UseVisualStyleBackColor = true;
            // 
            // labelMinimumFilesize
            // 
            this.labelMinimumFilesize.AutoSize = true;
            this.labelMinimumFilesize.Location = new System.Drawing.Point(6, 87);
            this.labelMinimumFilesize.Name = "labelMinimumFilesize";
            this.labelMinimumFilesize.Size = new System.Drawing.Size(85, 13);
            this.labelMinimumFilesize.TabIndex = 3;
            this.labelMinimumFilesize.Text = "Minimum filesize:";
            // 
            // labelMinimumArea
            // 
            this.labelMinimumArea.AutoSize = true;
            this.labelMinimumArea.Location = new System.Drawing.Point(6, 60);
            this.labelMinimumArea.Name = "labelMinimumArea";
            this.labelMinimumArea.Size = new System.Drawing.Size(75, 13);
            this.labelMinimumArea.TabIndex = 3;
            this.labelMinimumArea.Text = "Minimum area:";
            // 
            // labelMinimumHeight
            // 
            this.labelMinimumHeight.AutoSize = true;
            this.labelMinimumHeight.Location = new System.Drawing.Point(6, 33);
            this.labelMinimumHeight.Name = "labelMinimumHeight";
            this.labelMinimumHeight.Size = new System.Drawing.Size(83, 13);
            this.labelMinimumHeight.TabIndex = 2;
            this.labelMinimumHeight.Text = "Minimum height:";
            // 
            // labelMinimumWidth
            // 
            this.labelMinimumWidth.AutoSize = true;
            this.labelMinimumWidth.Location = new System.Drawing.Point(6, 6);
            this.labelMinimumWidth.Name = "labelMinimumWidth";
            this.labelMinimumWidth.Size = new System.Drawing.Size(81, 13);
            this.labelMinimumWidth.TabIndex = 1;
            this.labelMinimumWidth.Text = "Mimimum width:";
            // 
            // comboBoxMinimumFilesize
            // 
            this.comboBoxMinimumFilesize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMinimumFilesize.FormattingEnabled = true;
            this.comboBoxMinimumFilesize.Items.AddRange(new object[] {
            "1kb",
            "2kb",
            "3kb",
            "4kb",
            "5kb",
            "10kb",
            "20kb",
            "30kb",
            "40kb",
            "50kb",
            "100kb",
            "200kb",
            "300kb",
            "400kb",
            "500kb",
            "1mb",
            "2mb",
            "3mb",
            "4mb",
            "5mb",
            "10mb"});
            this.comboBoxMinimumFilesize.Location = new System.Drawing.Point(149, 84);
            this.comboBoxMinimumFilesize.Name = "comboBoxMinimumFilesize";
            this.comboBoxMinimumFilesize.Size = new System.Drawing.Size(95, 21);
            this.comboBoxMinimumFilesize.TabIndex = 0;
            this.comboBoxMinimumFilesize.Text = "30kb";
            // 
            // comboBoxMinimumHeight
            // 
            this.comboBoxMinimumHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMinimumHeight.FormattingEnabled = true;
            this.comboBoxMinimumHeight.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "200",
            "300",
            "400",
            "500",
            "1000",
            "2000"});
            this.comboBoxMinimumHeight.Location = new System.Drawing.Point(149, 3);
            this.comboBoxMinimumHeight.Name = "comboBoxMinimumHeight";
            this.comboBoxMinimumHeight.Size = new System.Drawing.Size(95, 21);
            this.comboBoxMinimumHeight.TabIndex = 0;
            this.comboBoxMinimumHeight.Text = "50";
            // 
            // comboBoxMinimumArea
            // 
            this.comboBoxMinimumArea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMinimumArea.FormattingEnabled = true;
            this.comboBoxMinimumArea.Items.AddRange(new object[] {
            "10x10",
            "20x20",
            "30x30",
            "40x40",
            "50x50",
            "100x100",
            "200x200",
            "300x300",
            "400x400",
            "500x500",
            "1000x1000",
            "2000x2000",
            "3000x3000",
            "4000x4000",
            "5000x5000"});
            this.comboBoxMinimumArea.Location = new System.Drawing.Point(149, 57);
            this.comboBoxMinimumArea.Name = "comboBoxMinimumArea";
            this.comboBoxMinimumArea.Size = new System.Drawing.Size(95, 21);
            this.comboBoxMinimumArea.TabIndex = 0;
            this.comboBoxMinimumArea.Text = "10x10";
            // 
            // comboBoxMinimumWidth
            // 
            this.comboBoxMinimumWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMinimumWidth.FormattingEnabled = true;
            this.comboBoxMinimumWidth.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "200",
            "300",
            "400",
            "500",
            "1000",
            "2000"});
            this.comboBoxMinimumWidth.Location = new System.Drawing.Point(149, 30);
            this.comboBoxMinimumWidth.Name = "comboBoxMinimumWidth";
            this.comboBoxMinimumWidth.Size = new System.Drawing.Size(95, 21);
            this.comboBoxMinimumWidth.TabIndex = 0;
            this.comboBoxMinimumWidth.Text = "50";
            // 
            // imageListView
            // 
            this.imageListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.imageListView.AutoArrange = false;
            this.imageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.imageListView.HideSelection = false;
            this.imageListView.LargeImageList = this.imageList;
            this.imageListView.Location = new System.Drawing.Point(0, 0);
            this.imageListView.Name = "imageListView";
            this.imageListView.Size = new System.Drawing.Size(903, 223);
            this.imageListView.TabIndex = 2;
            this.imageListView.UseCompatibleStateImageBehavior = false;
            this.imageListView.DoubleClick += new System.EventHandler(this.imageListView_DoubleClick);
            // 
            // panelCrawlerStatus
            // 
            this.panelCrawlerStatus.AutoScroll = true;
            this.panelCrawlerStatus.AutoSize = true;
            this.panelCrawlerStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelCrawlerStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCrawlerStatus.Location = new System.Drawing.Point(0, 36);
            this.panelCrawlerStatus.Name = "panelCrawlerStatus";
            this.panelCrawlerStatus.Padding = new System.Windows.Forms.Padding(3);
            this.panelCrawlerStatus.Size = new System.Drawing.Size(903, 97);
            this.panelCrawlerStatus.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPageBacklogStatus);
            this.panel1.Controls.Add(this.panelImageControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(903, 36);
            this.panel1.TabIndex = 17;
            // 
            // panelPageBacklogStatus
            // 
            this.panelPageBacklogStatus.Controls.Add(this.comboBoxThreadCount);
            this.panelPageBacklogStatus.Controls.Add(this.labelThreadCount);
            this.panelPageBacklogStatus.Controls.Add(this.labelBacklogStatus);
            this.panelPageBacklogStatus.Controls.Add(this.progressBarPageBacklog);
            this.panelPageBacklogStatus.Location = new System.Drawing.Point(0, 0);
            this.panelPageBacklogStatus.Name = "panelPageBacklogStatus";
            this.panelPageBacklogStatus.Size = new System.Drawing.Size(494, 35);
            this.panelPageBacklogStatus.TabIndex = 17;
            // 
            // comboBoxThreadCount
            // 
            this.comboBoxThreadCount.FormattingEnabled = true;
            this.comboBoxThreadCount.Items.AddRange(new object[] {
            "1",
            "2",
            "5",
            "10",
            "15",
            "20"});
            this.comboBoxThreadCount.Location = new System.Drawing.Point(431, 8);
            this.comboBoxThreadCount.Name = "comboBoxThreadCount";
            this.comboBoxThreadCount.Size = new System.Drawing.Size(51, 21);
            this.comboBoxThreadCount.TabIndex = 3;
            this.comboBoxThreadCount.Text = "10";
            this.comboBoxThreadCount.SelectedIndexChanged += new System.EventHandler(this.comboBoxThreadCount_SelectedIndexChanged);
            // 
            // labelThreadCount
            // 
            this.labelThreadCount.AutoSize = true;
            this.labelThreadCount.Location = new System.Drawing.Point(378, 11);
            this.labelThreadCount.Name = "labelThreadCount";
            this.labelThreadCount.Size = new System.Drawing.Size(49, 13);
            this.labelThreadCount.TabIndex = 2;
            this.labelThreadCount.Text = "Threads:";
            // 
            // labelBacklogStatus
            // 
            this.labelBacklogStatus.AutoSize = true;
            this.labelBacklogStatus.Location = new System.Drawing.Point(10, 10);
            this.labelBacklogStatus.Name = "labelBacklogStatus";
            this.labelBacklogStatus.Size = new System.Drawing.Size(162, 13);
            this.labelBacklogStatus.TabIndex = 1;
            this.labelBacklogStatus.Text = "Crawler has not been started yet.";
            // 
            // progressBarPageBacklog
            // 
            this.progressBarPageBacklog.Location = new System.Drawing.Point(234, 6);
            this.progressBarPageBacklog.Name = "progressBarPageBacklog";
            this.progressBarPageBacklog.Size = new System.Drawing.Size(131, 23);
            this.progressBarPageBacklog.TabIndex = 0;
            // 
            // panelImageControl
            // 
            this.panelImageControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelImageControl.Controls.Add(this.buttonImageFilter);
            this.panelImageControl.Controls.Add(this.comboBoxImageCount);
            this.panelImageControl.Controls.Add(this.labelImagesCount);
            this.panelImageControl.Location = new System.Drawing.Point(699, 0);
            this.panelImageControl.Name = "panelImageControl";
            this.panelImageControl.Size = new System.Drawing.Size(204, 35);
            this.panelImageControl.TabIndex = 18;
            // 
            // buttonImageFilter
            // 
            this.buttonImageFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImageFilter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonImageFilter.Location = new System.Drawing.Point(116, 8);
            this.buttonImageFilter.Name = "buttonImageFilter";
            this.buttonImageFilter.Size = new System.Drawing.Size(75, 21);
            this.buttonImageFilter.TabIndex = 12;
            this.buttonImageFilter.Text = "Filter...";
            this.buttonImageFilter.UseVisualStyleBackColor = true;
            this.buttonImageFilter.Click += new System.EventHandler(this.buttonImageFilter_Click);
            // 
            // comboBoxImageCount
            // 
            this.comboBoxImageCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxImageCount.FormattingEnabled = true;
            this.comboBoxImageCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "20",
            "50",
            "100",
            "200"});
            this.comboBoxImageCount.Location = new System.Drawing.Point(54, 8);
            this.comboBoxImageCount.Name = "comboBoxImageCount";
            this.comboBoxImageCount.Size = new System.Drawing.Size(52, 21);
            this.comboBoxImageCount.TabIndex = 11;
            this.comboBoxImageCount.Text = "20";
            // 
            // labelImagesCount
            // 
            this.labelImagesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImagesCount.AutoSize = true;
            this.labelImagesCount.Location = new System.Drawing.Point(8, 11);
            this.labelImagesCount.Name = "labelImagesCount";
            this.labelImagesCount.Size = new System.Drawing.Size(44, 13);
            this.labelImagesCount.TabIndex = 10;
            this.labelImagesCount.Text = "Images:";
            // 
            // buttonDonate
            // 
            this.buttonDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDonate.BackgroundImage = global::ImageCrawler.Properties.Resources.btn_donate_paypal_LG;
            this.buttonDonate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonDonate.Location = new System.Drawing.Point(580, 49);
            this.buttonDonate.Name = "buttonDonate";
            this.buttonDonate.Size = new System.Drawing.Size(89, 34);
            this.buttonDonate.TabIndex = 15;
            this.buttonDonate.UseVisualStyleBackColor = true;
            this.buttonDonate.Click += new System.EventHandler(this.buttonDonate_Click);
            // 
            // buttonImageLogFolder
            // 
            this.buttonImageLogFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImageLogFolder.BackgroundImage = global::ImageCrawler.Properties.Resources.download;
            this.buttonImageLogFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonImageLogFolder.Location = new System.Drawing.Point(675, 49);
            this.buttonImageLogFolder.Name = "buttonImageLogFolder";
            this.buttonImageLogFolder.Size = new System.Drawing.Size(32, 34);
            this.buttonImageLogFolder.TabIndex = 10;
            this.buttonImageLogFolder.UseVisualStyleBackColor = true;
            this.buttonImageLogFolder.Click += new System.EventHandler(this.buttonImageLogFolder_Click);
            // 
            // buttonStopCrawling
            // 
            this.buttonStopCrawling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStopCrawling.BackgroundImage = global::ImageCrawler.Properties.Resources.stop;
            this.buttonStopCrawling.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonStopCrawling.Location = new System.Drawing.Point(853, 19);
            this.buttonStopCrawling.Name = "buttonStopCrawling";
            this.buttonStopCrawling.Size = new System.Drawing.Size(64, 64);
            this.buttonStopCrawling.TabIndex = 5;
            this.buttonStopCrawling.UseVisualStyleBackColor = true;
            this.buttonStopCrawling.Click += new System.EventHandler(this.buttonStopCrawling_Click);
            // 
            // buttonSuspendCrawling
            // 
            this.buttonSuspendCrawling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSuspendCrawling.BackgroundImage = global::ImageCrawler.Properties.Resources.pause;
            this.buttonSuspendCrawling.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSuspendCrawling.Location = new System.Drawing.Point(783, 19);
            this.buttonSuspendCrawling.Name = "buttonSuspendCrawling";
            this.buttonSuspendCrawling.Size = new System.Drawing.Size(64, 64);
            this.buttonSuspendCrawling.TabIndex = 4;
            this.buttonSuspendCrawling.UseVisualStyleBackColor = true;
            this.buttonSuspendCrawling.Click += new System.EventHandler(this.buttonSuspendCrawling_Click);
            // 
            // buttonStartCrawling
            // 
            this.buttonStartCrawling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStartCrawling.BackgroundImage = global::ImageCrawler.Properties.Resources.play;
            this.buttonStartCrawling.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonStartCrawling.Location = new System.Drawing.Point(713, 19);
            this.buttonStartCrawling.Name = "buttonStartCrawling";
            this.buttonStartCrawling.Size = new System.Drawing.Size(64, 64);
            this.buttonStartCrawling.TabIndex = 3;
            this.buttonStartCrawling.UseVisualStyleBackColor = true;
            this.buttonStartCrawling.Click += new System.EventHandler(this.buttonStartCrawling_Click);
            // 
            // linkLabelCopyrightAndLicence
            // 
            this.linkLabelCopyrightAndLicence.AutoSize = true;
            this.linkLabelCopyrightAndLicence.LinkColor = System.Drawing.Color.DarkGray;
            this.linkLabelCopyrightAndLicence.Location = new System.Drawing.Point(48, 5);
            this.linkLabelCopyrightAndLicence.Name = "linkLabelCopyrightAndLicence";
            this.linkLabelCopyrightAndLicence.Size = new System.Drawing.Size(292, 13);
            this.linkLabelCopyrightAndLicence.TabIndex = 16;
            this.linkLabelCopyrightAndLicence.TabStop = true;
            this.linkLabelCopyrightAndLicence.Text = "Copyright 2010 Danny Kunz  Licensed under Terms of LGPL";
            this.linkLabelCopyrightAndLicence.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabelCopyrightAndLicence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCopyrightAndLicence_LinkClicked);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 488);
            this.Controls.Add(this.linkLabelCopyrightAndLicence);
            this.Controls.Add(this.buttonDonate);
            this.Controls.Add(this.splitContainerImageViewCrawlerStatus);
            this.Controls.Add(this.buttonImageLogFolder);
            this.Controls.Add(this.checkBoxIncludeExternalPages);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonStopCrawling);
            this.Controls.Add(this.buttonSuspendCrawling);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.comboBoxUrl);
            this.Controls.Add(this.buttonStartCrawling);
            this.MinimumSize = new System.Drawing.Size(560, 400);
            this.Name = "FormMain";
            this.Text = "Image Crawler";
            this.splitContainerImageViewCrawlerStatus.Panel1.ResumeLayout(false);
            this.splitContainerImageViewCrawlerStatus.Panel2.ResumeLayout(false);
            this.splitContainerImageViewCrawlerStatus.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImageViewCrawlerStatus)).EndInit();
            this.splitContainerImageViewCrawlerStatus.ResumeLayout(false);
            this.panelImageFilter.ResumeLayout(false);
            this.panelImageFilter.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panelPageBacklogStatus.ResumeLayout(false);
            this.panelPageBacklogStatus.PerformLayout();
            this.panelImageControl.ResumeLayout(false);
            this.panelImageControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStartCrawling;
        private System.Windows.Forms.ComboBox comboBoxUrl;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonSuspendCrawling;
        private System.Windows.Forms.Button buttonStopCrawling;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.CheckBox checkBoxIncludeExternalPages;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogImageLogDirectory;
        private System.Windows.Forms.Button buttonImageLogFolder;
        private System.Windows.Forms.SplitContainer splitContainerImageViewCrawlerStatus;
        private System.Windows.Forms.ListView imageListView;
        private System.Windows.Forms.Panel panelImageFilter;
        private System.Windows.Forms.Label labelFilterImagesWithSameSize;
        private System.Windows.Forms.Label labelFilterDummyResponse;
        private System.Windows.Forms.CheckBox checkBoxFilterImagesWithSameSize;
        private System.Windows.Forms.Label labelOnlyAnkerImages;
        private System.Windows.Forms.CheckBox checkBoxFilterDummyResponse;
        private System.Windows.Forms.CheckBox checkBoxOnlyAnkerImages;
        private System.Windows.Forms.Label labelMinimumFilesize;
        private System.Windows.Forms.Label labelMinimumArea;
        private System.Windows.Forms.Label labelMinimumHeight;
        private System.Windows.Forms.Label labelMinimumWidth;
        private System.Windows.Forms.ComboBox comboBoxMinimumFilesize;
        private System.Windows.Forms.ComboBox comboBoxMinimumHeight;
        private System.Windows.Forms.ComboBox comboBoxMinimumArea;
        private System.Windows.Forms.ComboBox comboBoxMinimumWidth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelPageBacklogStatus;
        private System.Windows.Forms.ComboBox comboBoxThreadCount;
        private System.Windows.Forms.Label labelThreadCount;
        private System.Windows.Forms.Label labelBacklogStatus;
        private System.Windows.Forms.ProgressBar progressBarPageBacklog;
        private System.Windows.Forms.Panel panelImageControl;
        private System.Windows.Forms.Button buttonImageFilter;
        private System.Windows.Forms.ComboBox comboBoxImageCount;
        private System.Windows.Forms.Label labelImagesCount;
        private System.Windows.Forms.Label labelFilterFreeFloatingUrls;
        private System.Windows.Forms.CheckBox checkBoxFilterFreeFloatingUrls;
        private System.Windows.Forms.Button buttonDonate;
        private System.Windows.Forms.Panel panelCrawlerStatus;
        private System.Windows.Forms.LinkLabel linkLabelCopyrightAndLicence;
    }
}

