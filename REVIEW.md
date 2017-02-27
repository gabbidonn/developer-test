# Review

**NOTE: I have committed some changes based on my review.  I have prefixed each commit with ‘Objective 3’ to indicate these are changes made as part of this review.** 

Application is built as a single layered application, with domain components and business logic in one.  I would want to split the application so it becomes multi-layered.  In my solution, I have separated the domain - splitting all Entity Framework models and the DBContext into the Domain project. 

Good use of builder and command pattern to build viewmodels.  The current location of command and builder objects seems too cluttered.  I would want to move the builders into a more organised location.  Possibly moving into the domain project that I created for domain models.  Also, I would consider using an object mapper, such as automapper/easymapper or a custom mapping routine to facilitate entity -> viewmodel reuse.  Also, I would consider implementing a repository for returning domain models.  This would also improve unit test implementation. 

Unit test coverage provided, but not enough, particularly for Seller-related logic.  I would consider implementing TDD to ensure coverage for all areas.  Also, coverage for entities, such as data annotations isn’t provided.  More tests to cover this would be added. 

Good use of indexes within migrations and use of eager loading for entities.  However, I would also explicitly turn off lazy loading through dbcontext.  Also, since a code first approach is being used, then I would create an integration test suite to test for coverage in multiple areas (business logic + database etc.)  

