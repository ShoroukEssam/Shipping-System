import { IPermission, IPermissionResponse } from '../../../shared/Models/Permissions/permission';
import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { PermissionsService } from './../../../shared/Services/permissions.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-permission-list',
  providers: [],
  templateUrl: './permission-list.component.html',
  styleUrl: './permission-list.component.css',
})
export class PermissionListComponent implements OnInit,OnDestroy {
  permissions: IPermission[]=[];
  PermissionSubscription: any;
  PermissionDeleteSubscription: any;
  loading : boolean = false;


  constructor(public permissionsService: PermissionsService) {}
  ngOnInit(): void {
    this.getPermissions();
  }
  getPermissions(): void {
    this.loading = true;
    this.PermissionSubscription = this.permissionsService.getPermissions().subscribe({
      next: (response: any) => {
        this.permissions = response.$values;
        this.loading = false;
      },
      error: (err) => {
        Swal.fire(
          'عرض !',
          'حدث خطأ في عرض الصلاحيات',
          'error'
        );
        console.log(err.message);
        this.loading = false;
      }
    });
  }
  onSearch(query: string): void {
    if (query.trim() !== '') {
      console.log('Searching for:', query);
      this.permissionsService.searchPermissions(query).subscribe({
        next: (response: any) => {
          console.log(response);
          this.permissions = response.$values;
        },
        error: (err) => {
          Swal.fire(
            'بحث !',
            'حدث خطأ في البحث عن هذه الصلاحية',
            'error'
          );
          console.error('Error searching permissions', err);
        }
      });
    } else {
      this.getPermissions();
    }
  }
  deletePermission(id: string): void {
    Swal.fire({
      title: 'هل انت متأكد',
      text: 'سيتم حذف هذا الصلاحيه',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم, قم بالحذف',
      cancelButtonText: 'لا, الغاء',
    }).then((result) => {
      if (result.isConfirmed) {
        this.PermissionDeleteSubscription = this.permissionsService.deleteRole(id).subscribe({
          next: () => {
            this.permissions = this.permissions.filter(p => p.id !== id);
            Swal.fire(
              'حذف صلاحية!',
              'تم حذف هذه الصلاحية.',
              'success'
            );
          },
          error: (err) => {
            Swal.fire(
              'حذف صلاحية!',
              'لم يتم حذف هذه الصلاحية.',
              'error'
            );
            console.log("error",err.message);
          }
        });
      }
    });
  }
  ngOnDestroy(): void {
    if (this.PermissionSubscription) {
      this.PermissionSubscription.unsubscribe();
    }
    if (this.PermissionDeleteSubscription) {
      this.PermissionDeleteSubscription.unsubscribe();
    }
  }
}
