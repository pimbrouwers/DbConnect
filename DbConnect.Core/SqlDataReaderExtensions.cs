﻿using FastMember;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Cinch
{
    
    public static class SqlDataReaderExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this SqlDataReader rd) where T : class, new()
        {
            while (rd.Read())
                yield return rd.ConvertToObject<T>();
        }

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
                    accessor.AssignValue(t, members, rd.GetName(i), rd.GetValue(i));
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
