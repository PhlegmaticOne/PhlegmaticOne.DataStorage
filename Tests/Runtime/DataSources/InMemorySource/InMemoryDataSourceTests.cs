using NUnit.Framework;
using PhlegmaticOne.DataStorage.DataSources.InMemorySource;
using Phlegmaticone.DataStorage.Tests.Runtime.Shared;
using PhlegmaticOne.DataStorage.Tests.Runtime.Shared;

namespace PhlegmaticOne.DataStorage.Tests.Runtime.DataSources.InMemorySource {
    public class InMemoryDataSourceTests {
        [Test]
        public void DataSource_ShouldBeNullValue_BecauseValueHasNotWritten() {
            var source = new InMemoryDataSource<PagedList>();
            
            var actual = source.ReadAsync().GetAwaiter().GetResult();

            Assert.AreEqual(null, actual);
        }
        
        [Test]
        public void DataSource_ReadValueFromPlayerPrefsShouldBeEqualToExpectedValue_BecauseValueHasBeenWritten() {
            var source = new InMemoryDataSource<PagedList>();
            var expected = Data.TestList;

            source.WriteAsync(expected).GetAwaiter().GetResult();
            var actual = source.ReadAsync().GetAwaiter().GetResult();

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void DataSource_PlayerPrefsKeyShouldNotExist_BecauseValueHasBeenDeleted() {
            var source = new InMemoryDataSource<PagedList>();
            var arranged = Data.TestList;

            source.WriteAsync(arranged).GetAwaiter().GetResult();
            source.DeleteAsync().GetAwaiter().GetResult();
            var actual = source.ReadAsync().GetAwaiter().GetResult();

            Assert.AreEqual(null, actual);
        }
    }
}