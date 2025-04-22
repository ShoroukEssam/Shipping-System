import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WeightSettingService } from '../../../shared/Services/weight-setting.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-wight-setting',
  templateUrl: './wight-setting.component.html',
  styleUrls: ['./wight-setting.component.css']
})
export class WightSettingComponent implements OnInit, OnDestroy {
  updateSubscription: any;
  weightSettingSubscription: any;
  weightSettingData = new FormGroup({
    standaredWeight: new FormControl("", [Validators.required, Validators.min(1)]),
    addition_Cost: new FormControl("", [Validators.required, Validators.min(10)]),
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private weightSettingService: WeightSettingService
  ) {}

  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
    if (this.weightSettingSubscription) {
      this.weightSettingSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.weightSettingSubscription = this.weightSettingService.getWeightSetting().subscribe({
      next: (response: any) => {
        this.weightSettingData.patchValue(response);
      },
      error: (err) => {
        console.log(err.message);
        Swal.fire(
          'عرض !',
          'حدث خطأ اثناء عرض اعدادات الوزن',
          'error'
        );
      }
    });
  }

  get getStandaredWeight() {
    return this.weightSettingData.controls['standaredWeight'];
  }

  get getAdditionCost() {
    return this.weightSettingData.controls['addition_Cost'];
  }

  onSubmit() {
    if (this.weightSettingData.valid) {
      const data: any = this.weightSettingData.value;
      data.id=1;
      console.log(data);
      this.updateSubscription = this.weightSettingService.updateWeightSetting(data).subscribe({
        next: () => {
          console.log('Weight Setting updated successfully');
          Swal.fire(
            'تم التعديل!',
            'اعدادات الوزن تم تعديلها',
            'success'
          );
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error updating Weight Setting', err);
          Swal.fire(
            'تعديل !',
            'حدث خطأ اثناء تعديل اعدادات الوزن',
            'error'
          );
        }
      });
    }
  }

  onCancel() {
    this.router.navigate(['/']);
  }
}
