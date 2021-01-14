using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{

    internal class FriendAnalyzer
    {

        internal FriendAnalyzer() { }

        internal double Analyze(User i_user)
        {

            int sumAvarage = 0;

            List<int> analyzeList = new List<int>();

            if (i_user != null)
            {

                try
                {

                    analyzeList.Add(i_user.Posts.Count);

                    analyzeList.Add(i_user.Events.Count);

                    analyzeList.Add(i_user.Albums.Count);

                    analyzeList.Add(i_user.Checkins.Count);

                    analyzeList.Add(i_user.Friends.Count);

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
