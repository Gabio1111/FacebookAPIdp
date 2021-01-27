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

    public class FeatureFactory //Singleton
    {

        private static FeatureFactory s_FeatureFactory;

        private FeatureFactory()
        {
           
        }

        public static FeatureFactory Instance
        {

            get
            {

                if(s_FeatureFactory == null)
                {

                    s_FeatureFactory = new FeatureFactory();

                }

                return s_FeatureFactory;

            }

        }

        public FeaturesFacade Create(User i_LoggedInUser)
        {
            
            return new FeaturesFacade(i_LoggedInUser);

        }

    }

}
