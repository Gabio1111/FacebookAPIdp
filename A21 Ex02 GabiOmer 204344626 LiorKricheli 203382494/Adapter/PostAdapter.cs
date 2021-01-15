using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{

    public interface IPostAdapterLIstener
    {
         int CountNewPosts { get; set; }
        //void update(int i_CountNewPosts);
    }

    public class PostAdapter
    {

        private readonly Post r_Post;

        private string m_PostDescription;
        public static int postCount  { get; private set; } = 0;
        private static readonly List<IPostAdapterLIstener> lIsteners=new List<IPostAdapterLIstener>();


        public PostAdapter(Post i_Post)
        {
            
            this.r_Post = i_Post;

        }

        public void AttachListener(IPostAdapterLIstener i_listener)
        {
            lIsteners.Add(i_listener);
        }

        public void DetachListener(IPostAdapterLIstener i_listener)
        {
            lIsteners.Remove(i_listener);
        }

        public string PostDescription
        {

            get { return this.m_PostDescription ?? this.r_Post.Message?? this.r_Post.Caption ; }
           
            set { this.m_PostDescription = value; }
       
        }

        public static List<PostAdapter> CreateAdapterPosts(FacebookObjectCollection<Post> i_Posts)
        {

            List<PostAdapter> wrappedPosts = new List<PostAdapter>();

            foreach (Post post in i_Posts)
            {

                wrappedPosts.Add(new PostAdapter(post));

            }
            if(wrappedPosts.Count > postCount)
            {
                postCount = wrappedPosts.Count;
                ReportWhenUpdate(wrappedPosts.Count-postCount);
            }
            
            return wrappedPosts;

        }

        private static void ReportWhenUpdate(int CountNewPosts)
        {
            notifyListeners(CountNewPosts);
        }

        private static void notifyListeners(int CountNewPosts)
        {
            foreach(IPostAdapterLIstener observer in lIsteners)
            {
               
                //observer.update(CountNewPosts);
            }
        }
    }

}
