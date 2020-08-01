using signalr_best_practice_api_models;
using signalr_best_practice_api_models.Models;
using signalr_best_practice_api_models.Models.Response;
using signalr_best_practice_bl.Services.Base;
using signalr_best_practice_core.Entities;
using signalr_best_practice_core.Interfaces.Managers;
using signalr_best_practice_core.Interfaces.Repositories;
using signalr_best_practice_core.Interfaces.Repositories.UserAccount;
using signalr_best_practice_core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalr_best_practice_bl.Services
{
    public class ToDoTaskService : BaseApiService<ToDoTaskAddApiModel, ToDoTaskGetFullApiModel, ToDoTask, string>, IToDoTaskService
    {
        private readonly IUserRepository _userRepository;

        public ToDoTaskService(IToDoTaskRepository repository, IUserRepository userRepository, IDataMapper dataMapper) : base(repository, dataMapper)
        {
            _userRepository = userRepository;
        }

        public override async Task<string> Add(ToDoTaskAddApiModel model)
        {
            var entity = await _userRepository.AnyAsync(x => x.Id == model.ToUserId);
            if (!entity)
            {
                throw new Exception("User recipient of task is not found");
            }

            return await base.Add(model);
        }

        public async Task<List<ToDoTaskGetFullApiModel>> AddForAll(ToDoTaskAddApiModel model)
        {
            var result = new List<ToDoTaskGetFullApiModel>();

            var users = await _userRepository.WhereAsync(x => x.Id != null);
            if (users != null && users?.ToList().Count > 0)
            {
                //1
                foreach (var user in users)
                {
                    if (user.Id == model.FromUserId) continue;

                    model.ToUserId = user.Id;
                    var entityId = await base.Add(model);

                    var singleToDoTaskResult = ToDoTaskGetFullModelBuild(model, entityId);
                    result.Add(singleToDoTaskResult);
                }

                ////2
                //var toDoTasksExecutable = users.Select(async user =>
                //{
                //    if (user.Id == model.FromUserId) return;

                //    model.ToUserId = user.Id;
                //    var entityId = await base.Add(model);

                //    var singleToDoTaskResult = ToDoTaskGetFullModelBuild(model, entityId);
                //    result.Add(singleToDoTaskResult);
                //});
                //await Task.WhenAll(toDoTasksExecutable);
            }

            return result;
        }

        public async Task<PaginationResponseApiModel<ToDoTaskGetFullApiModel>> GetSentToDoTasks(int start, int count,
            EntitySortingEnum sort, string userId)
        {
            List<Func<ToDoTask, bool>> filters = new List<Func<ToDoTask, bool>>();
            filters.Add(x => x.FromUserId == userId);

            var pagination = await _repository.Get(start, count, sort, filters.ToArray());

            var models = _dataMapper.ParseCollection<ToDoTask, ToDoTaskGetFullApiModel>(pagination.Entities);
            var result = new PaginationResponseApiModel<ToDoTaskGetFullApiModel>()
            {
                Total = pagination.Total,
                Start = pagination.Start,
                Count = pagination.Count,
                EntitySorting = pagination.EntitySorting,
                Models = models,
            };

            return result;
        }

        public async Task<PaginationResponseApiModel<ToDoTaskGetFullApiModel>> GetReceivedToDoTasks(int start, int count,
            EntitySortingEnum sort, string userId)
        {
            List<Func<ToDoTask, bool>> filters = new List<Func<ToDoTask, bool>>();
            filters.Add(x => x.ToUserId == userId);

            var pagination = await _repository.Get(start, count, sort, filters.ToArray());

            var models = _dataMapper.ParseCollection<ToDoTask, ToDoTaskGetFullApiModel>(pagination.Entities);
            var result = new PaginationResponseApiModel<ToDoTaskGetFullApiModel>()
            {
                Total = pagination.Total,
                Start = pagination.Start,
                Count = pagination.Count,
                EntitySorting = pagination.EntitySorting,
                Models = models,
            };

            return result;
        }

        public async Task ChangeProgressStatus(string userId, ToDoTaskChangeProgressStatusApiModel model)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
            {
                throw new Exception("Todotask is not found");
            }
            else if(entity.FromUserId != userId && entity.ToUserId != userId)
            {
                throw new Exception("Access denied for changing progress-status of todotask");
            }
            else if(entity.ToUserId == userId && entity.ProgressStatus != ToDoTaskStatusEnum.Cancelled &&
                model.ProgressStatus != ToDoTaskStatusEnum.Cancelled)
            {
                entity.ProgressStatus = model.ProgressStatus;
            }
            else if (entity.ToUserId == userId && entity.ProgressStatus == ToDoTaskStatusEnum.Cancelled &&
                model.ProgressStatus != ToDoTaskStatusEnum.Cancelled)
            {
                throw new Exception("Access denied. You can't change progress-status of todotasks which are Cancelled");
            }
            else if(entity.ToUserId == userId && model.ProgressStatus == ToDoTaskStatusEnum.Cancelled)
            {
                throw new Exception("Access denied for cancelling todotask");
            }
            else if(entity.FromUserId == userId && entity.ProgressStatus == ToDoTaskStatusEnum.New &&
                model.ProgressStatus == ToDoTaskStatusEnum.Cancelled)
            {
                entity.ProgressStatus = model.ProgressStatus;
            }
            else if (entity.FromUserId == userId && entity.ProgressStatus != ToDoTaskStatusEnum.New &&
                model.ProgressStatus == ToDoTaskStatusEnum.Cancelled)
            {
                throw new Exception("Access denied. You can cancell only tasks which are in progress-status - NEW");
            }
            else if (entity.FromUserId == userId && model.ProgressStatus != ToDoTaskStatusEnum.Cancelled)
            {
                throw new Exception("Access denied. You have access only for cancelling task");
            }

            await _repository.Update(entity);
        }

        private ToDoTaskGetFullApiModel ToDoTaskGetFullModelBuild(ToDoTaskAddApiModel model, string entityId)
        {
            return new ToDoTaskGetFullApiModel()
            {
                Id = entityId,
                Title = model.Title,
                Description = model.Description,
                ExpirationDate = model?.ExpirationDate,
                ProgressStatus = model.ProgressStatus,
                FromUserId = model.FromUserId,
                ToUserId = model.ToUserId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Status = EntityStatusEnum.Active,
            };
        }
    }
}
