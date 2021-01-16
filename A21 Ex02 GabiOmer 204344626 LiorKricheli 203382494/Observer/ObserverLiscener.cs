using A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494
{
    public class ObserverLiscener
    {

        protected readonly Action NotifyAbstractParentPopulateRowsCompleted;
        private readonly List<IObserverLiscener> r_Listeners = new List<IObserverLiscener>();

        public ObserverLiscener()
        {

            NotifyAbstractParentPopulateRowsCompleted += notifyAllRowsPopulatedObservers;

        }

        public void AttachListener(IObserverLiscener i_listener)
        {

            this.r_Listeners.Add(i_listener);

        }

        public void DetachListener(IObserverLiscener i_listener)
        {

            if (this.r_Listeners.Contains(i_listener))
            {

                this.r_Listeners.Remove(i_listener);

            }

        }

        private void notifyAllRowsPopulatedObservers()
        {

            foreach (IObserverLiscener observer in r_Listeners)
            {

                observer.Update();

            }

        }

    }
}
