# AE.Extensions.DependencyInjection
[![Build Status](https://travis-ci.org/app-enhance/ae-di.svg?branch=master)](https://travis-ci.org/app-enhance/ae-di)
[![Build status](https://ci.appveyor.com/api/projects/status/s5ej8f3uechsx3gs/branch/master?svg=true)](https://ci.appveyor.com/project/Ermesx/ae-di/branch/master)
[![app-enhance-dev MyGet Build Status](https://www.myget.org/BuildSource/Badge/app-enhance-dev?identifier=891bb83e-b009-4793-b622-495a6eab6afc)](https://www.myget.org/gallery/app-enhance-dev)

This is an extension library for `Microsoft.Extensions.DependencyInjection` which provides you with a declarative way to pick out lifetime of services and register them in just a few lines.

## Motivation
When you want to register a service (create dependency) in `IServiceCollection` you have to describe at least 3 things.

* Service interface
* Service implementation
* Lifetime scope

Ususally it should look like this (see below)
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
/// And service definition somewhere, deeper in the code
public interface IBankManager
{
    int OpenAccount(string clientName);
}

public class BankManager : IBankManager
{
    public int OpenAccount(string clientName)
    {
        // Account creating process...
        
        return Random.Next(1000000, 9999999);
    }
}
```

Every service must be described in `ConfigureServices` method manualy which means that something can be easily missed.
Many people who create libraries like MVC they add static extension methods to simplify registration. 
It means all descriptions of their services are hidden. 
They make our life easier but what about our own custom services ? What if you have dozens of them ?

I'm inspired by how Orchard team resolves that issue. 
They created interfaces which correspond to lifetime scopes. So every service interface inherits appropriate "scope interface" and describes dependencies to register in global container.
Im my case it looks like below - interfaces correspond exactly to `ServiceLifetime` enum.

```c#
// This one describes all dependencies
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

// Omit registration for special cases
public interface INotRegisterDependency
{
}
```

## How does it work?

In order to use this approach you have to do two things:

* Select dependency interface and inherit
```c#
// It will be registered as a 
// new ServiceDescriptor(typeof(IBankManager), typeof(BankManager), ServiceLifetime.Transient);
public interface IBankManager : ITransientDependency
{
    int OpenAccount(string clientName);
}

public class BankManager : IBankManager
{
    public int OpenAccount(string clientName)
    {
        // Account creating process...

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
