namespace ThFnsc.LoopbackRGB.Extensions;

public static class FloatExtensions
{
    public static float[] AveragedPortions(this float[] input, params int[] relativePortions)
    {
        var portionsSum = (float)relativePortions.Sum();
        var indexes = relativePortions.Select(p => (int)Math.Round(p / portionsSum * input.Length)).ToArray();
        var sums = new float[relativePortions.Length];

        for (int i = 0, end = 0; i < sums.Length; i++)
        {
            var start = end;
            end += indexes[i];
            for (var j = start; j < end; j++)
                sums[i] += input[j];
            sums[i] /= end - start;
        }

        return sums;
    }

    public static byte NormalizedFloatToFullRangeByte(this float input) =>
        (byte)Math.Round(input * 255);
}