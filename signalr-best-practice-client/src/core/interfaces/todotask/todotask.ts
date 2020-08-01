import { ToDoTaskStatusEnum, EntityStatusEnum } from 'src/core/models/enum';
import { IUser } from '../user/user';

export interface ITodotask{
    id: string,
    title: string,
    description: string,
    expiration_date: Date,
    progress_status: ToDoTaskStatusEnum,
    from_user_id: string;
    to_user_id: string;
    created: string;
    updated: string;
    status: EntityStatusEnum;
    from_user: IUser;
    to_user: IUser;
}