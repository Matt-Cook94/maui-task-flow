using Microsoft.Extensions.Logging;
using TaskFlowSqlite.Repositories;
using TaskFlow.ViewModels;
using TaskFlow.Services;
using TaskFlowSqlite.DataAccess;

namespace TaskFlow
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
                    fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
                });

            builder.Services.AddDbContext<TaskFlowDbContext>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
            builder.Services.AddSingleton<IListRepository, ListRepository>();
            builder.Services.AddTransient<IDialogService, DialogService>(); 

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
