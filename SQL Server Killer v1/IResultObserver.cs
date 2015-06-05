using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_Server_Killer_v1
{
    public interface IResultObserver
    {
        void observeResult(bool result, string connString);
        
    }
}
