# Lexis
- In order to apply the SOLID Principle, this API has been designed following the CRQS Mediator pattern. This way handlers that are responsible for creating or retrieving the entities.
- - Dependency Injection is set using .Net Container to add "AddSingleton" for IMongoClient; Autofac is also used to resolve the dependency for IMediator or IMapper
- To make the API more resilient the calls are all asynchronous, even if that is not enough it will mitigate the issue. In the code we can also update the response code to reflect the issue encountered by the request, for example retuened 404 when an entity is not found. 
A librairy called Polly (https://old.dotnetfoundation.org/projects/polly) can also be used as a nuget pact https://www.nuget.org/packages/Polly. With this librairy we could set some retry oerations, setting some timeout, etc.
 - Update the User model wher retrieving it in order to add the total of all the words user has written

 - CORS implemented to apply single responsability when handling the requests.
 
 - Adding Serilog and writting to a file. The "WriteTo" can be updated to send the logs to Grafana, DataDog, etc..
 - The http response is set to respond with a acomprehensive code and message when the request is not succesful
 - We are accessing the HttpContext in the Search Blogs handler in order to add a condition on the category of blog that the current user can see.
 - When requesting for Users it return the users with the count it's published blogs
 - Adding Author entity to the Blog output
 - Generating ApiClient with Microsoft Kiota Library
 - Docker for the Api
 - Integration tests
