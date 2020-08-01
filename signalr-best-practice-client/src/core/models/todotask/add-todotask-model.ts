import { ToDoTaskStatusEnum } from '../enum';

export class AddToDoTaskModel{
    public title: string;
    public description: string;
    public expiration_date: Date;
    public progress_status: ToDoTaskStatusEnum;
    public to_user_id: string;
}