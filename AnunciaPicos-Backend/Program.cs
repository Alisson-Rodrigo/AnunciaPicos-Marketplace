using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using NSwag;
using AnunciaPicos.Backend.API.Filters;
using AnunciaPicos.Backend.Aplicattion.Services.HashPassword;
using AnunciaPicos.Backend.Aplicattion.Services.LoggedUser;
using AnunciaPicos.Backend.Aplicattion.Services.SMTPEmail;
using AnunciaPicos.Backend.Aplicattion.Services.TokenLogin;
using AnunciaPicos.Backend.Aplicattion.Services.TokenUpdatePassword;
using AnunciaPicos.Backend.Aplicattion.Services.ValidatorCpf;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Login;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.ResetPassword;
using AnunciaPicos.Backend.Aplicattion.UseCases.Auth.UpdatePassword;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Update;
using AnunciaPicos.Backend.Infrastructure.Repositories.SaveChanges;
using AnunciaPicos.Backend.Infrastructure.Repositories.User;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.GetId;
using AnunciaPicos.Backend.Aplicattion.UseCases.Profile.Delete;
using AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Auth;
using AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Profile;
using AnunciaPicos.Backend.Infrastructure.Repositories.Product;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.GetId;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Update;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Delete;
using AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Product;
using AnunciaPicos.Backend.Infrastructure.Data;
using AnunciaPicos.Backend.Infrastructure.Repositories.Messages;
using AnunciaPicos.Backend.Aplicattion.Services.AutoMapping.Evoluation;
using AnunciaPicos.Backend.Infrastructure.Repositories.Evaluation;
using AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Register;  
using AnunciaPicos.Backend.Aplicattion.UseCases.Evaluation.Get;
using AnunciaPicos.Backend.Aplicattion.UseCases.PlanPayment.PostPlan;
using AnunciaPicos.Backend.Aplicattion.Services.Stripe;
using AnunciaPicos.Backend.Infrastructure.Repositories.Payment;
using AnunciaPicos.Backend.Aplicattion.UseCases.Product.Search;
using AnunciaPicos.Backend.Aplicattion.UseCases.Gemini;
using Microsoft.Extensions.FileProviders;
using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Delete;
using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Register;
using AnunciaPicos.Backend.Aplicattion.UseCases.Favorites.Get;
using AnunciaPicos.Backend.Infrastructure.Repositories.Favorite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Adicionando suporte a CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3001")  // Coloque a URL do seu frontend aqui
              .WithOrigins("http://localhost:3000")
              .WithOrigins("http://localhost:63789")
              .WithOrigins("http://localhost:5222")
              .WithOrigins("https://anunciapicos.shop")
              .WithOrigins("http://anunciapicos.shop")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();  // Permitir cookies/autenticação com credenciais
    });
});

builder.Services.AddSignalR();

// Adicionando Swagger com NSwag
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "AnunciaPicos API";
    config.Version = "v1";

    // Configuração do token JWT no Swagger
    config.AddSecurity("Bearer", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Informe o token JWT no campo 'Authorization' utilizando o prefixo 'Bearer '"
    });

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

builder.Services.AddHttpContextAccessor();

// Mapeamento de classes
builder.Services.AddAutoMapper(typeof(RegisterUserMapping));
builder.Services.AddAutoMapper(typeof(RegisterProductUseCase));
builder.Services.AddAutoMapper(typeof(GetUserProfileMapping));
builder.Services.AddAutoMapper(typeof(RegisterProductMapping));
builder.Services.AddAutoMapper(typeof(RegisterEvoluationMapping));
builder.Services.AddAutoMapper(typeof(GetEvaluationMapping));

// Dependency Injection
builder.Services.AddScoped<IRegisterUseCase, RegisterUseCase>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TokenJwt>();
builder.Services.AddScoped<IPasswordEncrypter, PasswordEncrypter>();
builder.Services.AddScoped<ISend, Send>();
builder.Services.AddScoped<IPasswordResetTokenService, PasswordResetTokenService>();
builder.Services.AddScoped<IResetUseCase, ResetUseCase>();
builder.Services.AddScoped<ILogged, Logged>();
builder.Services.AddScoped<IGetUserUseCase, GetUserUseCase>();
builder.Services.AddScoped<ICPF, CPF>();
builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
builder.Services.AddScoped<IGetUserIdUseCase, GetUserIdUseCase>();
builder.Services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
builder.Services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
builder.Services.AddScoped<IGetProductUseCase, GetProductUseCase>();
builder.Services.AddScoped<IGetProductIdUseCase, GetProductIdUseCase>();
builder.Services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
builder.Services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
builder.Services.AddScoped<IConversationRepository, MessagesRepository>();
builder.Services.AddScoped<IPostEvaluationUseCase, PostEvaluationUseCase>();
builder.Services.AddScoped<IGetEvaluationUseCase, GetEvaluationUseCase>();
builder.Services.AddScoped<IPlanPaymentUseCase, PlanPaymentUseCase>();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISearchAndFiltersUseCase, SearchAndFiltersUseCase>();
builder.Services.AddScoped<IRequestGeminiChatUseCase, RequestGeminiChatUseCase>();
builder.Services.AddScoped<IRegisterFavoriteUseCase, RegisterFavoriteUseCase>();
builder.Services.AddScoped<IDeleteFavoriteUseCase, DeleteFavoriteUseCase>();
builder.Services.AddScoped<IGetFavoriteUseCase, GetFavoriteUseCase>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<GetTokenRequest>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

// Banco de Dados - MySQL
string mySqlConnection = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddDbContextPool<AnunciaPicosDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

// Adicionando filtro global de exceções
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionsFilter)));

builder.Services.AddHostedService<PlanExpiredVerification>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configuração do Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/swagger";
        settings.DocumentPath = "/swagger/v1/swagger.json";
    });

    // Redirecionamento para a documentação Swagger
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

// Configuração do diretório de imagens
var uploadsPath = app.Environment.IsDevelopment()
    ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
    : "/var/www/anunciapicos/uploads";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});


// Outras configurações
app.MapHub<ChatHub>("/chathub");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Este UseStaticFiles agora lida apenas com arquivos padrão
app.UseStaticFiles();  // Serve arquivos estáticos para outros caminhos

app.MapControllers();
app.UseCors("AllowSpecificOrigin");  // Aplica a política CORS definida
app.Run();

