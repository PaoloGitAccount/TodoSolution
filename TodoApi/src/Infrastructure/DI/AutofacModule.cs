namespace Infrastructure.DI;
using Autofac;
using Infrastructure.Data;
using Application.Services;
using Microsoft.Extensions.Configuration;

public class AutofacModule : Module
{
    private readonly IConfiguration _configuration;

    public AutofacModule(IConfiguration configuration) => _configuration = configuration;

    protected override void Load(ContainerBuilder builder)
    {
        // Register DbContext with DI
        builder.Register(c =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            return new AppDbContext(optionsBuilder.Options);
        }).AsSelf().InstancePerLifetimeScope();

// Register UnitOfWork     
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

//Register TodoRepository 
        builder.RegisterType<TodoRepository>
().As<ITodoRepository>
().InstancePerLifetimeScope ();        
        builder.RegisterType<TodoService>().AsSelf();
    }
}
