# Definitions #

Lots of people use patterns like MVC or MVVM. I like some of the concepts, but the words aren't especially intention-revealing... for example, what exactly does "model" mean? The immediate response from many developers is simply, "data model!"  Okay, so what IS the "data model"?

Lots of programming words get overloaded and obfuscated behind the mists of "this is how WE do it", so I've decided to (*try to*) clarify the definitions as much as possible: 

|Term|Definition|
|---|---|
| Anemic | Having no behavior (no methods or closures. "Does" NOTHING.) <br> ex: Classes with no methods |
|DTO| "Data transfer object(s)": an anemic data object. <br> ex: An anemic class or JSON object representing some data to be shuffled around. |
| Entity | A persisted data object. <br>ex: Something with an ID, that gets stored in the DB |
| Service | A class that handles business logic. |
| Repository | A `service` that reads/writes to where the `entities` are stored. |
| Controller | A service that defines a (RESTful or RPC) endpoint. |
| [&lt;subject&gt;] Model | The algorithms and datatypes [of the &lt;subject&gt;]. <br> Business logic with no knowledge of the user or view. (No formatting code goes here!) |
| View | A visible element of the UI, bound to a ViewModel, with no knowledge of how the data appears there, or what any other piece of code will do *after* the user clicks a button. <br> ex: a cleanly-implemented and data-bound Angular `<component>.html`. |
| ViewModel | A linking class which connects the View to the Model. <br> Uses the model (algorithms and datatypes) to generate the formatted data for the view. Includes logic to handle user interaction (clicks, keystrokes, taps, and swipes). <br> ex: A cleanly-implemented and data-bound Angular `<component>.ts`. |
| Interface | An abstraction defining the properties and/or method signatures of a piece of code. <br> "Interfaces" are anemic, meaning that they don't implement any behavior, they only define the method signature. |


- Abstractions are useful for adhering to SOLID design principles (especially the S, L, and I) as well as (at least in C#) testing a unit of code (e.g., a class) without the tests having to depend on the test-subject's dependencies working properly (or even existing).

- Each piece of behavior should be hidden behind an interface, and interfaces will be only be extracted into separate files when they need to be (shared).

(NOTE: In C#, there are (at least?) two types of "Interface": interfaces and abstract classes.

Abstract classes are useful when you need multiple types of a class, which all inherit the same behavior. They're dangerous because they lock every subclass into that behavior. They're not BAD, just be aware of the possible drawbacks. Abstract classes make good factories, but when you add OTHER behavior, you risk violating the S, O, and I. This is why I'll use plain-old interfaces for DTOs, and abstract classes whenever a factory becomes useful [likely quite often].)
    


    
