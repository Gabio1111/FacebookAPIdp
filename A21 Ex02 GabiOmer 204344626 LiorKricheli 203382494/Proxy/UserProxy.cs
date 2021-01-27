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

    public class UserProxy
    {

        public User LoggedUser { get; set; }
        
        public UserProxy(User i_LoggedUser)
        {

            this.LoggedUser = i_LoggedUser;
            
        }

    }

}
