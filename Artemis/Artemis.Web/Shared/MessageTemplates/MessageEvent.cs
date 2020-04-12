using System;
using System.Collections.Generic;

namespace Artemis.Web.Shared.MessageTemplates
{
    public enum MessageEvent
    {
        EventCreated,
        EventUpdated,

        EventUpdateCreated,
    }

    public static class MessageEventExtensions
    {
        public static Dictionary<string, int> GetNameValueCollection(this MessageEvent messageEvent)
        {
            var names = Enum.GetNames(typeof(MessageEvent));
            var values = Enum.GetValues(typeof(MessageEvent));


            var dictionary = new Dictionary<string, int>();
            for (int i = 0; i < names.Length; i++)
            {
                dictionary.Add(names[i], (int) values.GetValue(i));
            }

            return dictionary;
        }
    }
}