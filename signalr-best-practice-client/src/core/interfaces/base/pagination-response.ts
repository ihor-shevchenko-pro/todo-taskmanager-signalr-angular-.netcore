import { EntitySortingEnum } from 'src/core/models/enum';

export interface IPaginationResponse<T> {
    start: number;
    count: number;
    total: number;
    sorting: EntitySortingEnum;
    models: Array<T>;
}