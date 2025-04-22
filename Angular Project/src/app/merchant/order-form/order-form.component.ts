import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../shared/Services/api.service';
import { IOrder } from '../../shared/Models/order/order';
import { IOrderProduct } from './../../shared/Models/order/order';
import { OrderService } from './../../shared/Services/order.service';
import { CityService } from '../../shared/Services/city.service';
import { city } from '../../shared/Models/city';
import { Branch } from '../../shared/Models/branch';
import { StateService } from './../../shared/Services/state.service';
import { Government } from '../../shared/Models/government';
import { OrderStatus, OrderType, PaymentType, ShippingType } from '../../shared/Models/order/constants';
import { TranslationService } from './../../shared/Services/translation.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements OnInit ,OnDestroy{
  orderForm: FormGroup;
  states: Government[] = [];
  cities: city[] = [];
  branches: Branch[] = [];
  shippingTypes = Object.values(ShippingType).filter(val => isNaN(Number(val)));
  paymentTypes = Object.values(PaymentType).filter(val => isNaN(Number(val)));
  orderTypes = Object.keys(OrderType).filter(val => isNaN(Number(val)));
  isEditMode: boolean = false;
  orderId: number = 0;
  merchantId: number = 0;
  Products:IOrderProduct[] = [{}] as IOrderProduct[];
  orderSubscription: any;
  citySubscription: any;
  branchSubscription: any;
  actionSubscription: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private orderService: OrderService,
    private stateService: StateService,
    private translationService: TranslationService
  ) {
    this.orderForm = new FormGroup({
      type: new FormControl('', Validators.required),
      clientName: new FormControl('', Validators.required),
      clientPhoneNumber1: new FormControl('', Validators.required),
      clientPhoneNumber2: new FormControl(''),
      clientEmail: new FormControl('', [Validators.required, Validators.email]),
      stateName: new FormControl('', Validators.required),
      cityName: new FormControl({ value: '', disabled: true }, Validators.required),
      branchName: new FormControl({ value: '', disabled: true }, Validators.required),
      streetName: new FormControl('', Validators.required),
      shippingType: new FormControl('', Validators.required),
      paymentType: new FormControl('', Validators.required),
      isVillage: new FormControl(false),
      shippingCost: new FormControl({ value: 0, disabled: true }),
      totalWeight: new FormControl({ value: 0, disabled: true }),
      orderCost: new FormControl(0),
      notes: new FormControl(''),
      orderProducts: new FormArray([])
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.orderId = +params['id'];
      if (this.orderId && this.orderId !== 0) {
        this.isEditMode = true;
        this.loadOrderData(this.orderId);
      }
    });

    this.orderProducts.valueChanges.subscribe(() => this.calculateTotals());
    this.stateService.getGovernments().subscribe({
      next: data => {
        this.states = data;
      },
      error: err => {
        console.log(err);
        Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات المحافظات', 'error');
      }
    });
  }

  get orderProducts(): FormArray {
    return this.orderForm.get('orderProducts') as FormArray;
  }

  loadOrderData(id: number): void {
    this.orderSubscription = this.orderService.getOrderReceipt(id).subscribe({
      next: (order: IOrder) => {
        console.log(order);
         this.merchantId = order.merchantId??0;
        this.orderForm.patchValue({
          ...order,
          orderProducts: []
        });
        this.orderProducts.clear();
        this.Products = Array.isArray(order.orderProducts.$values) ? order.orderProducts.$values : [];
        this.Products.forEach((product: IOrderProduct) => {
          this.orderProducts.push(
            new FormGroup({
              productName: new FormControl(product.productName),
              productQuantity: new FormControl(product.productQuantity),
              weight: new FormControl(product.weight)
            })
          );
        });
      },
      error: err => {
        console.log(err);
        Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات الطلب', 'error');
      }
    });
  }

  onStateChange(event: any): void {
    const state = event.target.value;
    if (state) {
      this.citySubscription = this.orderService.getCitiesByGovernment(state).subscribe({
        next: (data: any) => {
          this.cities = data.$values;
          this.orderForm.get('cityName')?.enable();
        },
        error: err => {
          console.log(err);
          Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات المدن', 'error');
        }
      });
      this.branchSubscription = this.orderService.getBranchesByGovernment(state).subscribe({
        next: (data: any) => {
          this.branches = data.$values;
          this.orderForm.get('branchName')?.enable();
        },
        error: err => {
          console.log(err);
          Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات الفروع', 'error');
        }
      });
    } else {
      this.cities = [];
      this.branches = [];
      this.orderForm.get('cityName')?.disable();
      this.orderForm.get('branchName')?.disable();
    }
  }

  addProduct(): void {
    const productFormGroup = new FormGroup({
      productName: new FormControl(''),
      productQuantity: new FormControl(1),
      weight: new FormControl(0)
    });
    this.orderProducts.push(productFormGroup);
    this.calculateTotals();
  }

  removeProduct(index: number): void {
    this.orderProducts.removeAt(index);
    this.calculateTotals();
  }

  calculateTotals(): void {
    const products = this.orderProducts.value;
    const totalWeight = products.reduce((sum: number, product: any) => sum + (product.weight * product.productQuantity), 0);
    this.orderForm.patchValue({ totalWeight }, { emitEvent: false });
  }

  onSubmit(): void {
    if (this.orderForm.valid) {
      this.orderForm.get('totalWeight')?.enable();
      const formValues = this.orderForm.value;

      const type = this.orderForm.controls['type'].value;
      const shippingType = this.orderForm.controls['shippingType'].value;
      const paymentType = this.orderForm.controls['paymentType'].value;

      const mappedType = this.translationService.mapOrderType(type);
      const mappedShippingType = this.translationService.mapShippingType(shippingType);
      const mappedPaymentType = this.translationService.mapPaymentType(paymentType);

      const formData = {
        ...formValues,
        type: mappedType,
        shippingType: mappedShippingType,
        paymentType: mappedPaymentType,
      };
  
      if (this.isEditMode) {
        formData.merchantId=this.merchantId;
        this.actionSubscription = this.orderService.editOrder(this.orderId, formData).subscribe({
          next: (data: any) => {
            Swal.fire('نجاح!', 'تم تعديل الطلب بنجاح', 'success');
            this.navigateToDashboard();
          },
          error: err => {
            console.log(err);
            Swal.fire('خطأ', 'حدث خطأ في تعديل الطلب', 'error');
          }
        });
      } else {
        this.actionSubscription = this.orderService.addOrder(formData).subscribe({
          next: (data: any) => {
            Swal.fire('نجاح!', 'تم اضافة الطلب بنجاح', 'success');
            this.navigateToDashboard();
          },
          error: err => {
            console.log(err);
            Swal.fire('خطأ', 'حدث خطأ في اضافة الطلب', 'error');
          }
        });
      }
    }
  }
  
  navigateToDashboard(): void {
    this.router.navigate(['/shared/dashboard']);
  }

  ngOnDestroy(): void {
    if (this.orderSubscription) this.orderSubscription.unsubscribe();
    if (this.citySubscription) this.citySubscription.unsubscribe();
    if (this.branchSubscription) this.branchSubscription.unsubscribe();
    if (this.actionSubscription) this.actionSubscription.unsubscribe();
  }
}
