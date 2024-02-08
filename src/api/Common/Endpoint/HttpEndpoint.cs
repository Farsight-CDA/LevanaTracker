using Common.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalAPITemplate.Api.Common.Attributes;
using MinimalAPITemplate.Api.Common.Contracts;
using MinimalAPITemplate.Api.Common.Exceptions;
using System.Reflection;
using RouteAttribute = MinimalAPITemplate.Api.Common.Attributes.Route.RouteAttribute;

namespace MinimalAPITemplate.Api.Common.Endpoint;

[IgnoreServiceLevels]
public abstract class HttpEndpoint<TSelf, TContract, TResponse> : Scoped, IHttpEndpoint<TContract>
    where TSelf : IHttpEndpoint<TContract>
    where TContract : IRequestContract
    where TResponse : IResponseContract
{
    public abstract ValueTask<IResult> HandleRequestAsync(HttpContext context, TContract request, CancellationToken cancellationToken);

    public IEndpointConventionBuilder RegisterRoute(IEndpointRouteBuilder routes)
    {
        var routeAttributes = GetType()
            .GetCustomAttributes<RouteAttribute>(false)
            .ToArray();
        var cacheAttributes = GetType()
            .GetCustomAttributes<CacheAttribute>(false)
            .ToArray();
        var apiInfoAttributes = GetType()
            .GetCustomAttributes<ApiInfoAttribute>(false)
            .ToArray();
        var responseCodeAttributes = GetType()
            .GetCustomAttributes<ResponseCodeAttribute>(false)
            .ToArray();
        var authorizeAttributes = GetType()
            .GetCustomAttributes<AuthorizeAttribute>(false)
            .ToArray();

        if(routeAttributes.Length == 0)
        {
            throw new MissingRouteAttributeException(GetType());
        }
        if(routeAttributes.Length > 1)
        {
            throw new DuplicateRouteAttributeException(GetType());
        }
        if(cacheAttributes.Length > 1)
        {
            throw new DuplicateAttributeException(GetType(), typeof(CacheAttribute));
        }
        if(apiInfoAttributes.Length > 1)
        {
            throw new DuplicateAttributeException(GetType(), typeof(ApiInfoAttribute));
        }
        if(authorizeAttributes.Length > 1)
        {
            throw new DuplicateAttributeException(GetType(), typeof(AuthorizeAttribute));
        }

        var routeAttribute = routeAttributes.Single()!;

        var routeHandlerBuilder = routeAttribute
            .Register(routes, RegisterRoute())
            .Produces<TResponse>();

        foreach(var responseCodeAttribute in responseCodeAttributes)
        {
            routeHandlerBuilder = routeHandlerBuilder.Produces(
                (int) responseCodeAttribute.StatusCode,
                responseCodeAttribute.ResponseType);
        }

        if(cacheAttributes.Length == 1)
        {
            var cacheExpiration = cacheAttributes[0].Expiration;
            routeHandlerBuilder = routeHandlerBuilder.CacheOutput(policy => policy.Cache().Expire(cacheExpiration));
        }

        if(apiInfoAttributes.Length == 1)
        {
            var apiInfoAttribute = apiInfoAttributes[0];
            routeHandlerBuilder = routeHandlerBuilder.WithName(apiInfoAttributes[0].Name);

            if(apiInfoAttribute.Description is not null)
            {
                routeHandlerBuilder = routeHandlerBuilder.WithDescription(apiInfoAttribute.Description);
            }

            //routeHandlerBuilder = routeHandlerBuilder.WithOpenApi(options =>
            //{
            //    options.Tags.Clear();
            //    options.Description = apiInfoAttribute.Description;
            //    return options;
            //});
        }

        if(authorizeAttributes.Length == 1)
        {
            var authorizeAttribute = authorizeAttributes[0];
            routeHandlerBuilder = routeHandlerBuilder.RequireAuthorization(authorizeAttribute);
        }

        return routeHandlerBuilder;
    }

    public virtual Delegate RegisterRoute()
        => async (HttpContext context, [AsParameters] TContract request,
                  [FromServices] TSelf handler, [FromServices] IValidator<TContract>? validator = null,
                  CancellationToken cancellationToken = default)
                    => await handler.HandleAsync(context, request, validator, cancellationToken);

    async Task<IResult> IHttpEndpoint<TContract>.HandleAsync(HttpContext context, TContract requestContract, IValidator<TContract>? validator,
        CancellationToken cancellationToken)
    {
        if(validator is not null)
        {
            var validation = await validator.ValidateAsync(requestContract, cancellationToken);

            if(!validation.IsValid)
            {
                return Results.ValidationProblem(
                    statusCode: StatusCodes.Status400BadRequest,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    errors: validation.Errors
                        .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                        .ToDictionary(x => x.Key, x => x.ToArray())
                );
            }
        }

        var result = await HandleRequestAsync(context, requestContract, cancellationToken);
        return result;
    }
}
