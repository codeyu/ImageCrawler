using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageCrawler
{
    public partial class DialogImageDetailView : Form
    {
        protected String filename = "";

        public DialogImageDetailView()
        {
            InitializeComponent();

            this.pictureBox.MouseDown += new MouseEventHandler(delegate(object sender, MouseEventArgs args)
            {
                //
                try
                {
                    //
                    this.pictureBox.Image.Save(this.filename);

                    //
                    DataObject dataObject = new DataObject(DataFormats.FileDrop, new String[] { this.filename });
                    DragDropEffects result = this.pictureBox.DoDragDrop(dataObject, DragDropEffects.All);
                    if (result != DragDropEffects.None)
                    {
                    }
                }
                catch (Exception e) { }

                //
                this.Close();
            });
            this.pictureBox.DragDrop += new DragEventHandler(delegate(object dragdropsender, DragEventArgs dragdropargs)
            {
                if (this.filename != null)
                {
                    FileInfo fileInfo = new FileInfo(this.filename);
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


            this.linkLabelPageLink.LinkClicked += new LinkLabelLinkClickedEventHandler(delegate(object sender, LinkLabelLinkClickedEventArgs args)
            {
                System.Diagnostics.Process.Start(this.linkLabelPageLink.Text);
                this.linkLabelPageLink.LinkVisited = true;
            });
        }

        public void setImage(Image image,String filename, String pageLinkUrlStr)
        {
            this.pictureBox.Image = image;
            this.filename = filename;
            this.linkLabelPageLink.Text = pageLinkUrlStr;

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {

        }

    }
}
