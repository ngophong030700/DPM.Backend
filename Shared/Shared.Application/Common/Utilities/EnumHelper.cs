using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Common.Utilities
{
    public static class EnumHelper
    {
        // Cache mô tả để tối ưu phản xạ (reflection)
        private static readonly ConcurrentDictionary<System.Enum, string> _descriptionCache
            = new ConcurrentDictionary<System.Enum, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<int, string>> _enumDictCache
            = new ConcurrentDictionary<Type, Dictionary<int, string>>();

        /// <summary>
        /// Lấy Description từ thuộc tính [Description]
        /// </summary>
        public static string GetDescription(System.Enum? enumValue)
        {
            if (enumValue == null)
                return string.Empty;

            return _descriptionCache.GetOrAdd(enumValue, e =>
            {
                var field = e.GetType().GetField(e.ToString());
                var attr = field?.GetCustomAttribute<DescriptionAttribute>();
                return attr?.Description ?? e.ToString();
            });
        }

        /// <summary>
        /// Lấy dictionary {value, description} cho toàn bộ enum
        /// </summary>
        public static Dictionary<int, string> ToDictionary<TEnum>() where TEnum : System.Enum
        {
            var type = typeof(TEnum);

            return _enumDictCache.GetOrAdd(type, _ =>
                System.Enum.GetValues(type)
                    .Cast<System.Enum>()
                    .ToDictionary(
                        e => Convert.ToInt32(e),
                        e => GetDescription(e)
                    )
            );
        }

        /// <summary>
        /// Lấy danh sách (value, description) để bind dropdown UI
        /// </summary>
        public static List<EnumItem> ToList<TEnum>() where TEnum : System.Enum
        {
            return ToDictionary<TEnum>()
                .Select(x => new EnumItem { Value = x.Key, Label = x.Value })
                .ToList();
        }

        /// <summary>
        /// Convert value (int) sang Enum
        /// </summary>
        public static TEnum FromValue<TEnum>(int value) where TEnum : System.Enum
        {
            return (TEnum)System.Enum.ToObject(typeof(TEnum), value);
        }

        /// <summary>
        /// Lấy Description từ giá trị int
        /// </summary>
        public static string GetDescriptionFromValue<TEnum>(int value) where TEnum : System.Enum
        {
            var enumValue = FromValue<TEnum>(value);
            return GetDescription(enumValue);
        }
    }

    public class EnumItem
    {
        public int Value { get; set; }
        public string? Label { get; set; }
    }
}
