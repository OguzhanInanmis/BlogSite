using Blog.Services.FluentValidations;
using Blog.Services.Helpers.Images;
using Blog.Services.Services.Abstractions;
using Blog.Services.Services.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Blog.Services.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddScoped<IArticleServices, ArticleServices>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(assembly);

            services.AddControllersWithViews()
                .AddFluentValidation(opt =>
                {
                    opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                    opt.DisableDataAnnotationsValidation = true;
                    opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
                });
            
            
            return services;
        }
    }
}
