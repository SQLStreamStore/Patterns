# DocSourcing

_(Sorry for the lame name. If anyone comes up with anything better, let me know.)_

A recepie for storing Documents in SQL StreamStore. Its bit like a key-value store with versions. 

### Why?

 - The model is CRUD-Y, you don't want to use an RDMBS but want to use CQRS, streams and projections.
 - The model doesn't care about domain events (or "business decisions"). The intermediate / final states are the only interesting things.
 - You want to retain some or all previous versions.
 - You want the _option_ to execute a query against your document without needing a projection.
 
### How?

 - Store the document state as the message itself in a stream.
 - Stream metadata decides how many previous copies to retain.
 - Loading a document is reading stream backwards by one message only.
 
### Use cases

 - User settings / system configuration etc.
 - Data coming from external systems where they are the source of truth.
 - Snapshots for streams that are (domain) event based.
 
### Explore

 - The [object](https://github.com/damianh/DocSourcing/blob/master/DocSourcing/Foo.cs) that is stored and retireved.
 - ... it's [memento representation](https://github.com/damianh/DocSourcing/blob/master/DocSourcing/FooMemento.cs)
 - ... the [repository to load and save it](https://github.com/damianh/DocSourcing/blob/master/DocSourcing/FooRepository.cs).

Best thing to do is clone the repo and [step through the tests](https://github.com/damianh/DocSourcing/blob/master/DocSourcing.Tests/FooTests.cs).
