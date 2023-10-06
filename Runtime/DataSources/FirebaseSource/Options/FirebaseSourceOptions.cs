namespace PhlegmaticOne.DataStorage.DataSources.FirebaseSource.Options {
    public class FirebaseSourceOptions {
        public bool IsTestUser { get; }
        public string TestUserId { get; }

        public FirebaseSourceOptions(bool isTestUser, string testUserId) {
            IsTestUser = isTestUser;
            TestUserId = testUserId;
        }
    }
}