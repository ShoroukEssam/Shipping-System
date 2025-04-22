import { Component, OnInit } from '@angular/core';
import { BranchService } from '../../../shared/Services/branch.service';
import { Branch, getAllBranch } from '../../../shared/Models/branch';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-branch-list',
  templateUrl: './branch-list.component.html',
  styleUrls: ['./branch-list.component.css'],
})
export class BranchListComponent implements OnInit {
  branchData: getAllBranch[] = [];
  searchterm = '';
  recordLimit: number = 5;
  branch: Branch = {
    id: 0,
    name: '',
    stateId: 0,
  };

  loading: boolean = false;
  constructor(private _BranchService: BranchService, private _Router: Router) {}

  ngOnInit(): void {
    this.loadBranches();
  }

  loadBranches(): void {
    this.loading = true;
    this._BranchService.getBranches().subscribe(
      (branches) => {
        this.branchData = branches;
        console.log(this.branchData);
        if (this.branchData.length === 0) {
          this.showServerNotWorkingAlert();
        }

        this.loading = false;
      },
      (error) => {
        Swal.fire('عرض !', 'حدث خطأ في عرض الفروع', 'error');
        console.error('Error loading branches:', error);
        this.showApiConnectionErrorAlert();
        this.loading = false;
      }
    );
  }
  updateStatus(event: any, id: number): void {
    const checkbox = event.target as HTMLInputElement;
    if (!checkbox) return;

    const checked = checkbox.checked;

    if (id === undefined) {
      console.error(
        'Could not find data-branch-id attribute or it is not valid.'
      );
      return;
    }

    const branchToUpdate = this.branchData.find((branch) => branch.id === id);
    if (branchToUpdate) {
      branchToUpdate.status = checked;
      this._BranchService.updateBranchById(id, branchToUpdate).subscribe(
        (updatedBranch) => {
          this.showStatusChangeSuccessAlert();
          console.log('Status updated successfully:', updatedBranch);
        },
        (error) => {
          this.showStatusChangeErrorAlert();
          console.error('Error updating status:', error);
        }
      );
    }
  }

  deleteBranch(id: number): void {
    this._BranchService.deleteBranch(id).subscribe(
      () => {
        this.showDeleteSuccessAlert();
        console.log('Branch deleted successfully');
        this.loadBranches(); // Optionally refresh data after delete
      },
      (error) => {
        this.showDeleteErrorAlert();
        console.error('Error deleting branch:', error);
      }
    );
  }

  private showStatusChangeSuccessAlert(): void {
    Swal.fire('نجاح', 'تم تغيير حالة الفرع بنجاح!', 'success');
  }

  private showStatusChangeErrorAlert(): void {
    Swal.fire('خطأ', 'فشل في تغيير حالة الفرع.', 'error');
  }

  private showDeleteSuccessAlert(): void {
    Swal.fire('نجاح', 'تم حذف الفرع بنجاح!', 'success');
  }

  private showDeleteErrorAlert(): void {
    Swal.fire('خطأ', 'فشل في حذف الفرع.', 'error');
  }

  private showApiConnectionErrorAlert(): void {
    Swal.fire('خطأ', 'فشل الاتصال ب API.', 'error');
  }
  private showServerNotWorkingAlert(): void {
    Swal.fire('مشكلة في الخادم', 'الخادم لا يستجيب.', 'warning');
  }
}
