﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanHang.Extensions
{
    public static class ReflextionExtension
    {
        public static string GetPropertyValue<T>(this T item,string propertyName)
        {
           return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
    }
}
