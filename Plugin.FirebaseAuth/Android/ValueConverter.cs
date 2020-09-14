using System;
using System.Collections.Generic;
using Android.Runtime;
using AndroidX.Collection;

namespace Plugin.FirebaseAuth
{
    public static class ValueConverter
    {
        public static object? Convert(object? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case Java.Lang.Boolean @boolean:
                    return (bool)@boolean;
                case bool @bool:
                    return @bool;
                case Java.Lang.Short @short:
                    return (long)(short)@short;
                case short @short:
                    return (long)@short;
                case Java.Lang.Integer @integer:
                    return (long)(int)@integer;
                case int @int:
                    return (long)@int;
                case Java.Lang.Long @long:
                    return (long)@long;
                case long @long:
                    return @long;
                case Java.Lang.Float @float:
                    return (double)(float)@float;
                case float @float:
                    return (double)@float;
                case Java.Lang.Double @double:
                    return (double)@double;
                case double @double:
                    return @double;
                case Java.Lang.Number @number:
                    {
                        var doubleValue = @number.DoubleValue();
                        var longValue = @number.LongValue();
                        if (doubleValue == longValue)
                        {
                            return longValue;
                        }
                        return doubleValue;
                    }
                case Java.Lang.String @string:
                    return @string.ToString();
                case string @string:
                    return @string;
                case JavaList javaList:
                    {
                        var list = new List<object?>();
                        foreach (var val in javaList)
                        {
                            list.Add(Convert(val));
                        }
                        return list;
                    }
                case JavaDictionary javaDictionary:
                    {
                        var dict = new Dictionary<string, object?>();
                        var enumerator = javaDictionary.GetEnumerator();

                        while (enumerator.MoveNext())
                        {
                            var entry = enumerator.Entry;
                            dict[entry.Key.ToString()] = Convert(entry.Value);
                        }
                        return dict;
                    }
                case ArrayMap arrayMap:
                    {
                        var dict = new Dictionary<string, object?>();
                        foreach (var key in arrayMap.KeySet())
                        {
                            var keyStr = key.ToString();
                            dict[keyStr] = Convert(arrayMap.Get(keyStr));
                        }
                        return dict;
                    }
                default:
                    if (value.ToString() == "null")
                    {
                        return null;
                    }
                    return value;
            }
        }
    }
}
