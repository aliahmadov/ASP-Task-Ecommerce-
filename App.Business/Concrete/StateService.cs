using App.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Concrete
{
    public class StateService : IStateService
    {


        public bool CurrentState { get; set; }
        public bool CurrentLHState { get; set; }

        public StateService()
        {
            CurrentState = true;
            CurrentLHState = true;
        }

        public void SetState(string url)
        {
            if (url.Contains("azState"))
                CurrentState = !CurrentState;

            if(url.Contains("lhState"))
                CurrentLHState = !CurrentLHState;
        }
    }
}
