export enum ShippingType {
   'توصيل_في_نفس_اليوم',
   'توصيل_سريع',
   'توصيل_عادي'
 }
 
 export enum PaymentType {
   'واجبة_التحصيل',
   'دفع_مقدم',
   'طرد_مقابل_طرد'
 }
 
 export enum OrderType {
    'تسليم_فالفرع'= 0,
   'توصيل_الي_المنزل' = 1
 }
 
 export enum OrderStatus {
  New = 'جديد',
  Pending = 'قيد_الانتظار',
  DeliveredToDelegate = 'تم_التسليم_للمندوب',
  Delivered = 'تم_التسليم',
  Unreachable = 'لا_يمكن_الوصول',
  Postponed = 'تم_التأجيل',
  PartiallyDelivered = 'تم_التسليم_جزئيا',
  CancelledByClient = 'تم_الإلغاء_من_جهة_العميل',
  RefusedWithPayment = 'تم_الرفض_مع_الدفع',
  RefusedWithPartialPayment = 'رفض_مع_سداد_جزء',
  RefusedWithoutPayment = 'رفض_ولم_يتم_الدفع',
}