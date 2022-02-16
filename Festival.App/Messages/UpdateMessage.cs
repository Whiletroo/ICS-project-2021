using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public class UpdateMessage<T> : Message<T>
        where T : IModel
    {
    }
}