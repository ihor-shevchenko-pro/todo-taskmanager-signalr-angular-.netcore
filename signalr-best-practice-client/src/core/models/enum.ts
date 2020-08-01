export enum EntitySortingEnum {
    ByCreate = 0,
    ByUpdate = 1,
}

export enum EntityStatusEnum {
    Active = 0,
    Inactive = 1,
    Deleted = 2,
}

export enum ToDoTaskStatusEnum {
    New = 0,
    InProgress = 1,
    Complited = 2,
    Cancelled = 3,
}

export enum ModelTypeEnum {
    ToDoTask = 0,
    User = 1,
    UserProfile = 2,
    Role = 3,
}

export enum NotificationStatusEnum {
    New = 0,
    Received = 1,
    InProgress = 2,
    Complited = 3,
}

export enum NotificationTypeEnum {
    // Base
    ModelAdd = 1000,
    ModelDelete = 1010,
    ModelUpdate = 1020,
    ModelChangeStatus = 1030,

    // UserOnline
    UserOnline = 2000,
    UserOffline = 2100,

    // ToDoTask
    ChangeProgressStatus = 3000,
}