import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../shared/Services/api.service';
import { IGovernmentDTO } from '../../../shared/Models/IGovernmentDTO';
import { ICityDTO } from '../../../shared/Models/ICityDTO';
import { IMerchantDTO, ISpecialPrice } from '../../../shared/Models/IMerchant';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-merchant-form',
  templateUrl: './merchant-form.component.html',
  styleUrls: ['./merchant-form.component.css']
})
export class MerchantFormComponent implements OnInit {
  merchantForm: FormGroup;
  governments: IGovernmentDTO[] = [];
  cities: ICityDTO[] = [];
  branches:any[] = [];
  specialCities: ICityDTO[][] = [];
  id:string|null = null;
  specialPrices:ISpecialPrice[] = [{}] as ISpecialPrice[];
  constructor(
    private apiService: ApiService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.merchantForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^01[0-9]{9}$')]],
      password: ['', Validators.minLength(8)],
      government: ['', Validators.required],
      city: [{ value: '', disabled: true}, Validators.required],
      branchName: [{value :'',disabled:true}, Validators.required],
      pickUpSpecialCost: [0],
      refusedOrderPercent: [0],
      specialCitiesPrices: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.loadGovernments();
    this.id = this.route.snapshot.paramMap.get('id');
    if (this.id && this.id !== '0') {
      console.log(this.id);
      this.loadMerchantDetails(this.id);
    }
  }

  loadMerchantDetails(id: string): void {
    this.apiService.get<IMerchantDTO>(`/Merchant/GetMerchantById/${id}`).subscribe({
      next: (res) => {
        this.merchantForm.patchValue({
          name: res.name,
          email: res.email,
          phone: res.phone,
          password: null,
          government: this.serializeState(this.deserializeState(res.government)),
          branchName: res.branchName,
          city: res.city,
          pickUpSpecialCost: res.pickUpSpecialCost,
          refusedOrderPercent: res.refusedOrderPercent,
          specialCitiesPrices: []
        });

        console.log(this.merchantForm.controls['specialCitiesPrices'].value);
        console.log(this.specialCitiesPrices.length);
        this.specialCitiesPrices.clear();
        this.specialPrices = Array.isArray(res.specialCitiesPrices.$values) ? res.specialCitiesPrices.$values : [];
        console.log(this.specialPrices);
        this.specialPrices.forEach((price: ISpecialPrice) => {
          const specialFormGroup = this.fb.group({
            government: [this.serializeState(this.deserializeState(price.government)), Validators.required],
            city: [{ value: price.city, disabled: true}, Validators.required],
            price: [price.price, Validators.required]
          });
          this.specialCitiesPrices.push(specialFormGroup);
        });
      },
      error: err => {
        console.log(err);
        Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات التاجر', 'error');
      }
    });
  }
  
  

  loadGovernments(): void {
    this.apiService.get<any>('/Government').subscribe({
      next: (res) => {
        this.governments = res.$values as IGovernmentDTO[];
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  onGovChange(): void {
    const gov = this.merchantForm.get('government')?.value;
    const state = JSON.parse(gov);
    if (state) {
      this.apiService.get<any>(`/City/government/${state.id}`).subscribe({
        next: (res) => {
          this.cities = res.$values as ICityDTO[];
          this.merchantForm.get('city')?.enable();
        },
        error: (err) => {
          console.error(err);
        }
      });
      this.apiService.get<any>(`/Order/GetBranchesByGovernment?government=${state.id}`).subscribe({
        next: (data: any) => {
          this.branches = data.$values;
          this.merchantForm.get('branchName')?.enable();
        },
        error: err => {
          console.log(err);
          Swal.fire('خطأ', 'حدث خطأ في تحميل بيانات الفروع', 'error');
        }
      })
    } else {
      this.cities = [];
      this.branches = [];
      this.merchantForm.get('city')?.setValue('');
      this.merchantForm.get('city')?.disable();
      this.merchantForm.get('branchName')?.setValue('');
      this.merchantForm.get('branchName')?.disable();
    }
  }

  onSpecialGovChange(index: number): void {
    const gov = this.specialCitiesPrices.at(index).get('government')?.value;
    const state = JSON.parse(gov);
    if (state) {
      this.apiService.get<any>(`/City/government/${state.id}`).subscribe({
        next: (res) => {
          this.specialCities[index] = res.$values as ICityDTO[];
          const cityControl = this.specialCitiesPrices.at(index).get('city');
          cityControl?.enable();
        },
        error: (err) => {
          console.error(err);
        }
      });
    }
  }

  get specialCitiesPrices(): FormArray {
    return this.merchantForm.get('specialCitiesPrices') as FormArray;
  }

  addspecialCitiesPrices(): void {
    const specialFormGroup = this.fb.group({
      government: ['', Validators.required],
      city: [{ value: '', disabled: true }, Validators.required],
      price: [0, Validators.required]
    });
    this.specialCitiesPrices.push(specialFormGroup);
    console.log(this.specialCitiesPrices.value);
  }

  removespecialCitiesPrices(index: number): void {
    this.specialCitiesPrices.removeAt(index);
    console.log(this.specialCitiesPrices.value);
  }

  onSubmit(): void {
    this.merchantForm.controls['specialCitiesPrices'].value.forEach((value: any) => {
      const government = JSON.parse(value.government);
      value.government = government.name;
    });
    const merchant = this.merchantForm.value;
    const serializedGovernment = this.merchantForm.controls['government'].value;
    const government = JSON.parse(serializedGovernment);
    merchant.government = government.name;
    console.log(merchant);
    if (this.merchantForm.valid) {
      if (this.id && this.id == '0') {
        this.apiService.post<any, IMerchantDTO>('/Merchant/AddMerchant', merchant).subscribe({
          next: (res) => {
            this.router.navigateByUrl('/employee/merchant');
          },
          error: (err) => {
            console.log(err);
          }
        });
      }
      else {
        merchant.id = this.id;
        this.apiService.put<any, IMerchantDTO>('/Merchant/UpdateMerchant' , merchant).subscribe({
          next: (res) => {
            this.router.navigateByUrl('/employee/merchant');
          },
          error: (err) => {
            console.log(err);
          }
        });
      }
    } else {
      this.merchantForm.markAllAsTouched();
    }
  }

  serializeState(state: any): string {
    return JSON.stringify(state);
  }

  deserializeState(state: string): any {
    const gov = this.governments.filter(gov => gov.name == state);
    return gov[0];
  }
  
  trackByGov(index: number, gov: IGovernmentDTO): number|undefined {
    return gov.id; // or unique identifier for government
  }

  trackByCity(index: number, city: ICityDTO): number|undefined  {
    return city.id; // or unique identifier for city
  }
}
