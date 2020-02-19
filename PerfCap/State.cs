using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap
{
    class State
    {
        private int Index;
        private Action TransitionAction;

        public State(int Index, Action TransitionAction)
        {
            this.Index = Index;
            this.TransitionAction = TransitionAction;
        }

        public int GetIndex()
        {
            return this.Index;
        }

        public void DoTransitionAction()
        {
            this.TransitionAction();
        }
    }
}
