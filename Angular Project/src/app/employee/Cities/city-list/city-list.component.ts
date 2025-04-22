import { Component, OnInit } from '@angular/core';
import { CityService } from '../../../shared/Services/city.service';
import { city } from '../../../shared/Models/city';
import Swal from 'sweetalert2';
import { ActivatedRoute, Router } from '@angular/router';
import { routes } from './../../../app.routes';

@Component({
  selector: 'app-city-list',
  templateUrl: './city-list.component.html',
  styleUrls: ['./city-list.component.css'],
})
export class CityListComponent implements OnInit {
  cityData: city[] = [];
  searchterm = '';
  recordLimit: number = 5;
  stateId: number = 0;
  loading : boolean = false;

  constructor(private _CityService: CityService,private activatedRoute: ActivatedRoute,private router: Router) {}

  ngOnInit(): void {
    this.loading = true;
    this.stateId = Number(this.activatedRoute.snapshot.paramMap.get('id'));
    this._CityService.getAllcities(this.stateId).subscribe({
      next: (response) => {
        this.cityData = response;
        console.log(this.cityData);
        this.loading = false;
      },
      error: (err) => {
        Swal.fire(
          'عرض !',
          'حدث خطأ في عرض المدن',
          'error'
        );
        console.error('Error fetching cities', err);
        this.showApiConnectionErrorAlert();
        this.loading = false;
      },
    });
  }

  deleteCity(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'لن تتمكن من استعادة هذه المدينة!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'نعم، احذفها!',
      cancelButtonText: 'لا، احتفظ بها',
    }).then((result) => {
      if (result.isConfirmed) {
        this._CityService.deleteCity(id).subscribe({
          next: () => {
            console.log(`City with id ${id} deleted successfully`);
            this.cityData = this.cityData.filter((city) => city.id !== id);
            this.showDeleteSuccessAlert();
          },
          error: (err) => {
            console.error(`Error deleting City with id ${id}`, err);
            this.showDeleteErrorAlert();
          },
        });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire('ملغاة', 'مدينتك آمنة :)', 'error');
      }
    });
  }

  changeStatus(city: city) {
    const newStatus = !city.status;
    this._CityService.changeCityStatus(city.id, newStatus).subscribe({
      next: (response) => {
        city.status = newStatus;
        console.log('City status changed successfully', response);
        this.showStatusChangeSuccessAlert();
      },
      error: (err) => {
        console.error('Error changing city status', err);
        this.showStatusChangeErrorAlert();
      },
    });
  }

  addCity() {
    this.router.navigate(['/employee/city', this.stateId,0]);
  }

  private showDeleteSuccessAlert(): void {
    Swal.fire('تم الحذف!', 'تم حذف المدينة بنجاح.', 'success');
  }

  private showDeleteErrorAlert(): void {
    Swal.fire('خطأ!', 'فشلت عملية حذف المدينة.', 'error');
  }

  private showStatusChangeSuccessAlert(): void {
    Swal.fire('نجاح!', 'تم تغيير حالة المدينة بنجاح.', 'success');
  }

  private showStatusChangeErrorAlert(): void {
    Swal.fire('خطأ!', 'فشلت عملية تغيير حالة المدينة.', 'error');
  }

  private showApiConnectionErrorAlert(): void {
    Swal.fire('خطأ', 'فشل الاتصال بواجهة البرمجة.', 'error');
  }
}
