namespace signalr_best_practice_api_models
{
    public enum EntitySortingEnum
    {
        ByCreate = 0,
        ByUpdate = 1,
    }

    public enum UserContactTypeEnum
    {
        email = 0,
        phone = 1,
    }

    public enum TokenTypeEnum
    {
        Recovery = 0,
        Verify = 1
    }

    public enum EntityStatusEnum
    {
        Active = 0,
        Inactive = 1,
        Deleted = 2,
    }

    public enum ToDoTaskStatusEnum
    {
        New = 0,
        InProgress = 1,
        Complited = 2,
    }

    public enum ModelType
    {
        ToDotask = 0,
        User = 1,
        UserProfile = 2,
        Role = 3,
    }

    public enum NotificationType
    {
        // Base
        ModelAdd = 1000,
        ModelDelete = 1010,
        ModelUpdate = 1020,
        ModelChangeStatus = 1030,

        // UserOnline
        UserOnline = 2000,
        UserOffline = 2100,
    }

    public enum NotificationStatus
    {
        New = 0,
        Sent = 1,
        Read = 2,
    }

    public enum DataTypeEnum
    {
        Timeline = 0,
    }
}
