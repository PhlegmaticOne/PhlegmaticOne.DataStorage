namespace PhlegmaticOne.DataStorage.Storage.ChangeTracking
{
    public class ChangeTrackerConfig
    {
        public ChangeTrackerConfig()
        {
            TrackInterval = 2;
            TrackStartDelay = 5;
        }

        public float TrackInterval { get; set; }
        public float TrackStartDelay { get; set; }
    }
}