using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{
    internal class MatchMyFriends
    {

        private User m_User;

        private Random m_RandomIndex;

        private List<User> m_ListMale;

        private List<User> m_ListFemale;
     
        internal MatchMyFriends(User i_loggedUser)
        {

            m_User = i_loggedUser;
            m_RandomIndex = new Random();
            
        }

        internal List<User> MatchingMyFriends()
        {

            List<User> matchingFriends = new List<User>();

            bool listIsEmpty = true;

            if (m_User.Friends.Count > 1 )
            {

                m_ListMale = new List<User>();

                m_ListFemale = new List<User>();

                sortGender();

                if (m_ListMale.Count > 0 && m_ListFemale.Count > 0)
                {

                    while (listIsEmpty)     //matchingFriends.Count!=2
                    {

                        int indexFriendForMatchMen = m_RandomIndex.Next(0, m_User.Friends.Count);

                        int indexFriendForMatchWomen = m_RandomIndex.Next(0, m_User.Friends.Count);

                        if (theAgeIsValid(m_ListMale[indexFriendForMatchMen].Birthday) == theAgeIsValid(m_ListFemale[indexFriendForMatchWomen].Birthday))
                        {

                            matchingFriends.Add(m_ListMale[indexFriendForMatchMen]);

                            matchingFriends.Add(m_ListFemale[indexFriendForMatchWomen]);

                            break;

                        }
                      
                    }

                }
                else
                {

                    MessageBox.Show("Your List Male or List Female Is Empty!!");

                }

            }
            else
            {

                MessageBox.Show("Your Friend List Is Empty!!");

            }

            return matchingFriends;

        }

        private bool theAgeIsValid(string i_FirstBirthday)
        {

            bool isValidAge = false;

            DateTime birthday = DateTime.Parse(i_FirstBirthday);

            if (DateTime.Now.Year - birthday.Year >= 18)
            {

                isValidAge = !isValidAge;

            }

            return isValidAge;

        }

        private void sortGender()
        {

                foreach (User gender in m_User.Friends)
                {

                    if (gender.Gender.Value == (User.eGender.male))
                    {

                        m_ListMale.Add(gender);

                    }
                    else
                    {

                        if (gender.Gender.Value == (User.eGender.female))
                        {

                            m_ListFemale.Add(gender);

                        }

                    }

                }

        }

    }

}
