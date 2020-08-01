using signalr_best_practice_api_models;

namespace signalr_best_practice_core.Interfaces.Managers
{
    public interface IModelTypeManager
    {
        ModelTypeEnum? GetModelType<T>(T model);
        ModelTypeEnum? GetModelType<T>();
    }
}
