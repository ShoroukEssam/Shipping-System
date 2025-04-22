import { Component, OnInit } from '@angular/core';
import { Delivery, IDelivery, IDeliverySearch } from '../../../shared/Models/Delivery/delivery';
import { DeliveryService } from '../../../shared/Services/delivery.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delivery-list',
  templateUrl: './delivery-list.component.html',
  styleUrls: ['./delivery-list.component.css']
})
export class DeliveryListComponent implements OnInit {
  deliveries: Delivery[] = [];
  allDeliveries: Delivery[] = [];
  searchterm = '';
  recordLimit: number = 5;
  delivery: IDeliverySearch = {
    id: 0,
    name: '',
    stateId: 0
  };

  loading : boolean = false;
  constructor(private deliveryService: DeliveryService,private router: Router) { }

  ngOnInit(): void {
    this.loadDeliveries();
  }

  loadDeliveries(): void {
    this.loading = true;
    this.deliveryService.getAllDeliveries().subscribe({
      next: (deliveries: any) => {
        this.deliveries = deliveries.$values;
        this.allDeliveries = deliveries.$values;
        this.loading = false;
      },
      error: (err) => {
        Swal.fire(
          'عرض !',
          'حدث خطأ في عرض الطلبات',
          'error'
        );
        console.error(err)
        this.loading = false
      }
    });
  }

  deleteDelivery(id: string): void {
    Swal.fire({
      title: 'هل انت متأكد',
      text: 'سيتم حذف هذا المندوب',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم, قم بالحذف',
      cancelButtonText: 'لا, الغاء',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deliveryService.deleteDelivery(id).subscribe({
          next: () => {
            this.deliveries = this.deliveries.filter(d => d.deliveryId !== id);
            this.allDeliveries = this.allDeliveries.filter(d => d.deliveryId !== id);
            Swal.fire(
              'حذف المندوب!',
              'تم حذف هذا المندوب.',
              'success'
            );
          },
          error: (err) => {
            Swal.fire(
              'حذف المندوب!',
              'لم يتم حذف هذا المندوب.',
              'error'
            );
            console.log("error", err);
          }
        });
      }
    });
  }
  toggleStateStatus(state: Delivery): void {
    if (!state.deliveryId) {
        console.error('State ID is undefined');
        Swal.fire(
            'خطأ في تحديث الحالة!',
            'معرف الحالة غير محدد.',
            'error'
        );
        return;
    }

    const previousStatus = state.status;
    state.status = !state.status;

    console.log('Sending request to change status for ID:', state.id);
    console.log('New status:', state.status);

    this.deliveryService.changeStatus(state.deliveryId, state.status).subscribe({
        next: () => {
            Swal.fire(
                'تحديث الحالة!',
                'تم تحديث حالة المندوب بنجاح.',
                'success'
            );
        },
        error: (error: any) => {
            Swal.fire(
                'تحديث الحالة!',
                `حدث خطأ أثناء تحديث الحالة: ${error.error || error.message}`,
                'error'
            );
            console.error('Error updating state status:', error);
            state.status = previousStatus;
        }
    });
}
}
