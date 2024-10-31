using DogsHouse.Application.Services;
using DogsHouse.Application.Services.DogServices;
using DogsHouse.Application.Services.DogServices.DogServicesInterfaces;
using DogsHouse.Domain.Entities;
using DogsHouse.Domain.Models.RequestDtos;
using DogsHouse.Persistence.Repositories;
using DogsHouse.Persistence.Repositories.DogRepository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouse.Infrastructure.Extensions
{
    public static class ServicesRegistrationExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IDogService, DogService>();
            services.AddScoped<IBaseService<Dog>, BaseService<Dog>>();

            services.AddScoped<IDogRepository, DogRepository>();
            services.AddScoped<IBaseRepository<Dog>, BaseRepository<Dog>>();
        }
    }
}
