using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Umbrella.Infrastructure.Firestore.Abstractions;
using Umbrella.Infrastructure.Firestore.Extensions;
using Umbrella.Infrastructure.Firestore.Tests.Entities;

namespace Umbrella.Infrastructure.Firestore.Tests.Extensions
{
    #region Used Types

    public interface IEntityService : IModelEntityRepository<TestEntity>
    {

    }

    public class TestFirestoreService : ModelEntityRepository<TestEntity, TestEntityDocument>, IEntityService
    {
        public TestFirestoreService(ILogger logger, 
                                    IFirestoreDocMapper<TestEntity, TestEntityDocument> mapper,
                                    IFirestoreDataRepository<TestEntityDocument> firestoreRepo)
            : base(logger, mapper, firestoreRepo)
        {}

    }

    #endregion

    public class ServiceCollectionExtensionsTests
    {
        ILogger _Logger;
        IFirestoreDocMapper<TestEntity, TestEntityDocument> _Mapper;
        Mock<IFirestoreDataRepository<TestEntityDocument>> _FirestoreRepo;

        [SetUp]
        public void Setup()
        {
            var logger = new Mock<ILogger>();
            this._Logger = logger.Object;

            var mapper = new Mock<IFirestoreDocMapper<TestEntity, TestEntityDocument>>();
            this._Mapper = mapper.Object;

            this._FirestoreRepo = new Mock<IFirestoreDataRepository<TestEntityDocument>>();
        }

        [Test]
        public void AddRepository_ThrowEx_IfServicesIsNull()
        {
            //******* GIVEN
            IServiceCollection services = null;
            string environment = "localhost";
            string path = @"C:\Temp\credentials.json";

            //******* WHEN
            TestDelegate testCode = () =>
            {
                services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(null, environment, path);
            };

            //******* ASSERT
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(testCode);
            Assert.That(ex.ParamName, Is.EqualTo("services"));
            Assert.Pass();
        }

        [Test]
        public void AddRepository_ThrowEx_IfEnvNameIsNull()
        {
            //******* GIVEN
            IServiceCollection services = new ServiceCollection();
            string environmentName = "";
            string path = @"C:\Temp\credentials.json";

            //******* WHEN
            TestDelegate testCode = () =>
            {
                services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(null, environmentName, path);
            };

            //******* ASSERT
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(testCode);
            Assert.That(ex.ParamName, Is.EqualTo("environmentName"));
            Assert.Pass();
        }

        [Test]
        public void AddRepository_ThrowEx_IfFactoryIsNull()
        {
            //******* GIVEN
            IServiceCollection services = new ServiceCollection();
            string environmentName = "localhost";
            Func<IServiceProvider, TestFirestoreService> instanceFactory = null;
            string path = @"C:\Temp\credentials.json";

            //******* WHEN
            TestDelegate testCode = () =>
            {
                services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(instanceFactory, environmentName, path);
            };

            //******* ASSERT
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(testCode);
            Assert.That(ex.ParamName, Is.EqualTo("instanceFactory"));
            Assert.Pass();
        }

        [Test]
        public void AddRepository_ThrowEx_IfCredentialsFilePathIsNull_InLocalhost()
        {
            //******* GIVEN
            IServiceCollection services = new ServiceCollection();
            string environmentName = "localhost";
            Func<IServiceProvider, TestFirestoreService> instanceFactory = x =>
            {
                return new TestFirestoreService(this._Logger, this._Mapper, this._FirestoreRepo.Object);
            };
            string path = @"";

            //******* WHEN
            TestDelegate testCode = () =>
            {
                services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(instanceFactory, environmentName, path);
            };

            //******* ASSERT
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(testCode);
            Assert.That(ex.ParamName, Is.EqualTo("jsonCredentialsFilePath"));
            Assert.Pass();
        }

        [Test]
        public void AddRepository_RegistersTheImplementation()
        {
            //******* GIVEN
            IServiceCollection services = new ServiceCollection();
            string environmentName = "dev";
            Func<IServiceProvider, TestFirestoreService> instanceFactory = x =>
            {
                return new TestFirestoreService(this._Logger,  this._Mapper, this._FirestoreRepo.Object);
            };

            //******* WHEN
            services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(instanceFactory, environmentName);

            //******* ASSERT
            var provider = services.BuildServiceProvider();
            Assert.False(provider == null, "Expected not null provider");
            var service = provider.GetService<IEntityService>();
            Assert.False(service == null, "Expected not null service");
            Assert.Pass();
        }

        [Test]
        public void AddRepository_RegistersTheImplementation_And_EnvVariable_ForLocalhost()
        {
            //******* GIVEN
            IServiceCollection services = new ServiceCollection();
            string environmentName = "localhost";
            Func<IServiceProvider, TestFirestoreService> instanceFactory = x =>
            {
                return new TestFirestoreService(this._Logger,  this._Mapper, this._FirestoreRepo.Object);
            };
            string path = new CredentialManager().CredentialsFilePath;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "");

            //******* WHEN
            services.AddRepository<IEntityService, TestFirestoreService, TestEntity>(instanceFactory, environmentName, path);

            //******* ASSERT
            var provider = services.BuildServiceProvider();
            Assert.False(provider == null, "Expected not null provider");
            var service = provider.GetService<IEntityService>();
            Assert.False(service == null, "Expected not null service");
            var variableValue = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            Assert.False(string.IsNullOrEmpty(variableValue));
            Assert.Pass();
        }
    }
}