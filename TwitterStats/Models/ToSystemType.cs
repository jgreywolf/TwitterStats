namespace TwitterStats.Models;

public abstract class ToSystemType<T>
{
    public static implicit operator T(ToSystemType<T> origin) => origin.AsSystemType();

    public abstract T AsSystemType();

    public override string ToString() => AsSystemType().ToString();
}