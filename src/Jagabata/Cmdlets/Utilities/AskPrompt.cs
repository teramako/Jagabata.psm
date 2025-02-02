using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;

namespace Jagabata.Cmdlets.Utilities
{
    internal class AskPrompt(PSHost host)
    {
        private void PrintHeader(string label, string defaultValue, string helpMessage = "", string helpIndicator = "", bool showDefault = true)
        {
            var gb = Console.BackgroundColor;
            host.UI.Write(ConsoleColor.Blue, gb, "<== ");
            host.UI.Write($"{label}");
            if (showDefault)
            {
                host.UI.Write(" (Default: ");
                host.UI.Write(ConsoleColor.DarkYellow, gb, $"{defaultValue}");
                host.UI.WriteLine(")");
            }
            else
            {
                host.UI.WriteLine();
            }
            if (!string.IsNullOrEmpty(helpMessage))
            {
                host.UI.WriteLine(helpMessage);
            }
            if (!string.IsNullOrEmpty(helpIndicator))
            {
                host.UI.WriteLine(ConsoleColor.DarkGray, gb, helpIndicator);
            }
        }
        private void WriteError(string errorMessage)
        {
            host.UI.WriteLine(ConsoleColor.Red, (ConsoleColor)(-1), errorMessage);
        }
        private void PrintHelp(string label, string helpMessage = "", string helpIndicator = "")
        {
            var gb = Console.BackgroundColor;
            host.UI.Write(ConsoleColor.Blue, gb, "<== ");
            host.UI.WriteLine(label);
            if (!string.IsNullOrEmpty(helpMessage))
            {
                host.UI.WriteLine(helpMessage);
            }
            if (!string.IsNullOrEmpty(helpIndicator))
            {
                host.UI.WriteLine(ConsoleColor.DarkGray, gb, helpIndicator);
            }
        }
        /// <summary>
        /// List input prompt
        /// </summary>
        /// <param name="label">Prompt label</param>
        /// <param name="promptKey"></param>
        /// <param name="defaultValues"></param>
        /// <param name="helpMessage"></param>
        /// <param name="answers"></param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskList<T>(string label,
                               string promptKey,
                               IEnumerable<string>? defaultValues,
                               string helpMessage,
                               [MaybeNullWhen(false)] out Answer<List<T>> answers)
        {
            answers = null;
            var results = new List<T>();
            var index = 0;
            var defaultValuString = $"[{string.Join(", ", defaultValues ?? [])}]";
            var helpIndicator = "(!? => Show help, !> => Suspend, !! => Comfirm even if the list is empty)";
            PrintHeader(label, defaultValuString, helpMessage, helpIndicator);
            if (string.IsNullOrEmpty(promptKey))
                promptKey = label;

            do
            {
                var fieldLabel = $"{promptKey}[{index}]";
                var currentHelpMessage = (string.IsNullOrEmpty(helpMessage) ? "" : $"{helpMessage}\n")
                                         + $"CurrentValues: [{string.Join(", ", results.Select(item => $"{item}"))}]";
                if (!TryPromptOneInput(fieldLabel, out var inputString))
                {
                    return false;
                }
                if (inputString.StartsWith('!'))
                {
                    var command = inputString[1..].Trim();
                    switch (command)
                    {
                        case "?":
                            PrintHelp(label, currentHelpMessage, helpIndicator);
                            continue;
                        case "!": // return as the list is fulfilled even if the list is empty.
                            answers = new Answer<List<T>>(results, false);
                            return true;
                        case ">":
                            host.EnterNestedPrompt();
                            continue;
                    }
                }

                if (string.IsNullOrEmpty(inputString))
                {
                    var isEmpty = results.Count == 0;
                    answers = new Answer<List<T>>(results, isEmpty);
                    return true;
                }
                else
                {
                    try
                    {
                        var val = LanguagePrimitives.ConvertTo<T>(inputString);
                        results.Add(val);
                        index = results.Count;
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex.Message);
                    }
                }
            }
            while (true);
        }
        /// <summary>
        /// String input prompt
        /// </summary>
        /// <param name="label">Prompt label</param>
        /// <param name="promptKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="helpMessage"></param>
        /// <param name="answer"></param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool Ask(string label,
                        string promptKey,
                        string? defaultValue,
                        string helpMessage,
                        [MaybeNullWhen(false)] out Answer<string> answer)
        {
            answer = null;
            var defaultValueString = $"\"{defaultValue}\"";
            var helpIndicator = """
                (!? => Show help, !! => Use default, !> => Suspend, Empty => Skip, ("", '', $null) => Specify empty string)
                """;
            PrintHeader(label, defaultValueString, helpMessage, helpIndicator);
            var help = (string.IsNullOrEmpty(helpMessage) ? "" : $"{helpMessage}\n")
                       + $"Default: {defaultValueString}";
            if (string.IsNullOrEmpty(promptKey))
                promptKey = label;

            do
            {
                var inputed = false;
                if (!TryPromptOneInput(promptKey, out var inputString))
                {
                    return false;
                }
                if (inputString.StartsWith('!'))
                {
                    var command = inputString[1..].Trim();
                    switch (command)
                    {
                        case "?":
                            PrintHelp(label, help, helpIndicator);
                            continue;
                        case "!":
                            answer = new Answer<string>(defaultValue ?? string.Empty);
                            return true;
                        case ">":
                            host.EnterNestedPrompt();
                            continue;
                        default:
                            inputString = command;
                            break;
                    }
                }
                switch (inputString)
                {
                    case "\"\"":
                    case "''":
                    case "$null":
                        inputed = true;
                        inputString = string.Empty;
                        break;

                }
                if (string.IsNullOrEmpty(inputString))
                {
                    answer = new Answer<string>(defaultValue ?? string.Empty, !inputed);
                    return true;
                }
                else
                {
                    answer = new Answer<string>(inputString);
                    return true;
                }
            }
            while (true);
        }
        /// <summary>
        /// Number input prompt
        /// </summary>
        /// <typeparam name="T">inputed data type (mainly number type)</typeparam>
        /// <param name="label">Prompt label</param>
        /// <param name="promptKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="helpMessage"></param>
        /// <param name="required"></param>
        /// <param name="answer"></param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool Ask<T>(string label,
                           string promptKey,
                           T? defaultValue,
                           string helpMessage,
                           bool required,
                           [MaybeNullWhen(false)] out Answer<T> answer) where T : struct
        {
            answer = null;
            string helpIndicator;
            string help = helpMessage;
            if (defaultValue is null)
            {
                helpIndicator = """
                    (!? => Show help, !> => Suspend)
                    """;
            }
            else
            {
                helpIndicator = """
                    (!? => Show help, !! => Use default, !> => Suspend, Empty => Skip)
                    """;
                help += (string.IsNullOrEmpty(help) ? "" : "\n") + $"Default: {defaultValue}";
            }

            PrintHeader(label, $"{defaultValue}", helpMessage, helpIndicator, showDefault: defaultValue is not null);
            if (string.IsNullOrEmpty(promptKey))
                promptKey = label;

            do
            {
                var inputed = false;
                if (!TryPromptOneInput(promptKey, out var inputString))
                {
                    return false;
                }
                if (inputString.StartsWith('!'))
                {
                    var command = inputString[1..].Trim();
                    switch (command)
                    {
                        case "?":
                            PrintHelp(label, help, helpIndicator);
                            continue;
                        case "!":
                            if (defaultValue is null) break;
                            inputed = true;
                            inputString = string.Empty;
                            break;
                        case ">":
                            host.EnterNestedPrompt();
                            continue;
                        default:
                            inputString = command;
                            break;
                    }
                }
                if (string.IsNullOrEmpty(inputString))
                {
                    if (required && defaultValue is null)
                    {
                        WriteError("Empty value is not allowed.");
                        continue;
                    }
                    answer = defaultValue is null
                        ? new Answer<T>(default, !inputed)
                        : new Answer<T>((T)defaultValue, !inputed);
                    return true;
                }
                else
                {
                    try
                    {
                        answer = new Answer<T>(LanguagePrimitives.ConvertTo<T>(inputString));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex.Message);
                    }
                }
            }
            while (true);
        }
        /// <summary>
        /// Boolean input prompt
        /// </summary>
        /// <param name="label">Prompt label</param>
        /// <param name="answer"></param>
        /// <param name="trueParameter"></param>
        /// <param name="falseParameter"></param>
        /// <param name="defaultValue"</param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskBool(string label, bool defaultValue,
                            (string label, string helpMessage) trueParameter,
                            (string label, string helpMessage) falseParameter,
                            [MaybeNullWhen(false)] out Answer<bool> answer)
        {
            answer = null;
            var choices = new Collection<ChoiceDescription>
            {
                new($"{trueParameter.label} (&Yes)", trueParameter.helpMessage),
                new($"{falseParameter.label} (&No)", falseParameter.helpMessage)
            };

            PrintHeader(label, "", $"{trueParameter.label} (Yes) or {falseParameter.label} (No)", showDefault: false);
            var res = host.UI.PromptForChoice("", "", choices, defaultValue ? 0 : 1);
            switch (res)
            {
                case 0:
                    answer = new Answer<bool>(true);
                    return true;
                case 1:
                    answer = new Answer<bool>(false);
                    return true;
                default:
                    return false;
            }

        }
        /// <summary>
        /// String input prompt
        /// </summary>
        /// <typeparam name="TEnum">inputed data type (Enum)</typeparam>
        /// <param name="label">Prompt label</param>
        /// <param name="answer"></param>
        /// <param name="helpMessage"></param>
        /// <param name="defaultValue"</param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskEnum<TEnum>(string label, TEnum defaultValue, string helpMessage, out Answer<TEnum> answer) where TEnum : Enum
        {
            var defaultValueString = $"{defaultValue:g} ({defaultValue:d})";
            PrintHeader(label, defaultValueString, helpMessage);
            var choices = new Collection<ChoiceDescription>();
            var defaultValueIndex = -1;
            var enumType = typeof(TEnum);

            var enumValues = (TEnum[])Enum.GetValues(enumType);
            for (var i = 0; i < enumValues.Length; i++)
            {
                var str = $"{enumValues[i]:g}";
                if (str == $"{defaultValue}")
                {
                    defaultValueIndex = i;
                }
                choices.Add(new ChoiceDescription($"{str}(&{i})", str));
            }
            var res = host.UI.PromptForChoice("", "", choices, defaultValueIndex);
            if (res >= 0 && res < enumValues.Length)
            {
                answer = new Answer<TEnum>(enumValues[res]);
                return true;
            }
            answer = new Answer<TEnum>(defaultValue, true);
            return false;
        }
        /// <summary>
        /// Show input prompt for select one.
        /// </summary>
        /// <param name="label">Prompt label</param>
        /// <param name="fields"></param>
        /// <param name="defaultValue"></param>
        /// <param name="helpMessage"></param>
        /// <param name="answer"</param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskSelectOne(string label,
                                 IList<(string Value, string Description)> fields,
                                 string defaultValue,
                                 string helpMessage,
                                 [MaybeNullWhen(false)] out Answer<string> answer)
        {
            answer = null;
            PrintHeader(label, defaultValue, helpMessage);
            var choices = new Collection<ChoiceDescription>();
            var defaultValueIndex = -1;
            for (var i = 0; i < fields.Count; i++)
            {
                var (Value, Description) = fields[i];
                if (Value == defaultValue)
                    defaultValueIndex = i;

                choices.Add(new ChoiceDescription($"{Value}(&{i})", Description));
            }
            int res = host.UI.PromptForChoice("", "", choices, defaultValueIndex);
            if (res >= 0 && res < fields.Count)
            {
                answer = new Answer<string>(fields[res].Value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Multi selectable prompts.
        /// </summary>
        /// <param name="label">Prompt label</param>
        /// <param name="promptKey"></param>
        /// <param name="fields"></param>
        /// <param name="defaultValues"></param>
        /// <param name="helpMessage"></param>
        /// <param name="answer"</param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskSelectMulti(string label, string promptKey,
                                   IList<(string Value, string Description)> fields,
                                   IList<string> defaultValues,
                                   string helpMessage,
                                   [MaybeNullWhen(false)] out Answer<string[]> answer)
        {
            answer = null;
            PrintHeader(label, $"[{string.Join(", ", defaultValues)}]", helpMessage);
            var maxCount = fields.Count;
            var results = new List<string>();
            var remainingFields = new List<(string Value, string Description)>(fields);
            var remainingDefaultValues = new List<string>(defaultValues);
            if (string.IsNullOrEmpty(promptKey))
                promptKey = label;

            do
            {
                var choices = new Collection<ChoiceDescription>();
                var fieldIndex = 1;
                var defaultValueIndex = 0;

                choices.Add(new ChoiceDescription("&Confirm", "Confirm and finish multi select prompts"));
                foreach (var (Value, Description) in remainingFields)
                {
                    if (remainingDefaultValues.Contains(Value))
                        defaultValueIndex = fieldIndex;

                    choices.Add(new ChoiceDescription($"{Value}(&{fieldIndex})", Description));
                    fieldIndex++;
                }

                int selectedIndex = host.UI.PromptForChoice("", $"{promptKey}[{results.Count}]", choices, defaultValueIndex);
                if (selectedIndex == 0)
                {
                    break;
                }
                else if (selectedIndex > 0 && selectedIndex < remainingFields.Count)
                {
                    var selectedValue = remainingFields[selectedIndex - 1].Value;
                    results.Add(selectedValue);
                    remainingFields.RemoveAt(selectedIndex - 1);
                    remainingDefaultValues.Remove(selectedValue);
                }
                else
                {
                    return false;
                }
            } while (results.Count <= maxCount);

            answer = new Answer<string[]>([.. results]);
            return true;
        }
        /// <summary>
        /// Password prompt
        /// </summary>
        /// <param name="caption">Header label</param>
        /// <param name="promptKey"></param>
        /// <param name="answer"></param>
        /// <param name="helpMessage"></param>
        /// <returns>Whether the prompt is inputed(<c>true</c>) or Canceled(<c>false</c>)</returns>
        public bool AskPassword(string caption,
                                string promptKey,
                                string helpMessage,
                                [MaybeNullWhen(false)] out Answer<SecureString> answer)
        {
            answer = null;
            PrintHeader(caption, "", helpMessage, showDefault: false);
            if (string.IsNullOrEmpty(promptKey))
                promptKey = "Password";

            var fd = new FieldDescription(promptKey);
            fd.SetParameterType(typeof(SecureString));
            var fdc = new Collection<FieldDescription>() { fd };
            Dictionary<string, PSObject>? result = host.UI.Prompt("", "", fdc);
            if (result is not null && result.TryGetValue(promptKey, out PSObject? pso))
            {
                if (pso is null || pso.BaseObject is null)
                {
                    return false;
                }
                if (pso.BaseObject is SecureString secureString)
                {
                    answer = new Answer<SecureString>(secureString);
                    return true;
                }
            }
            return false;
        }
        private bool TryPromptOneInput(string label, out string inputString)
        {
            inputString = string.Empty;
            var fd = new FieldDescription(label);
            fd.SetParameterType(typeof(string));
            var fdc = new Collection<FieldDescription>() { fd };
            Dictionary<string, PSObject?>? result = host.UI.Prompt("", "", fdc);
            if (result is not null && result.TryGetValue(label, out PSObject? val))
            {
                if (val is null || val.BaseObject is null)
                {
                    return false;
                }
                inputString = val.BaseObject as string ?? string.Empty;
                return true;
            }
            return false;
        }

        public class Answer<T>(T input, bool isEmpty = false)
        {
            public T Input { get; } = input;
            public bool IsEmpty { get; } = isEmpty;
        }
    }
}
