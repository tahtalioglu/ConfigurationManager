using ConfigurationManagement.Business.Contract;
using ConfigurationManagement.Business.Manager;
using ConfigurationManagement.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ConfigurationManagement.Business.UnitTests
{
    [TestFixture]
    [Category("UnitTests.ConfigurationReader")]
    public class ConfigurationReaderTest
    {
        IConfigurationStorage _configurationStorage;
        ICacheManager _cacheManager;
        ConfigurationStorageFactory _configurationStorageFactory;
        CacheManagerFactory _cacheManagerFactory;
        string applicationName = "applicationA";
        [SetUp]
        public void SetUp()
        {
            _configurationStorage = Substitute.For<IConfigurationStorage>();
            _configurationStorageFactory = Substitute.For<ConfigurationStorageFactory>();
            _cacheManager = Substitute.For<ICacheManager>();
            _cacheManagerFactory = Substitute.For<CacheManagerFactory>();
            ICacheManager cacheManager = _cacheManager;
            IConfigurationStorage storage = _configurationStorage;
            _configurationStorageFactory.CreateStorage("hede", "Mongo").ReturnsForAnyArgs(storage);


            _cacheManagerFactory.GetCacheProvider(10, "Redis").ReturnsForAnyArgs(cacheManager);

        }

        #region GetValue

        [Test]
        public void GetValue_CacheNotExists_Returns()
        {


            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String"
            };

            _configurationStorage.GetWithKey("site", applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>("site").Returns((IEnumerable<ConfigurationRecord>)null);
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValue<string>("site");
            Assert.AreEqual("boyner.com.tr", returnValue);
        }
        [Test]
        public void GetValue_CacheExists_Returns()
        {
            string applicationName = "applicationA";
            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String"
            };
            List<ConfigurationRecord> configurationRecords = new List<ConfigurationRecord>()
            {
                new ConfigurationRecord()
                {
                    Name ="site",
                    Value ="boyner.com.tr",
                    Type="String"
                }
            };
            _configurationStorage.GetWithKey("site", applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>(Arg.Any<string>()).Returns(configurationRecords);
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValue<string>("site");
            Assert.AreEqual("boyner.com.tr", returnValue);
        }

        [Test]
        public void GetValue_CacheNotExistsTypeInvalid_ReturnsDefault()
        {


            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String"
            };

            _configurationStorage.GetWithKey("site", applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>("site").Returns((IEnumerable<ConfigurationRecord>)null);
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValue<System.Int32>("site");
            Assert.AreEqual(0, returnValue);
        }
        [Test]
        public void GetValue_CacheExistsTypeInvalid_Returns()
        {

            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String"
            };
            List<ConfigurationRecord> configurationRecords = new List<ConfigurationRecord>()
            {
                new ConfigurationRecord()
                {
                    Name ="site",
                    Value ="boyner.com.tr",
                    Type="String"
                }
            };
            _configurationStorage.GetWithKey("site", applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>(Arg.Any<string>()).Returns(configurationRecords);
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValue<Int32>("site");
            Assert.AreEqual(0, returnValue);
        }
        #endregion

        #region Remove
        [Test]
        public void Remove_GuidNull_NotReceived()
        {

            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            manager.Remove(string.Empty);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>(Arg.Any<string>()).DidNotReceive();
        }

        [Test]
        public void Remove_GuidExists_Received()
        {

            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            manager.Remove("hede");
            _configurationStorage.Received().Remove("hede");
            _cacheManager.Received().Remove("applicationA");
        }

        #endregion

        #region GetValueWithId

        [Test]
        public void GetValueWithId_GuidNull_ReturnsNull()
        {
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValueWithId("");
            Assert.AreEqual(null, returnValue);
        }

        [Test]
        public void GetValueWithId_StorageNullCacheNull_ReturnsNull()
        {

            _configurationStorage.GetWithKey("site", applicationName).Returns((ConfigurationRecord)null);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>("site").Returns((IEnumerable<ConfigurationRecord>)null);
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValueWithId("site");
            Assert.AreEqual(null, returnValue);
        }

        [Test]
        public void GetValueWithId_StorageExistsCacheNull_Returns()
        {
            var objectId = new MongoDB.Bson.ObjectId();
            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String",
                GuId = objectId,
                Name = "site"
            };

            _configurationStorage.Get(Arg.Any<string>(), applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>("site").Returns((IEnumerable<ConfigurationRecord>)null);

            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValueWithId("site");

            Assert.AreEqual(configurationRecord.Value, returnValue.Value);
        }

        [Test]
        public void GetValueWithId_CacheExists_Returns()
        {
            var objectId = new MongoDB.Bson.ObjectId();
            var configurationRecord = new ConfigurationRecord()
            {
                Value = "boyner.com.tr",
                Type = "String",
                GuId = objectId,
                Name = "site"
            };
            List<ConfigurationRecord> configurationRecords = new List<ConfigurationRecord>()
            {
                new ConfigurationRecord()
                {
                    Name ="site",
                    Value ="boyner.com.tr",
                    Type="String"
                }
            };
            _configurationStorage.Get(Arg.Any<string>(), applicationName).Returns(configurationRecord);
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>("site").Returns(configurationRecords);

            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            var returnValue = manager.GetValueWithId("site");

            Assert.AreEqual(configurationRecords[0].Value, returnValue.Value);
        }

        #endregion

        #region Write
        
        [Test]
        public void Write_ValueNull_NotReceived()
        {

            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            manager.Write(new RecordDto());
            _cacheManager.Get<IEnumerable<ConfigurationRecord>>(Arg.Any<string>()).DidNotReceive();
        }

        [Test]
        public void Write_ValueNotEmpty_Received()
        {
            RecordDto record = new RecordDto
            {
                Value = "boyner.com.tr",
                Type = "String",
                ApplicationName="console",
                Name = "site"
            };
            var manager = new ConfigurationReader(applicationName, "hede", 10, _configurationStorageFactory, _cacheManagerFactory, _configurationStorage);
            manager.Write(record);
            _configurationStorage.Received().Add(Arg.Any<ConfigurationRecord>());
            _cacheManager.Received().Remove("applicationA");
        }
        #endregion

    }
}
