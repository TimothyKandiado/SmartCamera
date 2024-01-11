namespace Tmore.MathUtils;

public static class Map
{
    public static float MapRange(float initialNumber, (float start, float end) initial, (float start, float end) final)
    {
        var initialDelta = initial.end - initial.start;
        var finalDelta = final.end - final.start;

        if (initialDelta == 0 || finalDelta == 0) return 0;

        var scale = finalDelta / initialDelta;
        var negativeStart = -1 * initial.start;
        var offset = negativeStart * scale + final.start;
        var finalNumber = initialNumber * scale + offset;

        return finalNumber;
    }
}