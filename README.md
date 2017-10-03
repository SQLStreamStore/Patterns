# Memento Persistence

A recipe for persisting Mementos (documents / snapshots) in SQL Stream Store. It's a bit like a key-value store with versions. 

### Why?

 - The model is CRUD-Y, you don't want to use an RDMBS but want to use CQRS, streams and projections.
 - The model doesn't care about domain events (or "business decisions"). The intermediate / final states are the only interesting things.
 - You want to retain some or all previous versions.
 - You want the _option_ to execute a query against your memento backed object without needing a projection.
 
### How?

 - Store the memento as the message itself in a stream.
 - Stream metadata decides how many previous copies to retain.
 - Loading a memento is reading stream backwards by one message only so rehydrating the corresponding object is fast.
 
### Use cases

 - User settings, system configuration, documents etc.
 - Data coming from external systems where they are the source of truth.
 - Snapshots for streams that are (domain) event based.
 
### Explore

 - The [object](https://github.com/damianh/MementoPersistence/blob/master/MementoPersistence/Foo.cs) that is stored and retrieved.
 - ... it's [memento representation](https://github.com/damianh/MementoPersistence/blob/master/MementoPersistence/FooMemento.cs)
 - ... the [repository to load and save it](https://github.com/damianh/MementoPersistence/blob/master/MementoPersistence/FooRepository.cs).

Best thing to do is clone the repo and [step through the tests](https://github.com/damianh/MementoPersistence/blob/master/MementoPersistence.Tests/FooTests.cs).
