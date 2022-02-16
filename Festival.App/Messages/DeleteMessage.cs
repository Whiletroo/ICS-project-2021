using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public class DeleteMessage<T> : Message<T>
        where T : IModel
    {
    }
}