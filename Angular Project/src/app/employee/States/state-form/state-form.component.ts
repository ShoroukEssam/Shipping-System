import { Component, OnInit } from '@angular/core';

import { StateService } from '../../../shared/Services/state.service';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IState } from '../../../shared/Models/IState';


@Component({
  selector: 'app-state-form',
  templateUrl: './state-form.component.html',
  styleUrls: ['./state-form.component.css']
})
export class StateFormComponent implements OnInit {
  stateId: number = 0;
  form!: FormGroup;

  state: IState = {
      id: 0,
      name: '',
      status: false
  };

  isEditing: boolean = false;

  constructor(
      private stateService: StateService,
      private route: ActivatedRoute,
      private router: Router,
      private build: FormBuilder
  ) {}

  ngOnInit(): void {
      this.route.params.subscribe(params => {
          this.stateId = params['id'];
          if (this.stateId && this.stateId !== 0) {
              this.isEditing = true;
              this.loadState(this.stateId);
          }
      });

      this.form = this.build.group({
          name: ['', [Validators.required]],
          status: [null, [Validators.required]]
      });

      if (this.isEditing) {
          this.form.patchValue(this.state);
      }
  }

  loadState(stateId: number): void {
      this.stateService.getGovernmentById(stateId).subscribe({
          next: (state) => {
              this.state = state;
              this.form.patchValue(this.state);
          },
          error: (error) => {
              console.error('Error loading State:', error);
          }
      });
  }

  onSubmit(): void {
      if (this.form.valid) {
          this.state = { ...this.state, ...this.form.value };
          if (this.stateId != 0) {
              this.updateState();
          } else {
              this.addState();
          }
      }
  }

  addState(): void {
      this.isEditing = true;
      this.stateService.addNewGovernment(this.state).subscribe({
          next: (stateDto: IState) => {
              Swal.fire(
                  'إضافة محافظة!',
                  'تم إضافة المحافظة بنجاح.',
                  'success'
              );
              this.router.navigate(['/employee/state']);
          },
          error: (error: any) => {
              console.error('Error adding state:', error);
              let errorMessage = 'حدث خطأ أثناء إضافة المحافظة';
              if (error.error && error.error.message) {
                  errorMessage += `: ${error.error.message}`;
              }
              Swal.fire(
                  'إضافة محافظة!',
                  errorMessage,
                  'error'
              );
          }
      });
  }

  updateState(): void {
      this.isEditing = false;
      this.stateService.editGovernment(this.state.id, this.state).subscribe({
          next: () => {
              Swal.fire(
                  'تعديل محافظة!',
                  'تم تعديل المحافظة بنجاح.',
                  'success'
              );
              this.router.navigate(['/employee/state']);
          },
          error: (error: any) => {
              Swal.fire(
                  'تعديل محافظة!',
                  `حدث خطأ أثناء تعديل المحافظة: ${error.error}`,
                  'error'
              );
              console.error('Error updating state:', error);
          }
      });
  }
}
