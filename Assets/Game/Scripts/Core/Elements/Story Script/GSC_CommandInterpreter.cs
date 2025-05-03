using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GSC_CommandInterpreter : GSC_Singleton<GSC_CommandInterpreter>
{
    public List<GSC_ScriptAction> Commands = new List<GSC_ScriptAction>();

    public static List<string> SplitWithQuotes(string input)
    {
        var result = new List<string>();
        var sb = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (char.IsWhiteSpace(c) && !inQuotes)
            {
                if (sb.Length > 0)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
        }

        return result;
    }

    public bool TryToDecodeCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return false;

        string[] line = SplitWithQuotes(command).ToArray();

        if (line[0].StartsWith('@'))
        {
            if (TryDecodeCommand(line)) return true;
            Debug.Log("Unknown command: " + line[0]);
            return false;
        }
        else return TryDecodeToShowDialogue(line); //If no special character is on it, it can be a dialogue.
    }

    private bool TryDecodeCommand(string[] line)
    {
        if(Commands.IsNullOrEmpty()) return false;

        foreach(var command in Commands)
        {
            GSC_ContainerUnit unit = command.TryDecodeScript(line);
            if(unit != null)
            {
                GSC_ScriptManager.Instance.EnqueueCommand(unit);
            }
        }
        return false;
    }

    //@bg "path/to/sprite" 1 2 3.0 4.0 true
    private bool TryDecodeToChangeBackground(string[] line)
    {
        if (line[0].CompareInv("@bg"))
        {
            var args = line[1..].ToArray();

            // Verifica se o número de argumentos é válido
            if (args.Length != 6)
            {
                Debug.LogError("Bad command format.");
                return false;
            }

            try
            {
                // Extrai e converte os argumentos
                string spriteName = args[0];
                int layer = int.Parse(args[1]);
                int effect = int.Parse(args[2]);
                float duration = float.Parse(args[3]);
                float wait = float.Parse(args[4]);
                bool hide = bool.Parse(args[5]);

                // Cria e enfileira a ação de mudança de plano de fundo
                var action = new GSC_ChangeBackgroundAction();
                action.GraphicName = spriteName;
                action.Layer = layer;
                action.EffectType = (GSC_ChangeEffectType)effect;
                action.FadeTime = duration;
                action.WaitAfterFade = wait;
                action.HideAfterTime = hide;

                GSC_CommandManager.Instance.EnqueueCommand(action.GetAction());
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao processar o comando @bg: {ex.Message}");
                return false;
            }
        }
        else return false;
    }

    //name as something "Hello World!" -fade=0.5 -duration=2.0 -append=true -wait=1.0 -hide=false
    private bool TryDecodeToShowDialogue(string[] line)
    {
        var args = line.ToArray();

        try
        {
            // Extract character name and optional display name
            string nameToShow = string.Empty;
            string character = args[0].Trim();
            if (args.Length > 1 && args[1].CompareInv("as"))
            {
                nameToShow = args[2].Trim();
                args = args.Skip(3).ToArray();
            }
            else
            {
                args = args.Skip(1).ToArray();
            }

            // Ensure at least dialogue text is present
            if (args.Length == 0)
            {
                Debug.LogError("Dialogue Parsing Error: Missing dialogue text.");
                return false;
            }

            // Parse mandatory dialogue
            string dialogue = args[0].Trim();

            // Default values for optional parameters
            float fade = 0.5f;
            float duration = 1f;
            bool append = false;
            float wait = -1f;
            bool hide = false;

            // Parse named optional parameters in any order: key=value
            foreach (var token in args.Skip(1))
            {
                var parts = token.Split('=', 2);
                if (parts.Length != 2)
                {
                    if(parts[0].CompareInv("-a") || parts[0].CompareInv("-append"))
                    {
                        append = true;
                    }
                    else if (parts[0].CompareInv("-h") || parts[0].CompareInv("-hide"))
                    {
                        hide = true;
                    }
                    continue;
                }
                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "-fade":
                    case "-f":
                        float.TryParse(value, out fade);
                        break;
                    case "-duration":
                    case "-d":
                        float.TryParse(value, out duration);
                        break;
                    case "-append":
                    case "-a":
                        bool.TryParse(value, out append);
                        break;
                    case "-wait":
                    case "-w":
                        float.TryParse(value, out wait);
                        break;
                    case "-hide":
                    case "-h":
                        bool.TryParse(value, out hide);
                        break;
                }
            }

            // Create and enqueue the show-dialogue action
            var action = new GSC_ShowDialogueBoxAction
            {
                Character = character,
                NameToShow = nameToShow,
                Dialogue = dialogue,
                FadeTime = fade,
                Duration = duration,
                Append = append,
                WaitTime = wait,
                HideAfterEnd = hide
            };

            GSC_CommandManager.Instance.EnqueueCommand(action.GetAction());
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Dialogue Parsing Error: {ex.Message}");
            return false;
        }
    }

}


