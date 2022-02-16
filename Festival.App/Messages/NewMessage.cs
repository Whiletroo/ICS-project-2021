using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public class NewMessage<T> : Message<T>
        where T : IModel
    {
    }
}