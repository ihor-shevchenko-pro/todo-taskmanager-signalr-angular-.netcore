using signalr_best_practice_api_models;
using signalr_best_practice_core.Interfaces.Entities;
using System;

namespace signalr_best_practice_core.Entities.Base
{
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        public virtual TKey Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public EntityStatusEnum Status { get; set; }


        public void SetId()
        {
            if (this is BaseEntity<string>)
            {
                (this as BaseEntity<string>).Id = Guid.NewGuid().ToString();
            }
        }

        public void TrySetId()
        {
            if (Id == null) SetId();
        }
    }
}
