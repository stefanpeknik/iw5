﻿using System;
using System.Net.Http;
using TaHooK.Api.BL;
using Microsoft.Extensions.DependencyInjection;

namespace TaHooK.Web.BL.Installers;

public class WebBLInstaller
{
    public void Install(IServiceCollection serviceCollection, string apiBaseUrl)
    {
        serviceCollection.AddTransient<IQuizApiClient, QuizApiClient>(provider =>
        {
            var client = CreateApiHttpClient(provider, apiBaseUrl);
            return new QuizApiClient(client, apiBaseUrl);
        });
        
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<WebBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());
    }
    
    public HttpClient CreateApiHttpClient(IServiceProvider serviceProvider, string apiBaseUrl)
    {
        var client = new HttpClient() { BaseAddress = new Uri(apiBaseUrl) };
        client.BaseAddress = new Uri(apiBaseUrl);
        return client;
    }
}