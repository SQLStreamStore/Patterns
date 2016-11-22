namespace DocSourcing.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using SqlStreamStore;
    using SqlStreamStore.Streams;

    public class FooRepository
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly int _maxCount;
        private readonly IStreamStore _streamStore;

        public FooRepository(
            IStreamStore streamStore,
            JsonSerializerSettings serializerSettings,
            int maxCount = 10)
        {
            _streamStore = streamStore;
            _serializerSettings = serializerSettings;
            _maxCount = maxCount;
        }

        public async Task<Foo> Load(
            string id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var page = await _streamStore.ReadStreamBackwards(id, StreamVersion.End, 1, cancellationToken);
            if(page.Status == PageReadStatus.StreamNotFound)
            {
                return null;
            }
            var streamMessage = page.Messages.First();
            var jsonData = await streamMessage.GetJsonData(cancellationToken);
            var state = JsonConvert.DeserializeObject<FooMemento>(jsonData, _serializerSettings);
            return new Foo(id, state, page.LastStreamVersion);
        }

        public async Task Save(
            Foo streamDocument,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var state = streamDocument.GetState();
            var json = JsonConvert.SerializeObject(state, _serializerSettings);
            var newStreamMessage = new NewStreamMessage(Guid.NewGuid(), "FooDocState", json);

            var originalVersion = streamDocument.OriginalVersion;

            if (originalVersion < 0)
            {
                await _streamStore.AppendToStream(
                    streamDocument.Id,
                    ExpectedVersion.NoStream,
                    newStreamMessage,
                    cancellationToken);

                await _streamStore.SetStreamMetadata(
                    streamDocument.Id,
                    maxCount: _maxCount, 
                    cancellationToken: cancellationToken);
            }
            else
            {
                await _streamStore.AppendToStream(
                    streamDocument.Id,
                    originalVersion,
                    newStreamMessage,
                    cancellationToken);
            }
        }
    }
}