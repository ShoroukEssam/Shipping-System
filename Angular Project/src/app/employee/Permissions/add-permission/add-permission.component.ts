import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionsService } from '../../../shared/Services/permissions.service';
import { IPermission } from '../../../shared/Models/Permissions/permission';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-permission',
  templateUrl: './add-permission.component.html',
  styleUrl: './add-permission.component.css'
})
export class AddPermissionComponent implements OnInit,OnDestroy {
  permissionId: string | null = '';
  updateSubscription: any;
  addSubscription: any;
  PermisstionData = new FormGroup({
    roleName: new FormControl("",[Validators.required,Validators.minLength(3)]),
  })
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private permissionsService: PermissionsService
  ) { }
  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
    if (this.addSubscription) {
      this.addSubscription.unsubscribe();
    }
  }
  ngOnInit(): void {
    this.permissionId = this.route.snapshot.paramMap.get('id');
    if (this.permissionId && this.permissionId !== '0') {
      this.permissionsService.getPermissionById(this.permissionId).subscribe(permission => {
        this.PermisstionData.patchValue(permission);
      });
    }
  }

  get getRoleName() {
    return this.PermisstionData.controls['roleName'];
  }

  onSubmit() {
    if (this.PermisstionData.valid) {
      const permission: any = this.PermisstionData.value;
      if (this.permissionId && this.permissionId !== '0') {
        permission.id = this.permissionId; 
        let date=new Date();
        permission.date = date.toLocaleString(); 
        this.updateSubscription = this.permissionsService.updatePermission(permission).subscribe({
          next: () => {
            this.router.navigate(['/employee/permission']);
          },
          error: (err) => {
            Swal.fire(
              'تعديل !',
              'حدث خطأ اثناء تعديل هذه الصلاحية',
              'error'
            );
            console.error('Error updating permission', err);
          }
        })
      } else {
        this.addSubscription = this.permissionsService.addPermission(permission).subscribe({
          next: () => {
            this.router.navigate(['/employee/permission']);
          },
          error: (err) => {
            console.error('Error adding permission', err);
            Swal.fire(
              'اضافة !',
              'حدث خطأ اثناء اضافة هذه الصلاحية',
              'error'
            );
          }
        })
      }
    }
  }

  onCancel() {
    this.router.navigate(['/employee/permission']);
  }
}
