# AE.Extensions.DependencyInjection
[![Build Status](https://travis-ci.org/app-enhance/ae-di.svg?branch=master)](https://travis-ci.org/app-enhance/ae-di)
[![Build status](https://ci.appveyor.com/api/projects/status/s5ej8f3uechsx3gs/branch/master?svg=true)](https://ci.appveyor.com/project/Ermesx/ae-di/branch/master)
[![app-enhance-dev MyGet Build Status](https://www.myget.org/BuildSource/Badge/app-enhance-dev?identifier=891bb83e-b009-4793-b622-495a6eab6afc)](https://www.myget.org/gallery/app-enhance-dev)

This is extension library for `Microsoft.Extensions.DependencyInjection` to provide declarative way to pick out lifetime of services and register them in just a few lines.

## Motivation
When you want to register service (create dependency) in `IServiceCollection` you have to describe al lest 3 things.

* Service interface
* Service implementation
* Lifetime scope

In default way it looks like below
```c#
// Method ex. in Startup.cs
// doc: http://docs.asp.net/en/latest/fundamentals/dependency-injection.html

public void ConfigureServices(IServiceCollection services)
{
    var serviceDescriptor = new ServiceDescriptor(typeof(IBankManager), typeof(BankManager), ServiceLifetime.Transient);
    services.Add(serviceDescriptor);

    // Add MVC services to the services container.
    services.AddMvc();
}
```
```c#
/// And service detinition in somewhere deeper
public interface IBankManager
{
    int OpenAccount(string clientName);
}

public class BankManager : IBankManager
{
    public int OpenAccount(string clientName)
    {
        // Some account creating process...
        
        return Random.Next(1000000, 9999999);
    }
}
```

Every service must be manualy described in `ConfigureServices` method and it is easy to forgot about something.
Many people who create other libraries like EF add static extending methods to simplify registrations. 
It means all descriptions of their services are hidden. 
They make our life easier but what about own custom servies ? What if you have dozens of them ?

I'm inspired how Orchard resolves that issue. 
They create interfaces which correspond to lifetime scopes. Then every service interface inherit appropriate "scope interface" and describe dependencies to register in global container.
Im my case it looks like below - interfaces correspond exactly to `ServiceLifetime` enum.

```c#
// This one descibe all dependencies
public interface IDependency
{
}

public interface ISingletonDependency : IDependency
{
}

public interface IScopedDependency : IDependency
{
}

public interface ITransientDependency : IDependency
{
}

// Ommit registration for special cases
public interface INotRegisterDependency
{
}
```

## How it works?

To use this approach you have to do two things:

* Select dependency interface and inherit
```c#
// It will be registered as a new ServiceDescriptor(typeof(IBankManager), typeof(BankManager), ServiceLifetime.Transient);
public interface IBankManager : ITransientDependency
{
    int OpenAccount(string clientName);
}

public class BankManager : IBankManager
{
    public int OpenAccount(string clientName)
    {
        // Some account creating process...

        return Random.Next(1000000, 9999999);
    }
}
```
* Use `ServicesDescriber` to retrieve all dependencies from assemblies
```c#
public void ConfigureServices(IServiceCollection services)
{
    var assemblies = ... Get all assemblies ex. by ILibraryLoader or Assembly.GetExecutingAssembly(...) etc.
    var serviceDescriptors = ServicesDescriber.DescribeFromAssemblies(assemblies);

    services.AddRange(serviceDescriptors);
 
    // Add MVC services to the services container.
    services.AddMvc();
}
```

## Integrations

### Autofac 
(todo)

### Ninject 
(todo)

## Contribute
Before pushing new feature or improvement please read [CONTRIBUTING.md](https://github.com/app-enhance/ae-core/blob/master/CONTRIBUTING.md)
