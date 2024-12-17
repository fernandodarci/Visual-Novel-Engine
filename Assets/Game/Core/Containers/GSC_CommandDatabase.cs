using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GSC_CommandDatabase
{
    private class GSC_CommandUnit
    {
        public GSC_CommandHandlerBase commandHandler;
        public Dictionary<string, GSC_ParameterType> Parameters;

        public GSC_CommandUnit(GSC_CommandHandlerBase command, 
            Dictionary<string, GSC_ParameterType> parameters)
        {
            commandHandler = command;
            Parameters = parameters;
        }
    }

    private Dictionary<string, GSC_CommandUnit> Commands = new Dictionary<string, GSC_CommandUnit>();
    private GSC_CommandUnit Current;

    public static GSC_CommandDatabase New() => new GSC_CommandDatabase();

    private GSC_CommandDatabase AddCommand(string commandName, GSC_CommandHandlerBase handler)
    {
        if (!Commands.ContainsKey(commandName))
        {
            GSC_CommandUnit command = new GSC_CommandUnit(handler, new());
            Commands.Add(commandName, command);
            Current = command;
        }
        return this;
    }

    public GSC_CommandDatabase AddCommand(string commandName, Action<GSC_ContainerUnit> action)
    {
        GSC_CommandHandler handler = new GSC_CommandHandler(null, action);
        return AddCommand(commandName, handler);
    }
    public GSC_CommandDatabase AddCommand<TParam>(string commandName, Action<GSC_ContainerUnit<TParam>> action)
    {
        GSC_CommandHandler<TParam> handler = new GSC_CommandHandler<TParam>(null, action);
        return AddCommand(commandName, handler);
    }
    public GSC_CommandDatabase AddCommand(string commandName, Func<GSC_ContainerUnit, IEnumerator> coroutine,
        Action<GSC_ContainerUnit> action)
    {
        GSC_CommandHandler handler = new GSC_CommandHandler(null, action);
        return AddCommand(commandName, handler);
    }
    public GSC_CommandDatabase AddCommand<TParam>(string commandName, Func<GSC_ContainerUnit<TParam>, IEnumerator> coroutine, Action<GSC_ContainerUnit<TParam>> action)
    {
        GSC_CommandHandler<TParam> handler = new GSC_CommandHandler<TParam>(null, action);
        return AddCommand(commandName, handler);
    }

    public string[] ListCommands() => Commands.Keys.ToArray();

    public void HasCommand(string commandName) => Commands.ContainsKey(commandName);

    public GSC_CommandDatabase With(string parameter, GSC_ParameterType type)
    {
        if(Current != null)
        {
            Current.Parameters[parameter] = type;
        }
        return this;
    }

    public GSC_CommandHandlerBase GetHandler(GSC_ContainerUnit unit)
    {
        if (Commands.TryGetValue(unit.Calling, out GSC_CommandUnit command))
        {
            if (command != null && command.commandHandler != null && 
                command.Parameters != null)
            {
                GSC_CommandHandler handler = command.commandHandler as GSC_CommandHandler;
                handler.AttachUnit(unit);
                return handler;
            }
        }
        return null;
    }
    
    public GSC_CommandHandlerBase GetHandler<T>(GSC_ContainerUnit<T> unit)
    {
        if (Commands.TryGetValue(unit.Calling, out GSC_CommandUnit command))
        {
            if (command != null && command.commandHandler != null && 
                command.commandHandler is GSC_CommandHandler<T> @handler
                && command.Parameters != null
                && unit.Validate(unit.Calling, command.Parameters))
            {
                handler.AttachUnit(unit);
                return command.commandHandler;
            }
        }
        return null;
    }

    
}


