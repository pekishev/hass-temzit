using System;

namespace Temzit.Api
{
    public record TemzitActualState
    {
        public TemzitActualState(byte[] data)
        {
            State = BitConverter.ToInt16(data, 0);
            SchedulerNumber = BitConverter.ToInt16(data, 2);
            OutsideT = (float) BitConverter.ToInt16(data, 4) / 10;
            InsideT = (float) BitConverter.ToInt16(data, 6) / 10;
            OutHeatT = (float) BitConverter.ToInt16(data, 8) / 10;
            InHeatT = (float) BitConverter.ToInt16(data, 10) / 10;
            LiquidSpeed = BitConverter.ToInt16(data, 18) * 4.2f;
            InputPower = (float) BitConverter.ToInt16(data, 28) / 10;
            HotWaterT = (float) BitConverter.ToInt16(data, 16) / 10;
            Compressor1 = data[22];
            Error = BitConverter.ToInt16(data, 30);
        }

        public short Error { get; set; }

        public byte Compressor1 { get; set; }

        public float HotWaterT { get; set; }

        public float InputPower { get; set; }

        public float LiquidSpeed { get; set; }

        public float OutHeatT { get; set; }

        public float InHeatT { get; set; }

        public float InsideT { get; set; }

        public float OutsideT { get; init; }

        public short State { get; set; }
        public short SchedulerNumber { get; set; }
        // 4489.8 - temzit   4483 
        public float OutputPower => (OutHeatT - InHeatT) * 4200 * LiquidSpeed / 60 / 1000; //4371
        public float? COP => InputPower == 0 ? null : OutputPower / InputPower;
    }
}