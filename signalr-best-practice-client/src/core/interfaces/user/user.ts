import { IUserProfile } from './user-profile';
import { IRole } from '../role/role';
import { EntityStatusEnum } from 'src/core/models/enum';

export interface IUser{
    id: string,
    email: string,
    user_name: string,
    user_profile: IUserProfile,
    roles: IRole[],
    created: string;
    status: EntityStatusEnum;
}