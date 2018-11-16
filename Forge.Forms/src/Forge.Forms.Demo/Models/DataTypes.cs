using System;
using Forge.Forms.Annotations;

namespace Forge.Forms.Demo.Models
{
    [Title("Default data types")]
    [Form(Grid = new[] { 1d, 1d, 1d })]
    public class DataTypes
    {
        [Field(Row = "1", ToolTip = "@public string String { get; set; }")]
        public string String { get; set; }

        [Field(Row = "1", ToolTip = "@public DateTime DateTime { get; set; }")]
        public DateTime DateTime { get; set; }

        [Field(Row = "1", ToolTip = "@public bool Bool { get; set; }")]
        public bool Bool { get; set; }

        [Field(Row = "2", ToolTip = "@public char Char { get; set; }")]
        public char Char { get; set; }

        [Field(Row = "2", ToolTip = "@public short ShortInt { get; set; }")]
        public short ShortInt { get; set; }

        [Field(Row = "2", ToolTip = "@public int Int { get; set; }")]
        public int Int { get; set; }

        [Field(Row = "3", ToolTip = "@public long LongInt { get; set; }")]
        public long LongInt { get; set; }

        [Field(Row = "3", ToolTip = "@public ushort UnsignedShortInt { get; set; }")]
        public ushort UnsignedShortInt { get; set; }

        [Field(Row = "3", ToolTip = "@public uint UnsignedInt { get; set; }")]
        public uint UnsignedInt { get; set; }

        [Field(Row = "4", ToolTip = "@public ulong UnsignedLongInt { get; set; }")]
        public ulong UnsignedLongInt { get; set; }

        [Field(Row = "4", ToolTip = "@public byte Byte { get; set; }")]
        public byte Byte { get; set; }

        [Field(Row = "4", ToolTip = "@public sbyte SignedByte { get; set; }")]
        public sbyte SignedByte { get; set; }

        [Field(Row = "5", ToolTip = "@public float Float { get; set; }")]
        public float Float { get; set; }

        [Field(Row = "5", ToolTip = "@public double Double { get; set; }")]
        public double Double { get; set; }

        [Field(Row = "5", ToolTip = "@public decimal Decimal { get; set; }")]
        public decimal Decimal { get; set; }

        [Field(Row = "6", ToolTip = "@public DateTime? NullableDateTime { get; set; }")]
        public DateTime? NullableDateTime { get; set; }

        [Field(Row = "6", ToolTip = "@public bool? NullableBool { get; set; }")]
        public bool? NullableBool { get; set; }

        [Field(Row = "6", ToolTip = "@public char? NullableChar { get; set; }")]
        public char? NullableChar { get; set; }

        [Field(Row = "7", ToolTip = "@public short? NullableShortInt { get; set; }")]
        public short? NullableShortInt { get; set; }

        [Field(Row = "7", ToolTip = "@public int? NullableInt { get; set; }")]
        public int? NullableInt { get; set; }

        [Field(Row = "7", ToolTip = "@public long? NullableLongInt { get; set; }")]
        public long? NullableLongInt { get; set; }

        [Field(Row = "8", ToolTip = "@public ushort? NullableUnsignedShortInt { get; set; }")]
        public ushort? NullableUnsignedShortInt { get; set; }

        [Field(Row = "8", ToolTip = "@public uint? NullableUnsignedInt { get; set; }")]
        public uint? NullableUnsignedInt { get; set; }

        [Field(Row = "8", ToolTip = "@public ulong? NullableUnsignedLongInt { get; set; }")]
        public ulong? NullableUnsignedLongInt { get; set; }

        [Field(Row = "9", ToolTip = "@public byte? NullableByte { get; set; }")]
        public byte? NullableByte { get; set; }

        [Field(Row = "9", ToolTip = "@public sbyte? NullableSignedByte { get; set; }")]
        public sbyte? NullableSignedByte { get; set; }

        [Field(Row = "9", ToolTip = "@public float? NullableFloat { get; set; }")]
        public float? NullableFloat { get; set; }

        [Field(Row = "10", ToolTip = "@public double? NullableDouble { get; set; }")]
        public double? NullableDouble { get; set; }

        [Field(Row = "10", ToolTip = "@public decimal? NullableDecimal { get; set; }")]
        public decimal? NullableDecimal { get; set; }

        [Time]
        [Field(Row = "10", ToolTip = "@public DateTime Time { get; set; }")]
        public DateTime Time { get; set; }
    }
}
