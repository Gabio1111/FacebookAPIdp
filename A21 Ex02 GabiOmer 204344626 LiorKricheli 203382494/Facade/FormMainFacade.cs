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

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    public class FormMainFacade: IPostAdapterListener
    {

        public UserProxy    LoggedInUser { get; set; }
        public LoginResult  LoginResult { get; set; }
        private PostAdapter m_PostAdapter;
        private static FormMainFacade s_FormMainFacade;

        private FormMainFacade()
        {

            m_PostAdapter = new PostAdapter();
            m_PostAdapter.AttachListener(this as IPostAdapterListener);
            
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

        public string CountPosts
        {

            get
            {

                string countPosts;

                try
                {

                    countPosts = string.Format("{0}", PostAdapter.PostCount);

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

        public FacebookObjectCollection<Page> GetPages()
        {

            FacebookObjectCollection<Page> pages = null;

            if (LoggedInUser.LoggedUser.FavofriteTeams != null)
            {

                pages = new FacebookObjectCollection<Page>();

                foreach (Page favofriteTeams in LoggedInUser.LoggedUser.FavofriteTeams)
                {

                    pages.Add(favofriteTeams);

                }

            }      

            return pages;

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

        public FacebookObjectCollection<Album> GetAlbums()
        {

            FacebookObjectCollection<Album> albums = null;
            
            try
            {

                if(this.LoggedInUser.LoggedUser.Albums != null)
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

            FacebookObjectCollection<User> friends = null;

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
        
        public void LoggedOutFinished()
        {
            
            AppSettings.Instance.RememberUser = false;               
            AppSettings.Instance.SaveToFile();

            MessageBox.Show("You are now logged out!");

            Environment.Exit(Environment.ExitCode);

        }

        public void Update()
        {

            MessageBox.Show(string.Format("you have {0} new posts", PostAdapter.CountNewPosts));

        }

        public IEnumerator<FacebookObject> GetEnumerator(Enums.eFacebookObject i_ObjectToIterator) => new EnumerableUserData(this, i_ObjectToIterator);

        private class EnumerableUserData : IEnumerator<FacebookObject>
        {

            private FacebookObjectCollection<FacebookObject> m_FacebookObjects;
            private FormMainFacade m_MainFacade;
            private int m_CurrentObjectIndex = -1;
           
            public EnumerableUserData(FormMainFacade i_MainFacade, Enums.eFacebookObject i_FacebookObject)
            {

                m_FacebookObjects = new FacebookObjectCollection<FacebookObject>();
                m_MainFacade = i_MainFacade;
                Enums.eFacebookObject eFacebookObject = i_FacebookObject;

                try
                {

                    switch (eFacebookObject)
                    {

                        case Enums.eFacebookObject.Albums:

                            foreach (Album album in m_MainFacade.GetAlbums())
                            {

                                m_FacebookObjects.Add(album);

                            }

                            break;

                        case Enums.eFacebookObject.Friends:

                            foreach (User friend in m_MainFacade.GetFriends())
                            {

                                m_FacebookObjects.Add(friend);

                            }

                            break;

                        case Enums.eFacebookObject.FavouriteTeams:

                            foreach (Page pages in m_MainFacade.GetPages())
                            {

                                m_FacebookObjects.Add(pages);

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

                get { return m_FacebookObjects[m_CurrentObjectIndex]; }

            }

            object IEnumerator.Current
            {

                get { return this.Current; }

            }

            public bool MoveNext()
            {

                bool haveNewObject = false;
                if (m_CurrentObjectIndex + 1 < m_FacebookObjects.Count)
                {

                    m_CurrentObjectIndex++;
                    haveNewObject = !haveNewObject;

                }

                return haveNewObject;

            }

            public void Reset()
            {

                m_CurrentObjectIndex = -1;

            }

        }

    }

}
