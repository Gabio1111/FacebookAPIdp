using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper;
using Facebook;
using FacebookWrapper.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    public partial class LoginForm : Form
    {

        private UserProxy   m_LoggedInUser;
        private LoginResult m_LoginResult;
        private MainForm    m_ReloadFormMain;
        
        public LoginForm()
        {

            InitializeComponent();

            FacebookService.s_CollectionLimit = 200;
            m_LoggedInUser = FormMainFacade.Instance.LoginToMainForm();

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            this.Hide();
            m_ReloadFormMain = new MainForm();
            m_ReloadFormMain.ShowDialog();
            this.Visible = false;

        }

        protected override void OnShown(EventArgs e)
        {

            base.OnShown(e);

             AppSettings.Instance = AppSettings.LoadFromFile();

            if (AppSettings.Instance.RememberUser && !string.IsNullOrEmpty(AppSettings.Instance.RecentAccessToken))
            {

                m_LoginResult = FacebookService.Connect(AppSettings.Instance.RecentAccessToken);
                this.Hide();
                m_ReloadFormMain = new MainForm();
                m_ReloadFormMain.ShowDialog();

            }

        }

        private void cbRememberMe_CheckedChanged(object sender, EventArgs e)
        {

            bool buttonRemmberMe = false;

            if (checkBoxRememberMe.Checked)
            {

                AppSettings.Instance.RememberUser = !buttonRemmberMe;

            }

        }

    }

}
