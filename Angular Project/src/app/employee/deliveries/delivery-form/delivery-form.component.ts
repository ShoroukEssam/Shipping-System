import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DeliveryService } from '../../../shared/Services/delivery.service';
import { Subscription } from 'rxjs';
import Swal from 'sweetalert2';
import { Government } from '../../../shared/Models/government';
import { city } from '../../../shared/Models/city';
import { Branch } from '../../../shared/Models/branch';
import { OrderService } from '../../../shared/Services/order.service';
import { StateService } from '../../../shared/Services/state.service';
import { DiscountType } from '../../../shared/Models/Delivery/enum';
import { TranslationService } from '../../../shared/Services/translation.service';

@Component({
  selector: 'app-delivery-form',
  templateUrl: './delivery-form.component.html',
  styleUrls: ['./delivery-form.component.css']
})
export class DeliveryFormComponent implements OnInit, OnDestroy {
  deliveryForm: FormGroup;
  states: Government[] = [];
  branches: Branch[] = [];
  id: string | null = null;
  deliverySubscription: Subscription | undefined;
  updateSubscription: Subscription | undefined;
  discountTypes = Object.values(DiscountType).filter(val => isNaN(Number(val)));
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private deliveryService: DeliveryService,
    private OrderService: OrderService,
    private stateService: StateService,
    private translationService: TranslationService
  ) {
    this.deliveryForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      government: ['', Validators.required],
      address: ['', Validators.required],
      discountType: ['', Validators.required],
      companyPercentage: [0, Validators.required],
      branchId: [{value :'',disabled:true}, Validators.required],
      password: ['']
    });
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id && this.id !== '0') {
      this.deliverySubscription = this.deliveryService.getDeliveryById(this.id).subscribe({
        next: (delivery) => {
          this.deliveryForm.patchValue(delivery);
        },
        error: (err) => {
          console.error(err.message);
          Swal.fire(
            'Error!',
            'An error occurred while fetching the delivery data',
            'error'
          );
        }
      });
    }
    this.stateService.getGovernments().subscribe({
      next: data => {
        this.states = data;
      },
      error: err => {
        console.log(err);
        Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات المحافظات', 'error');
    }});
  }


  ngOnDestroy(): void {
    if (this.deliverySubscription) {
      this.deliverySubscription.unsubscribe();
    }
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
  }

  onSubmit(): void {
    if (this.deliveryForm.valid) {
      const delivery: any = this.deliveryForm.value;
      const serializedGovernment = this.deliveryForm.controls['government'].value;
      const government = JSON.parse(serializedGovernment);
      delivery.government = government.name;
      const discountType = this.deliveryForm.controls['discountType'].value;
      const mappedType = this.translationService.mapDiscountType(discountType);
      delivery.discountType = mappedType;
      if (this.id === '0') {
        this.updateSubscription = this.deliveryService.addDelivery(delivery).subscribe({
          next: () => {
            Swal.fire(
              'Success!',
              'Delivery added successfully',
              'success'
            );
            this.router.navigate(['/employee/delivery']);
          },
          error: (err) => {
            console.error('Error adding delivery', err);
            Swal.fire(
              'Error!',
              'An error occurred while adding the delivery',
              'error'
            );
          }
        });
      } else {
        delivery.deliveryId = this.id;
        this.updateSubscription = this.deliveryService.updateDelivery(this.id!, delivery).subscribe({
          next: () => {
            Swal.fire(
              'Success!',
              'Delivery updated successfully',
              'success'
            );
            this.router.navigate(['/employee/delivery']);
          },
          error: (err) => {
            console.error('Error updating delivery', err);
            Swal.fire(
              'Error!',
              'An error occurred while updating the delivery',
              'error'
            );
          }
        });
      }
    }
  }

  onStateChange(event: any): void {
    const serializedState = event.target.value;
    const state = JSON.parse(serializedState);
    console.log(state);
    if (state) {
      console.log(state);
      this.OrderService.getBranchesByGovernment(state.id).subscribe({
        next: (data: any) => {
          console.log(data);
          this.branches = data.$values;
          this.deliveryForm.get('branchId')?.enable();
        },
        error: err => {
          console.log(err);
        }
      });
    } else {
      this.branches = [];
      this.deliveryForm.get('branchId')?.disable();
    }
  }
  
  onCancel(): void {
    this.router.navigate(['/employee/delivery']);
  }

  navigateBack(): void {
    this.router.navigate(['/employee/delivery']);
  }

  serializeState(state: any): string {
    return JSON.stringify(state);
  }
  
}
