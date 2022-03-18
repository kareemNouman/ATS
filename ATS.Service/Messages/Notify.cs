using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Service.Messages
{
    public class Notify : INotify
    {
        public Notify()
        {
            _messages = new List<string>();
        }

        private List<string> _messages { get; set; }

        public bool HasErrors
        {
            get
            {
                return _messages.Any();
            }
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }

        public List<string> GetMessage()
        {
            return _messages;
        }
    }
}
