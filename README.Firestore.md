# Umbrella.Infrastructure.Firestore
Library to provide a client for GCP Firestore, with Repository Pattern implementation





To install it, use proper command:

```
dotnet add package Umbrella.Infrastructure.Firestore 
```

For more details about download, see [NuGet Web Site](https://www.nuget.org/packages/Umbrella.Infrastructure.Firestore/)

<b>Branch Develop</b>: [![Build Status](https://garaproject.visualstudio.com/UmbrellaFramework/_apis/build/status/Umbrella.Infrastructure.Firestore?branchName=development)](https://garaproject.visualstudio.com/UmbrellaFramework/_build/latest?definitionId=73&branchName=development)

<b>Test Coverage</b>
You find test coverage report under _reports\codeCoverage_ .

To run it locally, use the script: _scripts\runTestCoverage.bat_

remember to create a proper json file for credential to access GCP project.

please see class _src\Umbrella.Infrastructure.Firestore.Tests\CredentialManager.cs_

# HOWTO implementing repository
to implement the repository you have to follow the below steps.

## Create a firestore document class
create a class to map the firestore document to be store on database; remember to inherit from _FirestoreDocument_:

```c#
    /// <summary>
    /// class to map a firestore document for Diary
    /// </summary>
    public class DiaryDocument : FirestoreDocument
    {
        [FirestoreProperty]
        public string Username {get; set;}

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <returns></returns>
        public DiaryDocument() : base()
        {
            this.Username = "";
        }
    }
```

each property you add must to be decorated with attribute _FirestoreProperty_.

## Create a Mapper class

create a class to map the firestore document to the entity and viceversa, inheriting from _IFirestoreDocMapper_:

```c#
    /// <summary>
    /// Mapper between the firestore document <see cref="DiaryDocument"/> and you domain entity or Dto <see cref="DiaryDto"/> 
    /// </summary>
    public class DiaryDocumentMapper : IFirestoreDocMapper<DiaryDto, DiaryDocument>
    {
        /// <summary>
        /// Default constr
        /// </summary>
        public DiaryDocumentMapper()
        {
        }

        public DiaryDto FromFirestoreDoc(DiaryDocument doc)
        {
            if (doc == null)
                return null;

            var dto = new DiaryDto();

            . . .

            return dto
        }

        public DiaryDocument ToFirestoreDocument(DiaryDto dto)
        {
            if (dto == null)
                return null;

            var doc = new DiaryDocument();
            doc.SetDocumentId(dto.ID);
            doc.CreatedOn = dto.CreatedOn.ToFirestoreTimeStamp()
            doc.LastUpdatedOn = dto.LastUpdatedOn.ToFirestoreNullableTimeStamp();

            . . .

            return dto
        }
    }
```

Please notice that it's necessary to invoke extension _ToFirestoreTimeStamp_ or _ToFirestoreNullableTimeStamp_ to get right timestamp data.

## Create the repository

To create a repository for your dto class, you have ho ineherit from proper class:

```c#
    /// <summary>
    /// Implementation of repository pattern on Firestore
    /// </summary>
    public class FirestoreDiaryRepository : ModelEntityRepository<DiaryDto, DiaryDocument>
    {
        /// <summary>
        /// Default constr
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="projectId">GCP project Id where firestore DB has been provisioned</param>
        /// <param name="mapper">mapper to translate firestore document to DTO and viceversa</param>
        public FirestoreDiaryRepository(ILogger logger, string projectId, IFirestoreDocMapper<DiaryDto, DiaryDocument> mapper) 
            : base(logger, projectId,  false, "HeadacheDiary", mapper)
        {
        }
    }
```

Please notice that:

- autoGenerateId: TRUE to generate id of object; FALSE to use the id set outside the repo
- collectionName: name of collection to save data on Firestore instance

In order to perform custom queries, use the method provided by base class:

```c#

        DiaryDto GetByUsername(string username)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            // build the query on Firestore document
            var query = this._Repo.GetReference().WhereEqualTo("Username", username);
            var existing = (this._Repo.QueryRecordsAsync(query).Result as List<DiaryDocument>).FirstOrDefault();
            return this._Mapper.FromFirestoreDocumnet(existing);
        }

```
