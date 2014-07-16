using System;

namespace Mantle.Sample.PublisherConsole.Module.Models
{
    public class SampleModel
    {
        public bool SampleBool { get; set; }
        public byte SampleByte { get; set; }
        public DateTime SampleDateTime { get; set; }
        public decimal SampleDecimal { get; set; }
        public double SampleDouble { get; set; }
        public float SampleFloat { get; set; }
        public Guid SampleGuid { get; set; }
        public int SampleInt { get; set; }
        public long SampleLong { get; set; }
        public bool? SampleNullableBool { get; set; }
        public byte? SampleNullableByte { get; set; }
        public DateTime? SampleNullableDateTime { get; set; }
        public decimal? SampleNullableDecimal { get; set; }
        public double? SampleNullableDouble { get; set; }
        public float? SampleNullableFloat { get; set; }
        public Guid? SampleNullableGuid { get; set; }
        public int? SampleNullableInt { get; set; }
        public long? SampleNullableLong { get; set; }
        public SampleModel SampleObject { get; set; }
        public string SampleString { get; set; }
    }
}