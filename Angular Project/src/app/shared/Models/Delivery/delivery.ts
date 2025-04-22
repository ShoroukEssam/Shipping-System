export interface Delivery {
  id?: string;
  name: string;
  email: string;
  phone: string;
  branchId: number;
  government: string;
  address: string;
  discountType: string;
  companyPercent: number;
  status: boolean;
  branchName: string;
  stateName: string;
  deliveryId:string;
  password?: string;
  stateId?: number;
}
export interface IDelivery {
  $id:string;
  $values : Delivery[];


}
export interface IDeliverySearch {
  id: number;
  name: string;
  stateId: number;


}
