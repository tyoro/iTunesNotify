using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iTunesNotify
{
    public partial class Form1 : Form
    {
        iTunesCntrol itunesControl;

        public Form1()
        {
            InitializeComponent();
            itunesControl = new iTunesCntrol(this);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
//            notifyIcon1.Icon

        }

        private void QuitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            // タスクトレイからアイコンを取り除く
            Application.Exit();
            // アプリケーション終了
        }

        public void notify( String title, String message)
        {
            //バルーンヒントを表示する
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = message;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;

            notifyIcon1.ShowBalloonTip(10000);
        }

        delegate void ConnectDelegate();

        public void Connet()
        {
            ConnectMenuItem.Enabled = true;
        }

        public bool Connect
        {
            set
            {
                //if (!this.IsHandleCreated)
                //{
                //    return;
                //}

                if (this.InvokeRequired)
                {
                    this.Invoke(
                        (MethodInvoker)delegate()
                        {
                            this.ConnectMenuItem.Enabled = !value;
                        });
                }
                else
                {
                        ConnectMenuItem.Enabled = !value;
                }
            }
        }

        public bool Check
        {
            set
            {
                StopMenuItem.Checked = !value;
            }
        }

        private void ConnectMenuItem_Click(object sender, EventArgs e)
        {
            itunesControl.Connection();
        }

        private void StopMenuItem_Click(object sender, EventArgs e)
        {
            StopMenuItem.Checked = !StopMenuItem.Checked;
            itunesControl.Checking = !StopMenuItem.Checked;
        }

        protected override CreateParams CreateParams
        {
            [System.Security.Permissions.SecurityPermission(
                System.Security.Permissions.SecurityAction.LinkDemand,
                Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                const int WS_EX_TOOLWINDOW = 0x80;
                const long WS_POPUP = 0x80000000L;
                const int WS_VISIBLE = 0x10000000;
                const int WS_SYSMENU = 0x80000;
                const int WS_MAXIMIZEBOX = 0x10000;

                CreateParams cp = base.CreateParams;
                cp.ExStyle = WS_EX_TOOLWINDOW;
                cp.Style = unchecked((int)WS_POPUP) |
                    WS_VISIBLE | WS_SYSMENU | WS_MAXIMIZEBOX;
                cp.Width = 0;
                cp.Height = 0;

                return cp;
            }
        }
    }
}
