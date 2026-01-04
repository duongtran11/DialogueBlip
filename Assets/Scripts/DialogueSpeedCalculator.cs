public static class DialogueSpeedCalculator
{
    public static int CountPlayableChars(string text)
    {
        int count = 0;
        foreach (char c in text)
        {
            if (char.IsLetter(c))
                count++;
        }
        return count;
    }

    public static float CalculateSpeed(string text, float interval)
    {
        int chars = CountPlayableChars(text);
        return chars > 0 ? interval / chars : interval;
    }
}
