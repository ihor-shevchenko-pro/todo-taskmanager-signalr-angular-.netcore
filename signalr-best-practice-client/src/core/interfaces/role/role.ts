import { IUser } from '../user/user';

export interface IRole{
    id: string,
    name: string,
    created: string,
    users: IUser[],
}