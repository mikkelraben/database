using database.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace database.Services
{
    public class TodosService
    {
        private readonly IMongoCollection<Todo> _collection;

        public TodosService(IOptions<TodoDatabaseSettings> options)
        {
            var connectionString = Environment.GetEnvironmentVariable("COSMOS_CONNECTION");
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Todo>(options.Value.CollectionName);
        }

        public async Task<long> length()
        {
            return await _collection.CountDocumentsAsync(_ => true);
        }
        public async Task<string> createIndex()
        {
            var indexKeysDefinition = Builders<Todo>.IndexKeys.Ascending(todo => todo.Id);
            return await _collection.Indexes.CreateOneAsync(new CreateIndexModel<Todo>(indexKeysDefinition));
        }

        //get a list filtered to anything
        public async Task<List<Todo>> GetAsync() => await _collection.Find(_ => true).ToListAsync();

        //NOTE: not exposed
        //get a single todo
        public async Task<Todo> GetAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //insert a new todo
        public async Task CreateAsync(Todo newTodo) => await _collection.InsertOneAsync(newTodo);

        //update a todo
        public async Task UpdateAsync(string id, Todo updatedBook) => await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        //remove a todo
        public async Task RemoveAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
