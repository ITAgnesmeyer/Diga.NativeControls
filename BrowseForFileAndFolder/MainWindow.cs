using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreWindowsWrapper;
using CoreWindowsWrapper.Tools;
using Diga.Core.Api.Win32;

namespace BrowseForFileAndFolder
{
    class MainWindow:NativeWindow
    {
        private NativeTextBox _TextPath;
        private NativeButton _PathSelectButton;
        protected override void InitControls()
        {
            
            this.Text = "Browse for Folder and File";
            this.Width = 500;
            this.Height = 400;
            this.BackColor = ColorTool.White;
            this.StartUpPosition = WindowsStartupPosition.CenterScreen;
            this._TextPath = new NativeTextBox();

            this._TextPath.Left = 10;
            this._TextPath.Top = 10;
            this._TextPath.Width = 200;
            this._TextPath.Height = 20;
            this._TextPath.Style |= WindowStylesConst.WS_BORDER;

            this._PathSelectButton = new NativeButton();
            this._PathSelectButton.Left = this._TextPath.Left + this._TextPath.Width + 2;
            this._PathSelectButton.Top = this._TextPath.Top;
            this._PathSelectButton.Width = 30;
            this._PathSelectButton.Height = 20;
            this._PathSelectButton.Text = "…";
            this._PathSelectButton.Clicked += PathSelectButton_Click;

            this.Controls.Add(this._TextPath);
            this.Controls.Add(this._PathSelectButton);
        }

        private void PathSelectButton_Click(object sender, EventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            ofd.Caption = "Select your Folder";
            if(ofd.Show(this))
            {
                this._TextPath.Text = ofd.SelectedPath;
            }

        }
    }
}
