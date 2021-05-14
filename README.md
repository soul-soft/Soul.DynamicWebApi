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

``` C#
public class Todo1AppService : IRemoteService
    {
        readonly ILogger<Todo1AppService> _logger;
        public Todo1AppService(ILogger<Todo1AppService> logger)
        {
            _logger = logger;
        }
        private Task Createxxxp()
        {
            _logger.LogInformation("private Createxxx");
            return Task.CompletedTask;
        }
        public Task Createxxx()
        {
            _logger.LogInformation("Createxxx");
            return Task.CompletedTask;
        }

        public Task Updatexxx()
        {
            _logger.LogInformation("Updatexxx");
            return Task.CompletedTask;
        }
        public Task Updatexxx(int id, Todo1Dto dto)
        {
            _logger.LogInformation("Updatexxx (id ,dto)");
            return Task.CompletedTask;
        }
        public Task Deletexxx(int id)
        {
            _logger.LogInformation("Deletexxx (id)");
            return Task.CompletedTask;
        }
        public Task<Todo1Dto> Getxxx(int id)
        {
            _logger.LogInformation("Getxxx (id)");
            return Task.FromResult(new Todo1Dto());
        }
        public Task<List<Todo1Dto>> Getxxx(Todo1Model dto)
        {
            _logger.LogInformation("Getxxx (dto)");
            return Task.FromResult(new List<Todo1Dto>());
        }
    }
    public class Todo1Dto
    {
        public string Time { get; set; }
        public string Address { get; set; }
    }
    public class Todo1Model
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
```
