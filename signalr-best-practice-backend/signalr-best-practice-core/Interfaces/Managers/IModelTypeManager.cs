using signalr_best_practice_api_models;

namespace signalr_best_practice_core.Interfaces.Managers
{
    public interface IModelTypeManager
    {
        ModelType? GetModelType<T>(T model);
        ModelType? GetModelType<T>();
    }
}
