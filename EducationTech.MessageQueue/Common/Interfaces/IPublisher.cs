using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Interfaces
{
    public interface IPublisher<TMessage>
    {
        void Publish(TMessage message);
    }
}
