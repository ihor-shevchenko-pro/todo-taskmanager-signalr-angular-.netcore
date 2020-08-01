import { ModelTypeEnum, NotificationStatusEnum, NotificationTypeEnum } from 'src/core/models/enum';

export interface ISignalRNotificationModel{
    Created: string;
    Data: string;
    Description: string;
    NotificationDataType: ModelTypeEnum;
    NotificationStatus: NotificationStatusEnum;
    NotificationType: NotificationTypeEnum;
    Title: string;
    FromUserId: string;
    ToUserId: string;
}