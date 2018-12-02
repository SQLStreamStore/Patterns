namespace MementoPersistence
{
    using System;
    using System.Threading.Tasks;
    using DocSourcing;
    using Newtonsoft.Json;
    using Shouldly;
    using SqlStreamStore;
    using Xunit;

    public class FooTests : IDisposable
    {
        private readonly InMemoryStreamStore _streamStore;
        private readonly FooRepository _repository;

        public FooTests()
        {
            _streamStore = new InMemoryStreamStore();
            var serializerSettings = new JsonSerializerSettings();
            _repository = new FooRepository(_streamStore, serializerSettings);
        }

        [Fact]
        public async Task Can_save_and_retrieve_doc()
        {
            const string id = "id-1";
            var doc = new Foo(id);
            doc.Add(2);
            int balance = doc.Balance;

            await _repository.Save(doc);

            doc = await _repository.Load(id);

            doc.Balance.ShouldBe(balance);
        }

        [Fact]
        public async Task Can_update_doc()
        {
            const string id = "id-1";
            var doc = new Foo(id);
            doc.Add(2);
            await _repository.Save(doc);

            doc = await _repository.Load(id);
            doc.Add(3);
            await _repository.Save(doc);

            doc = await _repository.Load(id);
            doc.Balance.ShouldBe(5);
        }

        public void Dispose()
        {
            _streamStore.Dispose();
        }
    }
}
