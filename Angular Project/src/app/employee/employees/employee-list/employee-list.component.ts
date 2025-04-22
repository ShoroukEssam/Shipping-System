import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../../shared/Services/employee.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { IEmployeeData } from '../../../shared/Models/Employees';


@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {

  employeesData: IEmployeeData[] = [];
  searchterm = '';
  recordLimit: number = 5;
  loading :boolean = false;

  constructor(private empService: EmployeeService, private router: Router) {}

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.loading = true;
    this.empService.getAllEmployees().subscribe({
      next: (response) => {
        this.employeesData = response;
        this.loading = false;
      },
      error: (error) => {
        Swal.fire(
          'عرض !',
          'حدث خطأ في عرض الموظفين',
          'error'
        );
        console.error('Error fetching employees:', error);
        this.loading = false;
      }
    });
  }

  deleteEmployee(employeeId: string): void {
    Swal.fire({
      title: 'هل انت متأكد',
      text: 'سيتم حذف هذا الموظف',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم, قم بالحذف',
      cancelButtonText: 'لا, الغاء',
    }).then((result) => {
      if (result.isConfirmed) {
        this.empService.deleteEmployee(employeeId).subscribe({
          next: () => {
            // Update employeesData after successful deletion
            this.employeesData = this.employeesData.filter(p => p.id !== employeeId);
            Swal.fire(
              'حذف موظف!',
              'تم حذف هذا الموظف.',
              'success'
            );
          },
          error: (error: any) => {
            Swal.fire(
              'حذف الموظف!',
              'لم يتم حذف هذا الموظف.',
              'error'
            );
            console.error('Error deleting employee:', error); // Log detailed error message
          }
        });
      }
    });
  }

  toggleStateStatus(emp: IEmployeeData): void {
    const previousStatus = emp.status;
    emp.status = !emp.status;

    this.empService.updateEmployeeStatus(emp.id, emp.status).subscribe({
      next: (response) => {
        console.log('Update successful:', response);
        Swal.fire(
          'تحديث الحالة!',
          'تم تحديث حالة الموظف بنجاح.',
          'success'
        );

        const index = this.employeesData.findIndex(e => e.id === emp.id);
        if (index !== -1) {
          this.employeesData[index].status = emp.status;
        }
      },
      error: (error: any) => {
        console.error('Update error:', error);
        Swal.fire(
          'تحديث الحالة!',
          `حدث خطأ أثناء تحديث الحالة: ${error.error}`,
          'error'
        );

        emp.status = previousStatus;
      }
    });
  }

  viewEmployee(employeeId: string): void {
    this.router.navigate(['/employee/employee', employeeId]);
  }

  editEmployee(employeeId: string): void {
    this.router.navigate(['/employee/employee', employeeId]);
  }
  onSearch(searchValue: string): void {
    if (searchValue.trim() === '') {
      this.loadEmployees();
    } else {
      this.employeesData = this.employeesData.filter(employee =>
        employee.name.toLowerCase().includes(searchValue.toLowerCase())
      );
    }
  }
}
