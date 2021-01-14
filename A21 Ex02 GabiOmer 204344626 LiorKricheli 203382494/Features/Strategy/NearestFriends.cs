
using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{
    public struct FriendByCoordinate
    {

        public double m_Distance;

        public string m_Name;

    }

    internal class NearestFriends // Mechanism
    {

        private User m_loggedUser;
        private List<FriendByCoordinate> listFriendsInRadius;
        private FriendByCoordinate closeFriends;
        
        public Func<Enums.eDistanceMethod,double,int,bool> distanceStrategy { get; set; }

        internal NearestFriends(User i_UserSearchFriendsNeares)
        {

            m_loggedUser = i_UserSearchFriendsNeares;
            listFriendsInRadius = new List<FriendByCoordinate>();


        }

        internal List<FriendByCoordinate> NearestFriendsMeth(int i_radius,Enums.eDistanceMethod eDistance)
        {


            double distance = 0;

            int index = 0;

            try
            {

                if (m_loggedUser.Friends != null && m_loggedUser.Location.Name != null)
                {

                    listFriendsInRadius = new List<FriendByCoordinate>();

                    foreach (User friend in m_loggedUser.Friends)
                    {
                        if (friend.Location.Location != null)
                        {

                            distance = this.distance((double)m_loggedUser.Location.Location.Latitude,
                               (double)m_loggedUser.Location.Location.Longitude,
                               (double)friend.Location.Location.Latitude,
                               (double)friend.Location.Location.Longitude);

                            closeFriends.m_Distance = distance;
                            closeFriends.m_Name = friend.Name;
                            
                            if(distanceStrategy.Invoke(eDistance,distance,i_radius)) //injection
                            {
                                listFriendsInRadius.Insert(index++, closeFriends);
                            }


                        }
                        else
                        {

                            break;

                        }

                    }

                }

                return listFriendsInRadius;

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
