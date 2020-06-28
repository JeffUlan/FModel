﻿using PakReader.Parsers.Objects;

namespace PakReader.Parsers.PropertyTagData
{
    public class BaseProperty
    {
        internal static BaseProperty ReadAsObject(PackageReader reader, FPropertyTag tag, FName type, ReadType readType)
        {
            BaseProperty prop = type.String switch
            {
                "ByteProperty" => new ByteProperty(reader, tag, readType),
                "BoolProperty" => new BoolProperty(reader, tag, readType),
                "IntProperty" => new IntProperty(reader, tag),
                "FloatProperty" => new FloatProperty(reader, tag),
                "ObjectProperty" => new ObjectProperty(reader, tag),
                "NameProperty" => new NameProperty(reader, tag),
                "DelegateProperty" => new DelegateProperty(reader, tag),
                "DoubleProperty" => new DoubleProperty(reader, tag),
                "ArrayProperty" => new ArrayProperty(reader, tag),
                "StructProperty" => new StructProperty(reader, tag),
                "StrProperty" => new StrProperty(reader, tag),
                "TextProperty" => new TextProperty(reader, tag),
                "InterfaceProperty" => new InterfaceProperty(reader, tag),
                "MulticastDelegateProperty" => new MulticastDelegateProperty(reader, tag),
                "LazyObjectProperty" => new LazyObjectProperty(reader, tag),
                "SoftObjectProperty" => new SoftObjectProperty(reader, tag, readType),
                "UInt64Property" => new UInt64Property(reader, tag),
                "UInt32Property" => new UInt32Property(reader, tag),
                "UInt16Property" => new UInt16Property(reader, tag),
                "Int64Property" => new Int64Property(reader, tag),
                "Int16Property" => new Int16Property(reader, tag),
                "Int8Property" => new Int8Property(reader, tag),
                "MapProperty" => new MapProperty(reader, tag),
                "SetProperty" => new SetProperty(reader, tag),
                "EnumProperty" => new EnumProperty(reader, tag),
                _ => null, //throw new NotImplementedException($"Parsing of {type.String} types aren't supported yet."),
            };
            return prop;
        }

        internal static object ReadAsValue(PackageReader reader, FPropertyTag tag, FName type, ReadType readType)
        {
            var prop = type.String switch
            {
                "ByteProperty" => new ByteProperty(reader, tag, readType).Value,
                "BoolProperty" => new BoolProperty(reader, tag, readType).Value,
                "IntProperty" => new IntProperty(reader, tag).Value,
                "FloatProperty" => new FloatProperty(reader, tag).Value,
                "ObjectProperty" => new ObjectProperty(reader, tag).Value,
                "NameProperty" => new NameProperty(reader, tag).Value,
                "DelegateProperty" => new DelegateProperty(reader, tag),
                "DoubleProperty" => new DoubleProperty(reader, tag).Value,
                "ArrayProperty" => new ArrayProperty(reader, tag).Value,
                "StructProperty" => new StructProperty(reader, tag).Value,
                "StrProperty" => new StrProperty(reader, tag).Value,
                "TextProperty" => new TextProperty(reader, tag).Value,
                "InterfaceProperty" => new InterfaceProperty(reader, tag).Value,
                "MulticastDelegateProperty" => new MulticastDelegateProperty(reader, tag).Value,
                "LazyObjectProperty" => new LazyObjectProperty(reader, tag).Value,
                "SoftObjectProperty" => new SoftObjectProperty(reader, tag, readType).Value,
                "UInt64Property" => new UInt64Property(reader, tag).Value,
                "UInt32Property" => new UInt32Property(reader, tag).Value,
                "UInt16Property" => new UInt16Property(reader, tag).Value,
                "Int64Property" => new Int64Property(reader, tag).Value,
                "Int16Property" => new Int16Property(reader, tag).Value,
                "Int8Property" => new Int8Property(reader, tag).Value,
                "MapProperty" => new MapProperty(reader, tag).Value,
                "SetProperty" => new SetProperty(reader, tag).Value,
                "EnumProperty" => new EnumProperty(reader, tag).Value,
                _ => null, //throw new NotImplementedException($"Parsing of {type.String} types aren't supported yet."),
            };
            return prop;
        }
    }

    public class BaseProperty<T> : BaseProperty
    {
        public long Position { get; protected set; }
        public T Value { get; protected set; }
    }

    public enum ReadType
    {
        NORMAL,
        MAP,
        ARRAY
    }
}
