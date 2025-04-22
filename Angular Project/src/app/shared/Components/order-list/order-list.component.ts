import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { OrderStatus } from '../../Models/order/constants';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OrderService } from '../../Services/order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Delivery } from './../../Models/Delivery/delivery';
import { DeliveryService } from '../../Services/delivery.service';
import { IOrder } from '../../Models/order/order';
import Swal from 'sweetalert2';
import { PdfGeneratorService } from '../../Services/pdf-generator.service';
import { TranslationService } from '../../Services/translation.service';
import { AuthService } from './../../Services/auth.service';
@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent implements OnInit ,OnDestroy{
  @ViewChild('orderTable') orderTable!: ElementRef;
  orders:IOrder[] =[{}] as IOrder[] 
  orderStatus =Object.values(OrderStatus);
  delegates: Delivery[] = [{}] as Delivery[];
  filteredOrders = [...this.orders];
  searchType: string = 'status'; 
  selectedOrder:any= this.filteredOrders[0];
  role: string = '';
  startDate: string | null = null;
  endDate: string | null = null;
  pages = [1, 2, 3, 4];
  statusForm: FormGroup = new FormGroup({
      status: new FormControl(this.selectedOrder.status,{validators: [Validators.required]}),
    });
  deliveryForm: FormGroup = new FormGroup({
    delegateName: new FormControl(this.selectedOrder.delegateName,{validators: [Validators.required]}),
  });
  statusParam:string='';
  orderSubscription: any;
  deliverySubscription: any;
  statusSubscription: any;
  deliveryDeligateSubscription: any;
  actionSubscription: any;
  constructor(
    private pdfGeneratorService: PdfGeneratorService,
    private orderService: OrderService,
    private deliveryService: DeliveryService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private translationService: TranslationService
  ) { }

  ngOnInit(): void {
    this.getRole();
    this.route.paramMap.subscribe(params => {
      this.statusParam = params.get('status')??'';
      if (this.statusParam) {
        let translatedStatus = this.translationService.translateToArabic(this.statusParam);
        this.loadFilteredOrders(translatedStatus);
      } else {
        this.loadOrders();
      }
    });
  }

  loadOrders(): void {
    this.orderSubscription = this.orderService.getAllOrders().subscribe({
      next: (data: any) => {
        this.orders = data.$values;
        this.filteredOrders = [...this.orders];
      },
      error: (error: any) => {
        console.error('Error fetching orders:', error);
        Swal.fire('خطا', 'فشل تحميل بيانات الطلبات', 'error');
      }
    });
  }

  loadFilteredOrders(status: string): void {
    this.orderSubscription = this.orderService.getOrdersAfterFilter(status).subscribe({
      next: (data: any) => {
        this.orders = data.$values;
        this.filteredOrders = [...this.orders];
      },
      error: (error: any) => {
        console.error('Error fetching filtered orders:', error);
        Swal.fire('خطا', 'فشل تحميل بيانات الطلبات', 'error');
      }
    });
  }

  filterByDateRange(): void {
    if (this.startDate && this.endDate) {
      const start = new Date(this.startDate);
      const end = new Date(this.endDate);
      this.filteredOrders = this.filteredOrders.filter(order => {
        const orderDate = new Date(order.orderDate!);
        return orderDate >= start && orderDate <= end;
      });
    }
  }

  checkUserRole(permission:string): boolean {
    const selected = this.authService.hasPermission(permission);
    return selected
  }

  getRole(){
    this.role = this.authService.getUserData()?.role || '';
  }

  setSearchType(type: string): void {
    this.searchType = type;
  }

  onSearch(searchValue: string): void {
    if (searchValue.trim() === '') {
      this.filteredOrders = [...this.orders];
    } else {
      this.filteredOrders = this.orders.filter(order => {
        if (this.searchType === 'status') {
          return order.orderStatus.includes(searchValue);
        } else if (this.searchType === 'delegateName') {
          return order.deliveryId?.includes(searchValue);
        } else if (this.searchType === 'clientName') {
          return order.clientName.includes(searchValue);
        }
        return false;
      });
    }
  }

  getDelegatesByState(state: string) {
    this.deliverySubscription = this.deliveryService.getAllDeliveriesByState(state).subscribe({
      next: (data: any) => {
        this.delegates = data;
      },
      error: (error: any) => {
        console.error('Error fetching deliveries:', error);
        Swal.fire('خطا', 'فشل تحميل بيانات المناديب', 'error');
      }
    });
  }
  
  openModal(order: any, change:string): void {
    if (order) {
      this.selectedOrder = { ...order };
      let modalElement;
      if(change=="status"){
        this.statusForm.patchValue({
          orderStatus: this.selectedOrder.status
        });
         modalElement = document.getElementById('UpdateStatus');
      }else if(change=="delivery"){
        this.getDelegatesByState(this.selectedOrder.stateName);
        this.deliveryForm.patchValue({
          delegateName: this.selectedOrder.delegateName
        });
         modalElement = document.getElementById('UpdateDelivery');
      }
      if (modalElement) {
        modalElement?.classList.add('fade', 'show');
        modalElement?.setAttribute('style', 'display: block;');
      }
    }
  }

  closeModel(chnage:string): void {
    let modalElement;
    if(chnage=="status"){
       modalElement = document.getElementById('UpdateStatus');
    }else if(chnage=="delivery"){
       modalElement = document.getElementById('UpdateDelivery');
    }
    if (modalElement) {
      modalElement?.classList.remove('fade', 'show');
      modalElement?.setAttribute('style', 'display: none;');
    }
  }
  updateOrder(change:string): void {
    let modalElement;
    if(change=="status"){
      const newStatus = this.statusForm.controls['status'].value;
      this.statusSubscription = this.orderService.changeOrderStatus(this.selectedOrder.id, newStatus).subscribe({
        next: (data: any) => {
          if (this.statusParam) {
            let translatedStatus = this.translationService.translateToArabic(this.statusParam);
            this.loadFilteredOrders(translatedStatus);
          } else {
            this.loadOrders();
          }
          modalElement = document.getElementById('UpdateStatus');
          if (modalElement) {
            modalElement?.classList.remove('fade', 'show');
            modalElement?.setAttribute('style', 'display: none;');
          }
          Swal.fire('تم تغيير حالة الطلب', 'تم تغيير حالة الطلب بنجاح', 'success');
        },
        error: (error: any) => {
          console.error('Error changing order status:', error);
          Swal.fire('خطا', 'فشل تغيير حالة الطلب', 'error');
        }
      })
    }else if(change=="delivery"){
      const newDelegate = this.deliveryForm.value.delegateName;
      this.selectedOrder.delegateName = newDelegate;
      this.deliveryDeligateSubscription = this.orderService.changeOrderDelivery(this.selectedOrder.id,newDelegate ).subscribe({
        next: (data: any) => {
          if (this.statusParam) {
            let translatedStatus = this.translationService.translateToArabic(this.statusParam);
            this.loadFilteredOrders(translatedStatus);
          } else {
            this.loadOrders();
          }
          modalElement = document.getElementById('UpdateDelivery');
          if (modalElement) {
            modalElement?.classList.remove('fade', 'show');
            modalElement?.setAttribute('style', 'display: none;');
          }
          Swal.fire('تم تغيير المندوب', 'تم تغيير المندوب بنجاح', 'success');
        },
        error: (error: any) => {
          console.error('Error changing order status:', error);
          Swal.fire('خطا', 'فشل تغيير المندوب', 'error');
        }
      })
    }
  }

  printToPdf(order: any): void {
    const tableElement = this.orderTable.nativeElement;
    this.pdfGeneratorService.generatePdf(tableElement, `order_${order.id}`);
  }

  deleteOrder(id: number|undefined): void {
    if (id) {
      Swal.fire({
        title: 'هل انت متأكد',
        text: 'سيتم حذف هذا الطلب',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم, قم بالحذف',
        cancelButtonText: 'لا, الغاء',
      }).then((result) => {
        if (result.isConfirmed) {
          this.orderService.deleteOrder(id).subscribe({
            next: () => {
              this.orders = this.orders.filter(p => p.id !== id);
              this.filteredOrders = this.orders;
              Swal.fire(
                'حذف طلب!',
                'تم حذف هذا الطلب.',
                'success'
              );
            },
            error: (err) => {
              Swal.fire(
                'حذف طلب!',
                'لم يتم حذف هذا الطلب.',
                'error'
              );
              console.log("error",err.message);
            }
          });
        }
      });
    }
  }

  ngOnDestroy(): void {
    if(this.orderSubscription) this.orderSubscription.unsubscribe();
    if(this.actionSubscription) this.actionSubscription.unsubscribe();
    if(this.statusSubscription) this.statusSubscription.unsubscribe();
    if(this.deliverySubscription) this.deliverySubscription.unsubscribe();
    if(this.deliveryDeligateSubscription) this.deliveryDeligateSubscription.unsubscribe();
  }
}
