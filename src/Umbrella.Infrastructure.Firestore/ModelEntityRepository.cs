using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Umbrella.Infrastructure.Firestore.Abstractions;

namespace Umbrella.Infrastructure.Firestore
{
    /// <summary>
    /// SImple impelementation of a repository for a collection on Firestore. It can be overrided if needed
    /// </summary>
    /// <typeparam name="T">DTO that map the colletion in your application</typeparam>
    /// <typeparam name="Tdoc">Document representation of DTO on Firestore</typeparam>
    public abstract class ModelEntityRepository<T, Tdoc> : IModelEntityRepository<T>
        where Tdoc : IBaseFirestoreData
        where T : class
    {
        protected readonly ILogger _Logger;
        protected readonly IFirestoreDataRepository<Tdoc> _Repo;
        protected readonly IFirestoreDocMapper<T, Tdoc> _Mapper;

        /// <summary>
        /// Construcotr to make all dependencies explicit
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper">mapper to translate firestore document to DTO and viceversa</param>
        /// <param name="firestoreRepo">component that implemetn generic repository for Firestore Documents</param>
        /// <param name="forceVariablesCheck">True to check for exxistance of 'GOOGLE_APPLICATION_CREDENTIALS' in environment variables</param>
        protected ModelEntityRepository(ILogger logger, 
                                        IFirestoreDocMapper<T, Tdoc> mapper,
                                        IFirestoreDataRepository<Tdoc> firestoreRepo,
                                        bool forceVariablesCheck = false)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            // fill variable
            if(forceVariablesCheck && String.IsNullOrEmpty(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS")))
                throw new InvalidOperationException($"Missing Environment Variable: GOOGLE_APPLICATION_CREDENTIALS");

            this._Logger.LogDebug("{modelRepositoryType} : Instance Firestore db...", this.GetType());
            this._Repo = firestoreRepo ?? throw new ArgumentNullException(nameof(firestoreRepo));
        }

        /// <summary>
        /// Gets the complete list of objetcs
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            var docs = this._Repo.GetAllAsync().Result as List<Tdoc>;
            return docs.Select(x => this._Mapper.FromFirestoreDoc(x)).ToList();
        }
        /// <summary>
        ///  Gets the entity b its Id
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public virtual T GetById(string keyValue)
        {
            var doc = this._Repo.GetAsync(FirestoreDataReference.AsBaseFirestoreData(keyValue)).Result;
            if(doc != null)
                return this._Mapper.FromFirestoreDoc((Tdoc)doc);
            else 
                return default(T);
        }
        /// <summary>
        /// SAves the entity. It creates a new one or updates it
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>the entity Id</returns>
        public virtual string Save(T dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            this._Logger.LogDebug($"Converting to Doc the incoming dto {dto.GetType()}");
            var matchDoc = this._Mapper.ToFirestoreDocument(dto);

            this._Logger.LogDebug("Reading Doc {docid}", matchDoc.Id);
            var existing = this._Repo.GetAsync(matchDoc).Result;
            if (existing != null)
                matchDoc = this._Repo.UpdateAsync(matchDoc).Result;
            else
                matchDoc = this._Repo.AddAsync(matchDoc).Result;
            if(matchDoc == null)
                this._Logger.LogWarning("A Null Document has been returned from Firestoreof for type {repoType}", typeof(T).FullName);
            else
                this._Logger.LogDebug("Document {docId} of type {repoType} succesfully persisted on Firestore", matchDoc.Id, typeof(T).FullName);
            return matchDoc != null ? matchDoc.Id : "";
        }
        /// <summary>
        /// Replaces the collection with the given list
        /// </summary>
        /// <param name="dtos"></param>
        public virtual void SaveAll(IEnumerable<T> dtos)
        {
            dtos.ToList().ForEach(m => this.Save(m));
            this._Logger.LogInformation("Sucesfully persisted {counter} Documents on Firestore", dtos.Count());

            var existingList = (List<Tdoc>)this._Repo.GetAllAsync().Result;
            var upToDateList = dtos.Select(x => this._Mapper.ToFirestoreDocument(x)).ToList();
            int deleteCounter = 0;
            foreach (var doc in existingList)
            {
                if (!upToDateList.Any(x => x.Id == doc.Id))
                {
                    this._Logger.LogDebug("Found Obsolete Document: {docID} will be deleted from Firestore", doc.Id);
                    this._Repo.DeleteAsync(doc).Wait();
                    deleteCounter++;
                }
            }
            this._Logger.LogInformation("Deleted {deleteCounter} Documents from Firestore", deleteCounter);
        }
        /// <summary>
        /// Deletes physically the document
        /// </summary>
        /// <param name="keyValue"></param>
        public void Delete(string keyValue)
        {
            if (String.IsNullOrEmpty(keyValue))
                throw new ArgumentNullException(nameof(keyValue));

            var doc = this._Repo.GetAsync(FirestoreDataReference.AsBaseFirestoreData(keyValue)).Result;
            if(doc == null)
                throw new NullReferenceException("Unable to find document with id " + keyValue);
            this._Repo.DeleteAsync((IBaseFirestoreData)doc).Wait();
        }
    }
}