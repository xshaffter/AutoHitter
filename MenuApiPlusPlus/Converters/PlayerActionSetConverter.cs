﻿using Newtonsoft.Json;
using System;
using InControl;

namespace Modding.Converters
{
    /// <summary>
    /// JsonConverter to serialize and deserialize classes that derive from <c>PlayerActionSet</c>.<br/>
    /// The target class needs to have a parameterless constructor
    /// that initializes the player actions that get read/written.<br/>
    /// All of the added actions will get processed,
    /// so if there are unmappable actions, an <c>IMappablePlayerActions</c> interface should be added
    /// to filter the mappable keybinds.
    /// </summary>
    public class PlayerActionSetConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(PlayerActionSet));

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var set = (PlayerActionSet)Activator.CreateInstance(objectType);
            Predicate<string> filter = set is IMappablePlayerActions mpa ? mpa.IsMappable : _ => true;
            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var name = (string)reader.Value;
                if (!filter(name))
                {
                    // value
                    reader.Read();
                    // JsonToken.PropertyName
                    reader.Read();
                    continue;
                }
                var ac = set.GetPlayerActionByName(name);
                reader.Read();
                if (ac != null)
                {
                    if (reader.Value is string val)
                    {
                        if (val == new InputHandler.KeyOrMouseBinding().ToString())
                        {
                            ac.ClearBindings();
                        }
                        else if (KeybindUtil.ParseBinding(val) is InputHandler.KeyOrMouseBinding bind)
                        {
                            ac.ClearBindings();
                            ac.AddKeyOrMouseBinding(bind);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                }
                // JsonToken.PropertyName
                reader.Read();
            }
            return set;
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var set = (PlayerActionSet)value;
            Predicate<string> filter;
            if (set is IMappablePlayerActions mpa) filter = mpa.IsMappable;
            else filter = _ => true;
            writer.WriteStartObject();
            foreach (var ac in set.Actions)
            {
                if (filter(ac.Name))
                {
                    writer.WritePropertyName(ac.Name);
                    writer.WriteValue(ac.GetKeyOrMouseBinding().ToString());
                }
            }
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// An interface to signify mappable player actions to be used in conjunction with <c>PlayerActionSetConverter</c>.
    /// </summary>
    public interface IMappablePlayerActions
    {
        /// <summary>
        /// Checks if the passed in string should be read/written from the JSON stream
        /// </summary>
        /// <param name="name">The name of the player action</param>
        /// <returns></returns>
        public bool IsMappable(string name);
    }
}
