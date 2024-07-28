# From Zero to Hero: Vertical Slice Architecture

### Introducing Vertical Slice Architecture
- Architectual style in which organisation of your code is based on your system use cases(features).

Layers in Layered Architecture:
- Presentation
  - Responsible for presenting user interface and handling user interaction
  - Cotrollers and Views, Api's...
  - MVC
- Application
  - Contains core and bussiness logic rules of the application
  - Workflows, validation and data manipulation
- Domain
  - Encapsulates the domain model and domain specific logic related to that model
- Persistence
 - Known as data access layer
 - Interaction with databases or external databases

![image](https://github.com/user-attachments/assets/6ed8637d-f6eb-4db9-99cc-dae5161a9beb)

- These types of architectures are characterized by clear boundaries and clean responsibilites for each layer, which leads to high level of encapsulation and cohesion inside of that layer.
- Encapsulation is the principle of bundling data together with the methods that operates on that data or we can say limiting direct access to some of that data from outside the borders of that encapsulation.
- Cohesion means the degree of relatetness within single component in a software system. High cohesion means high level of focus. It is desirable. Clear and focus puprpose of components.
- Loose coupling is encouraged. Acheved by working with contracts of layers and not directly of implementations.
- Coupling referes to the degree of interdependence or connectivity between the layers or components. It measures how closely two or more components are connected to or dependent on eachother. Preffered for easier modification, testing and replacement of individual components.

Tradeoffs:
- Complexity to add feature to your system.
- Coupling

### Verical slice architecture
- Idea is that each slice in your architecture will represent a feature.
- Group code together based of business functionality inside of single file, class or package.
- Advantage is increased testability for the feature.
- Increase complexity and deacrease maintanibility.

### A Few Truths About Software Architecture
- Simplicity and low cost -> Layerd Architecture or Pipeline style acrhitecture
- Scalability is important and cost is not -> Microservices or Event-driven
- Identitfy all characterstic before applying the architecual style such as: 
  - Maintanibility
  - Performance -> lower simplicity
  - Cost
  - Scalability -> high cost
  - Testability -> lower simplicity
  - Complexity

### Comparing Vertical Slice Architecture to Other Popular Styles
- Layered Architecture
  - Maintaniblity +-
  - Testability   +-
  - Cost          ++
  - Simplicity    ++
  - Scalability   --

- Clean Architecture
  - Maintaniblity +
  - Testability   +-
  - Cost          +
  - Simplicity    +
  - Scalability   --

- Microservices
  - Maintaniblity ++
  - Testability   ++
  - Cost          ---
  - Simplicity    ---
  - Scalability   +++

- Vertical Slice Architecture
  - Maintaniblity +
  - Testability   +
  - Cost          +
  - Simplicity    +++
  - Scalability   --

Why Vertical Slice Architecture?
- It is a very good fit in agile environment, working on US and you want to interate and deploy fast.
- Feature can be developed and tested separtely than the others. -> Big advantage
