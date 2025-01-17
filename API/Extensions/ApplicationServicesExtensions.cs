﻿using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddScoped<IGenericRepository<ProductType>, GenericRepository<ProductType>>();
            services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
          

            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                      .Where(e => e.Value.Errors.Count > 0)
                      .SelectMany(x => x.Value.Errors)
                      .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            
            return services;
        }

    }
}
