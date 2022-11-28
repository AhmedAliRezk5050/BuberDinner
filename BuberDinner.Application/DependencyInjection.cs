using System.Reflection;
using BuberDinner.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace BuberDinner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<, >), typeof(ValidationBehavior<, >));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}