using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public abstract class Message<T> : IMessage where T : IModel
    {
        public Guid Id { get; set; }
    }
}
