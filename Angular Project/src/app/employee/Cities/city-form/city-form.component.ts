import { Component, OnInit } from '@angular/core';
import { city } from '../../../shared/Models/city';
import { Government } from '../../../shared/Models/government';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CityService } from '../../../shared/Services/city.service';
import { GovernmentService } from '../../../shared/Services/government.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-city-form',
  templateUrl: './city-form.component.html',
  styleUrls: ['./city-form.component.css'],
})
export class CityFormComponent implements OnInit {
  cityId: number | null = 0;
  city: city | null = null;
  stateId: number | null = 0;
  GovernmentData: Government[] = [];

  cityData = new FormGroup({
    roleName: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    shippingPrice: new FormControl(0, Validators.required),
    pickUpPrice: new FormControl(0, Validators.required),
  });

  constructor(
    private _ActivatedRoute: ActivatedRoute,
    private _Router: Router,
    private _cityService: CityService,
    private _GovernmentService: GovernmentService
  ) {}

  ngOnInit(): void {
    const id = this._ActivatedRoute.snapshot.paramMap.get('id');
    this.stateId = Number(this._ActivatedRoute.snapshot.paramMap.get('state'));
    this.cityId = id !== null ? +id : null;
    this._GovernmentService.getGovernments().subscribe({
      next: (response) => {
        this.GovernmentData = response;
        console.log(this.GovernmentData);
      },
      error: (err) => {
        console.error('Error fetching governments', err);
        this.showApiConnectionErrorAlert();
      },
    });
    if (this.cityId !== null && this.cityId != 0) {
      this._cityService.getCityById(this.cityId).subscribe({
        next: (response) => {
          this.city = response;
          this.cityData.patchValue({
            roleName: this.city.name,
            shippingPrice: this.city.shippingPrice,
            pickUpPrice: this.city.pickUpPrice,
          });
          console.log('City fetched successfully', this.city);
        },
        error: (err) => {
          console.error('Error fetching city', err);
          this.showApiConnectionErrorAlert();
        },
      });
    }
  }

  onSubmit() {
    if (this.cityData.valid) {
      const city: city = {
        id: this.cityId || 0,
        name: this.cityData.value.roleName!,
        status: true, // Assuming a default value
        governmentId: this.stateId || -1,
        shippingPrice: this.cityData.value.shippingPrice!,
        pickUpPrice: this.cityData.value.pickUpPrice!,
      };

      if (this.cityId && this.cityId !== 0) {
        console.log(city, this.cityId,this.stateId);
        this._cityService.updatecityById(this.cityId, city).subscribe({
          next: (response) => {
            console.log('City updated successfully', response);
            this.showUpdateSuccessAlert();
            this._Router.navigate([`/employee/city/${this.stateId}`]);
          },
          error: (err) => {
            console.error('Error updating city', err);
            this.showUpdateErrorAlert();
          },
        });
      } else {
        this._cityService.addCity(city).subscribe({
          next: (response) => {
            console.log('City added successfully', response);
            this.showAddSuccessAlert();
            this._Router.navigate([`/employee/city/${this.stateId}`]);
          },
          error: (err) => {
            console.error('Error adding city', err);
            this.showAddErrorAlert();
          },
        });
      }
    }
  }

  onCancel() {
    this._Router.navigate(['/employee/city']);
  }

  private showAddSuccessAlert(): void {
    Swal.fire('نجاح', 'تمت إضافة المدينة بنجاح!', 'success');
  }

  private showAddErrorAlert(): void {
    Swal.fire('خطأ', 'فشلت عملية إضافة المدينة.', 'error');
  }

  private showUpdateSuccessAlert(): void {
    Swal.fire('نجاح', 'تم تحديث بيانات المدينة بنجاح!', 'success');
  }

  private showUpdateErrorAlert(): void {
    Swal.fire('خطأ', 'فشلت عملية تحديث بيانات المدينة.', 'error');
  }

  private showApiConnectionErrorAlert(): void {
    Swal.fire('خطأ', 'فشل الاتصال بواجهة البرمجة.', 'error');
  }
}
