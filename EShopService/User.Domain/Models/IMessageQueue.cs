using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models;

public interface IMessageQueue
{
    void Enqueue<T>(T message);
    T? Dequeue<T>();
    IReadOnlyCollection<object> PeekAll();
}

