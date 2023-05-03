using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Abstract
{
    public interface IStateService
    {
        bool CurrentState { get; set; }
        bool CurrentLHState { get; set; }   
        void SetState(string url);

    }
}
