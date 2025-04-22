import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../shared/Services/employee.service';
import Swal from 'sweetalert2';
import { BranchService } from '../../../shared/Services/branch.service';
import { getAllBranch } from '../../../shared/Models/branch';
import { IEmployeeData } from '../../../shared/Models/Employees';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.css']
})
export class EmployeeFormComponent implements OnInit {

  employeeId: string = '';
  BranchData: getAllBranch[] = [];

  employee: IEmployeeData = {
    id: '',
    name: '',
    email: '',
    phone: '',
    branchId: 0,
    branchName: '',
    role: '',
    password: '',
    status: false
  };

  isEditing: boolean = false;

  form !: FormGroup;

  constructor(
    private empService: EmployeeService,
    private branchService: BranchService,
    private route: ActivatedRoute,
    private router: Router,
    private build: FormBuilder

  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.employeeId = params['id'];
      this.branchService.getBranches().subscribe({
        next: (response) => {
          this.BranchData = response;
        },
        error: (err) => {
          console.log(err);
        },
      });
      if (this.employeeId && this.employeeId !== '0') {
        this.isEditing = true;
        this.loadEmployee(this.employeeId);
      }
    });
  
    this.form = this.build.group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      branchId: ['', [Validators.required]],
      role: ['', [Validators.required]],
      password: ['']
    });
  }
  

  loadEmployee(employeeId: string): void {
    this.empService.getEmployeeById(employeeId).subscribe({
      next: (employee) => {
        this.form.patchValue(employee);
        // this.employee = employee;
      },
      error: (error) => {
        console.error('Error loading employee:', error);
      }
    });
  }

  onSubmit(): void {
    if (this.isEditing) {
      this.updateEmployee();
    } else {
      this.addEmployee();
    }
  }

  addEmployee(): void {
    this.empService.addNewEmployee(this.form.value).subscribe({
      next: (employeeDto: IEmployeeData) => {
        Swal.fire(
          'إضافة موظف!',
          'تم إضافة الموظف بنجاح.',
          'success'
        );
        this.router.navigate(['/employee/employee']);
      },
      error: (error: any) => {
        console.error('Error adding employee:', error);
        let errorMessage = 'حدث خطأ أثناء إضافة الموظف';
        if (error.error && error.error.message) {
          errorMessage += `: ${error.error.message}`;
        }
        Swal.fire(
          'إضافة موظف!',
          errorMessage,
          'error'
        );
      }
    });
  }

  updateEmployee(): void {
    const emp = this.form.value;
    emp.id=this.employeeId;
    this.empService.editEmployee(this.employeeId, emp).subscribe({
      next: () => {
        Swal.fire(
          'تعديل موظف!',
          'تم تعديل الموظف بنجاح.',
          'success'
        );
        this.router.navigate(['/employee/employee']);
      },
      error: (error: any) => {
        Swal.fire(
          'تعديل موظف!',
          `حدث خطأ أثناء تعديل الموظف: ${error.error}`,
          'error'
        );
        console.error('Error updating employee:', error);
      }
    });
  }

  onChangeBranch(event: any): void {
    const selectedBranch = this.BranchData.find(branch => branch.id === +event.target.value);
    if (selectedBranch) {
      this.employee.branchId = selectedBranch.id;
      this.employee.branchName = selectedBranch.name;
    }
  }

  toggleStatus(): void {
    this.employee.status = !this.employee.status;
  }
}
