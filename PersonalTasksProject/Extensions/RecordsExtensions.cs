namespace PersonalTasksProject.Extensions;

public static class RecordsExtensions
{
    public static bool AreAllPropertiesNullables<T>(this T record) where T : class
    {
        return record.GetType().GetProperties().Where(p => p.CanRead).All(p => p.GetValue(record) == null);
    }
}