import { Component, OnInit } from '@angular/core';
import { StateService } from './../../../shared/Services/state.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { IState } from '../../../shared/Models/IState';



@Component({
  selector: 'app-state-list',
  templateUrl: './state-list.component.html',
  styleUrls: ['./state-list.component.css']
})
export class StateListComponent implements OnInit {

  States: IState[] =[];
  searchterm = '';
  recordLimit: number = 5;
  loading : boolean = false;

  constructor(private stateService: StateService, private router: Router) {}

  ngOnInit(): void {
    this.loadStates();
  }

  loadStates(): void {
    this.loading = true;
    this.stateService.getGovernments().subscribe({
      next: (response) => {
        this.States = response;
        console.log(this.States);
        this.loading = false;
      },
      error: (error) => {
        Swal.fire(
          'عرض !',
          'حدث خطأ في عرض المحافظات',
          'error'
        );
        console.error('Error fetching states:', error);
        this.loading = false;
      }
    });
  }

  deleteState(stateId: number): void {
    Swal.fire({
      title: 'هل انت متأكد',
      text: 'سيتم حذف هذه المحافظة',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم, قم بالحذف',
      cancelButtonText: 'لا, الغاء',
    }).then((result) => {
      if (result.isConfirmed) {
        this.stateService.deleteGovernment(stateId).subscribe({
          next: () => {
            this.States = this.States.filter(p => p.id !== stateId);
            Swal.fire(
              'حذف محافظة!',
              'تم حذف هذه المحافظة.',
              'success'
            );
          },
          error: (error: any) => {
            Swal.fire(
              'حذف المحافظة!',
              'لم يتم حذف هذه المحافظة.',
              'error'
            );
            console.error('Error deleting state:', error);
          }
        });
      }
    });
  }

  toggleStateStatus(state: IState): void {
    state.status = !state.status;
    this.stateService.editGovernment(state.id, state).subscribe({
      next: () => {
        Swal.fire(
          'تحديث الحالة!',
          'تم تحديث حالة المحافظة بنجاح.',
          'success'
        );
      },
      error: (error: any) => {
        Swal.fire(
          'تحديث الحالة!',
          `حدث خطأ أثناء تحديث الحالة: ${error.error}`,
          'error'
        );
        console.error('Error updating state status:', error);
      }
    });
  }

  viewCities(stateId: number): void {
    this.router.navigate(['/employee/city', stateId]);
  }

  viewState(stateId: number): void {
    this.router.navigate(['/employee/state', stateId]);
  }

  editState(stateId: number): void {
    this.router.navigate(['/employee/state', stateId]);
  }
}
