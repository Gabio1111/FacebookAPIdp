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

    public class PostAdapter
    {

        private readonly Post r_Post;

        private string m_PostDescription;

        public PostAdapter(Post i_Post)
        {

            this.r_Post = i_Post;

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

            return wrappedPosts;

        }



    }

}
