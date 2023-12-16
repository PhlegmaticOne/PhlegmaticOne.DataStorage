using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using Phlegmaticone.DataStorage.Tests.Runtime.Shared;
using PhlegmaticOne.DataStorage.Tests.Runtime.Shared;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Tests.Runtime.DataSources.FileSource
{
    public class FileDataSourceTests
    {
        private const string DirectoryName = "Tests";
        private const string Key = "PagedList";

        private static readonly string FilePath =
            Path.Combine(Application.persistentDataPath, DirectoryName, Key + ".json");

        private static readonly string DirectoryPath =
            Path.Combine(Application.persistentDataPath, DirectoryName);

        [Test]
        public void DataSource_ShouldBeNullValue_BecauseValueHasNotWrittenInFile()
        {
            var source = Mock.FileSource.Get<PagedList>(DirectoryName);

            var value = source.ReadAsync().GetAwaiter().GetResult();

            Assert.AreEqual(null, value);
        }

        [Test]
        public void DataSource_FileShouldExist_BecauseValueHasBeenWritten()
        {
            var source = Mock.FileSource.Get<PagedList>(DirectoryName);

            source.WriteAsync(Data.TestList).GetAwaiter().GetResult();

            var isExists = File.Exists(FilePath);
            Assert.True(isExists);

            Directory.Delete(DirectoryPath, true);
        }

        [Test]
        public void DataSource_ReadValueFromFileShouldBeEqualToExpectedValue_BecauseValueHasBeenWritten()
        {
            var source = Mock.FileSource.Get<PagedList>(DirectoryName);
            var data = Data.TestList;
            var expected = JsonConvert.SerializeObject(data);

            source.WriteAsync(data).GetAwaiter().GetResult();

            var storedText = File.ReadAllText(FilePath);
            Assert.AreEqual(expected, storedText);

            Directory.Delete(DirectoryPath, true);
        }

        [Test]
        public void DataSource_FileShouldNotExist_BecauseValueHasBeenDeleted()
        {
            var source = Mock.FileSource.Get<PagedList>(DirectoryName);

            source.WriteAsync(Data.TestList).GetAwaiter().GetResult();
            source.DeleteAsync().GetAwaiter().GetResult();

            var isExists = File.Exists(FilePath);
            Assert.False(isExists);
        }
    }
}