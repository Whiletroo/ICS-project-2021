﻿using System;
using Festival.BL.Models;

namespace Festival.App.Messages
{
    public class SelectedMessage<T> : Message<T>
        where T : IModel
    {
    }
}