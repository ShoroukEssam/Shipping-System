export interface IMerchantDTO {
  id?: string;
  name: string;
  email: string;
  phone: string;
  status?: boolean;
  branchName?: string;
  branchId?: number;
  role?: string;
  password?: string;
  address: string;
  government: string;
  city: string;
  pickUpSpecialCost: number;
  refusedOrderPercent: number;
  specialCitiesPrices:{
    $id: string;
    $values: ISpecialPrice[];
  }
}
export interface ISpecialPrice{
  id?: number;
  government: string;
  city: string;
  price: number;
}