using signalr_best_practice_api_models;
using signalr_best_practice_core.Interfaces.Managers;
using System;

namespace signalr_best_practice_core.Managers
{
    public class ModelTypeManager : IModelTypeManager
    {
        public ModelType? GetModelType<T>(T model)
        {
            string typeName = model.GetType().Name.Replace("GetFullApiModel", "");

            if (Enum.TryParse(typeName, out ModelType type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }

        public ModelType? GetModelType<T>()
        {
            string typeName = typeof(T).Name.Replace("GetFullApiModel", "");

            if (Enum.TryParse(typeName, out ModelType type))
            {
                return type;
            }
            else
            {
                return null;
            }
        }
    }
}
