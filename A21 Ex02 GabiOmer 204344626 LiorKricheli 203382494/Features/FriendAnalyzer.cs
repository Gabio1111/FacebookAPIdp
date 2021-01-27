using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex03_GabiOmer_204344626_LiorKricheli_203382494
{

    internal class FriendAnalyzer
    {

        internal FriendAnalyzer() { }

        internal double Analyze(User i_User)
        {

            int sumAvarage = 0;

            List<int> analyzeList = new List<int>();

            if (i_User != null)
            {

                try
                {

                    analyzeList.Add(i_User.Posts.Count);

                    analyzeList.Add(i_User.Events.Count);

                    analyzeList.Add(i_User.Albums.Count);

                    analyzeList.Add(i_User.Checkins.Count);

                    analyzeList.Add(i_User.Friends.Count);

                }
                catch
                {

                    throw new Exception("Faceboook Exception");

                }

                foreach (int numberCountsOfFeatures in analyzeList)
                {

                    sumAvarage += numberCountsOfFeatures;

                }

            }

            return (sumAvarage / analyzeList.Count);

        }

    }

}
