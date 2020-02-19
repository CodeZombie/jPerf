
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap
{

    class StateMachine
    {
        private List<State> States;
        private int CurrentStateIndex;

        public StateMachine(State RootState)
        {
            this.States = new List<State>();
            AddState(RootState);
            this.SetState(RootState.GetIndex());
        }

        public int GetCurrentStateIndex()
        {
            return this.CurrentStateIndex;
        }

        public void AddState(State State)
        {
#if DEBUG
            Console.WriteLine("Registering state with the name: '" + State.GetIndex() + "'");
#endif
            this.States.Add(State);
        }

        public void SetState(int StateIndex)
        {
#if DEBUG
            Console.WriteLine("Attempting to set state to: '" + StateIndex.ToString() + "'");
#endif
            if (this.CurrentStateIndex == StateIndex)
            {
#if DEBUG
                Console.WriteLine("WARNING: Attempting to set program state to current state: '" + StateIndex.ToString() + "'");
#endif
            }

            var Match = this.States.FirstOrDefault(s => s.GetIndex().Equals(StateIndex));
            if(Match == null)
            {
#if DEBUG
                Console.WriteLine("ERROR: Could not change to unknown state with name: '" + StateIndex.ToString() + "'");
#endif
                return;
            }

            this.CurrentStateIndex = Match.GetIndex();
            Console.WriteLine("ACTIVE STATE: " + Match.GetIndex().ToString());
            Match.DoTransitionAction();
        }

    }
}
