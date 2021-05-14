# Soul.DynamicWebAPI
Soul.DynamicWebAPI

``` C#
 public class Startup
  {
      public Startup(IConfiguration configuration)
      {
          Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
          services.AddControllers();
          services.AddRemoteServices(o=> 
          {
              //o.ControllerNamePrefix = "api/";
              o.ServiceNamePrefix = "api/";
          });
          services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "Soul.DynamicWebAPI.Test", Version = "v1" });
          });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
          app.UseRouting();

          app.UseAuthorization();

          app.UseEndpoints(endpoints =>
          {
              endpoints.MapControllers();
          });
      }
  }
```
