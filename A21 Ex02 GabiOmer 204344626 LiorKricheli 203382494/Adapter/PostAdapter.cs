using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    public interface IPostAdapterListener
    {

        void Update();

    }

    public class PostAdapter
    {

        private readonly Post   r_Post;
        private string          m_PostDescription;
        public static int       PostCount  { get; private set; } = 0;
        public static int       CountNewPosts { get;  private set; }
        private static readonly List<IPostAdapterListener>  m_Ilsteners = new List<IPostAdapterListener>();


        public PostAdapter(Post i_Post=null) 
        {
            
            this.r_Post = i_Post;

        }

        public void AttachListener(IPostAdapterListener i_AttachListener)
        {

            m_Ilsteners.Add(i_AttachListener);

        }

        public void DetachListener(IPostAdapterListener i_DetachListener)
        {

            m_Ilsteners.Remove(i_DetachListener);

        }

        public string PostDescription
        {

            get { return this.m_PostDescription ?? this.r_Post.Message?? this.r_Post.Caption ; }
           
            set { this.m_PostDescription = value; }
       
        }

        public static List<PostAdapter> CreateAdapterPosts(FacebookObjectCollection<Post> i_Posts)
        {

            CountNewPosts = 0;
            List<PostAdapter> wrappedPosts = new List<PostAdapter>();

            foreach (Post post in i_Posts)
            {

                wrappedPosts.Add(new PostAdapter(post));

            }

            if(wrappedPosts.Count > PostCount)
            {

                CountNewPosts = wrappedPosts.Count - PostCount;
                PostCount = wrappedPosts.Count;
                notifyListenersAboutNewPosts();

            }
            
            return wrappedPosts;

        }

        private static void notifyListenersAboutNewPosts()
        {

            if(CountNewPosts != 0)
            {

                foreach (IPostAdapterListener observer in m_Ilsteners)
                {

                    observer.Update();

                }

            }
         
        }

    }

}
