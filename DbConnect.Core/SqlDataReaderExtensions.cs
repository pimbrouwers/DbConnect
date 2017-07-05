﻿using FastMember;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Cinch
{
    public static class SqlDataReaderExtensions
    {
        public static T ConvertToObject<T>(this SqlDataReader rd) where T : class, new()
        {
            

            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }

        public static Dictionary<string, object> ConvertToDictionary(this SqlDataReader rd)
        {
            var dict = new Dictionary<string, object>();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    dict.Add(rd.GetName(i), rd.GetValue(i));
                }
            }

            return dict;
        }
    }
}