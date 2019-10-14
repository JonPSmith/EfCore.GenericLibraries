# Release Notes

## TODO

GenericServices doesn't call access methods in classes which the top class inherits. e.g. in the example code below GenericServices would NOT call the method `InnerMethod`. This is different to properties, which reflection will find.

```c#
class Outer : Inner
{
	public int OuterInt {get; set;}
	public void OuterMethod (){}
}

class Inner 
{
	public int InnerInt { get; set; }
	public void InnerMethod() { }
}
```

This code could be added to the `DecodedEntityClass` class to correct this, but I'm haven't added this yet. 

```c#
	methodsToInspect = FindAllMethodsInType(entityType);
	var inherited = entityType.BaseType;
	while(inherited != typeof(object))
	{
       methodsToInspect.AddRange(FindAllMethodsInType(inherited));
		inherited = inherited.BaseType;
	}
```

## 3.0.0

- Support both EF Core >=2.1 and EF Core >=3.0 by supporting NetStandard2.0 and NetStandard2.1.
- Bug fix: AutoMapper upgrade to version 9.0.0 results in error: MissingMethodException IgnoreAllPropertiesWithAnInaccessibleSetter(). See issue #33
- Bug fix: GetAllErrors() should use Environment.NewLine.
- Style fix: Separator only has one E in it #35

## 2.1.0-preview001

- Special release for @adria3a3 and co to check that AutoMapper 9 now works with this library.

## 2.0.3

- Fix bug #31 - Now you can use a specific DbContext, e.g. EFCoreContext, in your DDD methods/ctors.
- Fix bug #30 - If DTO has no properties then it will give sensible error message.

## 2.0.2

- Fix bug #18 - When no default matching ctor/method is found it outputs a list of possible options.

## 2.0.1

- Bug fix: @MartijnSchoemaker provided a pull request that fixed a problem - see pull requuest #22 

## 2.0.0

- New feature: Now handles EF Core's DbQuery type (DbQuery type is only used for reads) - fixes Issue #16.
- New Feature: Added `ProjectFromEntityToDto<TEntity,TDto>` to the services. This allows you to read data with a query prior to the projection to a DTO. Fixes issue #10 and #15
- New Feature: Added `IGenericStatus BeforeSaveChanges(DbContext)` to configuration.
This allows you to inject code that is called just before SaveChanges/SaveChangesAsync is run. This allows you
to add some validation, logging etc. - see issue #14.
- Improvement: Previously the Sql error handler was only used if validation was turned on. 
Now, if the SaveChangesExceptionHandler property is not null, then that method is called, 
i.e. it is not longer dependant on the state of the ...ValidateOnSave flag in the config.  
- Breaking change (Minor): In version 1.3.1 both `DeleteAndSave` and `DeleteWithActionAndSave` used `IgnoreQueryFilters` to get all entities.
This was done so that soft deleted items would be found, but its dangerous in multi-tenant systems.
In 2.0.0 only `DeleteWithActionAndSave` will use `IgnoreQueryFilters` to get all entities. That is safer, as you can provide extra checks in the method you provide.
- Performance bug fix: There was a performance issue when using the setup methods use in unit testing and non-DI situations

## 1.3.3
- Bug Fix : Improved matching of DTOs to methods - orders by method params and picks the longest match

## 1.3.2
- Bug Fix : Fixed "Collection has changed" exception when validating entities

## 1.3.1
- Change : DeleteAndSave and similar now use a query that includes IgnoreQueryFilters so that it can delete SoftDeleted items etc.
- New feature: Update via JSON Patch now handles test and returns any errors via the status.

## 1.3.0

- New feature: Now supports JsonPatch in `UpdateAndSave`. Two versions:
    - `UpDateAndSave<TEntity>(JsonPatchDocument<TEntity> patch, params object[] keys)`
    - `UpdateAndSave<TEntity>(JsonPatchDocument<TEntity> patch, Expression<Func<TEntity, bool>> whereExpression)`

*NOTE: JsonPatch only works on properties with public setters.*

## 1.2.6

- Minor bug fix: Fixed issues of trying to write key value back to a DTO property with a non-public setter
- Minor new feature: There is now a SetupEntitiesDirect for setting up unit tests that use a direct access to the entity

## 1.2.5

- Minor bug fix: Fixed issues of trying to write key value back to a DTO property with a non-public setter
- Minor new feature: There is now a `SetupEntitiesDirect` extension method for setting up unit tests that use a direct access to the entity

## 1.2.4

- Bug fix: CrudServices were registered as Scoped - they should be registered as Transient
- Minor change: The default success message is changed from "Success!" to "Success"

## 1.2.3

- New Feature: Can turn off error on ReadSingle being null - useful in Web Api and other situations.  

## 1.2.2

- Internal change - Does not initialize AutoMapper as uses injected config.
This makes it work with AddAutoMapper extension method, which defaults to calling Mapper.Initialize

## 1.2.1

- New Feature: Added SqlErrorHandler to configuration and called in SaveChangesWithValidation/Async.
Allows you to intercept an exception in SaveChanges and do things like capture 
SQL errors and turn them into user-friendly error messages.
- New Feature: `bool DirectAccessValidateOnSave` to GenericServicesConfig to globally 
configure all direct CreateAndSave/UpdateAndSave/DeleteAndSave to use validation.
- New Feature: `bool DtoAccessValidateOnSave` to GenericServicesConfig to globally 
configure all via DTO  CreateAndSave/UpdateAndSave to use validation.
- New Feature: New GenericServicesSimpleSetup DI setup method that takes 
in a IGenericServicesConfig parameter

## 1.1.0

- Package: Updated to NET Core 2.1
- Minor breaking change: Now uses Display attribute to override name returned in CRUD messages (for localization)

## 1.0.0 EfCore.GenericServices & EfCore.GenericServices.AspNetCore

- First release

## Not yet implemented/tested

- I have implemented but not unit tested the handling of multiple DbContexts