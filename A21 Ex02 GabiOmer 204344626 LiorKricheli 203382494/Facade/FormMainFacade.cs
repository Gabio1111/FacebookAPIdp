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
using System.Collections;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{


    public class FormMainFacade:IPostAdapterLIstener
    {

        public UserProxy LoggedInUser { get; set; }

        public LoginResult LoginResult { get; set; }


        private static FormMainFacade s_FormMainFacade;

        private PostAdapter postAdapter;

        

       



        private FormMainFacade()
        {
            postAdapter = new PostAdapter();
           // posts = new List<PostAdapter>();
            postAdapter.AttachListener(this as IPostAdapterLIstener);
            
        }
                   

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
        public IEnumerator<FacebookObject> GetEnumerator(Enums.eFacebookObject objectToIterateOn) => new EnumerableUserData(this,objectToIterateOn);

        private class EnumerableUserData : IEnumerator<FacebookObject>
        {
            FormMainFacade m_mainFacade;
            Enums.eFacebookObject eFacebookObject;
            int m_CurrentPostInx = -1;
            FacebookObjectCollection<FacebookObject> m_FBobjects;


            public EnumerableUserData(FormMainFacade i_mainFacade,Enums.eFacebookObject i_facebookObject)
            {
                m_FBobjects = new FacebookObjectCollection<FacebookObject>();
                m_mainFacade = i_mainFacade;
                eFacebookObject = i_facebookObject;
                
                try
                {

                    switch (eFacebookObject)
                    {
                        case Enums.eFacebookObject.Albums:
                            
                            foreach (Album album in m_mainFacade.GetAlbums())
                            {
                                m_FBobjects.Add(album);
                            }
                            break;

                        case Enums.eFacebookObject.Friends:
                            foreach (User friend in m_mainFacade.GetFriends())
                            {
                                m_FBobjects.Add(friend);
                            }
                            break;

                        case Enums.eFacebookObject.FavouriteTeams:
                            foreach (Page FavTeam in m_mainFacade.GetFavouriteTeams())
                            {
                                m_FBobjects.Add(FavTeam);
                            }
                            break;

                    }
                }

                catch
                {
                    throw new Facebook.FacebookApiException("No Access Or null");
                }

            }

            public void Dispose()
            {
                Reset();

            }

            public FacebookObject Current
            {
                get { return m_FBobjects[m_CurrentPostInx]; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                if (m_CurrentPostInx + 1 < m_FBobjects.Count)
                {
                    m_CurrentPostInx++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public void Reset()
            {
                m_CurrentPostInx = -1;
            }

        }
        //public IEnumerator<Album> GetEnumerator() => new EnumerableUserData(this);

        //private class EnumerableUserData : IEnumerator<Album>
        //{
        //    FormMainFacade m_mainFacade;
        //    int m_CurrentPostInx = -1;
        //    FacebookObjectCollection<Album> albums1;

        //    public EnumerableUserData(FormMainFacade i_mainFacade)
        //    {
        //        albums1 = new FacebookObjectCollection<Album>();
        //        m_mainFacade = i_mainFacade;

        //        foreach (Album album in m_mainFacade.GetAlbums())
        //        {
        //            albums1.Add(album);
        //        }


        //    }

        //    public void Dispose()
        //    {
        //        Reset();

        //    }

        //    public Album Current
        //    {
        //        get { return albums1[m_CurrentPostInx]; }
        //    }

        //    object IEnumerator.Current
        //    {
        //        get { return this.Current; }
        //    }

        //    public bool MoveNext()
        //    {
        //        if (m_CurrentPostInx + 1 < albums1.Count)
        //        {
        //            m_CurrentPostInx++;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    public void Reset()
        //    {
        //        m_CurrentPostInx = -1;
        //    }

        //}


        public string CountPosts
        {

            get
            {

                string countPosts;

                try
                {
                    countPosts = string.Format("{0}", PostAdapter.postCount);
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
        
        //public string CountCheckins
        //{

        //    get
        //    {

        //        string countCheckins;

        //        try
        //        {
        //            countCheckins = string.Format("{0}", LoggedInUser.LoggedUser.Checkins.Count);
        //        }
        //        catch (Exception)
        //        {

        //            throw new Facebook.FacebookApiException("");
        //        }

        //        return countCheckins;
        //    }
        //}
        
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

  

        public FacebookObjectCollection<Page> GetFavouriteTeams()
        {

            FacebookObjectCollection<Page> Teams = new FacebookObjectCollection<Page>();

            if (LoggedInUser.LoggedUser.FavofriteTeams!=null)
            {
                foreach(Page favTeam in LoggedInUser.LoggedUser.FavofriteTeams)
                {
                    Teams.Add(favTeam);
                }
            }      
            return Teams;
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
        public void update()
        {
            MessageBox.Show(string.Format("you have {0} new posts", PostAdapter.CountNewPosts));
        }

        public FacebookObjectCollection<Album> GetAlbums()
        {
            FacebookObjectCollection<Album> albums = new FacebookObjectCollection<Album>();
            try
            {
                if(this.LoggedInUser.LoggedUser.Albums!=null)
                {
                    albums = this.LoggedInUser.LoggedUser.Albums;

                    
                }

            }
            catch (Exception)
            {

                 throw new Facebook.FacebookApiException("Couldn't fetch user's albums.");

            }

            return albums;

        }

        public FacebookObjectCollection<User> GetFriends()
        {
            FacebookObjectCollection<User> friends = new FacebookObjectCollection<User>();
            try
            {
                if (this.LoggedInUser.LoggedUser.Friends != null)
                {
                    friends = this.LoggedInUser.LoggedUser.Friends;

                }

            }
            catch (Exception)
            {

                throw new Facebook.FacebookApiException("Couldn't fetch user's friends.");

            }

            return friends;
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
