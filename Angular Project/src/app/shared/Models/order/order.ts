export interface IOrderProduct{
    id?: number;
    productName: string;
    productQuantity: number;
    weight: number;
  }
  
  export interface IOrder{
    id?: number;
    clientName: string;
    clientPhoneNumber1: number;
    clientPhoneNumber2?: number;
    clientEmail: string;
    orderCost: number;
    totalWeight: number;
    isVillage: boolean;
    stateName?: string;
    cityName: string;
    branchName: string;
    orderDate?: Date;
    streetName: string;
    notes?: string;
    isDeleted: boolean;
    shippingCost: number;
    totalCost?: number;
    orderProducts: {
      $id: string;
      $values :IOrderProduct[];
    }
    branchId?: number;
    deliveryId?: string;
    merchantId?: number;
    type: string;
    shippingType: string;
    paymentType: string;
    orderStatus: string;
  }
  