using A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494.Observer;
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{
  
    public class PostAdapter
    {

        private readonly Post r_Post;

        private string m_PostDescription;
        public static int postCount  { get; private set; } = 0;
        public static int CountNewPosts { get;  private set; }
       // private static readonly List<IObserver> lIsteners = new List<IObserver>();


        public PostAdapter(Post i_Post) // (for Lior)i think we can remove this c'tor, its only for databinding to be able to change post content
        {
            
            this.r_Post = i_Post;

        }

        //public void AttachListener(IObserver i_listener)
        //{

        //    lIsteners.Add(i_listener);

        //}

        //public void DetachListener(IObserver i_listener)
        //{

        //    lIsteners.Remove(i_listener);

        //}

        public string PostDescription
        {

            get { return this.m_PostDescription ?? this.r_Post.Message?? this.r_Post.Caption ; }
           
            set { this.m_PostDescription = value; }
       
        }

        public static List<PostAdapter> CreateAdapterPosts(FacebookObjectCollection<Post> i_Posts)
        {

           // CountNewPosts = 0;
            List<PostAdapter> wrappedPosts = new List<PostAdapter>();

            foreach (Post post in i_Posts)
            {

                wrappedPosts.Add(new PostAdapter(post));

            }

            //if(wrappedPosts.Count > postCount)
            //{
            //    CountNewPosts = wrappedPosts.Count - postCount;
            //    postCount = wrappedPosts.Count;
            //    //ReportWhenUpdate(wrappedPosts.Count-postCount);   
            //    notifyListenersAboutNewPosts();
            //}
            
            return wrappedPosts;

        }


        //private static void notifyListenersAboutNewPosts()
        //{

        //    if(CountNewPosts != 0)
        //    {

        //        foreach (IObserver observer in lIsteners)
        //        {

        //            observer.Update();

        //        }

        //    }
         
        //}
    }

}
