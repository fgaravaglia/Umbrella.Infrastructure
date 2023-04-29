using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Umbrella.Infrastructure.Firestore.Abstractions;

namespace Umbrella.Infrastructure.Firestore.Tests.Entities
{
    /// <summary>
    /// Class that implement repository for TestEntity that hides firestore details to outside.
    /// TestEntity is the representation of Entity inside Domain Model;
    /// TestEntityDocument is the representation of same entity inside Firestore, as a Document
    /// </summary>
    public class TestEntityRepository : ModelEntityRepository<TestEntity, TestEntityDocument>
    {
        public TestEntityRepository(ILogger logger,
                                    IFirestoreDocMapper<TestEntity, TestEntityDocument> mapper,
                                    IFirestoreDataRepository<TestEntityDocument> firestoreRepo,
                                    bool checkEnvironmentVariables = false)
            : base(logger,  mapper, firestoreRepo, checkEnvironmentVariables)
        {

        }

    }
}