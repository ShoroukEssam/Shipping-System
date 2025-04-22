import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BranchService } from '../../../shared/Services/branch.service';
import { Branch, getAllBranch } from '../../../shared/Models/branch';

import { GovernmentService } from '../../../shared/Services/government.service';
import Swal from 'sweetalert2';
import { Government } from '../../../shared/Models/government';

@Component({
  selector: 'app-branch-form',
  templateUrl: './branch-form.component.html',
  styleUrls: ['./branch-form.component.css'],
})
export class BranchFormComponent implements OnInit {
  branchId: number | null = 0;
  branch: Branch | null = null;
  GovernmentData: Government[] = [];

  BranchData = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
    ]),
    stateId: new FormControl(0, Validators.required),
  });

  constructor(
    private _ActivatedRoute: ActivatedRoute,
    private _Router: Router,
    private _BranchService: BranchService,
    private _GovernmentService: GovernmentService
  ) {}

  ngOnInit(): void {
    const id = this._ActivatedRoute.snapshot.paramMap.get('id');
    this.branchId = id !== null ? +id : null;
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
    if (this.branchId !== null && this.branchId !== 0) {
      this._BranchService.getBranchById(this.branchId).subscribe({
        next: (response) => {
          this.branch = response;
          this.BranchData.patchValue({
            name: this.branch.name,
            stateId: this.branch.stateId, // Assuming stateId corresponds to governmentId
          });
          console.log('Branch fetched successfully', this.branch);
        },
        error: (err) => {
          console.error('Error fetching branch', err);
          this.showApiConnectionErrorAlert();
        },
      });
    }
  }

  onSubmit() {
    if (this.BranchData.valid) {
      const branch: Branch = {
        id: this.branchId || 0,
        name: this.BranchData.value.name!,
        stateId: this.BranchData.value.stateId!,
      };

      if (this.branchId && this.branchId !== 0) {
        this._BranchService.updateBranchById(this.branchId, branch).subscribe({
          next: (response) => {
            console.log('Branch updated successfully', response);
            this.showUpdateSuccessAlert();
            this._Router.navigate(['/employee/branch']);
          },
          error: (err) => {
            console.error('Error updating branch', err);
            this.showUpdateErrorAlert();
          },
        });
      } else {
        //------------------------ type updated with (any) ----------------------------------------
        this._BranchService.addBranch(branch).subscribe({
          next: (response) => {
            console.log('Branch added successfully', response);
            this.showAddSuccessAlert();
            this._Router.navigate(['/employee/branch']);
          },
          error: (err) => {
            console.error('Error adding branch', err.error.text);
            if (err.error.text.includes('تمت إضافة الفرع بنجاح.')) {
              this.showAddSuccessAlert();
              this._Router.navigate(['/employee/branch']);
            } else this.showAddErrorAlert();
          },
        });
      }
    }
  }

  onCancel() {
    this._Router.navigate(['/employee/branch']);
  }

  private showAddSuccessAlert(): void {
    Swal.fire('نجاح', 'تمت إضافة الفرع بنجاح.', 'success');
  }

  private showAddErrorAlert(): void {
    Swal.fire('خطأ', 'فشلت عملية إضافة الفرع.', 'error');
  }

  private showUpdateSuccessAlert(): void {
    Swal.fire('نجاح', 'تم تحديث بيانات الفرع بنجاح.', 'success');
  }

  private showUpdateErrorAlert(): void {
    Swal.fire('خطأ', 'فشلت عملية تحديث بيانات الفرع.', 'error');
  }

  private showApiConnectionErrorAlert(): void {
    Swal.fire('خطأ', 'لم يتم الاتصال API.', 'error');
  }
}
