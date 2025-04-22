import { Injectable } from '@angular/core';
import { OrderStatus, OrderType, PaymentType, ShippingType } from '../Models/order/constants';
import { DiscountType } from '../Models/Delivery/enum';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  constructor() { }
  private arabicToEnglish: { [key: string]: string } = {
    [OrderStatus.New]: 'new',
    [OrderStatus.Pending]: 'pending',
    [OrderStatus.DeliveredToDelegate]: 'delivered_to_delegate',
    [OrderStatus.Delivered]: 'delivered',
    [OrderStatus.Unreachable]: 'unreachable',
    [OrderStatus.Postponed]: 'postponed',
    [OrderStatus.PartiallyDelivered]: 'partially_delivered',
    [OrderStatus.CancelledByClient]: 'cancelled_by_client',
    [OrderStatus.RefusedWithPayment]: 'refused_with_payment',
    [OrderStatus.RefusedWithPartialPayment]: 'refused_with_partial_payment',
    [OrderStatus.RefusedWithoutPayment]: 'refused_without_payment'
  };

  private englishToArabic: { [key: string]: string } = {
    'new': OrderStatus.New,
    'pending': OrderStatus.Pending,
    'delivered_to_delegate': OrderStatus.DeliveredToDelegate,
    'delivered': OrderStatus.Delivered,
    'unreachable': OrderStatus.Unreachable,
    'postponed': OrderStatus.Postponed,
    'partially_delivered': OrderStatus.PartiallyDelivered,
    'cancelled_by_client': OrderStatus.CancelledByClient,
    'refused_with_payment': OrderStatus.RefusedWithPayment,
    'refused_with_partial_payment': OrderStatus.RefusedWithPartialPayment,
    'refused_without_payment': OrderStatus.RefusedWithoutPayment
  };

  private arabicToOrderType: { [key: string]: OrderType } = {
    "تسليم_فالفرع": OrderType.تسليم_فالفرع,
    "توصيل_الي_المنزل": OrderType.توصيل_الي_المنزل
  };

  private arabicToPaymentType: { [key: string]: PaymentType } = {
    "واجبة_التحصيل": PaymentType.واجبة_التحصيل,
    "دفع_مقدم": PaymentType.دفع_مقدم,
    "طرد_مقابل_طرد": PaymentType.طرد_مقابل_طرد
  };

  private arabicToShippingType: { [key: string]: ShippingType } = {
    "توصيل_في_نفس_اليوم": ShippingType.توصيل_في_نفس_اليوم,
    "توصيل_سريع": ShippingType.توصيل_سريع,
    "توصيل_عادي": ShippingType.توصيل_عادي
  };

  private arabicdiscountType: { [key: string]: DiscountType } = {
    "نسبة_مئوية": DiscountType.نسبة_مئوية,
    "رقم": DiscountType.رقم
  };
  
  translateToEnglish(arabic: string): string {
    return this.arabicToEnglish[arabic] || arabic;
  }

  translateToArabic(english: string): string {
    return this.englishToArabic[english]  || english;
  }
  mapOrderType(type: string): OrderType {
    return this.arabicToOrderType[type];
  }

  mapPaymentType(type: string): PaymentType {
    return this.arabicToPaymentType[type];
  }

  mapShippingType(type: string): ShippingType {
    return this.arabicToShippingType[type];
  }

  mapDiscountType(type: string): DiscountType {
    return this.arabicdiscountType[type];
  }
}
