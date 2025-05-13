using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models;

public class MessageQueue : IMessageQueue
{
    private readonly Queue<object> _queue = new();

    public void Enqueue<T>(T message)
    {
        _queue.Enqueue(message!);
    }

    public T? Dequeue<T>()
    {
        if (_queue.Count == 0) return default;
        var item = _queue.Dequeue();
        return item is T typedItem ? typedItem : default;
    }

    public IReadOnlyCollection<object> PeekAll()
    {
        return _queue.ToList().AsReadOnly();
    }
}