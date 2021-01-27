
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    public struct FriendByCoordinate
    {

        public double m_Distance;
        public string m_Name;

    }

    internal class NearestFriends // Mechanism
    {

        private User                     m_LoggedUser;
        private List<FriendByCoordinate> m_ListFriendsInRadius;
        private FriendByCoordinate       m_CloseFriends;       
        public Func<Enums.eDistanceMethod,double,int,bool> DistanceStrategy { get; set; }

        internal NearestFriends(User i_UserSearchFriendsNeares)
        {

            m_LoggedUser = i_UserSearchFriendsNeares;
            m_ListFriendsInRadius = new List<FriendByCoordinate>();

        }

        internal List<FriendByCoordinate> NearestFriendsMeth(int i_Radius, Enums.eDistanceMethod eDistance)
        {

            double distance = 0;
            int index = 0;

            try
            {

                if (m_LoggedUser.Friends != null && m_LoggedUser.Location.Name != null)
                {

                    m_ListFriendsInRadius = new List<FriendByCoordinate>();

                    foreach (User friend in m_LoggedUser.Friends)
                    {

                        if (friend.Location.Location != null)
                        {

                            distance = this.distance((double)m_LoggedUser.Location.Location.Latitude,
                               (double)m_LoggedUser.Location.Location.Longitude,
                               (double)friend.Location.Location.Latitude,
                               (double)friend.Location.Location.Longitude);

                            m_CloseFriends.m_Distance = distance;
                            m_CloseFriends.m_Name = friend.Name;
                            
                            if(DistanceStrategy.Invoke(eDistance, distance, i_Radius)) //injection
                            {
                                m_ListFriendsInRadius.Insert(index++, m_CloseFriends);
                            }

                        }
                        else
                        {

                            break;

                        }

                    }

                }

                return m_ListFriendsInRadius;

            }
            catch
            {

                return null;

            }

        }

        internal double toRadians(double i_AngleIn10thofaDegree)
        {

            // Angle in 10th 
            // of a degree 
            return (i_AngleIn10thofaDegree * Math.PI) / 180;

        }

        internal double distance(double i_FirstLatitude, double i_SecondLatitude
                               , double i_FirstLongitude, double i_SecondLongitude)
        {

            // The math module contains  
            // a function named toRadians  
            // which converts from degrees  
            // to radians. 
            double firstLatitudeAndCalculateToRadian = toRadians(i_FirstLatitude);
            double secondLatitudeAndCalculateToRadian = toRadians(i_SecondLatitude);

            double firstLongitudeAndCalculateToRadian = toRadians(i_FirstLongitude);
            double secondLongitudeAndCalculateToRadian = toRadians(i_SecondLongitude);

            // Haversine formula  
            double distanceLongitude = secondLongitudeAndCalculateToRadian - firstLongitudeAndCalculateToRadian;
            double distanceLatitude = secondLatitudeAndCalculateToRadian - firstLatitudeAndCalculateToRadian;
            double summaryOfDistance = Math.Pow(Math.Sin(distanceLatitude / 2), 2) +
                       Math.Cos(i_FirstLatitude) * Math.Cos(i_SecondLatitude) *
                       Math.Pow(Math.Sin(distanceLongitude / 2), 2);

            double Angle = 2 * Math.Asin(Math.Sqrt(summaryOfDistance));

            // Radius of earth in  
            // kilometers. Use 3956  
            // for miles 
            double radiusOfEarth = 6371;

            // calculate the result 
            return (Angle * radiusOfEarth);

        }

    }

}