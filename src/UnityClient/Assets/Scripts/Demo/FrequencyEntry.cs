public class FrequencyEntry {

    public readonly float MinDistance;
    public readonly float MaxDistance;
    public readonly int MilliSeconds;
    public readonly SkillTarget Target;

    public FrequencyEntry(float min, float max, int freq, SkillTarget target) {
        MinDistance = min;
        MaxDistance = max;
        MilliSeconds = freq;
        Target = target;
    }
}
