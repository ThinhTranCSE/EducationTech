using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationTech.MessageQueue.Common.Interfaces
{
    public interface IConsumer<TMessage>
        where TMessage : class
    {
        void Consume(TMessage message);
    }
}
