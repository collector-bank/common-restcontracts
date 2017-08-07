
# Collector Common RestContracts

[![Build status](https://ci.appveyor.com/api/projects/status/1dg9u4px7dle5br6/branch/master?svg=true)](https://ci.appveyor.com/project/HoudiniCollector/common-restcontracts/branch/master)

Provides a set of interfaces an classes that the Collector Common RestClient and Collector Common RestApi will understand.

## Resources identifiers and Contracts

### Resource identifier
First, we need to introduce the concept of 'resource-identifier'. The resource identifier represents the resource location.
The resource identifier might contain variables, for instance, the resource might point to a specific resource.

The class must implement IResourceIdentifier. 

```csharp
public class MyEndpointResourceIdentifier : IResourceIdentifier
{
	public string Id { get; set; }
	public string Uri => $"api/my-endpoint/{Id}";
}
```

### Usage without response
Subclass the  RequestBase<TIidentifier> class and provide your resource identifier, the HTTPMethod, and the ConfigurationKey, normally, all requests to the same api should normally have the same configuration key (more on this, can be found in Common.RestClient).
Example PUT request, (No response object):

```csharp
public class MyEndpointPUTRequest<MyEndpointResourceIdentifier> : RequestBase<MyEndpointResourceIdentifier> 
{
	protected MyEndpointRequest(MyEndpointResourceIdentifier resourceIdentifier)
		: base(resourceIdentifier)
	{
	}
	
	public string SomeProperty { get; set; }
	
	/// <returns>The HTTP method for this request</returns>
	public override HttpMethod GetHttpMethod() => HttpMethod.PUT;
	
	/// <returns>The identifiaction for this api</returns>
	public override string GetConfigurationKey() => "MyApiContractKey";
}
```

### Usage with response 
Subclass the abstract RequestBase<TIidentifier, TResponse>, and provide your response type.

```csharp
public class MyEndpointGETRequest<MyEndpointResourceIdentifier> : RequestBase<MyEndpointResourceIdentifier, MyEndpointGetResponse> 
{
	protected MyEndpointRequest(MyEndpointResourceIdentifier resourceIdentifier)
		: base(resourceIdentifier)
	{
	}
			
	/// <returns>The HTTP method for this request</returns>
	public override HttpMethod GetHttpMethod() => HttpMethod.GET;
	
	/// <returns>The identifiaction for this api</returns>
	public override string GetConfigurationKey() => "MyApiContractKey";
}
```
Where the response class is just an POCO
```csharp
public class MyEndpointGetResponse
{
	public string SomeProperty { get; set; }
}
```

## Contract Validation
It's possible to have validation of the properties for a given Request. (Default is no validation)

In order to provide a validator to for the Request, override the ValidateRequest() method: 

```csharp
public class MyEndpointPUTRequest<MyEndpointResourceIdentifier> : RequestBase<MyEndpointResourceIdentifier> 
{
	protected MyEndpointRequest(MyEndpointResourceIdentifier resourceIdentifier)
		: base(resourceIdentifier)
	{
	}
	
	public string SomeProperty { get; set; }
	
	[StringLength(10, ErrorMessage = "THE_SHORT_STRING_LENGTH_EXCEEDED")]
	public string AShortString { get; set; }
	
	/// <returns>The HTTP method for this request</returns>
	public override HttpMethod GetHttpMethod() => HttpMethod.PUT;
	
	/// <returns>The identifiaction for this api</returns>
	public override string GetConfigurationKey() => "MyApiContractKey";
	
	/// <summary>
	/// Validates the request
	/// </summary>
	/// <returns>Validation error messages, if any</returns>
	protected override IEnumerable<string> ValidateRequest()
	{
	   ///... Your own validation result, with error messages, if any.
	}
}
```
