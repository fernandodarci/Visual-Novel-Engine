using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class GSC_ArrayExtensions
{
    #region IsNullOrEmpty

    // Arrays
    public static bool IsNullOrEmpty<T>(this T[] array) => array == null || array.Length == 0;

    // Listas
    public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;

    // IEnumerable<T> – usando Any() para evitar iteração completa
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

    // ICollection<T>
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;

    // Dicionários genéricos
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) => dictionary == null || dictionary.Count == 0;

    // Coleções não genéricas (ICollection)
    public static bool IsNullOrEmpty(this ICollection collection) => collection == null || collection.Count == 0;

    // Strings
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    #endregion

    #region InRange

    // Para arrays
    public static bool InRange<T>(this T[] array, int index) => !array.IsNullOrEmpty() && index >= 0 && index < array.Length;

    // Para IList<T>
    public static bool InRange<T>(this IList<T> list, int index) => !list.IsNullOrEmpty() && index >= 0 && index < list.Count;

    #endregion

    
}
