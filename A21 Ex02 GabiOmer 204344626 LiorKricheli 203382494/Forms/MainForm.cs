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
using System.Text.RegularExpressions;
using System.Net.Http.Headers;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{

    public partial class MainForm : Form
    {

        public MainForm()
        {

            InitializeComponent();

            r_UserDetailsThread = new Thread(setBindingSourceDataSource);

            r_FetchersThread = new Thread(fetchLoggedInUser);   
            
            r_UserDetailsThread.Priority = ThreadPriority.BelowNormal;

            r_FetchersThread.Priority = ThreadPriority.AboveNormal;

            r_UserDetailsThread.Start();

            r_FetchersThread.Start();
           
            
        }

        readonly Thread r_UserDetailsThread;

        private readonly Thread r_FetchersThread;

        private double m_Analyzer = 0;

        private Random m_RandomForIndexChoseFriend = new Random();
        
        private void setBindingSourceDataSource()
        {

            if (InvokeRequired)
            {

                Invoke(new Action(() => userBindingSource1.DataSource = FormMainFacade.Instance.LoggedInUser.LoggedUser));
                
            }
            else
            {

                this.userBindingSource1.DataSource = FormMainFacade.Instance.LoggedInUser.LoggedUser;
            
            }

        }

        private void fetchLoggedInUser()
        {

            new Thread(fetchPosts).Start();

            new Thread(fetchAlbums).Start();

            new Thread(featchFriendList).Start();

            new Thread(featchStaticticOfUser).Start();

            new Thread(fetchPages).Start();

        }

        private void featchStaticticOfUser()
        {

            labelCountPosts.Invoke(new Action(() => labelCountPosts.Text = string.Format("{0}", FormMainFacade.Instance.CountPosts)));
            
            labelCountEvents.Invoke(new Action(() => labelCountEvents.Text = string.Format("{0}", FormMainFacade.Instance.CountEvents)));

            labelCountFriends.Invoke(new Action(() => labelCountFriends.Text = string.Format("{0}",FormMainFacade.Instance.CountFriends)));

            labelCountAlbums.Invoke(new Action(() => labelCountAlbums.Text = string.Format("{0}", FormMainFacade.Instance.CountAlbums)));

            labelCountCheckins.Invoke(new Action(() => labelCountCheckins.Text = string.Format("{0}", FormMainFacade.Instance.CountCheckins)));
            
        }
       
        protected override void OnShown(EventArgs e)
        {

            base.OnShown(e);

            tabControlCreatePost.Enabled = true;

            imageNormalPictureBox.Visible = true;

            textBoxBdayChange.Visible = false;

            textBoxEmailChange.Visible = false;

            textBoxNameChange.Visible = false;

            buttonSaveChanges.Visible = false;

            if (numericUpDownRadius.Value == 0)
            {

                buttonNearestFriends.Enabled = false;

            }
            else
            {

                buttonNearestFriends.Enabled = true;

            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            base.OnFormClosing(e);

           AppSettings.Instance.RecentWindowSize = this.Size;

            AppSettings.Instance.RecentWindowLocation = this.Location;

            if (AppSettings.Instance.RememberUser)
            {

                AppSettings.Instance.RecentAccessToken = FormMainFacade.Instance.LoginResult.AccessToken;

            }
            else
            {

                AppSettings.Instance.RecentAccessToken = null;

            }

            AppSettings.Instance.SaveToFile();

            Environment.Exit(Environment.ExitCode);

        }



        //Posts
        private void fetchPosts()
        {

            if (InvokeRequired)
            {

                this.Invoke(new Action(() => postAdapterBindingSource.DataSource = FormMainFacade.Instance.GetPosts()));

            }
            else
            {

                postAdapterBindingSource.DataSource = FormMainFacade.Instance.GetPosts();

            }

            r_FetchersThread.Join();
           
        }

        //Albums
        #region Album And Selected Albums
        private void fetchAlbums()
        {

            //listBoxAlbums.Invoke(new Action(() => listBoxAlbums.DisplayMember = "Name"));

            //if (FormMainFacade.Instance.LoggedInUser.LoggedUser.Albums == null)
            //{

            //    MessageBox.Show("No Albums to retrieve :(");

            //}

            //foreach (Album album in FormMainFacade.Instance.LoggedInUser.LoggedUser.Albums)
            //{

            //    listBoxAlbums.Invoke(new Action(() => listBoxAlbums.Items.Add(album)));

            //}

            if (InvokeRequired)
            {

                Invoke(new Action(() => albumsBindingSource.DataSource = FormMainFacade.Instance.GetAlbums()));

            }
            else
            {

                this.userBindingSource1.DataSource = FormMainFacade.Instance.GetAlbums();

            }

        }

        //private void displaySelectedAlbum()
        //{
          
        //    if (listBoxAlbums.SelectedItems.Count == 1) 
        //    {

        //        Album chosenAlbum = listBoxAlbums.SelectedItem as Album;

        //        if (chosenAlbum.PictureAlbumURL != null)
        //        {

        //            pictureBoxAlbums.LoadAsync(chosenAlbum.PictureAlbumURL);

        //        }
        //        else
        //        {

        //            pictureBoxAlbums.Image = pictureBoxAlbums.ErrorImage;

        //        }

        //    }

        //}

        //private void listBoxAlbums_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    if(r_FetchersThread.Join(1))
        //    {

        //        displaySelectedAlbum();

        //    }

        //}
        #endregion

        //Friend List
        #region Friend List And Selected Friend
        private void featchFriendList()
        {
          
            listBoxFriendsList.Invoke(new Action(() => listBoxFriendsList.DisplayMember = "Name"));

            foreach (User friend in FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends)
            {

                listBoxFriendsList.Invoke(new Action(() => listBoxFriendsList.Items.Add(friend)));

            }

        }

        private void displaySelectedFriend()
        {
          
            if (listBoxFriendsList.SelectedItems.Count == 1) //chose a line in the list box
            {
                User chosenFriend = listBoxFriendsList.SelectedItem as User;

                if (chosenFriend != null)
                {

                    pictureBoxFriendProfile.LoadAsync(chosenFriend.PictureNormalURL);
                    labelNameOfFriend.Text = chosenFriend.Name;

                }
                else
                {

                    pictureBoxFriendProfile.Image = pictureBoxFriendProfile.ErrorImage;

                }

            }
            
        }

        private void listBoxFriendsList_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (r_FetchersThread.Join(1))
            {

                displaySelectedFriend();

            }

        }

        private void buttonShowFirendList_Click(object sender, EventArgs e)
        {

            if(r_FetchersThread.Join(1))
            {

                featchFriendList();

            }

        }

        #endregion
        
        //Page
        #region Pages And Selected Index Changed
        private void fetchPages()
        {
            if(FormMainFacade.Instance.LoggedInUser.LoggedUser.FavofriteTeams!=null)
            {
                listBoxPages.Invoke(new Action(() => listBoxPages.DisplayMember = "Name"));

                foreach (Page favTeam in FormMainFacade.Instance.LoggedInUser.LoggedUser.FavofriteTeams)
                {

                    listBoxPages.Invoke(new Action(() => listBoxPages.Items.Add(favTeam)));

                }
            }

            else
            {
                listBoxPages.Invoke(new Action(() => listBoxPages.Items.Add("No Teams")));
            }


            r_FetchersThread.Join();

        }

        private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBoxPages.SelectedItems.Count == 1)
            {

                Page chosenTeam = listBoxPages.SelectedItem as Page;

                if (chosenTeam.PictureNormalURL != null)
                {

                    pictureBoxFavTeams.LoadAsync(chosenTeam.PictureNormalURL);

                }
                else
                {

                    pictureBoxFavTeams.Image = pictureBoxFavTeams.ErrorImage;

                }

            }

        }

        #endregion
        
        //Logout
        private void btLogout_Click(object sender, EventArgs e)
        {

            FormMainFacade.Instance.LoggedOutFinished();

        }

        ////Featchers

        #region Nearest Friend

        private void bNearestFriends_Click(object sender, EventArgs e)
        {

            lbClosestFriends.Items.Clear();

            int chosenRadius = Convert.ToInt32(numericUpDownRadius.Value);

            if(checkBoxFarFriends.Checked)
            {
               
                featchNearesrFriend(chosenRadius=0,Enums.eDistanceMethod.farOffFriends);
            }
            else
                featchNearesrFriend(chosenRadius,Enums.eDistanceMethod.closestFriends);


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            if(numericUpDownRadius.Value!=0)
            {

                buttonNearestFriends.Enabled = true;

            }
            else
            {

                buttonNearestFriends.Enabled = false;

            }
           
        }

        private void featchNearesrFriend(int i_radius,Enums.eDistanceMethod eDistance)
        {

            List<FriendByCoordinate> listOfNearesrFriend = FeatureFactory.Instance.Create(FormMainFacade.Instance.LoggedInUser.LoggedUser).InitiateCloseFriends(i_radius,eDistance);

            if (listOfNearesrFriend.Count == 0)
            {

                lbClosestFriends.Items.Add("No Friends :(");

            }
            else if (listOfNearesrFriend.Count > 0)
            {

                foreach (FriendByCoordinate item in listOfNearesrFriend)
                {

                    lbClosestFriends.Items.Add(item.m_Name);

                }

            }

        }

        #endregion

        #region Friend Analyzer 
        private void fetchFriendAnalyzer()
        {

            int randomIndex = m_RandomForIndexChoseFriend.Next(0, FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends.Count);

            double avarageOfFriends = FeatureFactory.Instance.Create(FormMainFacade.Instance.LoggedInUser.LoggedUser).InitiateFriendAnalyzer(FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends[randomIndex]);

            if (FormMainFacade.Instance.LoggedInUser.LoggedUser.PictureSmallURL != null)
            {

                pictureBoxUserFA.LoadAsync(FormMainFacade.Instance.LoggedInUser.LoggedUser.PictureSmallURL);

            }
            else
            {

                pictureBoxUserFA.Image = pictureBoxUserFA.ErrorImage;

            }

            if (FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends[randomIndex].PictureSmallURL != null)
            {

                pictureBoxFriendFA.LoadAsync(FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends[randomIndex].PictureSmallURL);

            }
            else
            {

                pictureBoxFriendFA.Image = pictureBoxFriendFA.ErrorImage;

            }

            if (FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends[randomIndex].Name != null)
            {

                labelFriendNameFA.Text = FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends[randomIndex].Name;

            }
            else
            {

                labelFriendNameFA.Text = string.Format("Null");

            }
         

            if (FormMainFacade.Instance.LoggedInUser.LoggedUser.Name != null)
            {

                labelUserNameFA.Text = FormMainFacade.Instance.LoggedInUser.LoggedUser.Name;

            }
            else
            {

                labelUserNameFA.Text = string.Format("Null");

            }

            labelMSum.Text = string.Format("{0}", m_Analyzer);

            labelFsum.Text = string.Format("{0}", avarageOfFriends);

        }

        private void buttonClearFA_Click(object sender, EventArgs e)
        {

            pictureBoxFriendFA.Image = null;

            pictureBoxUserFA.Image = null;

            labelFriendNameFA.Text = string.Empty;

            labelUserNameFA.Text = string.Empty;

            labelMSum.Text = string.Empty;

            labelFsum.Text = string.Empty;

        }

        private void buttonFA_Click(object sender, EventArgs e)
        {

            fetchFriendAnalyzer();

        }

        #endregion

        #region Friend Matcher

        private void fetchFriendMatcher()
        {

            if (FormMainFacade.Instance.LoggedInUser.LoggedUser.Friends.Count > 1)
            {

                List<User> maleAndFemaleList = FeatureFactory.Instance.Create(FormMainFacade.Instance.LoggedInUser.LoggedUser).InitiateFriendMatcher();

                if (maleAndFemaleList[0].PictureNormalURL != null)
                {

                    pictureBoxMatch1.LoadAsync(maleAndFemaleList[0].PictureNormalURL);

                }
                else
                {

                    pictureBoxMatch1.Image = pictureBoxMatch1.ErrorImage;

                }

                if (maleAndFemaleList[1].PictureNormalURL != null)
                {

                    pictureBoxMatch2.LoadAsync(maleAndFemaleList[1].PictureNormalURL);

                }
                else
                {

                    pictureBoxMatch2.Image = pictureBoxMatch2.ErrorImage;

                }

                if (maleAndFemaleList[0].Name != null)
                {

                    labelFriend1Name.Text = maleAndFemaleList[0].Name;

                }
                else
                {

                    labelFriend1Name.Text = string.Format("Null");

                }

                if (maleAndFemaleList[1].Name != null)
                {

                    labelFriend2Name.Text = maleAndFemaleList[1].Name;

                }
                else
                {

                    labelFriend2Name.Text = string.Format("Null");

                }
                panel5.Enabled = true;

            }
            else
            {

                MessageBox.Show("Not enough friends to create match");

            }

        }

        private void buttonFriendMatcher_Click(object sender, EventArgs e)
        {
            
            fetchFriendMatcher();

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {

            resetFriendMatcher();

        }

        private void resetFriendMatcher()
        {

            pictureBoxMatch1.Image = null;
            pictureBoxMatch2.Image = null;
            labelFriend1Name.Text = string.Format("[First Match]");
            labelFriend2Name.Text = string.Format("[Second Match]");
            RadioButton checkedButton = panel7.Controls.OfType<RadioButton>().FirstOrDefault(radioButton => radioButton.Checked = false);

            MessageBox.Show("Click the 'Show possible match' button to try again");

        }

        #endregion

        private void tabControl1_Enter(object sender, EventArgs e)
        {

            if (m_Analyzer == 0)
            {

                new Thread(() =>
                {

                    this.Invoke(new Action(() =>
                    {

                        m_Analyzer = FeatureFactory.Instance.Create(FormMainFacade.Instance.LoggedInUser.LoggedUser).InitiateFriendAnalyzer(FormMainFacade.Instance.LoggedInUser.LoggedUser);

                    }));

                }).Start();

            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

            RadioButton checkedButton = panel7.Controls.OfType<RadioButton>().FirstOrDefault(radioButton => radioButton.Checked);

            listBoxFriendsAndMatch.Items.Add(string.Format("{0}, {1}, Rank: {2}", (labelFriend1Name.Text, labelFriend2Name.Text, checkedButton.Tag.ToString())));
            
        }

        private void buttonEditAbout_Click(object sender, EventArgs e)
        {

            textBoxBdayChange.Visible = true;

            textBoxEmailChange.Visible = true;

            textBoxNameChange.Visible = true;

            buttonSaveChanges.Visible = true;

        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {

            saveChanges();

        }

        private void saveChanges()
        {

            if (textBoxBdayChange.Text == string.Empty)
            {

                textBoxBdayChange.Text = FormMainFacade.Instance.LoggedInUser.LoggedUser.Birthday;

            }

            if (textBoxEmailChange.Text == string.Empty)
            {

                textBoxEmailChange.Text = FormMainFacade.Instance.LoggedInUser.LoggedUser.Email;

            }

            if (textBoxNameChange.Text == string.Empty)
            {

                textBoxNameChange.Text = FormMainFacade.Instance.LoggedInUser.LoggedUser.Name;

            }

            FormMainFacade.Instance.LoggedInUser.LoggedUser.Email = textBoxEmailChange.Text;

            FormMainFacade.Instance.LoggedInUser.LoggedUser.Birthday = textBoxBdayChange.Text;

            FormMainFacade.Instance.LoggedInUser.LoggedUser.Name = textBoxNameChange.Text;
         
            textBoxBdayChange.Visible = false;

            textBoxEmailChange.Visible = false;

            textBoxNameChange.Visible = false;

            MessageBox.Show("Saved Changes");

            buttonSaveChanges.Visible = false;

        }

        private void textBoxEmailChange_Validated(object sender, EventArgs e)
        {

            mailValidate();
                 
        }

        private void mailValidate()
        {

            string email = textBoxEmailChange.Text;

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            Match match = regex.Match(email);

            if (!match.Success)
            {

                MessageBox.Show("Email not valid!");

                buttonSaveChanges.Enabled = false;

            }
            else
            {

                buttonSaveChanges.Enabled = true;

            }

        }

        private void textBoxBdayChange_Validating(object sender, CancelEventArgs e)
        {

            Regex patternBday = new Regex(@"^(\d{1,2})/(\d{1,2})/(\d{4})$");

            Match match = patternBday.Match(textBoxBdayChange.Text);

            if (match.Success)
            {

                int dd = int.Parse(match.Groups[1].Value);

                int mm = int.Parse(match.Groups[2].Value);

                int yyyy = int.Parse(match.Groups[3].Value);

                e.Cancel = dd < 1 || dd > 31 || mm < 1 || mm > 12;
            }
            else
            {
                e.Cancel = true;
            
            }

            if (e.Cancel)
            {

                if (MessageBox.Show("Wrong date format. The correct format is dd/mm/yyyy\n+ dd should be between 1 and 31.\n+ mm should be between 1 and 12", "Invalid date", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                
                    e.Cancel = false;

            }

        }

        //private void postDescriptionTextBox_Validated(object sender, EventArgs e)
        //{

        //    MessageBox.Show("Tap on list box to apply");

        //}

        /// <summary>
        /// Refresh 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxRefresh_Click(object sender, EventArgs e)
        {

            if(r_FetchersThread.Join(1) && r_UserDetailsThread.Join(1))
            {

                listBoxAlbums.Controls.Clear();
                listBoxPages.Controls.Clear();
                listBoxPosts.Controls.Clear();
                listBoxFriendsList.Controls.Clear();
                new Thread(fetchLoggedInUser).Start();
                tabControlCreatePost.Show();

            }    
           
        }

        private void buttonCreatePost_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBoxCreatePosts.Text))
            {

                PostAdapter post = new PostAdapter(null);
                post.PostDescription = textBoxCreatePosts.Text;

            }

        }
    }

}
