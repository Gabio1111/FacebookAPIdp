using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A21_Ex02_GabiOmer_204344626_LiorKricheli_203382494.Decorator
{
    class ScrollableListBoxDecorator:ListBox
    {
        private VScrollBar scrollBar;
        private ListBox ListBox;
        public ScrollableListBoxDecorator()
        {
            scrollBar = new VScrollBar();
            scrollBar.Dock = DockStyle.Right;
        }

        public void Scroll()
        {

        }
    }
}
