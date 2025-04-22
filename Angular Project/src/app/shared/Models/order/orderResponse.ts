import { IOrder } from "./order"

export interface IorderResponse{
    $id:string;
    $values: IOrder[]
}