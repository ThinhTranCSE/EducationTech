using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Interfaces
{
    public interface IMessageQueue
    {
        string Name { get; }
        string Key { get; }
        bool Durable { get; }
        bool Exclusive { get; }
        bool AutoDelete { get; }
        IDictionary<string, object> Arguments { get; }
    }
}
