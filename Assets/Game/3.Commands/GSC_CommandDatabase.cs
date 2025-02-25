using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GSC_CommandDatabase
{
    private class GSC_CommandUnit
    {
        public GSC_CommandHandler commandHandler;
        public Dictionary<string, GSC_Parameter> Parameters;

        public GSC_CommandUnit(GSC_CommandHandler command, 
            Dictionary<string, GSC_Parameter> parameters)
        {
            commandHandler = command;
            Parameters = parameters;
        }
    }

    private Dictionary<string, GSC_CommandUnit> Commands = new Dictionary<string, GSC_CommandUnit>();
    private GSC_CommandUnit Current;

    public static GSC_CommandDatabase New() => new GSC_CommandDatabase();

    private GSC_CommandDatabase AddCommand(string commandName, GSC_CommandHandler handler)
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

    public GSC_CommandDatabase AddCommand(string commandName, 
        Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator> func)
    {
        GSC_CommandHandler handler = new GSC_CommandHandler(func, null);
        return AddCommand(commandName, handler);
    }

    public GSC_CommandDatabase AddCommand(string commandName, 
        Func<GSC_ContainerUnit, Func<bool>, Func<bool>, IEnumerator> func, Action<GSC_ContainerUnit> action)
    {
        GSC_CommandHandler handler = new GSC_CommandHandler(func, action);
        return AddCommand(commandName, handler);
    }

    public List<GSC_UnitStructure> GetUnitStructures()
    {
        if(Commands.Count == 0) return new List<GSC_UnitStructure>();

        List<GSC_UnitStructure> units = new List<GSC_UnitStructure>();
        foreach(var Kvp in Commands)
        {
            GSC_UnitStructure structure = new GSC_UnitStructure()
            {
                Calling = Kvp.Key,
            };
            
            if(Kvp.Value != null && Kvp.Value.Parameters != null && Kvp.Value.Parameters.Count > 0)
            {
                structure.Parameters = new(Kvp.Value.Parameters.Values.ToArray());
            }
            units.Add(structure);
        }
        return units;
    }
    
    public void HasCommand(string commandName) => Commands.ContainsKey(commandName);

    public GSC_CommandDatabase With(GSC_Parameter type)
    {
        if(Current != null)
        {
            Current.Parameters[type.ParameterName] = type;
        }
        return this;
    }

    public GSC_CommandHandler GetHandler(GSC_ContainerUnit unit)
    {
        if (Commands.TryGetValue(unit.Calling, out GSC_CommandUnit command))
        {
            if (command != null && command.commandHandler != null && 
                command.Parameters != null)
            {
                GSC_CommandHandler handler = command.commandHandler;
                handler.SetUnit(unit);
                Debug.Log($"Return common unit {unit.Calling}");
                return handler;
            }
        }
        return null;
    }
}
