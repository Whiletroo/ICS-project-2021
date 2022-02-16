using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public class AddedMessage<T> : Message<T>
        where T : IModel
    {
    }
}
