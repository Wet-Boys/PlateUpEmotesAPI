using System.Reflection;

namespace PlateUpEmotesApi.Utils;

internal static class ReflectionUtils
{
    internal static TField GetField<TField>(this object instance, string fieldName)
    {
        FieldInfo? fieldInfo = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (fieldInfo is null)
            throw new NullReferenceException();

        return (TField)fieldInfo.GetValue(instance);
    }

    internal static TProperty GetProperty<TProperty>(this object instance, string propertyName)
    {
        PropertyInfo? propertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (propertyInfo is null)
            throw new NullReferenceException();

        return (TProperty)propertyInfo.GetValue(instance);
    }

    internal static TReturn InvokeMethod<TReturn>(this object instance, string methodName)
    {
        MethodInfo? methodInfo = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (methodInfo is null)
            throw new NullReferenceException();

        return (TReturn)methodInfo.Invoke(instance, Array.Empty<object>());
    }
}