using System;

namespace TwitterStats.Models;

public class StringType : ToSystemType<string>, IComparable
{
    private readonly string _value;

    public StringType(string value)
    {
        _value = value;
    }

    public override string AsSystemType() => _value;

    public override string ToString() => AsSystemType();

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (obj is not StringType stringType) return false;

        return _value == stringType._value;
    }

    public override int GetHashCode() => _value.GetHashCode();

    public int CompareTo(object obj)
    {
        if(obj is not Hashtag tag) return 0;

        return string.CompareOrdinal(_value, tag._value);
    }
}