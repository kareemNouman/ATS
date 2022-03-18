using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Service.Messages
{
    public interface INotify
    {
        bool HasErrors { get; }

        void AddMessage(string message);


        List<string> GetMessage();
    }
}
