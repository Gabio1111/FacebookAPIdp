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

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{

    public class FormMainFacade:IPostAdapterLIstener
    {

        public UserProxy LoggedInUser { get; set; }

        public LoginResult LoginResult { get; set; }


        private static FormMainFacade s_FormMainFacade;

        private FormMainFacade() { }
                   

        public UserProxy LoginToMainForm()
        {

            LoginResult = FacebookService.Login("747979639134063",
                    "public_profile",
                    "email",
                    "publish_to_groups",
                    "user_birthday",
                    "user_age_range",
                    "user_gender",
                    "user_link",
                    "user_tagged_places",
                    "user_videos",
                    "publish_to_groups",
                    "groups_access_member_info",
                    "user_friends",
                    "user_events",
                    "user_likes",
                    "user_location",
                    "user_photos",
                    "user_posts",
                    "user_hometown");

            if (!string.IsNullOrEmpty(LoginResult.AccessToken))
            {

                LoggedInUser = new UserProxy(LoginResult.LoggedInUser);

            }
            else
            {

                MessageBox.Show(LoginResult.ErrorMessage);

            }

            return LoggedInUser;

        }

        public static FormMainFacade Instance
        {

            get
            {

                if (s_FormMainFacade == null)
                {

                    s_FormMainFacade = new FormMainFacade();

                }

                return s_FormMainFacade;

            }

        }


        public string CountPosts
        {

            get
            {

                string countPosts;

                try
                {
                    countPosts = string.Format("{0}", LoggedInUser.LoggedUser.Posts.Count);
                }
                catch (Exception)
                {

                    throw new Facebook.FacebookApiException("");
                }

                return countPosts;
            }

        }
        
        public string CountAlbums
        {

            get
            {

                string countAlbums;

                try
                {
                    countAlbums = string.Format("{0}", LoggedInUser.LoggedUser.Albums.Count);
                }
                catch (Exception)
                {

                    throw new Facebook.FacebookApiException("");
                }

                return countAlbums;
            }

        }
        
        public string CountFriends
        {

            get
            {

                string countFriends;

                try
                {
                    countFriends = string.Format("{0}", LoggedInUser.LoggedUser.Friends.Count);
                }
                catch (Exception)
                {

                    throw new Facebook.FacebookApiException("");
                }

                return countFriends;

            }

        }
        
        public string CountCheckins
        {

            get
            {

                string countCheckins;

                try
                {
                    countCheckins = string.Format("{0}", LoggedInUser.LoggedUser.Checkins.Count);
                }
                catch (Exception)
                {

                    throw new Facebook.FacebookApiException("");
                }

                return countCheckins;
            }
        }
        
        public string CountEvents
        {

            get
            {

                string countEvents;

                try
                {
                    countEvents = string.Format("{0}", LoggedInUser.LoggedUser.Events.Count);
                }
                catch (Exception)
                {

                    throw new Facebook.FacebookApiException("");
                }

                return countEvents;
            }

        }

        public List<PostAdapter> GetPosts()
        {

            List<PostAdapter> posts;

            try
            {

                posts = PostAdapter.CreateAdapterPosts(this.LoggedInUser.LoggedUser.Posts);

            }
            catch (Exception)
            {

                throw new Facebook.FacebookApiException("Couldn't fetch user's posts.");

            }

            return posts;

        }

         public void update(int i_CountNewPosts)
        {
            return 
        }
      

        

        public FacebookObjectCollection<Checkin> GetCheckins()
        {

            FacebookObjectCollection<Checkin> checkin;

            try
            {

                checkin = LoggedInUser.LoggedUser.Checkins;

            }
            catch (Exception)
            {

                throw new Facebook.FacebookApiException("Couldn't fetch user's checkins.");

            }

            return checkin;

        }

        public void LoggedOutFinished()
        {
            
            AppSettings.Instance.RememberUser = false;     
            
            AppSettings.Instance.SaveToFile();

            MessageBox.Show("You are now logged out!");

            Environment.Exit(Environment.ExitCode);

        }

    }

}
