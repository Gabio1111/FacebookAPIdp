
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    public class FeaturesFacade
    {

        private NearestFriends m_CloseFriends;
        private FriendAnalyzer m_FriendAnalyzer;
        private MatchMyFriends m_FriendMatcher;
        private User           m_LoggedUser;

        public FeaturesFacade(User i_UserLogged)
        {

            m_LoggedUser = i_UserLogged;

        }


        public List<FriendByCoordinate> InitiateCloseFriends(int i_Radius, Enums.eDistanceMethod eDistance )
        {

            if (eDistance.Equals(Enums.eDistanceMethod.farOffFriends))
            {

                m_CloseFriends = new NearestFriends(m_LoggedUser) { DistanceStrategy = (distType, distance, radius) => distType == Enums.eDistanceMethod.farOffFriends && distance >= radius };

            }
            else
            {
                
                m_CloseFriends = new NearestFriends(m_LoggedUser) { DistanceStrategy = (distType, distance, radius) => distType == Enums.eDistanceMethod.closestFriends && distance <= radius };

            }

            return m_CloseFriends.NearestFriendsMeth(i_Radius, eDistance);

        }

        public double InitiateFriendAnalyzer(User i_User)
        {

            m_FriendAnalyzer = new FriendAnalyzer();

            return m_FriendAnalyzer.Analyze(i_User);

        }

        public List<User> InitiateFriendMatcher()
        {

            m_FriendMatcher = new MatchMyFriends(m_LoggedUser);

            return m_FriendMatcher.MatchingMyFriends();

        }

    }

}
