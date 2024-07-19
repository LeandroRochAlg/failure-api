namespace failure_api.Models
{
    public class BadgeValuesModel
    {
        public int FollowValue { get; set; } = 0;
        public int FollowedValue { get; set; } = 0;
        public int JobApplicationValue { get; set; } = 0;
        public int ApplicationStepValue { get; set; } = 0;
        public int ApplicationStepProgressedValue { get; set; } = 0;
        public int GotJobValue { get; set; } = 0;
        public int DidNotGetJobValue { get; set; } = 0;
        public float GotJobMultiplier { get; set; } = 0;
        public float DidNotGetJobMultiplier { get; set; } = 0;
    }
}