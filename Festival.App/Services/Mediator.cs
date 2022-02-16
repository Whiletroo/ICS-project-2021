using System;
using System.Collections.Generic;
using System.Linq;
using Festival.App.Messages;

namespace Festival.App.Services
{
    public class Mediator : IMediator
    {
        private readonly Dictionary<Type, List<Delegate>> _registeredActions = new();

        public void Register<TMessage>(Action<TMessage> action) where TMessage : IMessage
        {
            var key = typeof(TMessage);
            if(!_registeredActions.TryGetValue(key, out _))
            {
                _registeredActions[key] = new List<Delegate>();
            }
            _registeredActions[key].Add(action);
        }

        public void UnRegister<TMessage>(Action<TMessage> action) where TMessage : IMessage
        {
            var key = typeof(TMessage);
            if (!_registeredActions.TryGetValue(key, out var actions))
                return;

            var actionList = actions.ToList();
            actionList.Remove(action);
            _registeredActions[key] = new List<Delegate>(actionList);
        }

        public void Send<TMessage>(TMessage message) where TMessage : IMessage
        {
            if (!_registeredActions.TryGetValue(message.GetType(), out var actions))
                return;

            foreach(var action in actions.Where(action => action != null))
            {
                action.DynamicInvoke(message);
            }
        }
    }
}
