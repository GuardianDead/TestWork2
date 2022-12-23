using Scrutor;

namespace TestWork2.Configurations
{
    public static class ValidatorConfiguration
    {
        public static IServiceCollection AddAppValidators(this IServiceCollection services)
        {
            services.Scan(
                options =>
                {
                    options.FromCallingAssembly()

                    .AddClasses(i => i.Where(c => c.Name.EndsWith("Validator")))
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
                });

            return services;
        }
    }
}
