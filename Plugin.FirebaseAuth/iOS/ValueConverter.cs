using System;
using System.Collections.Generic;
using Foundation;

namespace Plugin.FirebaseAuth
{
    internal static class ValueConverter
    {
        public static object? Convert(NSObject? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case NSNumber number when number.IsBoolean():
                    return number.BoolValue;
                case NSNumber number when number.IsInteger():
                    return (long)number.LongValue;
                case NSNumber number:
                    return number.DoubleValue;
                case NSString @string:
                    return @string.ToString();
                case NSArray array:
                    {
                        var list = new List<object?>();
                        for (nuint i = 0; i < array.Count; i++)
                        {
                            list.Add(Convert(array.GetItem<NSObject>(i)));
                        }
                        return list;
                    }
                case NSDictionary dictionary:
                    {
                        var dict = new Dictionary<string, object?>();
                        foreach (var (key, val) in dictionary)
                        {
                            dict.Add(key.ToString(), Convert(val));
                        }
                        return dict;
                    }
                case NSNull _:
                    return null;
                default:
                    return value;
            }
        }
    }
}
