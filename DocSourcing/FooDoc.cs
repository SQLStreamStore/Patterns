namespace DocSourcing
{
    public class FooDoc
    {
        public FooDoc(string id)
        {
            Id = id;
        }

        public FooDoc(string id, FooDocState state, int originalVersion)
        {
            Id = id;
            Balance = state.Balance;
            OriginalVersion = originalVersion;
        }

        public string Id { get; private set; }

        public int Balance { get; private set; }

        public int OriginalVersion { get; private set; } = -1;

        public void Add(int i)
        {
            Balance = Balance + i;
        }

        internal FooDocState GetState()
        {
            return new FooDocState
            {
                Balance = Balance
            };
        }
    }
}
